using System;
using System.Collections.Generic;
using System.Text;

namespace PGShoppingBasket.Domain
{
    public class BasketProduct : Entity
    {
        public string Name { get; }
        public double Price { get; }
        public Guid CategoryId { get; }
        public int Quantity { get; }
        public double Total => Quantity * Price;

        public BasketProduct(string name, double price, Guid categoryId, int quantity)
        {
            Name = name;
            Price = price;
            CategoryId = categoryId;
            Quantity = quantity;
        }
    }
}
