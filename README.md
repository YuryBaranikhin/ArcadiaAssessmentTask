Тестовое задание для Аркадии.

Заготовка использована в том виде, в котором была представлена; без исправления каких-либо проблем с контрактами и/или реализацией

Using Test-First Development build the functionality below -
• There is a Testlet with a fixed set of 10 items. 6 of the items are operational and 4 of them are pretest items.
• The requirement is that the _order_ of these items should be randomized such that -
o The first 2 items are always pretest items selected randomly from the 4 pretest items.
o The next 8 items are mix of pretest and operational items ordered randomly from the remaining 8
items.

To get started you may use the boilerplate structure below:
```
public class Testlet
{
    public string TestletId;
    private List<Item> Items;
    public Testlet(string testletId, List<Item> items)
    {
        TestletId = testletId;
        Items = items;
    }
    
    public List<Item> Randomize()
    {
        //Items private collection has 6 Operational and 4 Pretest Items.
        Randomize the order of these items as per the requirement (with TDD)
        //The assignment will be reviewed on the basis of – Tests written first, Correct
        logic, Well structured & clean readable code.
    }
}

public class Item
{
    public string ItemId;
    public ItemTypeEnum ItemType;
}
public enum ItemTypeEnum
{
    Pretest = 0,
    Operational = 1
}
```
