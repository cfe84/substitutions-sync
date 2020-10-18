using System.Threading.Tasks;

namespace Filesync
{
  public delegate Task SubstitutionsUpdatedHandlerAsync(Substitutions substitutions);

  public interface ISubstitutionProvider
  {
    Task UpdateSubstitutionsAsync(Substitutions substitutions);
    event SubstitutionsUpdatedHandlerAsync OnSubstitutionsUpdated;
  }
}