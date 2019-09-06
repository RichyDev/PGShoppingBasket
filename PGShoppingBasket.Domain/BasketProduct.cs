using System;
using System.Collections.Generic;
using System.Text;

namespace PGShoppingBasket.Domain
{
    public class BasketProduct : Entity
    {
        public string Name { get; }
        public decimal Price { get; }
        public ProductCategory Category { get; }
        public int Quantity { get; set; }
        public decimal Total => Quantity * Price;

        public BasketProduct(string name, decimal price, ProductCategory category, int quantity)
        {
            Name = name;
            Price = price;
            Category = category;
            Quantity = quantity;
        }
    }
}
