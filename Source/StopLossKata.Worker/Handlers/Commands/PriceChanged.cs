using System;

namespace StopLossKata.Worker.Handlers.Commands
{
    public class PriceChanged
    {
        public Guid StockId { get; protected set; }

        public decimal Price { get; protected set; }

        public PriceChanged(Guid stockId, decimal price)
        {
            StockId = stockId;
            Price = price;
        }
    }
}