using System;
using System.Collections.Generic;
using System.Text;

namespace PGShoppingBasket.Domain
{
    public class BasketProduct
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }

        public decimal Total => Quantity * Product.Price;

        public BasketProduct(Product product, int quantity)
        {
            Product = product ?? throw new ArgumentNullException(nameof(product));

            Quantity = quantity > 0 ? quantity : throw new ArgumentNullException(nameof(quantity));
        }
    }
}
