namespace Filesync
{
  class Config
  {
    public ProviderConfiguration[] Providers { get; set; }
  }

  public class ProviderConfiguration
  {
    public string Type { get; set; }
    public string Path { get; set; }
    public bool IsPrimarySource { get; set; } = false;
  }
}