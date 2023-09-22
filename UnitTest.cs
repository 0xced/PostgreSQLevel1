namespace PostgreSQLevel1;

public abstract class UnitTest : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder().WithImage("postgres:16.0").Build();
    private readonly ITestOutputHelper _testOutputHelper;
    private DbContextOptions<AppContext> _options = null!;

    protected UnitTest(ITestOutputHelper testOutputHelper) => _testOutputHelper = testOutputHelper;

    protected abstract string CreateDatabaseScript { get; }

    async Task IAsyncLifetime.InitializeAsync()
    {
        await _container.StartAsync();
        await _container.ExecScriptAsync(CreateDatabaseScript);
        await _container.ExecScriptAsync("""
        INSERT INTO Addresses (City) VALUES ('Zürich');
        """);
        _options = new DbContextOptionsBuilder<AppContext>().UseNpgsql(_container.GetConnectionString())
            .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddXUnit(_testOutputHelper)))
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging()
            .Options;
    }

    [Theory]
    [InlineData("zurich")]
    [InlineData("Zürich")]
    [InlineData("ZURICH")]
    [InlineData("ZÜRICH")]
    public async Task TestInsensitiveSearch(string city)
    {
        await using var context = new AppContext(_options, usesCitext: GetType() == typeof(UnitTest_Citext));

        var result = await context.Addresses.Where(e => e.City.Contains(city)).ToListAsync();

        result.Should().ContainSingle();
    }

    async Task IAsyncLifetime.DisposeAsync() => await _container.DisposeAsync();
}
