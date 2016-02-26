using NUnit.Framework;
using System;
using lesson4;

namespace Tests
{
    [TestFixture]
    public class PriceTest
    {
        [Test]
        public void CanCreatePrice()
        {
            var x = new Price(1, Currency.EUR);
            Assert.IsTrue(x.Amount == 1);
            Assert.IsTrue(x.Unit == Currency.EUR);
        }
        
        [Test]
        public void CanConvertPrice()
        {
            var x = new Price(1, Currency.EUR);
            Assert.IsTrue(x.ConvertTo(Currency.JPY).Amount > 1);
        }

        [Test]
        public void CanAddPrices()
        {
            var x = new Price(1, Currency.EUR) + new Price(1, Currency.EUR);
            Assert.IsTrue(x.Unit == Currency.EUR);
            Assert.IsTrue(x.Amount == 2);
        }

        [Test]
        public void CanAddPricesWithDifferentCurrency()
        {
            var x = new Price(1, Currency.EUR) + new Price(1, Currency.USD);
            Assert.IsTrue(x.Unit == Currency.EUR);
            Assert.IsTrue(x.Amount > 1);
        }

        [Test]
        public void CanComparePrices()
        {
            var a = new Price(1, Currency.EUR);
            var b = new Price(1, Currency.EUR);
            Assert.IsFalse(a < b);
            Assert.IsTrue(a <= b);
            Assert.IsTrue(a == b);
            Assert.IsFalse(a != b);
            Assert.IsTrue(a >= b);
            Assert.IsFalse(a > b);
        }
    }
}

