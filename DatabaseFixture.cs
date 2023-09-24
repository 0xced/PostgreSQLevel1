using System;
using DotNet.Testcontainers.Configurations;
using MartinCostello.Logging.XUnit;
using Xunit.Sdk;

namespace PostgreSQLevel1;

public abstract class DatabaseFixture : IAsyncLifetime, ITestOutputHelperAccessor, ILogger
{
    private readonly IMessageSink _messageSink;
    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder().WithImage("postgres:16.0").Build();

    protected DatabaseFixture(IMessageSink messageSink)
    {
        _messageSink = messageSink;
        TestcontainersSettings.Logger = this;
    }

    public DbContextOptions<AppContext> Options { get; private set; } = null!;

    public ITestOutputHelper? OutputHelper { get; set; }

    protected abstract string CreateDatabaseScript { get; }

    async Task IAsyncLifetime.InitializeAsync()
    {
        await _container.StartAsync();
        await _container.ExecScriptAsync(CreateDatabaseScript);
        await _container.ExecScriptAsync("""
        INSERT INTO Suppliers (Address, City) VALUES ('Bahnhofstrasse 21, 8001 Zürich', 'Zürich');
        INSERT INTO Suppliers (Address, City) VALUES ('Bahnhofstrasse 21, 8001 ZÜRICH', 'ZÜRICH');
        INSERT INTO Suppliers (Address, City) VALUES ('Bahnhofstrasse 21, 8001 Zurich', 'Zurich');
        INSERT INTO Suppliers (Address, City) VALUES ('Bahnhofstrasse 21, 8001 ZURICH', 'ZURICH');
        """);

        Options = new DbContextOptionsBuilder<AppContext>().UseNpgsql(_container.GetConnectionString())
            .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddXUnit(this)))
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging()
            .Options;
    }

    async Task IAsyncLifetime.DisposeAsync() => await _container.DisposeAsync();

    void ILogger.Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        => _messageSink.OnMessage(new DiagnosticMessage($"[Testcontainers] {formatter(state, exception)}"));

    bool ILogger.IsEnabled(LogLevel logLevel) => true;

    IDisposable? ILogger.BeginScope<TState>(TState state) => null;
}