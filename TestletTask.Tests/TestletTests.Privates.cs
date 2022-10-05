using AutoFixture;

namespace TestletTask.Tests;

public partial class TestletTests
{
    private Fixture _fixture = new();
    private const int TestletItemsCount = 10;
    private const int TestletPretestItemsCount = 4;
    private const int TestletOperationalItemsCount = 6;

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
