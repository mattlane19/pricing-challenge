using System;
using System.Collections.Generic;
using System.Linq;
using Pricing_Challenge.Classes;
using Pricing_Challenge.Interfaces;
using Pricing_Challenge.Services;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Pricing_Challenge_Tests.ServiceTests
{
    [TestFixture]
    public class PriceBasketServiceTests
    {
        #region Fields

        private PriceBasketService priceBasketService;
        private Mock<IDataContext> mockDataContext;

        #endregion

        #region Setup

        [SetUp]
        public void Setup()
        {
            mockDataContext = new Mock<IDataContext>();
            priceBasketService = new PriceBasketService(mockDataContext.Object);
        }

        #endregion

        #region Tests

        [Test]
        public void This_ImplementsIPriceBasketService()
        {
            // Assert
            typeof(IPriceBasketService).IsAssignableFrom(typeof(PriceBasketService)).Should().BeTrue();
        }

        [Test]
        public void GeneratePriceBasket_WithNullArgs_ShouldThrowException()
        {
            // Act
            var priceBasketInputnull = new Action(() => priceBasketService.GeneratePriceBasket(null));

            // Assert
            priceBasketInputnull.Should().Throw<NullReferenceException>();
        }

        [Test]
        public void GeneratePriceBasket_WithValidInputItems_ReturnsPriceBasket()
        {
            // Arrange
            var testInput = "Apples Milk Bread Apples Soup";
            mockDataContext.Setup(x => x.GetProducts()).Returns(SetupProductList());

            // Act
            var result = priceBasketService.GeneratePriceBasket(testInput);

            // Assert
            Assert.IsNotNull(result.BasketContents);
            Assert.AreEqual(5, result.BasketContents.Count);
        }

        [Test]
        public void GeneratePriceBasket_WithNoValidItems_ReturnsArgumentException()
        {
            // Arrange
            var testInput = "Oranges Water Beans";
            var products = SetupProductList();
            mockDataContext.Setup(x => x.GetProducts()).Returns(products);
            var validProducts = string.Join(" ", products.Select(x => x.ProductName).ToArray());

            // Act
            var result = new Action(() => priceBasketService.GeneratePriceBasket(testInput));

            // Assert
            result.Should().Throw<ArgumentException>().And.Message.Should().Be("No valid products in the Basket." + Environment.NewLine + "Available Products are: " + validProducts);
        }

        [Test]
        public void GeneratePriceBasket_WithValidAndInvalidItems_ReturnsArgumentException()
        {
            // Arrange
            var validInput = "Apples Bread ";
            var invalidInput = "Oranges";
            var products = SetupProductList();
            mockDataContext.Setup(x => x.GetProducts()).Returns(products);
            var validProducts = string.Join(" ", products.Select(x => x.ProductName).ToArray());

            // Act
            var result = new Action(() => priceBasketService.GeneratePriceBasket(validInput + invalidInput));

            // Assert
            result.Should().Throw<ArgumentException>().And.Message.Should().Be("Invalid products in Basket - " + invalidInput + Environment.NewLine + "Available Products are: " + validProducts);
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