namespace GildedRoseKata;

public class BackstagePassItem : InventoryItem
{
    public const string Name = "Backstage passes to a TAFKAL80ETC concert";
    public BackstagePassItem(Item item) : base(item)
    {
    }

    public enum BackstagePassLimits
    {
        ZERO_DAYS = 0,
        FIVE_DAYS = 5,
        TEN_DAYS = 10
    }

    public int CalculatePassesQuality(int sellIn)
    {
        int qualityChange = 0;

        if(sellIn > (int)BackstagePassLimits.TEN_DAYS)
        {
            qualityChange = QualityDeltas["Slow"];
        }
        else if (sellIn <= (int)BackstagePassLimits.TEN_DAYS && 
                 sellIn  > (int)BackstagePassLimits.FIVE_DAYS)
        {
            qualityChange = QualityDeltas["Medium"];
        }
        else if (sellIn <= (int)BackstagePassLimits.FIVE_DAYS && 
                 sellIn  > (int)BackstagePassLimits.ZERO_DAYS)
        {
            qualityChange = QualityDeltas["Fast"];
        }
        return qualityChange;
    }
    public override int GetQualityChange()
    {
        return (SellIn > 0) ? CalculatePassesQuality(SellIn) : -Quality;
    }
}
