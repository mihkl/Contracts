using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DataContext(DbContextOptions options): DbContext(options)
{
    public DbSet<Contract> Contracts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contract>()
            .HasMany(c => c.Fields)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
    }
} 
