using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2
{
    class Program
    {
        enum CacheMode { OBJ=1, WEB=2, NO=3 }

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
            string cacheFilePath = Path.Combine(Environment.CurrentDirectory, "objcache.json");

            CacheMode cMode = CacheMode.OBJ;
            CacheMode dcMode = (File.Exists(cacheFilePath) ? CacheMode.OBJ : CacheMode.WEB);

            Console.WriteLine("Where should I load Data from?");

            if (dcMode == cMode)
                Console.WriteLine("  1) Object Cache");

            Console.WriteLine("  2) Web Cache");
            Console.WriteLine("  3) Nowhere\n");

            Console.Write("[{0}]:", (int)dcMode);

            string input = Console.ReadLine();

            Console.Clear();

            switch (input)
            {
                case "1": cMode = CacheMode.OBJ; break;
                case "2": cMode = CacheMode.WEB; break;
                case "3": cMode = CacheMode.NO;  break;
                default: cMode = dcMode; break;
            }

            List<Book> books = new List<Book>();

            if (cMode == CacheMode.OBJ)
                books = loadFromCache(cacheFilePath);
            else
                books = loadFromWeb(cMode, testNumbers);

            saveToCache(cacheFilePath, books);
                
            string hr = new string('-', 79);
            Console.WriteLine(hr);
            Console.WriteLine("| {0,17} | {1,30} | {2,10} | {3,9} |", "ISBN", "Title", "Published","Price");
            Console.WriteLine(hr);
            foreach (var book in books)
                Console.WriteLine("| {0,17} | {1,30} | {2,10} | {3,5} EUR |", book.ISBN, book.Title, book.PublishedAt, book.PriceInEUR);
            Console.WriteLine(hr);
            Console.ReadLine();
        }

        private static void saveToCache(string cacheFilePath, List<Book> books)
        {
            string json = JsonConvert.SerializeObject(books);
            using (StreamWriter sr = new StreamWriter(cacheFilePath))
                sr.Write(json);
        }

        private static List<Book> loadFromWeb(CacheMode cMode, string[] testNumbers)
        {
            List<Book> books = new List<Book>();

            foreach (string number in testNumbers)
            {
                Book newBook = new Book(number);
                newBook.LoadFromApi(cMode == CacheMode.NO);
                books.Add(newBook);
            }

            return books;
        }

        private static List<Book> loadFromCache(string cacheFilePath)
        {
            List<Book> books = new List<Book>();

            using(StreamReader sr = new StreamReader(cacheFilePath))
                books = JsonConvert.DeserializeObject<List<Book>>(sr.ReadToEnd());

            return books;
        }
    }
}
