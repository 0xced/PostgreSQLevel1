using System;
using System.Collections.Generic;
using FluentAssertions.Execution;

namespace PostgreSQLevel1;

public abstract class UnitTest<T> : IDisposable, IClassFixture<T> where T : DatabaseFixture
{
    private readonly DbContextOptions<AppContext> _options;
    private readonly AssertionScope _assertionScope;

    protected UnitTest(T dbFixture, ITestOutputHelper testOutputHelper)
    {
        _options = dbFixture.Options;
        dbFixture.OutputHelper = testOutputHelper;
        _assertionScope = new AssertionScope();
    }

    public void Dispose() => _assertionScope.Dispose();

    public static IEnumerable<object[]> GetQueries()
    {
        yield return new object[] { "zürich" };
        yield return new object[] { "Zürich" };
        yield return new object[] { "ZÜRICH" };
        yield return new object[] { "zurich" };
        yield return new object[] { "Zurich" };
        yield return new object[] { "ZURICH" };
    }

    [Theory]
    [MemberData(nameof(GetQueries))]
    public async Task Address_Contains(string query)
    {
        await using var context = new AppContext(_options, usesCitext: GetType() == typeof(UnitTest_Citext));

        var result = await context.Suppliers.Where(e => e.Address.Contains(query)).Select(e => e.Address).ToListAsync();

        result.Should().HaveCount(4);
        result.Should().BeEquivalentTo(new[]
        {
            "Bahnhofstrasse 21, 8001 Zürich",
            "Bahnhofstrasse 21, 8001 ZÜRICH",
            "Bahnhofstrasse 21, 8001 Zurich",
            "Bahnhofstrasse 21, 8001 ZURICH",
        });
    }

    [Theory]
    [MemberData(nameof(GetQueries))]
    public async Task Address_Ilike(string query)
    {
        await using var context = new AppContext(_options, usesCitext: GetType() == typeof(UnitTest_Citext));

        var result = await context.Suppliers.Where(e => EF.Functions.ILike(e.Address, query)).Select(e => e.Address).ToListAsync();

        result.Should().HaveCount(4);
        result.Should().BeEquivalentTo(new[]
        {
            "Bahnhofstrasse 21, 8001 Zürich",
            "Bahnhofstrasse 21, 8001 ZÜRICH",
            "Bahnhofstrasse 21, 8001 Zurich",
            "Bahnhofstrasse 21, 8001 ZURICH",
        });
    }

    [Theory]
    [MemberData(nameof(GetQueries))]
    public async Task City_Equals(string query)
    {
        await using var context = new AppContext(_options, usesCitext: GetType() == typeof(UnitTest_Citext));

        var result = await context.Suppliers.Where(e => e.City == query).Select(e => e.Address).ToListAsync();

        result.Should().HaveCount(4);
        result.Should().BeEquivalentTo(new[]
        {
            "Bahnhofstrasse 21, 8001 Zürich",
            "Bahnhofstrasse 21, 8001 ZÜRICH",
            "Bahnhofstrasse 21, 8001 Zurich",
            "Bahnhofstrasse 21, 8001 ZURICH",
        });
    }

    [SkippableTheory]
    [MemberData(nameof(GetQueries))]
    public async Task City_TrigramsSimilarity(string query)
    {
        Skip.IfNot(GetType() == typeof(UnitTest_Trigram), "Trigrams are only supported when the pg_trgm extension is enabled");

        await using var context = new AppContext(_options, usesCitext: GetType() == typeof(UnitTest_Citext));

        var result = await context.Suppliers.Where(e => EF.Functions.TrigramsSimilarity(e.City, query) > 0.3).Select(e => e.Address).ToListAsync();

        result.Should().HaveCount(4);
        result.Should().BeEquivalentTo(new[]
        {
            "Bahnhofstrasse 21, 8001 Zürich",
            "Bahnhofstrasse 21, 8001 ZÜRICH",
            "Bahnhofstrasse 21, 8001 Zurich",
            "Bahnhofstrasse 21, 8001 ZURICH",
        });
    }
}
