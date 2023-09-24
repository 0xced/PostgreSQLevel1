namespace PostgreSQLevel1;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class UnitTest_Collation : UnitTest<CollationDatabaseFixture>
{
    public UnitTest_Collation(CollationDatabaseFixture dbFixture, ITestOutputHelper testOutputHelper) : base(dbFixture, testOutputHelper)
    {
    }
}

public class CollationDatabaseFixture : DatabaseFixture
{
    public CollationDatabaseFixture(IMessageSink messageSink) : base(messageSink)
    {
    }

    protected override string CreateDatabaseScript => """
        CREATE COLLATION und_level1 (provider = icu, locale = 'und-u-ks-level1', deterministic = false);
        CREATE TABLE Suppliers
        (
            Id serial PRIMARY KEY,
            Address varchar(100) COLLATE und_level1,
            City varchar(100) COLLATE und_level1
        );
        """;
}