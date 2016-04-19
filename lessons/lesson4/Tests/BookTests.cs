using NUnit.Framework;
using System;
using lesson4;

namespace Tests
{
    [TestFixture]
    public class BookTests
    {
        [Test]
        public void CanCreateBook()
        {
            var x = new Book("Real-Time Rendering", "978-1568814247", 78.95m, Currency.EUR);

            Assert.IsTrue(x.Title == "Real-Time Rendering");
            Assert.IsTrue(x.ISBN == "978-1568814247");
            Assert.IsTrue(x.Price.Amount == 78.95m);
            Assert.IsTrue(x.Price.Unit == Currency.EUR);
        }

        [Test]
        public void CannotCreateBookWithEmptyTitle1()
        {
            Assert.Catch(() =>
            {
                var x = new Book(null, "978-1568814247", 78.95m, Currency.EUR);
            });
        }

        [Test]
        public void CannotCreateBookWithEmptyTitle2()
        {
            Assert.Catch(() =>
            {
                var x = new Book("", "978-1568814247", 78.95m, Currency.EUR);
            });
        }

        [Test]
        public void CannotCreateBookWithEmptyIsbn1()
        {
            Assert.Catch(() =>
            {
                var x = new Book("Real-Time Rendering", null, 78.95m, Currency.EUR);
            });
        }

        [Test]
        public void CannotCreateBookWithEmptyIsbn2()
        {
            Assert.Catch(() =>
            {
                var x = new Book("Real-Time Rendering", "", 78.95m, Currency.EUR);
            });
        }

        [Test]
        public void CannotCreateBookWithNegativePrice()
        {
            Assert.Catch(() =>
            {
                var x = new Book("Real-Time Rendering", "978-1568814247", -1, Currency.EUR);
            });
        }

        [Test]
        public void CanUpdateBookWithPrice()
        {
            var x = new Book("Real-Time Rendering", "978-1568814247", 78.95m, Currency.EUR);
            x.UpdatePrice(39.90m, Currency.CHF);

            Assert.IsTrue(x.Price.Amount == 39.90m);
            Assert.IsTrue(x.Price.Unit == Currency.CHF);
        }

        [Test]
        public void CannotUpdateBookWithNegativePrice()
        {
            Assert.Catch(() =>
            {
                var x = new Book("Real-Time Rendering", "978-1568814247", 78.95m, Currency.EUR);
                x.UpdatePrice(-9.90m, Currency.CHF);
            });
        }
    }
}

