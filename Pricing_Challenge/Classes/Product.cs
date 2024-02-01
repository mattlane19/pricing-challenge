namespace Pricing_Challenge.Classes
{
    public class Product
    {
        #region Properties

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public decimal ProductPrice { get; set; }

        public virtual Offer Offer { get; set; }

        public virtual MultibuyOfferOptions MultiBuyOffer { get; set; }

        #endregion
    }
}
