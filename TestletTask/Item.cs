namespace TestletTask;

//The code provided by the condition of the task remained unchanged,
//but some possible issues were noted with comments

// Consider transforming data class to immutable one with record, structure or readonly members
public class Item
{
    // Consider exposing public members with properties
    // Consider making identity member readonly
    public string ItemId;
    public ItemTypeEnum ItemType;
}
