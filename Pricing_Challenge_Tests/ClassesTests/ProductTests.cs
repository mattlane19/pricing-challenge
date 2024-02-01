using Pricing_Challenge.Classes;
using NUnit.Framework;

namespace Pricing_Challenge_Tests.ClassesTests
{
    [TestFixture]
    public class ProductTests
    {
        #region Tests

        [Test]
        public void Is_InitialisedCorrectly()
        {
            // Act
            var product = new Product();
        }

        [Test]
        public void Can_SetProperties()
        {
            // Arrange
            var productId = 1;
            var productName = "Product Name";
            var productPrice = 0.80m;
            var offer = new Offer();
            var multibuyOfferOptions = new MultibuyOfferOptions();

            // Act
            var product = new Product
            {
                ProductId = productId,
                ProductName = productName,
                ProductPrice = productPrice,
                Offer = offer,
                MultiBuyOffer = multibuyOfferOptions
            };

            // Assert
            Assert.AreEqual(productId, product.ProductId);
            Assert.AreEqual(productName, product.ProductName);
            Assert.AreEqual(productPrice, product.ProductPrice);
            Assert.AreEqual(offer, product.Offer);
            Assert.AreEqual(multibuyOfferOptions, product.MultiBuyOffer);
        }

        #endregion
    }
}