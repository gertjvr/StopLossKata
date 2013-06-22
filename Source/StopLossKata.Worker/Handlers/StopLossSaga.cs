using System;
using System.Linq;
using StopLossKata.Worker.Bus;
using StopLossKata.Worker.Handlers.Commands;
using StopLossKata.Worker.Handlers.Timeouts;

namespace StopLossKata.Worker.Handlers
{
    public class StopLossSaga
    {
        public IBus Bus { get; set; }
        
        public StopLossSagaData Data { get; set; }

        public bool Completed { get; protected set; }

        public void Handle(PositionAcquired message)
        {
            Data.StockId = message.StockId;
            Data.OriginalPrice = message.Price;

            RequestTimeout<CheckForPriceIncrease>(TimeSpan.FromSeconds(15));
            RequestTimeout<CheckForPriceDecrease>(TimeSpan.FromSeconds(30));
        }

        public void Handle(PriceChanged message)
        {
            Data.CurrentPrice = message.Price;

            RequestTimeout<CheckForPriceIncrease>(TimeSpan.FromSeconds(15));
            RequestTimeout<CheckForPriceDecrease>(TimeSpan.FromSeconds(30));
        }

        public void Handle(AdjustSellPoint message)
        {
            Data.RecalculateSellPoint();
        }

        public void Timeout(CheckForPriceIncrease state)
        {
            if (Data.CurrentPrice < Data.Prices.Last())
                return;

            Bus.Send(new AdjustSellPoint(Data.StockId));
        }

        public void Timeout(CheckForPriceDecrease state)
        {
            if (Data.CurrentPrice > Data.SellPoint)
                return;
            
            Bus.Publish(new SellStock(Data.StockId, Data.CurrentPrice));

            MarkAsComplete();
        }

        public void MarkAsComplete()
        {
            Completed = true;
        }

        private void RequestTimeout<T>(TimeSpan delay)
        {
            Bus.Defer(delay, default(T));
        }
    }
}