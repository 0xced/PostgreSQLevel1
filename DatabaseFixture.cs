using System.Threading.Tasks;
using Testcontainers.PostgreSql;
using Xunit;

namespace PostgreSQLevel1;

public class DatabaseFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder().Build();

    public string ConnectionString => _container.GetConnectionString();

    async Task IAsyncLifetime.InitializeAsync() => await _container.StartAsync();

    async Task IAsyncLifetime.DisposeAsync() => await _container.StopAsync();
}