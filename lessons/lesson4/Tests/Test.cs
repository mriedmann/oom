using NUnit.Framework;
using System;
using lesson4;

namespace Tests
{
    [TestFixture]
    public class Test
    {
        #region Price & ExchangeRates

        [Test]
        public void CanCreatePrice()
        {
            var x = new Price(1, Currency.EUR);
            Assert.IsTrue(x.Amount == 1);
            Assert.IsTrue(x.Unit == Currency.EUR);
        }

        [Test]
        public void ExchangeRateForSameCurrencyIsOne()
        {
            var x = ExchangeRates.Get(Currency.EUR, Currency.EUR);
            Assert.IsTrue(x == 1);
        }

        [Test]
        public void ExchangeRateForDifferentCurrencyIsNotOne()
        {
            var x = ExchangeRates.Get(Currency.EUR, Currency.JPY);
            Assert.IsTrue(x != 1);
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

        #endregion

        #region GiftCard

        [Test]
        public void CanCreateGiftCard()
        {
            var x = new GiftCard(100, Currency.EUR);
            Assert.IsTrue(x.IsRedeemed == false);
        }

        [Test]
        public void CanRedeemGiftCard()
        {
            var x = new GiftCard(100, Currency.EUR);
            Assert.IsTrue(x.IsRedeemed == false);
            x.Redeem();
            Assert.IsTrue(x.IsRedeemed == true);
        }

        [Test]
        public void CannotRedeemGiftCardTwice()
        {
            var x = new GiftCard(100, Currency.EUR);
            Assert.IsTrue(x.IsRedeemed == false);
            x.Redeem();
            Assert.IsTrue(x.IsRedeemed == true);

            try
            {
                x.Redeem();
                Assert.Fail();
            }
            catch
            {
            }
        }

        #endregion
    }
}

