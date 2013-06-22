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
    public class When_stock_trending_down : AutoNSubsituteSpecificationFor<StopLossSaga>
    {
        protected Guid StockId;
        protected decimal OriginalPrice;
        protected decimal CurrentPrice;

        protected override StopLossSaga Given()
        {
            OriginalPrice = 100;

            return Fixture
                .Build<StopLossSaga>()
                .Without(p => p.Data)
                .Do(p => p.Data = new StopLossSagaData { StockId = StockId })
                .Create();
        }

        protected override void When()
        {
            var priceGenerator = Fixture.Create<Generator<decimal>>();
            
            Subject.Handle(new PositionAcquired(StockId, OriginalPrice));

            CurrentPrice = priceGenerator.First(p => p < OriginalPrice);
            Subject.Handle(new PriceChanged(StockId, CurrentPrice));
            
            Subject.Timeout(new CheckForPriceIncrease());

            CurrentPrice = priceGenerator.First(p => p < CurrentPrice);
            Subject.Handle(new PriceChanged(StockId, CurrentPrice));
            
            Subject.Timeout(new CheckForPriceIncrease());
            Subject.Timeout(new CheckForPriceDecrease());
        }

        [Then]
        public void Should_have_sold_stock()
        {
            Subject.Bus.Received(1).Publish(Arg.Any<SellStock>());
        }

        [Then]
        public void Should_clear_stockloss()
        {
            Assert.True(Subject.Completed);
        }

        [Then]
        public void Should_set_3_timeout_to_check_for_price_increase()
        {
            Subject.Bus.Received(3).Defer(TimeSpan.FromSeconds(15), Arg.Any<CheckForPriceIncrease>());
        }

        [Then]
        public void Should_set_3_timeout_to_check_for_price_decrease()
        {
            Subject.Bus.Received(3).Defer(TimeSpan.FromSeconds(30), Arg.Any<CheckForPriceDecrease>());
        }
    }
}