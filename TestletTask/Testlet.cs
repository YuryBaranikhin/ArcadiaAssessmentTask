namespace TestletTask;

public class Testlet
{
    public string TestletId;
    private List<Item> Items;

    private readonly Random Random;

    public Testlet(string testletId, List<Item> items)
    {
        TestletId = testletId;
        Items = items;

        Random = new Random();
    }

    public List<Item> Randomize()
    {
        var result = new List<Item>(Items.Count);

        result.AddRange(Shuffle(Items.Where(x => x.ItemType == ItemTypeEnum.Pretest).ToList()));
        result.AddRange(Shuffle(Items.Except(result).ToList()));

        return result;
    }

    /// <summary>
    /// <see href="https://en.wikipedia.org/wiki/Fisher–Yates_shuffle">Fisher–Yates shuffle</see>
    /// </summary>
    private IReadOnlyList<Item> Shuffle(IReadOnlyList<Item> itemsToShuffle)
    {
        var result = new List<Item>(itemsToShuffle);

        for (var n = result.Count - 1; n > 0; --n)
        {
            var k = Random.Next(n + 1);
            (result[n], result[k]) = (result[k], result[n]);
        }

        return result;
    }
}
