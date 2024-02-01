using Pricing_Challenge.Classes;

namespace Pricing_Challenge.Interfaces
{
    public interface IPriceBasketService
    {
        PriceBasket GeneratePriceBasket(string priceBasketInput);
    }
}
