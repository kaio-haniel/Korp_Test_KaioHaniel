using InvoiceService.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceService.Api.Data;

public class InvoiceDbContext : DbContext
{
    public InvoiceDbContext(DbContextOptions<InvoiceDbContext> options) : base(options)
    {
    }

    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InvoiceItem> InvoiceItems => Set<InvoiceItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasIndex(i => i.Number).IsUnique();
        });

        modelBuilder.Entity<InvoiceItem>(entity =>
        {
            entity.HasOne(item => item.Invoice)
                  .WithMany(invoice => invoice.Items)
                  .HasForeignKey(item => item.InvoiceId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}