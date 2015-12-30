using System;
using System.Linq;

namespace lesson2
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			var books = new []
			{
				new Book("Real-Time Rendering", "978-1568814247", 78.95m, Currency.EUR),
				new Book("The Hitchhiker's Guide to the Galaxy", "978-0345391803", 6.60m, Currency.EUR),
				new Book("C# 6.0 in a Nutshell", "978-1491927069", 44.95m, Currency.EUR),
				new Book("Trillions: Thriving in the Emerging Information Ecology", "978-1118176078", 35.24m, Currency.EUR),
				new Book("Cryptonomicon", "978-0060512804", 7.70m, Currency.EUR),
			};

			var currency = Currency.EUR;
			foreach (var b in books)
			{
				Console.WriteLine("{0} {1,-40} {2,8:0.00} {3}", b.ISBN, b.Title.Truncate(40), b.GetPrice(currency), currency);
			}

			var isbns = books.Select(x => x.ISBN).OrderBy(x => x);
			Console.WriteLine();
			Console.WriteLine("ISBNs");
			foreach (var x in isbns) Console.WriteLine(x);
		}
	}
}
