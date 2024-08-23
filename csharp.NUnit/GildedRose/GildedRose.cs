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
    public static readonly int[] qualityChangeRates = {1, 2, 4};

    public GildedRose(IList<Item> Items)
    {
        this.Items = Items;
    }

    public int CalculatePassesQuality(int sellIn)
    {
        int[] backstagePassLimits = {10, 5, 0};
        int qualityChange = 0;

        if(sellIn > backstagePassLimits[0])
        {
            qualityChange = 1;
        }
        else if (sellIn<= backstagePassLimits[0] && 
                 sellIn>  backstagePassLimits[1])
        {
            qualityChange = 2;
        }
        else if (sellIn<= backstagePassLimits[1] && 
                 sellIn>  backstagePassLimits[2])
        {
            qualityChange = 3;
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
                qualityChange = (item.SellIn > 0) ? qualityChangeRates[0] : qualityChangeRates[1];
            break;

            case("Backstage passes to a TAFKAL80ETC concert"):
                qualityChange = (item.SellIn > 0) ? CalculatePassesQuality(item.SellIn) : -item.Quality;
            break;

            // Quality decrementing cases:
            case("Conjured Mana Cake"):
                qualityChange = (item.SellIn > 0) ? -qualityChangeRates[1] : -qualityChangeRates[2];
            break;

            default:
                qualityChange = (item.SellIn > 0) ? -qualityChangeRates[0] : -qualityChangeRates[1];
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