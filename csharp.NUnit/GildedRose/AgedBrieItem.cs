namespace GildedRoseKata;

public class AgedBrieItem : InventoryItem
{
    public const string Name = "Aged Brie";

    public AgedBrieItem(Item item) : base(item)
    {
    }

    public override int GetQualityChange()
    {
        return (SellIn > 0) ?  QualityDeltas["Slow"]: QualityDeltas["Medium"];
    }
}
