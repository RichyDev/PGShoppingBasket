using System;
using System.Collections.Generic;
using System.Text;

namespace PGShoppingBasket.Domain
{
    public abstract class Voucher
    {
        public string Code { get; }
        public decimal Amount { get; }

        protected Voucher(string code, decimal amount)
        {
            if (string.IsNullOrEmpty(code))
                throw new ArgumentNullException(nameof(code));

            Code = code;

            if (amount <= 0.00m)
                throw new ArgumentOutOfRangeException(nameof(amount));

            Amount = amount;
        }
    }
}
