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
            Code = code;
            Amount = amount;
        }

        public override string ToString()
        {
            return $"{Amount} Gift Voucher {Code}";
        }
    }
}
