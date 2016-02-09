using System;
using System.Collections.Generic;

namespace T2
{
    public interface IBook
    {
        List<string> Authors { get; set; }
        ISBN ISBN { get; set; }
        DateTime PriceDate { get; }
        int PriceInEUR { get; }
        string PublishedAt { get; set; }
        string Title { get; set; }

        void LoadFromApi(bool force = false);
        void UpdatePriceInEUR(int price);
    }
}