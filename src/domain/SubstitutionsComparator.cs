using System.Collections.Generic;
using System.Linq;

namespace Filesync
{
  public class SubstitutionsComparator
  {
    public static IEnumerable<Substitution> FindDeletedSubstitutions(
      IEnumerable<Substitution> existingSubstitutions,
      IEnumerable<Substitution> newSubstitutions)
    {
      // Find all existing subs where no newSub is found with the same word.
      var deletedSubstitutions = existingSubstitutions
        .Where(existingSubstitution => !newSubstitutions
          .Any(newSubstitution => newSubstitution.Shortcut == existingSubstitution.Shortcut));
      return deletedSubstitutions;
    }

    public static IEnumerable<Substitution> FindAddedSubstitutions(
      IEnumerable<Substitution> existingSubstitutions,
      IEnumerable<Substitution> newSubstitutions)
    {
      // Find all new subs where no existing is found with the same word.
      var addedSubstitutions = newSubstitutions
        .Where(newSubstitution => !existingSubstitutions
          .Any(existingSubstitution => newSubstitution.Shortcut == existingSubstitution.Shortcut));
      return addedSubstitutions;
    }

    public static IEnumerable<Substitution> FindModifiedSubstitutions(
      IEnumerable<Substitution> existingSubstitutions,
      IEnumerable<Substitution> newSubstitutions)
    {
      // Find all new subs where existing is found with word is different.
      var addedSubstitutions = newSubstitutions
        .Where(newSubstitution => existingSubstitutions
          .Any(existingSubstitution => newSubstitution.Shortcut == existingSubstitution.Shortcut
            && newSubstitution.Word != existingSubstitution.Word));
      return addedSubstitutions;
    }
  }
}