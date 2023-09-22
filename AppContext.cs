using Microsoft.EntityFrameworkCore;

namespace PostgreSQLevel1;

public class AppContext : DbContext
{
    public AppContext(DbContextOptions<AppContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>().HasKey(e => e.Id);
        modelBuilder.Entity<Address>().Property(e => e.City).HasColumnType("varchar(100)");
        modelBuilder.Entity<Address>().HasData(
            new Address { Id = 1, City = "Zürich" },
            new Address { Id = 2, City = "Neuchatel" }
        );
    }

    public DbSet<Address> Addresses { get; set; } = null!;
}

public class Address
{
    public int Id { get; set; }
    public string? City { get; set; }
}