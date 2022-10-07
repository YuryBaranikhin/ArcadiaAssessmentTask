namespace TestletTask;

//The code provided by the condition of the task remained unchanged,
//but some possible issues were noted with comments
public class Testlet
{
    // Consider exposing public members with properties
    // Consider making identity member readonly
    public string TestletId;

    // Consider exposing testlet state with public readonly member
    // Consider using minimal required contract instead of concrete implementation, for instance: IReadOnlyList<>
    private List<Item> Items;

    protected static readonly Random Random = Random.Shared;

    // Consider using minimal required contract instead of concrete implementation, for instance: IEnumerable<>
    public Testlet(string testletId, List<Item> items)
    {
        // Consider validating input parameters
        TestletId = testletId;

        // Consider ordering items according to domain requirements
        Items = items;
    }

    // Consider returning minimal required contract instead of concrete implementation, for instance: IReadOnlyList<>
    // Consider using one of the following contracts as more expected:
    //      public void Randomize() for instance method of class, which exposes its state
    //      public Testlet Randomize() for instance method of immutable class, which exposes its state
    //      public static IReadOnlyList<Item> Randomize(IEnumerable<Item> items) for static method
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
