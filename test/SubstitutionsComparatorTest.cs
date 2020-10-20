using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeItEasy;
using Xunit;

namespace Filesync
{
  public class SubstitutionsComparatorTest
  {
    [Fact]
    public void TestDelete()
    {
      List<Substitution> existingSubstitutions = CreateExistingSubstitutions();
      List<Substitution> newSubstitutions = CreateNewSubstitutions();

      // when
      var deletedSubstitutions = SubstitutionsComparator.FindDeletedSubstitutions(existingSubstitutions, newSubstitutions);

      // then
      Assert.Equal(1, deletedSubstitutions.Count());
      Assert.Contains(existingSubstitutions[2], deletedSubstitutions);
    }


    [Fact]
    public void TestAdded()
    {
      List<Substitution> existingSubstitutions = CreateExistingSubstitutions();
      List<Substitution> newSubstitutions = CreateNewSubstitutions();

      // when
      var addedSubstitutions = SubstitutionsComparator.FindAddedSubstitutions(existingSubstitutions, newSubstitutions);

      // then
      Assert.Equal(1, addedSubstitutions.Count());
      Assert.Contains(newSubstitutions[3], addedSubstitutions);
    }

    [Fact]
    public void TestModified()
    {
      List<Substitution> existingSubstitutions = CreateExistingSubstitutions();
      List<Substitution> newSubstitutions = CreateNewSubstitutions();

      // when
      var modifiedSubstitutions = SubstitutionsComparator.FindModifiedSubstitutions(existingSubstitutions, newSubstitutions);

      // then
      Assert.Equal(1, modifiedSubstitutions.Count());
      Assert.Contains(newSubstitutions[1], modifiedSubstitutions);
    }

    private static List<Substitution> CreateNewSubstitutions()
    {
      return new List<Substitution>{
        new Substitution { Shortcut = "a", Word = "aword" },
        new Substitution { Shortcut = "b", Word = "changed word" },
        new Substitution { Shortcut = "d", Word = "dword" },
        new Substitution { Shortcut = "e", Word = "eword" },
        };
    }

    private static List<Substitution> CreateExistingSubstitutions()
    {
      // given
      return new List<Substitution>{
        new Substitution { Shortcut = "a", Word = "aword" },
        new Substitution { Shortcut = "b", Word = "bword" },
        new Substitution { Shortcut = "c", Word = "cword" },
        new Substitution { Shortcut = "d", Word = "dword" },
        };
    }
  }
}