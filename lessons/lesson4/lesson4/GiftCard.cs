using System;
using System.Net;
using System.Globalization;
using Newtonsoft.Json;

namespace lesson4
{
    public class GiftCard : IItem
    {
        /// <summary>
        /// Creates a new GiftCard.
        /// </summary>
        /// <param name="amount">Amount must be greater than 0.</param>
        public GiftCard (decimal amount, Currency currency)
            : this (new Price (amount, currency), Guid.NewGuid().ToString(), false)
        {
        }

        /// <summary>
        /// Creates a new GiftCard.
        /// </summary>
        /// <param name="amount">Amount must be greater than 0.</param>
        [JsonConstructor]
        private GiftCard(Price amount, string code, bool isRedeemed)
        {
            if (amount.Amount <= 0) throw new ArgumentException("Amount must be greater than 0.", nameof(amount));

            Amount = amount;
            Code = code;
            IsRedeemed = isRedeemed;
        }

        /// <summary>
        /// Value of this gift card.
        /// </summary>
        public Price Amount { get; }

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

        [JsonIgnore]
        public Price Price => Amount;

        [JsonIgnore]
        public string Description => "GiftCard " + Code;

        #endregion
    }
}

