using FakeItEasy;
using Xunit;

namespace Filesync
{
  public class SubstitutionsMonitorTest
  {
    [Fact]
    public async void ShouldUpdateSubstitutionsWhenOneUpdates()
    {
      // given
      var provider1 = A.Fake<ISubstitutionProvider>();
      var provider2 = A.Fake<ISubstitutionProvider>();
      var provider3 = A.Fake<ISubstitutionProvider>();
      var monitor = new SubstitutionsMonitor(new[] { provider1, provider2, provider3 });
      var substitutions = new Substitutions();

      // when
      provider1.OnSubstitutionsUpdated += Raise.FreeForm.With(substitutions);

      // then
      A.CallTo(() => provider2.UpdateSubstitutionsAsync(substitutions))
        .MustHaveHappenedOnceExactly();
      A.CallTo(() => provider3.UpdateSubstitutionsAsync(substitutions))
        .MustHaveHappenedOnceExactly();
      A.CallTo(() => provider1.UpdateSubstitutionsAsync(substitutions))
        .MustNotHaveHappened();
    }
  }
}