using System;
using System.Collections.Generic;

namespace StopLossKata.Worker.Handlers
{
    public class StopLossSagaData
    {
        private readonly List<decimal> _prices;

        private decimal _originalPrice;
        private decimal _currentPrice;

        public StopLossSagaData()
        {
             _prices = new List<decimal>();
        }

        public Guid StockId { get; set; }

        public decimal OriginalPrice
        {
            get
            {
                return _originalPrice;
            }
            set
            {
                _originalPrice = value;
                CurrentPrice = value;

                RecalculateSellPoint();
            }
        }

        public IEnumerable<decimal> Prices {
            get
            {
                return _prices;
            }
        }

        public decimal CurrentPrice { 
            get
            {
                return _currentPrice;
            }
            set
            {
                if (_currentPrice > 0)
                    _prices.Add(_currentPrice);

                _currentPrice = value;
            }
        }

        public decimal SellPoint { get; protected set; }

        public void RecalculateSellPoint(decimal margin = 0.1m)
        {
            SellPoint = CurrentPrice * (1 - margin);
        }
    }
}