using System;
using System.Collections.Generic;
using System.Linq;
using Pricing_Challenge.Classes;
using Pricing_Challenge.Interfaces;

namespace Pricing_Challenge.Services
{
    public class PriceBasketService : IPriceBasketService
    {
        #region Fields

        private readonly IDataContext dataContext;

        #endregion

        #region Constructor

        public PriceBasketService(IDataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        #endregion

        #region IPriceBasketServcie Implementation

        public PriceBasket GeneratePriceBasket(string priceBasketInput)
        {
            var products = GetProducts();
            return CreatePriceBasketFromInput(priceBasketInput, products);
        }

        #endregion

        #region Private Methods

        // Get Products list from the dataContext
        private List<Product> GetProducts()
        {
            return dataContext.GetProducts();
        }

        // For each item in the priceBasketInput, add to the priceBasket or log as an invalid product and generate an appropriate error.
        private static PriceBasket CreatePriceBasketFromInput(string priceBasketInput, List<Product> products)
        {
            var basketItems = priceBasketInput.Split(new char[] { ' ' }).ToList();
            var priceBasket = new PriceBasket();
            var invalidProductList = new List<string>();

            foreach (var item in basketItems)
            {
                if (IsValidProduct(item, products))
                {
                    priceBasket.BasketContents.Add(products.FirstOrDefault(x => x.ProductName.ToLower() == item.ToLower()));
                }
                else
                {
                    invalidProductList.Add(item);
                }
            }

            CheckPriceBasketInputErrors(priceBasket, products, invalidProductList);
            return priceBasket;
        }

        // Check for any errors, throw an ArgumentException with an appropriate error if there is an error.
        private static void CheckPriceBasketInputErrors(PriceBasket priceBasket, List<Product> products, List<string> invalidProductList)
        {
            if (priceBasket.BasketContents.Count == 0)
            {
                var validProducts = string.Join(" ", products.Select(x => x.ProductName).ToArray());
                throw new ArgumentException("No valid products in the Basket." + Environment.NewLine + "Available Products are: " + validProducts);
            }
            else if (invalidProductList.Count > 0)
            {
                var invalidProducts = string.Join(" ", invalidProductList.ToArray());
                var validProducts = string.Join(" ", products.Select(x => x.ProductName).ToArray());
                throw new ArgumentException("Invalid products in Basket - " + invalidProducts + Environment.NewLine + "Available Products are: " + validProducts);
            }
        }

        // Check the entered item is a valid product.
        private static bool IsValidProduct(string productName, List<Product> products)
        {
            return products.Any(x => x.ProductName.ToLower() == productName.ToLower());
        }

        #endregion
    }
}
