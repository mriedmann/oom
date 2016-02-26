using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// add IItem
// Book:IItem
// add GiftCard:IItem (copy&paste GetPrice currency conversion)
// Main: add gift cards to items array
// Main: foreach item print description/price (polymorphic)
// add Json.NET
// add (de)serialization example

namespace lesson3
{
    class MainClass
    {
        public static void Main (string[] args)
        {
            var items = new IItem[]
            {
                new Book("Real-Time Rendering", "978-1568814247", 78.95m, Currency.EUR),
                new Book("The Hitchhiker's Guide to the Galaxy", "978-0345391803", 6.60m, Currency.EUR),
                new Book("C# 6.0 in a Nutshell", "978-1491927069", 44.95m, Currency.EUR),
                new Book("Trillions: Thriving in the Emerging Information Ecology", "978-1118176078", 35.24m, Currency.EUR),
                new Book("Cryptonomicon", "978-0060512804", 7.70m, Currency.EUR),
                new GiftCard(50, Currency.EUR),
                new GiftCard(10, Currency.EUR),
                new GiftCard(100, Currency.EUR),
            };

            var currency = Currency.EUR;
            foreach (var x in items)
            {
                Console.WriteLine($"{x.Description.Truncate(50),-50} {x.GetPrice(currency),8:0.00} {currency}");
            }
        }
    }
}
