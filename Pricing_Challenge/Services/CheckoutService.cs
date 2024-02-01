using System;
using Pricing_Challenge.Classes;
using Pricing_Challenge.Interfaces;

namespace Pricing_Challenge.Services
{
    public class CheckoutService : ICheckoutService
    {
        #region Fields

        private readonly IPricingService PricingService;
        private readonly IPriceBasketService PriceBasketService;
        private readonly IConsole ConsoleService;
        private readonly IEnvironment EnvironmentService;

        #endregion

        #region Constructor

        public CheckoutService(IPricingService pricingService, IPriceBasketService priceBasketService, IConsole consoleService, IEnvironment environmentService)
        {
            PricingService = pricingService;
            PriceBasketService = priceBasketService;
            ConsoleService = consoleService;
            EnvironmentService = environmentService;
        }

        #endregion

        #region ICheckoutService Implementation

        // Main Run method for the Checkout app.
        public void Run()
        {
            var priceBasket = GetPriceBasket();
            PricingService.CalculateTotals(priceBasket);
            DisplayResult(priceBasket);
        }

        #endregion

        #region Private Methods

        // Display the initial message in the console and read the user input for the PriceBasket.
        private PriceBasket GetPriceBasket()
        {
            ConsoleService.WriteLine("Please enter the items in your basket:");
            ConsoleService.Write("PriceBasket ");
            var basketInput = ConsoleService.ReadLine();

            return ReadBasketInput(basketInput);
        }

        // Returns a PriceBasket - use the PriceBasketService to generate a priceBasket from the user input.
        // Handle error if input is null or empty, or has invalid item/s.
        private PriceBasket ReadBasketInput(string priceBasketInput)
        {
            if (string.IsNullOrEmpty(priceBasketInput))
            {
                DisplayEmptyBasketMessageAndRestart();
            }

            try
            {
                return PriceBasketService.GeneratePriceBasket(priceBasketInput);
            }
            catch (Exception e)
            {
                DisplayErrorMessageAndRestart(e);
            }

            return new PriceBasket();
        }

        private void DisplayEmptyBasketMessageAndRestart()
        {
            ConsoleService.WriteLine(string.Empty);
            ConsoleService.WriteLine("Please enter at least one item to your Basket to continue...");
            ConsoleService.WriteLine(string.Empty);
            Run();
        }

        private void DisplayErrorMessageAndRestart(Exception e)
        {
            ConsoleService.WriteLine(string.Empty);
            ConsoleService.WriteLine("Error when generating Price Basket. Error message is: {0}", e.Message);
            ConsoleService.WriteLine(string.Empty);
            Run();
        }

        // Display the result of the user input - priceBasket Subtotal, Applied Offers, and Total.
        private void DisplayResult(PriceBasket priceBasket)
        {
            DisplaySubTotal(priceBasket);

            DisplayAppliedOffers(priceBasket);

            DisplayFinalTotal(priceBasket);
        }

        // Display the priceBasket Subtotal.
        private void DisplaySubTotal(PriceBasket priceBasket)
        {
            ConsoleService.WriteLine(string.Empty);
            ConsoleService.WriteLine("Subtotal: " + priceBasket.Subtotal.ToString("C"));
            ConsoleService.WriteLine(string.Empty);
        }

        // Display the offers applied to the basket, and how many times each is applied - else display no offers message.
        private void DisplayAppliedOffers(PriceBasket priceBasket)
        {
            if (priceBasket.BasketOffers.Count > 0)
            {
                foreach (var offer in priceBasket.BasketOffers)
                {
                    ConsoleService.Write(offer.Message + ": " + "-" + offer.DiscountAmount.ToString("C"));
                    if (offer.Count > 1)
                    {
                        ConsoleService.WriteLine(" x" + offer.Count);
                    }
                    else
                    {
                        ConsoleService.WriteLine(string.Empty);
                    }

                    ConsoleService.WriteLine(string.Empty);
                }
            }
            else
            {
                ConsoleService.WriteLine("{no offers available}");
                ConsoleService.WriteLine(string.Empty);
            }
        }

        // Display final total and give exit message - exit application on keypress.
        private void DisplayFinalTotal(PriceBasket priceBasket)
        {
            ConsoleService.WriteLine("Total: " + priceBasket.Total.ToString("C"));
            ConsoleService.WriteLine(string.Empty);
            ConsoleService.WriteLine("Press any key to leave the Checkout.");
            ConsoleService.ReadKey();
            EnvironmentService.Exit(0);
        }

        #endregion
    }
}
