using System;
using System.Collections.Generic;
using System.Text;
using PGShoppingBasket.Domain;

namespace PGShoppingBasket.Test
{
    public static class BasketExtensions
    {
        public static string Overview(this Basket basket)
        {
            var basketBuilder = new StringBuilder();

            basketBuilder.AppendLine("Basket");

            foreach (var p in basket.Products)
            {
                basketBuilder.AppendLine($"{p.Quantity} {p.Name} @ £{p.Total}");
            }

            basketBuilder.AppendLine("------------");

            foreach (var g in basket.GiftVouchers)
            {
                basketBuilder.AppendLine($"1 x {g} applied");
            }

            foreach (var o in basket.OfferVouchers)
            {
                basketBuilder.AppendLine($"1 x {o} applied");
            }

            basketBuilder.AppendLine("------------");

            basketBuilder.AppendLine($"Total: {basket.Total}");

            basketBuilder.AppendLine("------------");

            foreach (var m in basket.Messages)
            {
                basketBuilder.AppendLine($"Message: {m}");
            }

            return basketBuilder.ToString();
        }
    }
}
