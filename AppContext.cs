namespace PostgreSQLevel1;

public class AppContext : DbContext
{
    private readonly bool _usesCitext;

    public AppContext(DbContextOptions<AppContext> options, bool usesCitext) : base(options)
    {
        _usesCitext = usesCitext;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var addresses = modelBuilder.Entity<Address>().ToTable("addresses");
        addresses.Property(e => e.Id).HasColumnName("id");
        var city = addresses.Property(e => e.City).HasColumnName("city");
        if (_usesCitext)
        {
            city.HasColumnType("citext");
        }
    }

    public DbSet<Address> Addresses { get; set; } = null!;
}

public class Address
{
    public int Id { get; set; }
    public string City { get; set; } = "";
}