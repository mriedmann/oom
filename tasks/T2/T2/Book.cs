using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace T2
{
    

    public class Book : IBook
    {
        public string Title { get; set; }
        public ISBN ISBN { get; set; }
        public string PublishedAt { get; set; }
        public List<string> Authors { get; set; }

        public int PriceInEUR { get; private set; }

        public DateTime PriceDate { get; private set; }

        private const string apiBaseUrl = "https://www.googleapis.com/books/v1/volumes?q=isbn:";
        private readonly TimeSpan maxCacheAge = TimeSpan.FromHours(12);

        private string cacheFilePath {
            get {
                return Path.Combine(Environment.CurrentDirectory, ISBN.ValueWithoutHyphens + ".json");
            }
        }

        private Book()
        {
        }

        public Book(string isbnText) 
        {
            int isbnLength = isbnText.Replace("-", "").Replace(" ", "").Length;
            if (isbnLength == 10)
                ISBN = new ISBN10(isbnText);
            else if (isbnLength == 13)
                ISBN = new ISBN13(isbnText);
            else
                throw new ArgumentException("Invalid Argument", "isbnText");
        }

        public Book(ISBN isbn)
        {
            ISBN = isbn;
        }

        public void UpdatePriceInEUR(int price)
        {
            PriceInEUR = price;
            PriceDate = DateTime.Now;
        }

        public void LoadFromApi(bool force=false)
        {
            if (force || getCacheAge() > maxCacheAge)
                updateCacheFromApi();

            string json = readCache();

            dynamic obj = JsonConvert.DeserializeObject(json);
            dynamic volInfo = obj.items[0].volumeInfo;

            Title = volInfo.title;
            Authors = ((JArray)volInfo.authors).Select(x => x.ToString()).ToList();
            PublishedAt = volInfo.publishedDate;
        }

        private TimeSpan getCacheAge()
        {
            if (!File.Exists(cacheFilePath))
                return TimeSpan.MaxValue;

            return DateTime.Now - File.GetLastWriteTime(cacheFilePath);
        }

        private void writeCache(string json)
        {
            using (StreamWriter sw = new StreamWriter(cacheFilePath))
            {
                sw.Write(json);
            }
        }

        private string readCache()
        {
            using (StreamReader sr = new StreamReader(cacheFilePath))
            {
                return sr.ReadToEnd();
            }
        }

        private void updateCacheFromApi()
        {
            string apiUrl = apiBaseUrl + ISBN.ValueWithoutHyphens;

            WebRequest request = WebRequest.Create(apiUrl);
            WebResponse response = request.GetResponse();

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                string json = sr.ReadToEnd();
                writeCache(json);
            };
        }
    }
}
