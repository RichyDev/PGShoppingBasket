using System;
using System.Collections.Generic;
using System.Text;

namespace PGShoppingBasket.Domain
{
    public class ProductCategory : Entity
    {
        public string Name { get; }
        public bool CannotBeDiscounted { get; }

        public ProductCategory(string name, bool cannotBeDiscounted = false)
        {
            Name = name;
            CannotBeDiscounted = cannotBeDiscounted;
        }
    }
}
