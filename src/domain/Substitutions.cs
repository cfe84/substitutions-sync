using System.Collections.Generic;


namespace Filesync
{
  public class Substitutions
  {
    public IEnumerable<Substitution> substitutions { get; set; } = new List<Substitution>();
  }
}