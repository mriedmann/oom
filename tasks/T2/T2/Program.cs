using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] testNumbers = new string[]
            {
                "978-3-492-28515-5",
                "978-3-492-97021-1",
                "3-492-28515-5",
                "3-492-28621-6",
                "978-3-492-97223-9"
            };

            List<Book> books = new List<Book>();

            foreach(string number in testNumbers)
            {
                Book newBook = new Book(number);
                newBook.LoadFromApi();
                books.Add(newBook);
            }
            string hr = new string('-', 79);
            Console.WriteLine(hr);
            Console.WriteLine("| {0,17} | {1,30} | {2,10} | {3,9} |", "ISBN", "Title", "Published","Price");
            Console.WriteLine(hr);
            foreach (var book in books)
                Console.WriteLine("| {0,17} | {1,30} | {2,10} | {3,5} EUR |", book.ISBN, book.Title, book.PublishedAt, book.PriceInEUR);
            Console.WriteLine(hr);
            Console.ReadLine();
        }
    }
}
