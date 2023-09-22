namespace PostgreSQLevel1;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class UnitTest_Collation : UnitTest
{
    public UnitTest_Collation(ITestOutputHelper testOutputHelper) : base(testOutputHelper) {}

    protected override string CreateDatabaseScript => """
        CREATE COLLATION und_level1 (provider = icu, locale = 'und-u-ks-level1', deterministic = false);
        CREATE TABLE Addresses
        (
            Id serial PRIMARY KEY,
            City varchar(100) COLLATE und_level1
        );
        """;
}