using System;
using System.Collections.Generic;

namespace AuctionMaster.App.Model
{
    public partial class Item
    {
        public Item()
        {
            Auction = new HashSet<Auction>();
        }

        public int ItmId { get; set; }
        public string ItmName { get; set; }
        public int ItemQuality { get; set; }
        public string ItmIcon { get; set; }
        public sbyte ItmStackable { get; set; }
        public int ItmLevelreq { get; set; }
        public int ItmPurchasePrice { get; set; }
        public int ItmSellPrice { get; set; }

        public virtual ItemQuality ItemQualityNavigation { get; set; }
        public virtual ICollection<Auction> Auction { get; set; }
    }
}
