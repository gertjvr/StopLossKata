using NUnit.Framework;
using Ploeh.AutoFixture;
using StopLossKata.Tests.Convensions;
using StopLossKata.Worker.Handlers;

namespace StopLossKata.Tests
{
    [TestFixture]
    public class When_setting_the_original_price : AutoNSubsituteSpecificationFor<StopLossSagaData>
    {
        protected decimal OriginalPrice;

        protected override StopLossSagaData Given()
        {
            OriginalPrice = Fixture.Create<decimal>();

            return Fixture.Create<StopLossSagaData>();
        }

        protected override void When()
        {
            Subject.OriginalPrice = OriginalPrice;
        }

        [Then]
        public void Should_set_the_original_price()
        {
            Assert.AreEqual(OriginalPrice, Subject.OriginalPrice);
        }

        [Then]
        public void Should_set_the_current_price()
        {
            Assert.AreEqual(OriginalPrice, Subject.CurrentPrice);
        }

        [Then]
        public void Should_set_the_selling_point()
        {
            Assert.AreEqual(OriginalPrice - (OriginalPrice * 0.1m), Subject.SellPoint);
        }
    }
}