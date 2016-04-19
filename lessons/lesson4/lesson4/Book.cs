using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using Newtonsoft.Json;

namespace lesson4
{
    public class Book : IItem
    {
        private Price m_price;

        /// <summary>
        /// Creates a new book object.
        /// </summary>
        /// <param name="title">Title must not be empty.</param>
        /// <param name="isbn">International Standard Book Number.</param>
        /// <param name="price">Price must not be negative.</param>
        public Book(string title, string isbn, decimal price, Currency currency)
            : this(title, isbn, new Price(price, currency))
        {
        }

        /// <summary>
        /// Creates a new book object.
        /// </summary>
        /// <param name="title">Title must not be empty.</param>
        /// <param name="isbn">International Standard Book Number.</param>
        /// <param name="price">Price must not be negative.</param>
        [JsonConstructor]
        public Book(string title, string isbn, Price price)
        {
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title must not be empty.", nameof(title));
            if (string.IsNullOrWhiteSpace(isbn)) throw new ArgumentException("ISBN must not be empty.", nameof(isbn));

            Title = title;
            ISBN = isbn;
            UpdatePrice(price.Amount, price.Unit);
        }

        /// <summary>
        /// Gets the book title.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets the International Standard Book Number.
        /// </summary>
        public string ISBN { get; }

        /// <summary>
        /// Updates the book's price.
        /// </summary>
        /// <param name="newPrice">Price must not be negative.</param>
        /// <param name="newCurrency">Currency.</param>
        public void UpdatePrice(decimal newPrice, Currency currency)
        {
            if (newPrice < 0) throw new ArgumentException("Price must not be negative.", nameof(newPrice));
            m_price = new Price(newPrice, currency);
        }

        /// <summary>
        /// Updates the book's price.
        /// </summary>
        /// <param name="newPrice">Price must not be negative.</param>
        public void UpdatePrice(Price newPrice)
        {
            if (newPrice.Amount < 0) throw new ArgumentException("Price must not be negative.", nameof(newPrice));
            m_price = newPrice;
        }

        #region IItem implementation

        /// <summary>
        /// Gets a textual description of this item.
        /// </summary>
        public string Description => Title;

        /// <summary>
        /// Gets the book's price in the given currency.
        /// </summary>
        public Price Price => m_price;

        #endregion
    }
}

