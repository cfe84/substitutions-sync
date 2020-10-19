using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Filesync
{
  public class SubstitutionsMonitor
  {
    public SubstitutionsMonitor(IEnumerable<ISubstitutionProvider> providers, ISubstitutionProvider initialSubstitutionProvider = null)
    {
      Substitutions initialSubstitutions = null;
      if (initialSubstitutionProvider != null)
      {
        var task = initialSubstitutionProvider.GetSubstitutionsAsync();
        task.Wait();
        initialSubstitutions = task.Result;
      }

      foreach (var provider in providers)
      {
        provider.OnSubstitutionsUpdated += async (Substitutions substitutions) =>
        {
          await Task.WhenAll(providers
            .Where(p => p != provider)
            .Select(p => p.UpdateSubstitutionsAsync(substitutions)));
        };
        if (initialSubstitutions != null && !initialSubstitutionProvider.Equals(provider))
        {
          provider.UpdateSubstitutionsAsync(initialSubstitutions).Wait();
        }
      }
    }
  }
}