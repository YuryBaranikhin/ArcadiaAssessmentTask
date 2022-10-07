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

    [Fact]
    // The test is necessary to avoid a situation where each time 4 pretest items come first and then come 6 operational items
    // Implementation without using a probabilistic approach can lead to strong coupling
    // The probability of a false negative outcome is less than 10^-301
    // A few values for comparison: the number of atoms in the observable universe is about 10^81
    // The number of seconds elapsed from the moment of the Big Bang to the present time is 10^17
    public void Randomize_should_eventually_return_operational_item_for_3d_or_4th_element()
    {
        // arrange
        var testlet = CreateTestlet();
        bool succeded = false;

        // act
        for (int i = 0; !succeded && i < 1000; i++)
        {
            var result = testlet.Randomize();
            succeded = result[2].ItemType == ItemTypeEnum.Operational ||
                       result[3].ItemType == ItemTypeEnum.Operational;
        }

        // assert
        succeded.Should().BeTrue();
    }
}
