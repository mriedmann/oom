using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
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

        private DateTime lastSuccessfullApiCall = DateTime.MinValue;

        private const string apiBaseUrl = "https://www.googleapis.com/books/v1/volumes?q=isbn:";

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
            if (!force && (DateTime.Now - lastSuccessfullApiCall).TotalMinutes < 1)
                return; //still up-to-date

            getDataFromApi();
            lastSuccessfullApiCall = DateTime.Now;
        }



        private void getDataFromApi()
        {
            string apiUrl = apiBaseUrl + ISBN.ValueWithoutHyphens;

            WebRequest request = WebRequest.Create(apiUrl);
            WebResponse response = request.GetResponse();
            using (var jsonReader = JsonReaderWriterFactory.CreateJsonReader(response.GetResponseStream(), new System.Xml.XmlDictionaryReaderQuotas()))
            {
                var root = XElement.Load(jsonReader);
                Title = root.XPathSelectElement("//title").Value;
                PublishedAt = root.XPathSelectElement("//publishedDate").Value;
                Authors = root.XPathSelectElement("//authors").Elements().Select(x => x.Value).ToList();
            };
        }
    }
}
