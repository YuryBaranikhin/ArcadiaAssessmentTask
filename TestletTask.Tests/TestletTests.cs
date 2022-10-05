using FluentAssertions;
using Xunit;

namespace TestletTask.Tests;

public partial class TestletTests
{
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
}
