using System;
using System.Collections.Generic;
using System.Text;

namespace PGShoppingBasket.Domain
{
    public class ProductCategory : Entity
    {
        public string Name { get; }

        public ProductCategory(string name)
        {
            Name = name;
        }
    }
}
