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
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;

            CannotBeDiscounted = cannotBeDiscounted;
        }
    }
}
