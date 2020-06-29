using System;
using System.Collections.Generic;

namespace AuctionMaster.App.Model
{
    public partial class ConnectedRealm
    {
        public ConnectedRealm()
        {
            Realm = new HashSet<Realm>();
            TaskAuctionScan = new HashSet<TaskAuctionScan>();
        }

        public int Id { get; set; }
        public int RealmRegion { get; set; }

        public virtual ConnectedRealmRegion RealmRegionNavigation { get; set; }
        public virtual ICollection<Realm> Realm { get; set; }
        public virtual ICollection<TaskAuctionScan> TaskAuctionScan { get; set; }
    }
}
