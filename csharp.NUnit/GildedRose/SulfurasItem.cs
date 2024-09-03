namespace GildedRoseKata;

public class SulfurasItem : InventoryItem
{
    public const string Name = "Sulfuras, Hand of Ragnaros";

    // Override quality limits specifically for Sulfuras
    public override int MinQuality => 80;
    public override int MaxQuality => 80;

    public SulfurasItem(Item item) : base(item)
    {
    }

    // Legendary items, such as Sulfuras, don't get their stats updated - their stats change is 0.
    public override int GetQualityChange()
    {
        return 0;
    }

    public override int GetSellinChange()
    {
        return 0;
    }
}
