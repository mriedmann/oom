using NUnit.Framework;
using System;
using lesson4;

namespace Tests
{
    [TestFixture]
    public class ExchangeRatesTest
    {
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
        public void ExchangeRateFromCurrencyAtoCurrencyB_ShouldBeInverseOfCurrencyBtoCurrencyA()
        {
            var x = ExchangeRates.Get(Currency.USD, Currency.GBP);
            var y = ExchangeRates.Get(Currency.GBP, Currency.USD);

            var p = x * y; // should be approximately 1.0
            Assert.IsTrue(p > 0.999m && p < 1.001m);
        }
    }
}

