using System;
using System.Collections.Generic;
using System.Linq;
using Pricing_Challenge.Classes;
using Pricing_Challenge.Enums;
using Pricing_Challenge.Interfaces;

namespace Pricing_Challenge.Services
{
    public class OffersService : IOffersService
    {
        #region Fields

        private readonly IDataContext dataContext;

        #endregion

        #region Constructor

        public OffersService(IDataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        #endregion

        #region IOffersService Implementation

        public List<Offer> GetBasketOffers(PriceBasket priceBasket)
        {
            var offers = GetOffers();
            var basketOffers = new List<Offer>();

            return GetApplicableBasketOffers(priceBasket, offers, basketOffers);
        }

        public decimal ApplyOffers(PriceBasket priceBasket)
        {
            priceBasket.Total = priceBasket.Subtotal;

            ApplyOffersToBasket(priceBasket);

            return priceBasket.Total - priceBasket.TotalDiscounts;
        }

        #endregion

        #region Private Methods

        // Get Offers list from the dataContext
        private List<Offer> GetOffers()
        {
            return dataContext.GetOffers();
        }

        // Returns a list of the Offers which apply to this priceBasket.
        private static List<Offer> GetApplicableBasketOffers(PriceBasket priceBasket, List<Offer> offers, List<Offer> basketOffers)
        {
            foreach (var offer in offers)
            {
                if (offer.OfferType == OfferType.Timed)
                {
                    if (TimedOfferApplies(priceBasket, offer))
                    {
                        basketOffers.Add(offer);
                    }

                }
                else if (offer.OfferType == OfferType.Multibuy)
                {
                    if (MultibuyOfferApplies(priceBasket, offer))
                    {
                        basketOffers.Add(offer);
                    }
                }
            }

            return basketOffers;
        }

        // Applies the BasketOffers to the priceBasket, adding the discounts to priceBasket.TotalDiscount.
        private static void ApplyOffersToBasket(PriceBasket priceBasket)
        {
            foreach (var offer in priceBasket.BasketOffers)
            {
                if (offer.OfferType == OfferType.Timed)
                {
                    ApplyTimedOffer(priceBasket, offer);
                }
                else if (offer.OfferType == OfferType.Multibuy)
                {
                    ApplyMultibuyOffer(priceBasket, offer);
                }
            }
        }

        // Returns bool value indicating if the Timed Offer applies to the priceBasket.
        private static bool TimedOfferApplies(PriceBasket priceBasket, Offer offer)
        {
            return priceBasket.BasketContents.Any(x => x.ProductId == offer.AppliesTo.ProductId) && IsTimedOfferInDate(offer);
        }

        // Returns bool value indicating if the Multibuy Offer applies to the priceBasket.
        private static bool MultibuyOfferApplies(PriceBasket priceBasket, Offer offer)
        {
            return priceBasket.BasketContents.Any(x => x.ProductId == offer.AppliesTo.ProductId) && IsMultiBuyOfferTriggered(priceBasket, offer);
        }

        // Gets the product from the priceBasket that the Offer applies to and the number of times the offer has been triggered and applies the discount accordingly.
        private static void ApplyMultibuyOffer(PriceBasket priceBasket, Offer offer)
        {
            var product = priceBasket.BasketContents.FirstOrDefault(x => x.ProductId == offer.AppliesTo.ProductId);
            double triggerItemCount = GetMultibuyTriggerItemCount(priceBasket, offer);
            var offerTriggerCount = GetMultibuyOfferTriggerCount(priceBasket, offer, triggerItemCount);

            if (offerTriggerCount > 0)
            {
                for (var i = 0; i < offerTriggerCount; i++)
                {
                    priceBasket.TotalDiscounts += AddDiscount(offer, product);
                    offer.Count++;
                }
            }
        }

        // Apply the Timed Offer discount to any items in the priceBasket which match the AppliesTo Id.
        private static void ApplyTimedOffer(PriceBasket priceBasket, Offer offer)
        {
            foreach (var product in priceBasket.BasketContents.Where(product => product.ProductId == offer.AppliesTo.ProductId))
            {
                priceBasket.TotalDiscounts += AddDiscount(offer, product);
                offer.Count++;
            }
        }

        // Returns the Discount Amount based on the discount percentage of the Offer and the product price.
        private static decimal AddDiscount(Offer offer, Product product)
        {
            var discount = (product.ProductPrice / 100 * offer.DiscountPercentage);
            offer.DiscountAmount = Math.Round(discount, 2);
            return offer.DiscountAmount;
        }

        // Returns int value indicating how many times the MultibuyOffer should be applied to the priceBasket -
        // taking into account both the number of times the offer is triggered, and the number of items it can apply to
        private static int GetMultibuyOfferTriggerCount(PriceBasket priceBasket, Offer offer, double triggerItemCount)
        {
            var offerTriggerCount = (int)Math.Floor(triggerItemCount / offer.MultibuyOfferOptions.MultibuyQuantity);
            var appliesToCount = priceBasket.BasketContents.Count(x => x.ProductId == offer.AppliesTo.ProductId);

            return appliesToCount < offerTriggerCount ? appliesToCount : offerTriggerCount;
        }

        // Returns bool value indicating if the MultibuyOffer has been triggered (if enough of the trigger items are in the priceBasket).
        private static bool IsMultiBuyOfferTriggered(PriceBasket priceBasket, Offer offer)
        {
            return GetMultibuyTriggerItemCount(priceBasket, offer) >= offer.MultibuyOfferOptions.MultibuyQuantity;
        }

        // Returns int value indicating how many of the MultibuyOffer trigger items are in the priceBasket.
        private static int GetMultibuyTriggerItemCount(PriceBasket priceBasket, Offer offer)
        {
            return priceBasket.BasketContents.Count(x => x.ProductId == offer.MultibuyOfferOptions.MultibuyTrigger.ProductId);
        }

        // Returns bool value indicating if the Timed Offer is in date.
        private static bool IsTimedOfferInDate(Offer offer)
        {
            return DateTime.Now >= offer.TimedOfferOptions.StartDate && DateTime.Now <= offer.TimedOfferOptions.EndDate;
        }

        #endregion
    }
}
