namespace PostgreSQLevel1;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class UnitTest_Standard : UnitTest<DatabaseFixtureStandard>
{
    public UnitTest_Standard(DatabaseFixtureStandard dbFixture, ITestOutputHelper testOutputHelper) : base(dbFixture, testOutputHelper)
    {
    }
}

public class DatabaseFixtureStandard : DatabaseFixture
{
    public DatabaseFixtureStandard(IMessageSink messageSink) : base(messageSink)
    {
    }

    protected override string CreateDatabaseScript => """
        CREATE TABLE Suppliers
        (
            Id serial PRIMARY KEY,
            Address varchar(100),
            City varchar(100)
        );
        """;
}