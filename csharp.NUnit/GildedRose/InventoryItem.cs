using System;
using System.Collections.Generic;

namespace GildedRoseKata
{
    // Item class can't be changed - InventoryItem wraps it.
    public class InventoryItem
    {
        // Increasingly ordered quality change rates
        public static readonly Dictionary<string, int> QualityDeltas = new Dictionary<string, int>
        {
            { "Slow",       1 },
            { "Medium",     2 },
            { "Fast",       3 },
            { "VeryFast",   4 }
        };

        // Default quality limits for most items
        public virtual int MinQuality => 0;
        public virtual int MaxQuality => 50;

        protected Item Item { get; }

        // Constructor
        public InventoryItem(Item item)
        {
            this.Item = item;
        }

        // Static Factory Method - returns a specific subclass based on the item name
        public static InventoryItem CreateInventoryItem(Item item) => item.Name switch
        {
            AgedBrieItem.Name       => new AgedBrieItem(item),
            BackstagePassItem.Name  => new BackstagePassItem(item),
            ConjuredItem.Name       => new ConjuredItem(item),
            SulfurasItem.Name       => new SulfurasItem(item),
            _                       => new InventoryItem(item)
        };

        // Daily update logic for all items
        public void DailyUpdate()
        {
            // Update quality and selling days
            Quality += GetQualityChange();
            SellIn  += GetSellinChange();
        }

        // Default quality change logic
        public virtual int GetQualityChange() => (Item.SellIn > 0) ? -QualityDeltas["Slow"] : -QualityDeltas["Medium"];

        // Default sell-in change logic
        public virtual int GetSellinChange() => -1;

        // Property for SellIn
        public int SellIn
        {
            get => Item.SellIn;
            set => Item.SellIn = value;
        }

        // Property for Quality, constrained to [MinQuality, MaxQuality]
        public int Quality
        {
            get => Item.Quality;
            set => Item.Quality = Math.Clamp(value, MinQuality, MaxQuality);
        }
    }
}
