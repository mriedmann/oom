using System;
using System.Net;
using System.Globalization;

namespace lesson3
{
    public class GiftCard : IItem
    {
        /// <summary>
        /// Creates a new GiftCard.
        /// </summary>
        /// <param name="amount">Amount must be greater than 0.</param>
        public GiftCard(decimal amount, Currency currency)
        {
            if (amount <= 0) throw new ArgumentException("Amount must be greater than 0.", nameof(amount));

            Amount = amount;
            Currency = currency;
            Code = Guid.NewGuid().ToString();
            IsRedeemed = false;
        }

        /// <summary>
        /// Value of this gift card.
        /// </summary>
        public decimal Amount { get; }

        /// <summary>
        /// Currency of Amount.
        /// </summary>
        public Currency Currency { get; }

        /// <summary>
        /// The unique code to redeem this gift card.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Redeems this gift card. Can only be performed once.
        /// </summary>
        public void Redeem()
        {
            if (IsRedeemed) throw new InvalidOperationException ($"Gift card {Code} has already been redeemed.");
            IsRedeemed = true;
        }

        /// <summary>
        /// True, if this gift card has been redeemed. 
        /// </summary>
        public bool IsRedeemed { get; private set; }

        #region IItem implementation

        public decimal GetPrice (Currency currency)
        {
            // if the price is requested in it's own currency, then simply return the stored price
            if (currency == Currency) return Amount;

            // use web service to query current exchange rate
            // request : http://download.finance.yahoo.com/d/quotes.csv?s=EURUSD=X&f=sl1d1t1c1ohgv&e=.csv
            // response: "EURUSD=X",1.0930,"12/29/2015","6:06pm",-0.0043,1.0971,1.0995,1.0899,0
            var key = string.Format("{0}{1}", Currency, currency); // e.g. EURUSD means "How much is 1 EUR in USD?".

            // create the request URL, ...
            var url = string.Format(@"http://download.finance.yahoo.com/d/quotes.csv?s={0}=X&f=sl1d1t1c1ohgv&e=.csv", key);
            // download the response as string
            var data = new WebClient().DownloadString(url);
            // split the string at ','
            var parts = data.Split(',');
            // convert the exchange rate part to a decimal 
            var rate = decimal.Parse(parts[1], CultureInfo.InvariantCulture);

            // and finally perform the currency conversion
            return Amount * rate;
        }

        public string Description => "GiftCard " + Code;

        #endregion
    }
}

