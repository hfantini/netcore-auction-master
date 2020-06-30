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
        public int Buyout { get; set; }
        public int Bid { get; set; }
        public int Quantity { get; set; }

        public virtual ConnectedRealm ConnectedRealmNavigation { get; set; }
        public virtual Item ItemNavigation { get; set; }
        public virtual ScheduledTaskLog ScheduledTaskLogNavigation { get; set; }
    }
}
