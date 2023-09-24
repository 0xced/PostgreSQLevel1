namespace PostgreSQLevel1;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class UnitTest_Trigram : UnitTest<TrigramDatabaseFixture>
{
    public UnitTest_Trigram(TrigramDatabaseFixture dbFixture, ITestOutputHelper testOutputHelper) : base(dbFixture, testOutputHelper)
    {
    }
}

public class TrigramDatabaseFixture : DatabaseFixture
{
    public TrigramDatabaseFixture(IMessageSink messageSink) : base(messageSink)
    {
    }

    protected override string CreateDatabaseScript => """
        CREATE EXTENSION pg_trgm;
        CREATE TABLE Suppliers
        (
            Id serial PRIMARY KEY,
            Address varchar(100),
            City varchar(100)
        );
        """;
}