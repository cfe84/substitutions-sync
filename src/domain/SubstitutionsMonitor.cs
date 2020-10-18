using System.Linq;
using System.Threading.Tasks;

namespace Filesync
{
  public class SubstitutionsMonitor
  {
    public SubstitutionsMonitor(ISubstitutionProvider[] providers)
    {
      foreach (var provider in providers)
      {
        provider.OnSubstitutionsUpdated += async (Substitutions substitutions) =>
        {
          await Task.WhenAll(providers
            .Where(p => p != provider)
            .Select(p => p.UpdateSubstitutionsAsync(substitutions)));
        };
      }
    }
  }
}