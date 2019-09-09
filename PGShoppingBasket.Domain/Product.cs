using System;
using System.Collections.Generic;
using System.Text;

namespace PGShoppingBasket.Domain
{
    public class Product : Entity
    {
        public string Name { get; }
        public decimal Price { get; }
        public ProductCategory Category { get; }
        public int Quantity { get; set; }
        public decimal Total => Quantity * Price;

        public Product(string name, decimal price, int quantity, ProductCategory category = null)
        {
            if(string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;

            Price = price;

            Category = category;

            if (quantity <= 0)
                throw new ArgumentNullException(nameof(quantity));

            Quantity = quantity;
        }
    }
}
