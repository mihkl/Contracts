using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DataContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Contract> Contracts { get; set; }
    public DbSet<Template> Templates { get; set; }
    public DbSet<ContractSignature> Signatures { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contract>()
            .HasMany(c => c.Fields)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Template>()
            .HasMany(t => t.Fields)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Contract>()
            .HasMany(c => c.SubmittedFields)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Contract>()
            .HasMany(c => c.Signatures)
            .WithOne()
            .HasForeignKey(cs => cs.ContractId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
