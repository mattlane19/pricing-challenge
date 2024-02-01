using System.Collections.Generic;
using Pricing_Challenge.Classes;

namespace Pricing_Challenge.Interfaces
{
    public interface IDataContext
    {
        List<Offer> GetOffers();

        List<Product> GetProducts();
    }
}
