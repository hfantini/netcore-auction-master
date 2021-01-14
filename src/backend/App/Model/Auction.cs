using System;
using System.Collections.Generic;

namespace AuctionMaster.App.Model
{
    public partial class Auction
    {
        public int Id { get; set; }
        public int ScheduledTaskLog { get; set; }
        public int ConnectedRealm { get; set; }
        public int Item { get; set; }
        public long? Buyout { get; set; }
        public long? UnitPrice { get; set; }
        public long? Bid { get; set; }
        public int Quantity { get; set; }

        public virtual ConnectedRealm ConnectedRealmNavigation { get; set; }
        public virtual Item ItemNavigation { get; set; }
        public virtual ScheduledTaskLog ScheduledTaskLogNavigation { get; set; }
    }
}
