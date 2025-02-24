using Microsoft.EntityFrameworkCore;
using PharmaWeb.Models;

namespace PharmaWeb.Persistencia
{
    public class PharmaWebContext : DbContext
    {
        public PharmaWebContext(DbContextOptions options) : base(options) { }
        public DbSet<RawMaterial> RawMaterials { get; set; }
        public DbSet<Medicine> Medicines { get; set;}
        public DbSet<Client> Clients { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<MedicineRawMaterial> MedicinesRawMaterials { get;set; }
        public DbSet<OrderMedicine> OrdersMedicines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MedicineRawMaterial>()
                .HasKey(m => new { m.MedicineId, m.RawMaterialId });

            modelBuilder.Entity<MedicineRawMaterial>()
                .HasOne(m => m.Medicine)
                .WithMany(mrm => mrm.Composition)
                .HasForeignKey(m => m.MedicineId);

            modelBuilder.Entity<MedicineRawMaterial>()
                .HasOne(mr => mr.RawMaterial)
                .WithMany(mrm => mrm.Composition)
                .HasForeignKey(m => m.RawMaterialId);

            modelBuilder.Entity<OrderMedicine>()
                .HasKey(o => new { o.OrderId, o.MedicineId });

            modelBuilder.Entity<OrderMedicine>()
                .HasOne(o => o.Order)
                .WithMany(om => om.OrdersMedicines)
                .HasForeignKey(o => o.OrderId);

            modelBuilder.Entity<OrderMedicine>()
                .HasOne(m => m.Medicine)
                .WithMany(om => om.OrdersMedicines)
                .HasForeignKey(m => m.MedicineId);

            base.OnModelCreating(modelBuilder);

            // define a precisão e a escala para a propriedade Price
            modelBuilder.Entity<Medicine>()
                .Property(m => m.Price)
                .HasPrecision(18, 6);

            modelBuilder.Entity<Order>()
                .Property(o => o.OrderTotal)
                .HasPrecision(18, 2);
        }
    }       
}
