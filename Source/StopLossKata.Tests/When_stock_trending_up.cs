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
    public class When_stock_trending_up : AutoNSubsituteSpecificationFor<StopLossSaga>
    {
        protected Guid StockId;
        protected decimal OriginalPrice;
        protected decimal CurrentPrice;

        protected override StopLossSaga Given()
        {
            OriginalPrice = Fixture.Create<decimal>();

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

            CurrentPrice = priceGenerator.First(p => p > OriginalPrice);
            Subject.Handle(new PriceChanged(StockId, CurrentPrice));
            
            Subject.Timeout(new CheckForPriceIncrease());
            Subject.Handle(new AdjustSellPoint(StockId));

            CurrentPrice = priceGenerator.First(p => p > CurrentPrice);
            Subject.Handle(new PriceChanged(StockId, CurrentPrice));
            
            Subject.Timeout(new CheckForPriceIncrease());
            Subject.Handle(new AdjustSellPoint(StockId));

            Subject.Timeout(new CheckForPriceDecrease());

            CurrentPrice = priceGenerator.First(p => p > CurrentPrice);
            Subject.Handle(new PriceChanged(StockId, CurrentPrice));
            
            Subject.Timeout(new CheckForPriceIncrease());
            Subject.Handle(new AdjustSellPoint(StockId));

            CurrentPrice = priceGenerator.First(p => p > CurrentPrice);
            Subject.Handle(new PriceChanged(StockId, CurrentPrice));
            
            Subject.Timeout(new CheckForPriceIncrease());
            Subject.Handle(new AdjustSellPoint(StockId));

            Subject.Timeout(new CheckForPriceDecrease());
        }

        [Then]
        public void Should_have_adjusted_selling_point()
        {
            Subject.Bus.Received(4).Send(Arg.Any<AdjustSellPoint>());
        }

        [Then]
        public void Should_have_recalulated_selling_point()
        {
            Assert.AreEqual(CurrentPrice - (CurrentPrice * 0.1m), Subject.Data.SellPoint);
        }

        [Then]
        public void Should_keep_stoploss_active()
        {
            Assert.False(Subject.Completed);
        }

        [Then]
        public void Should_set_5_timeout_to_check_for_price_increase()
        {
            Subject.Bus.Received(5).Defer(TimeSpan.FromSeconds(15), Arg.Any<CheckForPriceIncrease>());
        }

        [Then]
        public void Should_set_5_timeout_to_check_for_price_decrease()
        {
            Subject.Bus.Received(5).Defer(TimeSpan.FromSeconds(30), Arg.Any<CheckForPriceDecrease>());
        }
    }
}