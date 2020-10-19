using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Filesync
{
  class JsonFileLoader : IFileLoader
  {
    public async Task<Substitutions> loadFileAsync(string path)
    {
      Console.WriteLine("Reading JSON");
      var content = await File.ReadAllTextAsync(path);
      var substitutions = JsonSerializer.Deserialize<Substitutions>(content, new JsonSerializerOptions
      {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
      });
      return substitutions;
    }

    public async Task saveFileAsync(Substitutions substitutions, string path)
    {
      Console.WriteLine("Writing JSON");
      var content = JsonSerializer.Serialize(substitutions, new JsonSerializerOptions
      {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
      });
      await File.WriteAllTextAsync(path, content);
    }
  }
}