using System;

namespace StopLossKata.Worker.Handlers.Commands
{
    public class SellStock
    {
        public Guid StockId { get; protected set; }

        public decimal Price { get; protected set; }

        public SellStock(Guid stockId, decimal price)
        {
            StockId = stockId;
            Price = price;
        }
    }
}