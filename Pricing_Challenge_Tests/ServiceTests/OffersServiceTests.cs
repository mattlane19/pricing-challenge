using System;
using System.Collections.Generic;
using System.Linq;
using Pricing_Challenge.Classes;
using Pricing_Challenge.Enums;
using Pricing_Challenge.Interfaces;
using Pricing_Challenge.Services;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Pricing_Challenge_Tests.ServiceTests
{
    [TestFixture]
    public class OffersServiceTests
    {
        #region Fields

        private OffersService offersService;
        private Mock<IDataContext> mockDataContext;

        #endregion

        #region Setup

        [SetUp]
        public void Setup()
        {
            mockDataContext = new Mock<IDataContext>();
            offersService = new OffersService(mockDataContext.Object);
        }

        #endregion

        #region Tests

        [Test]
        public void This_ImplementsIOffersService()
        {
            // Assert
            typeof(IOffersService).IsAssignableFrom(typeof(OffersService)).Should().BeTrue();
        }

        [Test]
        public void GetBasketOffers_WithNullArgs_ShouldThrowException()
        {
            // Act
            var priceBasketInputnull = new Action(() => offersService.GetBasketOffers(null));

            // Assert
            priceBasketInputnull.Should().Throw<NullReferenceException>();
        }

        [Test]
        public void ApplyOffers_WithNullArgs_ShouldThrowException()
        {
            // Act
            var priceBasketInputnull = new Action(() => offersService.ApplyOffers(null));

            // Assert
            priceBasketInputnull.Should().Throw<NullReferenceException>();
        }

        [Test]
        public void GetBasketOffers_WithNoApplicableOffers_ShouldReturnEmptyOffersList()
        {
            // Arrange
            var priceBasket = new PriceBasket
            {
                BasketContents = SetupProductList()
            };

            var offersList = SetupOffersList(priceBasket);
            mockDataContext.Setup(x => x.GetOffers()).Returns(offersList);
            priceBasket.BasketContents.RemoveAt(0);

            // Act
            var result = offersService.GetBasketOffers(priceBasket);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(new List<Offer>(), result);
        }

        [Test]
        public void GetBasketOffers_WithOneApplicableOffer_ShouldReturnOneOffer()
        {
            // Arrange
            var priceBasket = new PriceBasket
            {
                BasketContents = SetupProductList()
            };

            var offersList = SetupOffersList(priceBasket);

            mockDataContext.Setup(x => x.GetOffers()).Returns(offersList);

            // Act
            var result = offersService.GetBasketOffers(priceBasket);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(offersList.ElementAt(0), result.FirstOrDefault());
        }

        [Test]
        public void GetBasketOffers_WithMultipleApplicableOffers_ShouldReturnMultipleOffer()
        {
            // Arrange
            var priceBasket = new PriceBasket
            {
                BasketContents = SetupProductList()
            };

            foreach (var product in SetupProductList()) priceBasket.BasketContents.Add(product);

            var offersList = SetupOffersList(priceBasket);

            mockDataContext.Setup(x => x.GetOffers()).Returns(offersList);

            // Act
            var result = offersService.GetBasketOffers(priceBasket);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(offersList.Count, result.Count);
            Assert.AreEqual(offersList, result);
        }

        [Test]
        public void ApplyOffers_WithTimedOffer_ShouldReturnTotalWithDiscount()
        {
            // Arrange
            var priceBasket = new PriceBasket
            {
                BasketContents = SetupProductList()
            };

            var offersList = SetupOffersList(priceBasket);
            priceBasket.BasketOffers.Add(offersList.ElementAt(0));
            priceBasket.Subtotal = priceBasket.BasketContents.Sum(item => item.ProductPrice);

            var discount = (priceBasket.BasketContents.ElementAt(0).ProductPrice / 100 * offersList.ElementAt(0).DiscountPercentage);
            var discountAmount = Math.Round(discount, 2);
            var expectedResult = priceBasket.Subtotal - discountAmount;

            // Act
            var result = offersService.ApplyOffers(priceBasket);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void ApplyOffers_WithMultibuyOffer_ShouldReturnTotalWithDiscount()
        {
            // Arrange
            var priceBasket = new PriceBasket
            {
                BasketContents = SetupProductList()
            };

            foreach (var product in SetupProductList()) priceBasket.BasketContents.Add(product);
            var offersList = SetupOffersList(priceBasket);
            priceBasket.BasketOffers.Add(offersList.ElementAt(1));
            priceBasket.Subtotal = priceBasket.BasketContents.Sum(item => item.ProductPrice);

            var discount = (priceBasket.BasketContents.ElementAt(3).ProductPrice / 100 * offersList.ElementAt(1).DiscountPercentage);
            var discountAmount = Math.Round(discount, 2);
            var expectedResult = priceBasket.Subtotal - discountAmount;

            // Act
            var result = offersService.ApplyOffers(priceBasket);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void ApplyOffers_WithTimedAndMultibuyOffer_ShouldReturnTotalWithDiscount()
        {
            // Arrange
            var priceBasket = new PriceBasket
            {
                BasketContents = SetupProductList()
            };

            foreach (var product in SetupProductList()) priceBasket.BasketContents.Add(product);
            priceBasket.BasketContents.RemoveAt(4);
            var offersList = SetupOffersList(priceBasket);
            priceBasket.BasketOffers.Add(offersList.ElementAt(0));
            priceBasket.BasketOffers.Add(offersList.ElementAt(1));
            priceBasket.Subtotal = priceBasket.BasketContents.Sum(item => item.ProductPrice);

            var multibuyDiscount = (priceBasket.BasketContents.ElementAt(3).ProductPrice / 100 * offersList.ElementAt(1).DiscountPercentage);
            var timedDiscount = (priceBasket.BasketContents.ElementAt(0).ProductPrice / 100 * offersList.ElementAt(0).DiscountPercentage);
            var discountAmount = Math.Round(multibuyDiscount, 2) + Math.Round(timedDiscount, 2);
            var expectedResult = priceBasket.Subtotal - discountAmount;

            // Act
            var result = offersService.ApplyOffers(priceBasket);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result);
        }

        #endregion

        #region SetupMethods

        private List<Offer> SetupOffersList(PriceBasket priceBasket)
        {
            var offers = new List<Offer>
            {
                new Offer { OfferId = 1, OfferType = OfferType.Timed, Message = "Apples 10% off", AppliesTo = priceBasket.BasketContents.ElementAt(0), DiscountPercentage = 10,
                    TimedOfferOptions = new TimedOfferOptions { StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(7) } },

                new Offer { OfferId = 2, OfferType = OfferType.Multibuy, Message = "Multibuy Soup - 50% off Bread", AppliesTo = priceBasket.BasketContents.ElementAt(3), DiscountPercentage = 50,
                    MultibuyOfferOptions = new MultibuyOfferOptions { MultibuyQuantity = 2, MultibuyTrigger = priceBasket.BasketContents.ElementAt(2)}}
            };

            return offers;
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