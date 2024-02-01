using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Pricing_Challenge.Classes;
using Pricing_Challenge.Interfaces;

namespace Pricing_Challenge.Context
{
    public class DataContext : DbContext, IDataContext
    {
        #region Constructor

        public DataContext() : base("connectionString")
        {
        }

        #endregion

        #region Fields

        public DbSet<Product> Products { get; set; }
        public DbSet<Offer> Offers { get; set; }

        #endregion

        #region IDataContext Implementation

        public List<Offer> GetOffers()
        {
            var context = new DataContext();
            return context.Offers.ToList();
        }

        public List<Product> GetProducts()
        {
            var context = new DataContext();
            return context.Products.ToList();
        }

        #endregion

        #region Protected Methods

        // Configuration
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<Product>().HasKey(x => new { x.ProductId });

            modelBuilder.Entity<Offer>().ToTable("Offer");
            modelBuilder.Entity<Offer>().HasKey(x => new { x.OfferId });

            modelBuilder.Entity<Offer>()
                .HasOptional(x => x.MultibuyOfferOptions)
                .WithRequired(x => x.Offer);

            modelBuilder.Entity<Offer>()
                .HasOptional(x => x.TimedOfferOptions)
                .WithRequired(x => x.Offer);

            modelBuilder.Entity<Product>()
                .HasOptional(x => x.Offer)
                .WithRequired(x => x.AppliesTo)
                .Map(a => a.MapKey("AppliesTo"));

            modelBuilder.Entity<Product>()
                .HasOptional(x => x.MultiBuyOffer)
                .WithRequired(x => x.MultibuyTrigger)
                .Map(a => a.MapKey("MultibuyTrigger"));
        }

        #endregion
    }
}
