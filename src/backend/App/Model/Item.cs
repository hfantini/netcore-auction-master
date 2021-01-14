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

        public int Id { get; set; }
        public string Name { get; set; }
        public int Quality { get; set; }
        public string Icon { get; set; }
        public sbyte Stackable { get; set; }
        public int? Levelreq { get; set; }
        public int? PurchasePrice { get; set; }
        public int? SellPrice { get; set; }

        public virtual ItemQuality QualityNavigation { get; set; }
        public virtual ICollection<Auction> Auction { get; set; }
    }
}
