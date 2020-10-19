using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System;

namespace Filesync
{
  class MacFileLoader : IFileLoader
  {
    public async Task<Substitutions> loadFileAsync(string path)
    {
      Console.WriteLine("Reading Mac");
      var content = await File.ReadAllTextAsync(path);
      var lines = content.Split("\n");
      var substitutions = lines
        .Select(line =>
        {
          var splat = line.Split(":");
          if (splat.Length != 2)
          {
            return null;
          }
          return new Substitution
          {
            Shortcut = splat[0],
            Word = splat[1]
          };
        })
        .Where(line => line != null);
      return new Substitutions { substitutions = substitutions };
    }

    public async Task saveFileAsync(Substitutions substitutions, string path)
    {
      Console.WriteLine("Writing Mac");
      var content = String.Join(
        '\n',
        substitutions.substitutions.Select(substitution => $"{substitution.Shortcut}:{substitution.Word}"));
      await File.WriteAllTextAsync(path, content);
    }
  }
}