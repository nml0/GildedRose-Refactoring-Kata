using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Collections.Immutable;

namespace GildedRoseKata;

public class GildedRose
{
    IList<Item> Items;

    // The following list contains items that shall not be updated on their stats.
    public static readonly ImmutableList<string> LegendaryItemsNames = ImmutableList.Create("Sulfuras, Hand of Ragnaros");

    // For all updatable items, their quality stat must be restricted to [0, 50].
    public const int MinQuality = 0;
    public const int MaxQuality = 50;

    // Increasingly ordered quality change rates.
    public static readonly Dictionary<string, int> QualityDeltas = new Dictionary<string, int>
    {
        { "Slow",       1 },
        { "Medium",     2 },
        { "Fast",       3 },
        { "VeryFast",   4 }
    };

    public enum BackstagePassLimits
    {
        ZERO_DAYS = 0,
        FIVE_DAYS = 5,
        TEN_DAYS = 10
    }

    public GildedRose(IList<Item> Items)
    {
        this.Items = Items;
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

    public int GetQualityChange(Item item)
    {
        int qualityChange;

        switch(item.Name)
        {
            // Quality incrementing cases:
            case("Aged Brie"):
                qualityChange = (item.SellIn > 0) ?  QualityDeltas["Slow"]: QualityDeltas["Medium"];
            break;

            case("Backstage passes to a TAFKAL80ETC concert"):
                qualityChange = (item.SellIn > 0) ? CalculatePassesQuality(item.SellIn) : -item.Quality;
            break;

            // Quality decrementing cases:
            case("Conjured Mana Cake"):
                qualityChange = (item.SellIn > 0) ? -QualityDeltas["Medium"] : -QualityDeltas["VeryFast"];
            break;

            default:
                qualityChange = (item.SellIn > 0) ? -QualityDeltas["Slow"] : -QualityDeltas["Medium"];
            break;
        }

        return qualityChange;
    }

    public void UpdateQuality()
    {
        foreach (Item item in this.Items)
        {
            // When item is legendary, avoid updating as its stats are constant.
            if(LegendaryItemsNames.Contains(item.Name))
                continue;

            // Update the quality of the item.
            item.Quality = Math.Clamp(item.Quality + GetQualityChange(item), MinQuality, MaxQuality);
            
            // Decrement the selling remaining days of the item.
            item.SellIn--;
        }
    }
}