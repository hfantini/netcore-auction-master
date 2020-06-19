using System;
using System.Collections.Generic;

namespace AuctionMaster.App.Model
{
    public partial class ItemQuality
    {
        public ItemQuality()
        {
            Item = new HashSet<Item>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Item> Item { get; set; }
    }
}
