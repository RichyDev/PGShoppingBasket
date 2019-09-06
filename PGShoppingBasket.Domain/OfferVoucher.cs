using System;
using System.Collections.Generic;
using System.Text;

namespace PGShoppingBasket.Domain
{
    public class OfferVoucher
    {
        public string Code { get; }
        public decimal Amount { get; }
        public decimal BasketThreshold { get; }
        public ProductCategory Category { get; }

        public OfferVoucher(string code, decimal amount, decimal basketThreshold, ProductCategory category = null)
        {
            Code = code;
            Amount = amount;
            BasketThreshold = basketThreshold;
            Category = category;
        }

        public override string ToString()
        {
            var voucherBuilder = new StringBuilder($"£{Amount} off");

            if (Category != null)
                voucherBuilder.Append($" {Category.Name} in");

            voucherBuilder.Append($" baskets over £{BasketThreshold} Offer Voucher {Code}");

            return voucherBuilder.ToString();
        }
    }
}
