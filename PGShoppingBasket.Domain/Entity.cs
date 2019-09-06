using System;
using System.Collections.Generic;
using System.Text;

namespace PGShoppingBasket.Domain
{
    public class Entity
    {
        public Guid Id { get; protected set; } = Guid.NewGuid();
    }
}
