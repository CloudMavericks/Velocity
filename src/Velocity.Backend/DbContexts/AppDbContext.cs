using Microsoft.EntityFrameworkCore;
using Velocity.Shared.Entities;

namespace Velocity.Backend.DbContexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Supplier> Suppliers { get; set; }
    
    public DbSet<Customer> Customers { get; set; }
    
    public DbSet<Product> Products { get; set; }

    public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
    public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }
    
    public DbSet<PurchaseInvoice> PurchaseInvoices { get; set; }
    public DbSet<PurchaseInvoiceItem> PurchaseInvoiceItems { get; set; }
    
    public DbSet<SalesInvoice> SalesInvoices { get; set; }
    public DbSet<SalesInvoiceItem> SalesInvoiceItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(t => t.GetProperties())
                     .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
        {
            property.SetColumnType("decimal(18,2)");
        }
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<PurchaseOrderItem>(entity => entity.ToTable("PurchaseOrderItem"));
        modelBuilder.Entity<PurchaseInvoiceItem>(entity => entity.ToTable("PurchaseInvoiceItem"));
        modelBuilder.Entity<SalesInvoiceItem>(entity => entity.ToTable("SalesInvoiceItem"));
        modelBuilder.Entity<PurchaseOrder>(entity =>
        {
            entity
                .HasMany(x => x.Items)
                .WithOne(x => x.PurchaseOrder)
                .OnDelete(DeleteBehavior.NoAction);
        });
        modelBuilder.Entity<PurchaseOrderItem>(entity =>
        {
            entity
                .HasOne(x => x.PurchaseOrder)
                .WithMany(x => x.Items)
                .OnDelete(DeleteBehavior.NoAction);
        });
        modelBuilder.Entity<PurchaseInvoice>(entity =>
        {
            entity
                .HasMany(x => x.Items)
                .WithOne(x => x.PurchaseInvoice)
                .OnDelete(DeleteBehavior.NoAction);
        });
        modelBuilder.Entity<PurchaseInvoiceItem>(entity =>
        {
            entity
                .HasOne(x => x.PurchaseInvoice)
                .WithMany(x => x.Items)
                .OnDelete(DeleteBehavior.NoAction);
        });
        modelBuilder.Entity<SalesInvoice>(entity =>
        {
            entity
                .HasMany(x => x.Items)
                .WithOne(x => x.SalesInvoice)
                .OnDelete(DeleteBehavior.NoAction);
        });
        modelBuilder.Entity<SalesInvoiceItem>(entity =>
        {
            entity
                .HasOne(x => x.SalesInvoice)
                .WithMany(x => x.Items)
                .OnDelete(DeleteBehavior.NoAction);
        });
    }
}