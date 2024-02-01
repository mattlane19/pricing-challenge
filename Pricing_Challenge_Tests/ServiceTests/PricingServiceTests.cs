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
    public class PricingServiceTests
    {
        #region Fields

        private PricingService pricingService;
        private Mock<IOffersService> mockOffersService;

        #endregion

        #region Setup

        [SetUp]
        public void Setup()
        {
            mockOffersService = new Mock<IOffersService>();
            pricingService = new PricingService(mockOffersService.Object);
        }

        #endregion

        #region Tests

        [Test]
        public void This_ImplementsIPricingService()
        {
            // Assert
            typeof(IPricingService).IsAssignableFrom(typeof(PricingService)).Should().BeTrue();
        }

        [Test]
        public void CalculateTotals_WithNullArgs_ShouldThrowException()
        {
            // Act
            var priceBasketnull = new Action(() => pricingService.CalculateTotals(null));

            // Assert
            priceBasketnull.Should().Throw<NullReferenceException>();
        }

        [Test]
        public void CalculateTotals_WithoutOffers_ReturnsPriceBasketWithTotals()
        {
            // Arrange
            var priceBasket = new PriceBasket
            {
                BasketContents = SetupProductList()
            };

            // Act
            var result = pricingService.CalculateTotals(priceBasket);

            // Assert
            Assert.AreEqual(priceBasket.BasketContents.Count, result.BasketContents.Count);
            mockOffersService.Verify(x => x.GetBasketOffers(priceBasket), Times.Once);
            mockOffersService.Verify(x => x.ApplyOffers(priceBasket), Times.Never);
            Assert.IsNotNull(result.Subtotal);
            Assert.IsNotNull(result.Total);
        }

        [Test]
        public void CalculateTotals_WithOffers_ReturnsPriceBasketWithTotals()
        {
            // Arrange
            var priceBasket = new PriceBasket
            {
                BasketContents = SetupProductList()
            };

            SetupBasketOffers(priceBasket);
            mockOffersService.Setup(x => x.GetBasketOffers(priceBasket)).Returns(priceBasket.BasketOffers);

            // Act
            var result = pricingService.CalculateTotals(priceBasket);

            // Assert
            Assert.AreEqual(priceBasket.BasketContents.Count, result.BasketContents.Count);
            mockOffersService.Verify(mos => mos.GetBasketOffers(priceBasket), Times.Once);
            mockOffersService.Verify(mos => mos.ApplyOffers(priceBasket), Times.Once);
            Assert.IsNotNull(result.Subtotal);
            Assert.IsNotNull(result.Total);
        }

        #endregion

        #region SetupMethods

        private void SetupBasketOffers(PriceBasket priceBasket)
        {
            var offers = new List<Offer>
            {
                new TimedOfferOptions { OfferId = 1, Message = "Apples 10% off", AppliesTo = priceBasket.BasketContents.ElementAt(0), DiscountPercentage = 10, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(7) },
                new MultibuyOfferOptions { OfferId = 2, Message = "Multibuy Soup - 50% off Bread", AppliesTo = priceBasket.BasketContents.ElementAt(3), DiscountPercentage = 50 }
            };

            priceBasket.BasketOffers = offers;
        }

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