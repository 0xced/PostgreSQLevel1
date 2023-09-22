using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace PostgreSQLevel1;

public class UnitTest : IClassFixture<DatabaseFixture>
{
    private readonly DbContextOptions<AppContext> _options;

    public UnitTest(DatabaseFixture fixture, ITestOutputHelper testOutputHelper)
    {
        _options = new DbContextOptionsBuilder<AppContext>().UseNpgsql(fixture.ConnectionString)
            .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddXUnit(testOutputHelper)))
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging()
            .Options;

        using var context = new AppContext(_options);
        context.Database.EnsureCreated();
    }

    [Fact]
    public async Task Test()
    {
        await using var context = new AppContext(_options);
    }
}

