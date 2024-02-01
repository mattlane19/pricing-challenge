using Pricing_Challenge.Classes;

namespace Pricing_Challenge.Interfaces
{
    public interface IPricingService
    {
        PriceBasket CalculateTotals(PriceBasket priceBasket);
    }
}
