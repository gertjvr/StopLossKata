using System;
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
    public class When_receiving_stock_acquition : AutoNSubsituteSpecificationFor<StopLossSaga>
    {
        protected Guid StockId;
        protected decimal OriginalPrice;

        protected override StopLossSaga Given()
        {
            StockId = Fixture.Create<Guid>();

            OriginalPrice = Fixture.Create<decimal>();

            return Fixture
                .Build<StopLossSaga>()
                .Without(p => p.Data)
                .Do(p => p.Data = new StopLossSagaData { StockId = StockId })
                .Create();
        }

        protected override void When()
        {
            Subject.Handle(new PositionAcquired(StockId, OriginalPrice));
        }

        [Then]
        public void Should_set_a_timeout_to_check_for_price_increase()
        {
            Subject.Bus.Received(1).Defer(TimeSpan.FromSeconds(15), Arg.Any<CheckForPriceIncrease>());
        }

        [Then]
        public void Should_set_a_timeout_to_check_for_price_decrease()
        {
            Subject.Bus.Received(1).Defer(TimeSpan.FromSeconds(30), Arg.Any<CheckForPriceDecrease>());
        }
    }
}