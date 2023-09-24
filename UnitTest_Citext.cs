namespace PostgreSQLevel1;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class UnitTest_Citext : UnitTest<CitextDatabaseFixture>
{
    public UnitTest_Citext(CitextDatabaseFixture dbFixture, ITestOutputHelper testOutputHelper) : base(dbFixture, testOutputHelper)
    {
    }
}

public class CitextDatabaseFixture : DatabaseFixture
{
    public CitextDatabaseFixture(IMessageSink messageSink) : base(messageSink)
    {
    }

    protected override string CreateDatabaseScript => """
        CREATE EXTENSION citext;
        CREATE TABLE Suppliers
        (
            Id serial PRIMARY KEY,
            Address citext,
            City citext
        );
        """;
}
