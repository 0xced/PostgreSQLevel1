using Microsoft.EntityFrameworkCore.Infrastructure;

namespace PostgreSQLevel1;

public class AppContext : DbContext
{
    public bool UsesCitext { get; }

    public AppContext(DbContextOptions<AppContext> options, bool usesCitext) : base(options)
    {
        UsesCitext = usesCitext;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var suppliers = modelBuilder.Entity<Supplier>().ToTable("suppliers");
        suppliers.Property(e => e.Id).HasColumnName("id");
        var address = suppliers.Property(e => e.Address).HasColumnName("address");
        var city = suppliers.Property(e => e.City).HasColumnName("city");
        if (UsesCitext)
        {
            address.HasColumnType("citext");
            city.HasColumnType("citext");
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ReplaceService<IModelCacheKeyFactory, AppContextModelCacheKeyFactory>();
        base.OnConfiguring(optionsBuilder);
    }

    public DbSet<Supplier> Suppliers { get; set; } = null!;

    private class AppContextModelCacheKeyFactory : IModelCacheKeyFactory
    {
        public object Create(DbContext context, bool designTime)
            => context is AppContext appContext ? (context.GetType(), appContext.UsesCitext, designTime) : context.GetType();
    }
}

public class Supplier
{
    public int Id { get; set; }
    public string Address { get; set; } = "";
    public string City { get; set; } = "";
}