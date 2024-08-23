using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GildedRoseKata;

public class GildedRose
{
    IList<Item> Items;

    List<string> LegendaryItemsNames = new List<string> {"Sulfuras, Hand of Ragnaros"};

    public const int MinQuality = 0;
    public const int MaxQuality = 50;

    public GildedRose(IList<Item> Items)
    {
        this.Items = Items;
    }

    public void UpdateQuality()
    {
        foreach (Item item in this.Items)
        {
            // if item in list of legendary items - continue as its stats are constant
            if(LegendaryItemsNames.Contains(item.Name))
            {
                continue;
            }

            if (item.Name != "Aged Brie" && item.Name != "Backstage passes to a TAFKAL80ETC concert")
            {
                item.Quality = Math.Max(MinQuality, item.Quality - 1);
            }
            else
            {
                if (item.Quality < 50)
                {
                    item.Quality = item.Quality + 1;

                    if (item.Name == "Backstage passes to a TAFKAL80ETC concert")
                    {
                        if (item.SellIn < 11)
                        {
                            item.Quality = Math.Min(MaxQuality, item.Quality + 1);
                        }

                        if (item.SellIn < 6)
                        {
                            item.Quality = Math.Min(MaxQuality, item.Quality + 1);
                        }
                    }
                }
            }

            // Decrement the sellIn remaining days
            item.SellIn = item.SellIn - 1;

            if (item.SellIn < 0)
            {
                if (item.Name != "Aged Brie")
                {
                    if (item.Name != "Backstage passes to a TAFKAL80ETC concert")
                    {
                        item.Quality = Math.Max(MinQuality, item.Quality - 1);
                    }
                    else
                    {
                        item.Quality = 0;
                    }
                }
                else
                {
                    item.Quality = Math.Min(MaxQuality, item.Quality + 1);
                }
            }
        }
    }
}