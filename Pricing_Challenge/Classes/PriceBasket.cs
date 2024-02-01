using System.Collections.Generic;

namespace Pricing_Challenge.Classes
{
    public class PriceBasket
    {
        #region Constructor
        public PriceBasket()
        {
            BasketContents = new List<Product>();
            BasketOffers = new List<Offer>();
        }

        #endregion

        #region Properties

        public List<Product> BasketContents;

        public List<Offer> BasketOffers;

        public decimal Subtotal;

        public decimal TotalDiscounts;

        public decimal Total;

        #endregion
    }
}
