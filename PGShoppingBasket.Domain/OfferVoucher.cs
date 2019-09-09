using System;
using System.Collections.Generic;
using System.Text;

namespace PGShoppingBasket.Domain
{
    public class OfferVoucher
    {
        public string Code { get; }
        public decimal Amount { get; }
        public decimal? BasketThreshold { get; }
        public ProductCategory Category { get; }

        public OfferVoucher(string code, decimal amount, decimal? basketThreshold = null, ProductCategory category = null)
        {
            if (string.IsNullOrEmpty(code))
                throw new ArgumentNullException(nameof(code));

            Code = code;

            if (amount <= 0.00m)
                throw new ArgumentOutOfRangeException(nameof(amount));

            Amount = amount;

            if(basketThreshold != null && basketThreshold <= 0.00m)
                throw new ArgumentOutOfRangeException(nameof(basketThreshold));

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
