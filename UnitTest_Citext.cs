namespace PostgreSQLevel1;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class UnitTest_Citext : UnitTest
{
    public UnitTest_Citext(ITestOutputHelper testOutputHelper) : base(testOutputHelper) {}

    protected override string CreateDatabaseScript => """
        CREATE EXTENSION citext;
        CREATE TABLE Addresses
        (
            Id serial PRIMARY KEY,
            City citext
        );
        """;
}