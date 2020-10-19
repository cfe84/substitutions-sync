using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Filesync
{
  interface IFileLoader
  {
    Task<Substitutions> loadFileAsync(string path);
    Task saveFileAsync(Substitutions substitutions, string path);
  }

  class FileSubstitutionsProvider : ISubstitutionProvider
  {
    private FileSystemWatcher watcher;
    private string file;
    private IFileLoader fileLoader;
    public FileSubstitutionsProvider(string file, IFileLoader fileLoader)
    {
      Console.WriteLine($"New file substitution provider in {file} using {fileLoader} loader");
      if (!File.Exists(file))
      {
        throw new FileNotFoundException($"File does not exist: {file}");
      }
      this.fileLoader = fileLoader;
      this.file = file;
      var fileName = Path.GetFileName(file);
      var filePath = Path.GetDirectoryName(file);
      watcher = new FileSystemWatcher(filePath, fileName);
      var changeLock = new Object();
      var lastChange = DateTime.Now;
      watcher.Changed += async (object source, FileSystemEventArgs e) =>
      {
        if (this.OnSubstitutionsUpdated != null
        && DateTime.Now.Subtract(lastChange).TotalMilliseconds > 1100) // Sometimes 2 events get triggered for same change
        {
          lastChange = DateTime.Now;
          Thread.Sleep(1000);
          var substitutions = await fileLoader.loadFileAsync(file);
          await OnSubstitutionsUpdated(substitutions);
        }
      };
      watcher.EnableRaisingEvents = true;
    }

    public string id = Guid.NewGuid().ToString();
    public event SubstitutionsUpdatedHandlerAsync OnSubstitutionsUpdated;

    public async Task UpdateSubstitutionsAsync(Substitutions substitutions)
    {
      watcher.EnableRaisingEvents = false;
      await this.fileLoader.saveFileAsync(substitutions, this.file);
      watcher.EnableRaisingEvents = true;
    }

    public async Task<Substitutions> GetSubstitutionsAsync()
    {
      return await this.fileLoader.loadFileAsync(file);
    }
  }
}