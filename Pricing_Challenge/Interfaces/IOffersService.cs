using System.Collections.Generic;
using Pricing_Challenge.Classes;

namespace Pricing_Challenge.Interfaces
{
    public interface IOffersService 
    { 
        List<Offer> GetBasketOffers(PriceBasket priceBasket);

        decimal ApplyOffers(PriceBasket priceBasket);
    }
}
