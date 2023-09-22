namespace PostgreSQLevel1;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class UnitTest_None : UnitTest
{
    public UnitTest_None(ITestOutputHelper testOutputHelper) : base(testOutputHelper) {}

    protected override string CreateDatabaseScript => """
        CREATE TABLE Addresses
        (
            Id serial PRIMARY KEY,
            City varchar(100)
        );
        """;
}