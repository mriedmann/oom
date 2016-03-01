using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// add unit tests project, reference lesson4 project
// refactor: extract Price + unit tests
// refactor: IItem: Price Price { get; }, [JsonIgnore], [JsonConstructor]
// https://ci.appveyor.com (lessons\lesson4\lesson4.sln, nuget restore lessons\lesson4\lesson4.sln)

namespace lesson4
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
                Console.WriteLine($"{x.Description.Truncate(50),-50} {x.Price.ConvertTo(currency).Amount,8:0.00} {currency}");
            }

            SerializationExample.Run(items);
        }
    }
}
