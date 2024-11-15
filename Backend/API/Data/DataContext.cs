using API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class User : IdentityUser
{
    public List<Contract> Contracts { get; set; } = [];
    public List<Template> Templates { get; set; } = [];
}

public class DataContext(DbContextOptions options) : IdentityDbContext(options)
{
    public DbSet<Contract> Contracts { get; set; }
    public DbSet<Template> Templates { get; set; }
    public new DbSet<User> Users { get; set; }
    public DbSet<ContractSignature> Signatures { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>()
            .HasMany(u => u.Contracts)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Templates)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

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
