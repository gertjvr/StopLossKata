using System;

namespace StopLossKata.Worker.Handlers.Commands
{
    public class AdjustSellPoint
    {
        public Guid StockId { get; set; }

        public AdjustSellPoint(Guid stockId)
        {
            StockId = stockId;
        }
    }
}