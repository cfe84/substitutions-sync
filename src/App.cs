
using System;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Filesync
{
  class App
  {
    public App(string configFile)
    {
      var configText = File.ReadAllText(configFile);
      var config = JsonSerializer.Deserialize<Config>(configText, new JsonSerializerOptions
      {
        PropertyNameCaseInsensitive = false,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
      });
      ISubstitutionProvider main = null;
      var providers = config.Providers
        .Select((providerConfig) =>
        {
          ISubstitutionProvider provider = null;
          if (providerConfig.Type == "json")
            provider = new FileSubstitutionsProvider(providerConfig.Path, new JsonFileLoader());
          if (providerConfig.Type == "mac")
            provider = new FileSubstitutionsProvider(providerConfig.Path, new MacFileLoader());
          if (providerConfig.IsPrimarySource)
            main = provider;
          if (provider == null)
            Console.Error.WriteLine($"Provider {providerConfig.Type} unknown, skipping");
          return provider;
        })
        .Where(provider => provider != null)
        .ToList();
      Console.WriteLine($"{providers.Count()} providers");
      var monitor = new SubstitutionsMonitor(providers, main);
    }
  }
}