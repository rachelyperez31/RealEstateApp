using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Infrastructure.Persistence.Contexts
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        DbSet<Property> Properties { get; set; }
        DbSet<Improvement> Improvements { get; set; }
        DbSet<FavProperty> FavProperties { get; set; }
        DbSet<TypeOfProperty> TypeOfProperties { get; set; }
        DbSet<TypeOfSale> TypeOfSales { get; set; }
        DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Tables
            modelBuilder.Entity<Property>().ToTable("Properties");
            modelBuilder.Entity<Improvement>().ToTable("Improvements");
            modelBuilder.Entity<FavProperty>().ToTable("FavProperties");
            modelBuilder.Entity<TypeOfProperty>().ToTable("TypeOfProperties");
            modelBuilder.Entity<TypeOfSale>().ToTable("TypeOfSales");
            modelBuilder.Entity<Image>().ToTable("Images");
            #endregion

            #region Primary Keys
            modelBuilder.Entity<Property>().HasKey(p => p.Id);
            modelBuilder.Entity<Improvement>().HasKey(i => i.Id);
            modelBuilder.Entity<FavProperty>().HasKey(f => new { f.UserId, f.PropertyCode });
            modelBuilder.Entity<TypeOfProperty>().HasKey(b => b.Id);
            modelBuilder.Entity<TypeOfSale>().HasKey(b => b.Id);
            modelBuilder.Entity<Image>().HasKey(p => p.Id);
            #endregion

            #region Relationships
            modelBuilder.Entity<Property>()
                .HasOne(p => p.TypeOfProperty)
                .WithMany(t => t.Properties)
                .HasForeignKey(p => p.TypeOfPropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Property>()
                .HasOne(p => p.TypeOfSale)
                .WithMany(t => t.Properties)
                .HasForeignKey(p => p.TypeOfSaleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Property>()
                .HasMany(property => property.Improvements)
                .WithMany(improvement => improvement.Properties)
                    .UsingEntity<PropertyImprovement>(
                                        x => x
                        .HasOne(improvementProperty => improvementProperty.Improvement)
                        .WithMany(property => property.PropertyImprovements)
                        .HasForeignKey(improvementProperty => improvementProperty.ImprovementId),
                                        x => x
                        .HasOne(improvementProperty => improvementProperty.Property)
                        .WithMany(improvement => improvement.PropertyImprovements)
                        .HasForeignKey(improvementProperty => improvementProperty.PropertyId),
                                        x =>
                {
                    x.ToTable("ImprovementProperties");
                    x.HasKey(x => new { x.PropertyId, x.ImprovementId });
                }
            );

            modelBuilder.Entity<Property>()
                .HasMany(p => p.Images)
                .WithOne(i => i.Property)
                .HasForeignKey(i => i.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region Properties
            #region Property
            modelBuilder.Entity<Property>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");
            #endregion
            #endregion
        }

    }
}
