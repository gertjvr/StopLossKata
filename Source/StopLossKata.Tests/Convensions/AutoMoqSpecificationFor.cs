using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace StopLossKata.Tests.Convensions
{
    public abstract class AutoMoqSpecificationFor<T> : AutoFixtureSpecificationFor<T>
    {
        protected AutoMoqSpecificationFor(IFixture fixture)
            : base(fixture.Customize(new AutoMoqCustomization()))
        {
        }
        
        protected AutoMoqSpecificationFor()
            : base(new Fixture().Customize(new AutoMoqCustomization()))
        {
        }
    }
}