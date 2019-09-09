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

        public Product(string name, decimal price, ProductCategory category = null)
        {
            if(string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;

            Price = price;

            Category = category;
        }
    }
}
