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

    // The following list contains items that get more valuable over time.
    public static readonly ImmutableList<string> AppreciatingItemsNames = ImmutableList.Create("Aged Brie", "Backstage passes to a TAFKAL80ETC concert");


    // For all updatable items, their quality stat must be restricted to [0, 50].
    public const int MinQuality = 0;
    public const int MaxQuality = 50;

    public const int passLimit1  = 10; // : quality+= 1
    public const int passLimit2  = 5; // : quality+= 2
    public const int passLimit3  = 0; // : quality+= 3 // : quality reset


    public GildedRose(IList<Item> Items)
    {
        this.Items = Items;
    }

    public int CalculateQualityIncrement(Item item)
    {
        int qualityInc = 0;

        if (item.Name == "Aged Brie")
        {
            if(item.SellIn > 0)
            {
                qualityInc = 1;
            }
            else
            {
                qualityInc = 2;
            }
        }
        else if (item.Name == "Backstage passes to a TAFKAL80ETC concert")
        {
            if(item.SellIn > passLimit1)
            {
                qualityInc = 1;
            }
            else if (item.SellIn <= passLimit1 && item.SellIn > passLimit2)
            {
                qualityInc = 2;
            }
            else if (item.SellIn <= passLimit2 && item.SellIn > passLimit3)
            {
                qualityInc = 3;
            }
            else
            {
                qualityInc = -item.Quality;
            }
        }

        return qualityInc;
    }

    public int CalculateQualityDecrement(Item item)
    {
        int qualityDec = 0;

        if(item.SellIn > 0)
        {
            qualityDec = 1;
        }
        else
        {
            qualityDec = 2;
        }

        return qualityDec;
    }

    public void UpdateQuality()
    {
        foreach (Item item in this.Items)
        {
            // When item is legendary, avoid updating as its stats are constant.
            if(LegendaryItemsNames.Contains(item.Name))
            {
                continue;
            }

            // When item is appreciating (i.e., may increment its value over time), calculate the quality increment rate and update
            if (AppreciatingItemsNames.Contains(item.Name))
            {
                item.Quality = Math.Min(MaxQuality, item.Quality + CalculateQualityIncrement(item));
            }
            else // normal items decrement their quality over time
            {
                item.Quality = Math.Max(MinQuality, item.Quality - CalculateQualityDecrement(item));
            }

            // Decrement the sellIn remaining days
            item.SellIn = item.SellIn - 1;
        }
    }
}