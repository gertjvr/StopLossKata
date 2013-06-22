using System;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using Ploeh.AutoFixture;
using StopLossKata.Tests.Convensions;
using StopLossKata.Worker.Handlers;
using StopLossKata.Worker.Handlers.Commands;
using StopLossKata.Worker.Handlers.Timeouts;

namespace StopLossKata.Tests
{
    [TestFixture]
    public class When_checking_for_price_deccrease_and_the_stock_price_has_decreased : AutoNSubsituteSpecificationFor<StopLossSaga>
    {
        protected Guid StockId;
        protected decimal OriginalPrice;
        protected decimal CurrentPrice;

        protected override StopLossSaga Given()
        {
            StockId = Fixture.Create<Guid>();

            OriginalPrice = Fixture.Create<decimal>();
            
            CurrentPrice = Fixture.Create<Generator<decimal>>().First(p => p < OriginalPrice * 0.9m);

            return Fixture
                .Build<StopLossSaga>()
                .Without(p => p.Data)
                .Do(p => p.Data = new StopLossSagaData { StockId = StockId })
                .Create();
        }

        protected override void When()
        {
            Subject.Handle(new PositionAcquired(StockId, OriginalPrice));
            
            Subject.Handle(new PriceChanged(StockId, CurrentPrice));

            Subject.Timeout(new CheckForPriceDecrease());
        }

        [Then]
        public void Should_sell_the_stock()
        {
            Subject.Bus.Received(1).Publish(Arg.Any<SellStock>());
        }

        [Then]
        public void Should_complete_the_saga()
        {
            Assert.True(Subject.Completed);
        }
    }
}