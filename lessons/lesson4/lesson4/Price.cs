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

        #region Operators

        public static Price operator +(Price a, Price b) => BinaryOp(a, b, (x, y) => x + y);
        public static Price operator -(Price a, Price b) => BinaryOp(a, b, (x, y) => x - y);
        public static Price operator *(Price a, Price b) => BinaryOp(a, b, (x, y) => x * y);
        public static Price operator /(Price a, Price b) => BinaryOp(a, b, (x, y) => x / y);
        public static bool operator <(Price a, Price b) => BinaryOp(a, b, (x, y) => x < y);
        public static bool operator <=(Price a, Price b) => BinaryOp(a, b, (x, y) => x <= y);
        public static bool operator ==(Price a, Price b) => BinaryOp(a, b, (x, y) => x == y);
        public static bool operator !=(Price a, Price b) => BinaryOp(a, b, (x, y) => x != y);
        public static bool operator >=(Price a, Price b) => BinaryOp(a, b, (x, y) => x >= y);
        public static bool operator >(Price a, Price b) => BinaryOp(a, b, (x, y) => x > y);

        private static Price BinaryOp(Price x, Price y, Func<decimal, decimal, decimal> op)
        {
            if (object.ReferenceEquals(x, null) || object.ReferenceEquals(y, null)) throw new ArgumentNullException();
            if (x.Unit == y.Unit) return new Price(op(x.Amount, y.Amount), x.Unit);
            return new Price(op(x.Amount, y.ConvertTo(x.Unit).Amount), x.Unit);
        }

        private static bool BinaryOp(Price x, Price y, Func<decimal, decimal, bool> op)
        {
            if (object.ReferenceEquals(x, null) || object.ReferenceEquals(y, null)) throw new ArgumentNullException();
            if (x.Unit == y.Unit) return op(x.Amount, y.Amount);
            return op(x.Amount, y.ConvertTo(x.Unit).Amount);
        }

        public override bool Equals(object obj)
        {
            var other = obj as Price;
            if (object.ReferenceEquals(other, null)) return false;
            return this == other;
        }

        public override int GetHashCode()
        {
            return Amount.GetHashCode() ^ Unit.GetHashCode();
        }

        #endregion
    }
}

