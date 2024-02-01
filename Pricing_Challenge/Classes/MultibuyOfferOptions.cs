using System.ComponentModel.DataAnnotations.Schema;
using Pricing_Challenge.Enums;

namespace Pricing_Challenge.Classes
{
    [Table("MultibuyOfferOptions")]
    public class MultibuyOfferOptions : Offer
    {
        #region Constructor

        public MultibuyOfferOptions()
        {
            OfferType = OfferType.Multibuy;
        }

        #endregion

        #region Properties

        [ForeignKey("OfferId")]
        public virtual Offer Offer { get; set; }

        public virtual Product MultibuyTrigger { get; set; }

        public int MultibuyQuantity { get; set; }

        #endregion
    }
}
