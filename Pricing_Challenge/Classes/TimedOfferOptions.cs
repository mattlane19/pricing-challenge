using System;
using System.ComponentModel.DataAnnotations.Schema;
using Pricing_Challenge.Enums;

namespace Pricing_Challenge.Classes
{
    [Table("TimedOfferOptions")]
    public class TimedOfferOptions : Offer
    {
        #region Constructor

        public TimedOfferOptions()
        {
            OfferType = OfferType.Timed;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now.AddDays(7);
        }

        #endregion

        #region Properties

        [ForeignKey("OfferId")]
        public virtual Offer Offer { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        #endregion
    }
}
