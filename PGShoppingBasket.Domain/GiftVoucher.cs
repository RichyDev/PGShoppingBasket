using System;
using System.Collections.Generic;
using System.Text;

namespace PGShoppingBasket.Domain
{
    public class GiftVoucher
    {
        public string Code { get; }
        public double Amount { get; }

        public GiftVoucher(string code, double amount)
        {
            Code = code;
            Amount = amount;
        }
    }
}
