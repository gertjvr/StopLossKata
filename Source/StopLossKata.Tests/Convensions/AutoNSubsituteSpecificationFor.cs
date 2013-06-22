using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoNSubstitute;

namespace StopLossKata.Tests.Convensions
{
    public abstract class AutoNSubsituteSpecificationFor<T> : AutoFixtureSpecificationFor<T>
    {
        protected AutoNSubsituteSpecificationFor(IFixture fixture)
            : base(fixture.Customize(new AutoNSubstituteCustomization()))
        {
        }
        
        protected AutoNSubsituteSpecificationFor()
            : base(new Fixture().Customize(new AutoNSubstituteCustomization()))
        {
        }
    }
}