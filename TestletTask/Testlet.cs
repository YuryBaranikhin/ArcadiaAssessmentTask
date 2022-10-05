namespace TestletTask;

public class Testlet
{
    public string TestletId;
    private List<Item> Items;

    protected static readonly Random Random = Random.Shared;

    public Testlet(string testletId, List<Item> items)
    {
        TestletId = testletId;
        Items = items;
    }

    public List<Item> Randomize()
    {
        var result = new List<Item>(Items.Count);

        result.AddRange(Shuffle(Items.Where(x => x.ItemType == ItemTypeEnum.Pretest)).Take(2));
        result.AddRange(Shuffle(Items.Except(result)));

        return result;
    }

    /// <summary>
    /// <see href="https://en.wikipedia.org/wiki/Fisher–Yates_shuffle">Fisher–Yates shuffle</see>
    /// </summary>
    private IReadOnlyList<Item> Shuffle(IEnumerable<Item> itemsToShuffle)
    {
        var trgRandom = GetRandom();
        var result = new List<Item>(itemsToShuffle);

        for (var n = result.Count - 1; n > 0; --n)
        {
            var k = trgRandom.Next(n + 1);
            (result[n], result[k]) = (result[k], result[n]);
        }

        return result;
    }

    protected virtual Random GetRandom()
    {
        return Random;
    }
}
