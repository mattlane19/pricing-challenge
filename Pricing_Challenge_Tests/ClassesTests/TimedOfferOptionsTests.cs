using Pricing_Challenge.Classes;
using Pricing_Challenge.Enums;
using NUnit.Framework;

namespace Pricing_Challenge_Tests.ClassesTests
{
    [TestFixture]
    public class TimedOfferOptionsTests
    {
        #region Tests

        [Test]
        public void Inherits_From_Offer()
        {
            // Assert
            Assert.IsTrue(typeof(Offer).IsAssignableFrom(typeof(TimedOfferOptions)));
        }

        [Test]
        public void Is_InitialisedCorrectly()
        {
            // Act
            var timedOfferOptions = new TimedOfferOptions();

            // Assert
            Assert.AreEqual(OfferType.Timed, timedOfferOptions.OfferType);
            Assert.IsNotNull(timedOfferOptions.StartDate);
            Assert.IsNotNull(timedOfferOptions.EndDate);
        }

        [Test]
        public void Can_SetProperties()
        {
            // Arrange
            var offerId = 1;
            var offerType = OfferType.Timed;
            var message = "Offer Message";
            var appliesTo = new Product();
            var discountPercentage = 10;
            var timedOfferOptions = new TimedOfferOptions();
            var multibuyOfferOptions = new MultibuyOfferOptions();
            var discountAmount = 1.30m;
            var count = 2;

            // Act
            var offer = new Offer
            {
                OfferId = offerId,
                OfferType = offerType,
                Message = message,
                AppliesTo = appliesTo,
                DiscountPercentage = discountPercentage,
                TimedOfferOptions = timedOfferOptions,
                MultibuyOfferOptions = multibuyOfferOptions,
                DiscountAmount = discountAmount,
                Count = count
            };

            // Assert
            Assert.AreEqual(offerId, offer.OfferId);
            Assert.AreEqual(offerType, offer.OfferType);
            Assert.AreEqual(message, offer.Message);
            Assert.AreEqual(appliesTo, offer.AppliesTo);
            Assert.AreEqual(discountPercentage, offer.DiscountPercentage);
            Assert.AreEqual(timedOfferOptions, offer.TimedOfferOptions);
            Assert.AreEqual(multibuyOfferOptions, offer.MultibuyOfferOptions);
            Assert.AreEqual(discountAmount, offer.DiscountAmount);
            Assert.AreEqual(count, offer.Count);
        }

        #endregion
    }
}