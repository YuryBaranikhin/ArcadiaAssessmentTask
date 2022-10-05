using AutoFixture;
using FluentAssertions;
using Xunit;

namespace TestletTask.Tests;

public class TestletTests
{
    private Fixture _fixture = new();
    private const int TestletItemsCount = 10;
    private const int TestletPretestItemsCount = 4;
    private const int TestletOperationalItemsCount = 6;

    [Fact]
    public void Randomize_should_not_change_testlet_items()
    {
        // arrange
        var originalItems = CreateInitialTestletItemsCollection();
        var testlet = CreateTestlet(originalItems);

        // act
        var result = testlet.Randomize();

        // assert
        result.Should().BeEquivalentTo(originalItems);
    }

    [Fact]
    // The requirement is that the _order_ of these items should be randomized such that -
    // The first 2 items are always pretest items selected randomly from the 4 pretest items.
    // The next 8 items are mix of pretest and operational items ordered randomly from the remaining 8
    //items.
    public void Randomize_should_return_items_according_to_required_order()
    {
        // arrange
        var testlet = CreateTestlet();

        // act
        var result = testlet.Randomize();

        // assert
        result.Should().HaveCount(TestletItemsCount);
        result.Take(2).Should().OnlyContain(x => x.ItemType == ItemTypeEnum.Pretest);
        result.Skip(2).Should()
            .Match(x => x.Count(y => y.ItemType == ItemTypeEnum.Pretest) == 2)
            .And
            .Match(x => x.Count(y => y.ItemType == ItemTypeEnum.Operational) == 6);
    }

    [Fact]
    public void Randomize_should_return_different_results()
    {
        // arrange
        var testlet = CreateTestletWrapper();

        // act
        // set randomizer with specific seed to avoid situation of getting same random results two times in a row
        testlet.Randomizer = new Random(1);
        var result1 = testlet.Randomize();
        testlet.Randomizer = new Random(2);
        var result2 = testlet.Randomize();

        // assert
        result1.Should().NotContainInOrder(result2);
    }

    private Testlet CreateTestlet(IReadOnlyList<Item>? items = null)
    {
        return new Testlet(_fixture.Create<string>(),
            items?.ToList() ?? CreateInitialTestletItemsCollection().ToList());
    }

    private TestletWrapper CreateTestletWrapper(IReadOnlyList<Item>? items = null)
    {
        return new TestletWrapper(_fixture.Create<string>(),
            items?.ToList() ?? CreateInitialTestletItemsCollection().ToList());
    }

    private IReadOnlyList<Item> CreateInitialTestletItemsCollection()
    {
        var result = new List<Item>(TestletPretestItemsCount + TestletOperationalItemsCount);

        var pretestItems = _fixture.Build<Item>()
            .With(x => x.ItemType, ItemTypeEnum.Pretest)
            .CreateMany(TestletPretestItemsCount);

        var operationalItems = _fixture.Build<Item>()
            .With(x => x.ItemType, ItemTypeEnum.Operational)
            .CreateMany(TestletOperationalItemsCount);

        result.AddRange(pretestItems);
        result.AddRange(operationalItems);

        return result;
    }

    private class TestletWrapper : Testlet
    {
        public Random? Randomizer { get; set; }
        public TestletWrapper(string testletId, List<Item> items) : base(testletId, items)
        {
        }

        protected override Random GetRandom()
        {
            return Randomizer ?? base.GetRandom();
        }
    }
}
