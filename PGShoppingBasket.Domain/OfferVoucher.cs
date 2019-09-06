using System;
using System.Collections.Generic;
using System.Text;

namespace PGShoppingBasket.Domain
{
    public class OfferVoucher
    {
        public string Code { get; }
        public double Amount { get; }
        public double BasketThreshold { get; }
        public Guid[] CategoryIds { get; }

        public OfferVoucher(string code, double amount, double basketThreshold, Guid[] categoryIds = null)
        {
            Code = code;
            Amount = amount;
            BasketThreshold = basketThreshold;
            CategoryIds = categoryIds ?? new Guid[0];
        }
    }
}
