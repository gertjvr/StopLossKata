using System;

namespace StopLossKata.Worker.Handlers.Commands
{
    public class PositionAcquired
    {
        public Guid StockId { get; protected set; }

        public decimal Price { get; protected set; }

        public PositionAcquired(Guid stockId, decimal price)
        {
            StockId = stockId;
            Price = price;
        }
    }
}