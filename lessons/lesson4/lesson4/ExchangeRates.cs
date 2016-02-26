using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;

namespace lesson4
{
    public static class ExchangeRates
    {
        private static Dictionary<string, decimal> s_rates = new Dictionary<string, decimal>();

        /// <summary>
        /// Gets exchange rate 'from' currency 'to' another currency.
        /// </summary>
        public static decimal Get(Currency from, Currency to)
        {
            // exchange rate is 1:1 for same currency
            if (from == to) return 1;
            
            // use web service to query current exchange rate
            // request : http://download.finance.yahoo.com/d/quotes.csv?s=EURUSD=X&f=sl1d1t1c1ohgv&e=.csv
            // response: "EURUSD=X",1.0930,"12/29/2015","6:06pm",-0.0043,1.0971,1.0995,1.0899,0
            var key = string.Format("{0}{1}", from, to); // e.g. EURUSD means "How much is 1 EUR in USD?".
            // if we've already downloaded this exchange rate, use the cached value
            if (s_rates.ContainsKey(key)) return s_rates[key];

            // otherwise create the request URL, ...
            var url = string.Format(@"http://download.finance.yahoo.com/d/quotes.csv?s={0}=X&f=sl1d1t1c1ohgv&e=.csv", key);
            // download the response as string
            var data = new WebClient().DownloadString(url);
            // split the string at ','
            var parts = data.Split(',');
            // convert the exchange rate part to a decimal 
            var rate = decimal.Parse(parts[1], CultureInfo.InvariantCulture);
            // cache the exchange rate
            s_rates[key] = rate;

            // and finally perform the currency conversion
            return rate;
        }
    }
}

