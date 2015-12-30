using System;

namespace lesson3
{
    public interface IItem
    {
        /// <summary>
        /// Gets a textual description of this item.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets the item's price in the specified currency.
        /// </summary>
        decimal GetPrice(Currency currency);
    }
}

