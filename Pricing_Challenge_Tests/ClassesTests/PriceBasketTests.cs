using System.Collections.Generic;
using Pricing_Challenge.Classes;
using NUnit.Framework;

namespace Pricing_Challenge_Tests.ClassesTests
{
    [TestFixture]
    public class PriceBasketTests
    {
        #region Tests

        [Test]
        public void Is_InitialisedCorrectly()
        {
            // Act
            var priceBasket = new PriceBasket();

            // Assert
            Assert.IsNotNull(priceBasket.BasketContents);
            Assert.IsNotNull(priceBasket.BasketOffers);
        }

        [Test]
        public void Can_SetProperties()
        {
            // Arrange
            var basketContents = new List<Product>();
            var basketOffers = new List<Offer>();
            var subtotal = 4.50m;
            var totalDiscount = 0;
            var total = 3.50m;

            // Act
            var priceBasket = new PriceBasket
            {
                BasketContents = basketContents,
                BasketOffers = basketOffers,
                Subtotal = subtotal,
                TotalDiscounts = totalDiscount,
                Total = total
            };

            // Assert
            Assert.AreEqual(basketContents, priceBasket.BasketContents);
            Assert.AreEqual(basketOffers, priceBasket.BasketOffers);
            Assert.AreEqual(subtotal, priceBasket.Subtotal);
            Assert.AreEqual(totalDiscount, priceBasket.TotalDiscounts);
            Assert.AreEqual(total, priceBasket.Total);
        }

        #endregion
    }
}