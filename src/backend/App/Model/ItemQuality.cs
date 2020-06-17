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

        public int ItmqId { get; set; }
        public string ItmqName { get; set; }

        public virtual ICollection<Item> Item { get; set; }
    }
}
