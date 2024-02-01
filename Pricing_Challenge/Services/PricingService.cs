using System.Collections.Generic;
using System.Linq;
using Pricing_Challenge.Classes;
using Pricing_Challenge.Interfaces;

namespace Pricing_Challenge.Services
{
    public class PricingService : IPricingService
    {
        #region Fields

        private readonly IOffersService offersService;

        #endregion

        #region Constructor

        public PricingService(IOffersService offersService)
        {
            this.offersService = offersService;
        }

        #endregion

        #region IPricingService Implementation

        public PriceBasket CalculateTotals(PriceBasket priceBasket)
        {
            priceBasket.Subtotal = CalculateSubtotal(priceBasket.BasketContents);
            priceBasket.BasketOffers = offersService.GetBasketOffers(priceBasket);

            CalculateTotal(priceBasket);

            return priceBasket;
        }

        #endregion

        #region Private Methods

        // Returns a decimal value of the priceBasket Subtotal.
        private static decimal CalculateSubtotal(IEnumerable<Product> priceBasketContents)
        {
            return priceBasketContents.Sum(item => item.ProductPrice);
        }

        // Sets the priceBasket Total value - if no offers apply then this is the same as Subtotal.
        // If not, use the offersService to apply the offers and return the Total.
        private void CalculateTotal(PriceBasket priceBasket)
        {
            priceBasket.Total = priceBasket.BasketOffers?.Count > 0 ? offersService.ApplyOffers(priceBasket) : priceBasket.Subtotal;
        }

        #endregion
    }
}
