namespace GildedRoseKata;

public class ConjuredItem : InventoryItem
{
    public const string Name = "Conjured Mana Cake";

    public ConjuredItem(Item item) : base(item)
    {
    }

    public override int GetQualityChange()
    {
        return (SellIn > 0) ? -QualityDeltas["Medium"] : -QualityDeltas["VeryFast"];
    }
}
