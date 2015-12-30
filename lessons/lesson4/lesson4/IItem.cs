using System;

namespace lesson4
{
    public interface IItem
    {
        /// <summary>
        /// Gets a textual description of this item.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets the item's price.
        /// </summary>
        Price Price { get; }
    }
}

