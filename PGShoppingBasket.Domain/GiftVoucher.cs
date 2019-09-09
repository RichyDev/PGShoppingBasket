using System;
using System.Collections.Generic;
using System.Text;

namespace PGShoppingBasket.Domain
{
    public class GiftVoucher : Voucher
    {
        public GiftVoucher(string code, decimal amount) : base(code, amount)
        {

        }

        public override string ToString()
        {
            return $"{Amount} Gift Voucher {Code}";
        }
    }
}
