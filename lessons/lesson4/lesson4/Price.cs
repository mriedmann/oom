using System;
using System.Net;
using System.Collections.Generic;
using System.Globalization;

namespace lesson4
{
    /// <summary>
    /// Price with currency.
    /// </summary>
    public class Price
    {
        /// <summary>
        /// Creates a price in given currency.
        /// </summary>
        public Price(decimal amount, Currency unit)
        {
            Amount = amount;
            Unit = unit;
        }

        /// <summary>
        /// Gets the amount.
        /// </summary>
        public decimal Amount { get; }

        /// <summary>
        /// Amount's currency.
        /// </summary>
        public Currency Unit { get; }

        /// <summary>
        /// Converts price to given currency.
        /// </summary>
        public Price ConvertTo(Currency targetCurrency)
        {
            if (targetCurrency == Unit) return this;
            return new Price(Amount * ExchangeRates.Get (Unit, targetCurrency), targetCurrency);
        }
    }
}

