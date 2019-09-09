using System;
using System.Collections.Generic;
using System.Text;

namespace PGShoppingBasket.Domain
{
    public class GiftVoucher
    {
        public string Code { get; }
        public decimal Amount { get; }

        public GiftVoucher(string code, decimal amount)
        {
            if (string.IsNullOrEmpty(code))
                throw new ArgumentNullException(nameof(code));

            Code = code;

            if (amount <= 0.00m)
                throw new ArgumentOutOfRangeException(nameof(amount));

            Amount = amount;
        }

        public override string ToString()
        {
            return $"{Amount} Gift Voucher {Code}";
        }
    }
}
