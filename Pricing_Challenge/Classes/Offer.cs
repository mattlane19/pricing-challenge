using Pricing_Challenge.Enums;

namespace Pricing_Challenge.Classes
{
    public class Offer
    {
        #region Properties

        public int OfferId { get; set; }

        public OfferType OfferType { get; set; }

        public string Message { get; set; }

        public virtual Product AppliesTo { get; set; }

        public int DiscountPercentage { get; set; }

        public virtual TimedOfferOptions TimedOfferOptions { get; set; }

        public virtual MultibuyOfferOptions MultibuyOfferOptions { get; set; }

        public decimal DiscountAmount;

        public int Count;

        #endregion
    }
}