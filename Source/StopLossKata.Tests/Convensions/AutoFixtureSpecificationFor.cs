using Ploeh.AutoFixture;

namespace StopLossKata.Tests.Convensions
{
    public abstract class AutoFixtureSpecificationFor<T> : SpecificationFor<T>
    {
        protected IFixture Fixture;

        protected AutoFixtureSpecificationFor(IFixture fixture)
        {
            Fixture = fixture;
        }
    }
}