using NUnit.Framework;
using System;
using lesson4;

namespace Tests
{
    [TestFixture]
    public class GiftCardTests
    {
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
    }
}

