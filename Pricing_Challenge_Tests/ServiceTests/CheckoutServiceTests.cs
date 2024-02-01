using System;
using System.Collections.Generic;
using Pricing_Challenge.Classes;
using Pricing_Challenge.Interfaces;
using Pricing_Challenge.Services;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Pricing_Challenge_Tests.ServiceTests
{
    [TestFixture]
    public class CheckoutServiceTests
    {
        #region Fields

        private CheckoutService checkoutService;
        private Mock<IPricingService> mockPricingService;
        private Mock<IPriceBasketService> mockPriceBasketService;
        private Mock<IConsole> mockConsoleService;
        private Mock<IEnvironment> mockEnvironmentService;

        #endregion

        #region Setup

        [SetUp]
        public void Setup()
        {
            mockPricingService = new Mock<IPricingService>();
            mockPriceBasketService = new Mock<IPriceBasketService>();
            mockConsoleService = new Mock<IConsole>();
            mockEnvironmentService = new Mock<IEnvironment>();
            checkoutService = new CheckoutService(mockPricingService.Object, mockPriceBasketService.Object, mockConsoleService.Object, mockEnvironmentService.Object);
        }

        #endregion

        #region Tests

        [Test]
        public void This_ImplementsICheckoutService()
        {
            // Assert
            typeof(ICheckoutService).IsAssignableFrom(typeof(CheckoutService)).Should().BeTrue();
        }

        [Test]
        public void Run_WithNoErrors_CallsGeneratePriceBasketAndCalculateTotals_AndExitsApplication()
        {
            // Arrange
            var priceBasket = new PriceBasket
            {
                BasketContents = SetupProductList()
            };

            var input = "Apples Milk Soup Bread";

            mockConsoleService.Setup(x => x.ReadLine()).Returns(input);
            mockConsoleService.Setup(x => x.ReadKey()).Returns(It.IsAny<ConsoleKeyInfo>());
            mockConsoleService.Setup(x => x.WriteLine(It.IsAny<string>()));
            mockConsoleService.Setup(x => x.Write(It.IsAny<string>()));
            mockPriceBasketService.Setup(x => x.GeneratePriceBasket(input)).Returns(priceBasket);
            mockPricingService.Setup(x => x.CalculateTotals(priceBasket)).Returns(priceBasket);

            // Act
            checkoutService.Run();

            // Assert
            mockConsoleService.Verify(x => x.Write(It.IsAny<string>()), Times.AtLeastOnce());
            mockConsoleService.Verify(x => x.WriteLine(It.IsAny<string>()), Times.AtLeastOnce);
            mockConsoleService.Verify(x => x.ReadLine(), Times.AtLeastOnce);
            mockPriceBasketService.Verify(x => x.GeneratePriceBasket(input), Times.Once);
            mockPricingService.Verify(x => x.CalculateTotals(priceBasket), Times.Once);
            mockEnvironmentService.Verify(x => x.Exit(0));
        }

        #endregion

        #region SetupMethods

        private List<Product> SetupProductList()
        {
            var products = new List<Product>
            {
                new Product { ProductId = 1, ProductName = "Apples", ProductPrice = 1.00m },
                new Product { ProductId = 2, ProductName = "Milk", ProductPrice = 1.30m },
                new Product { ProductId = 3, ProductName = "Soup", ProductPrice = 0.65m },
                new Product { ProductId = 4, ProductName = "Bread", ProductPrice = 0.80m }
            };

            return products;
        }

        #endregion
    }
}