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

        public int CreId { get; set; }
        public int RealmRegion { get; set; }
        public int RealmPopulation { get; set; }

        public virtual RealmPopulation RealmPopulationNavigation { get; set; }
        public virtual RealmRegion RealmRegionNavigation { get; set; }
        public virtual ICollection<Realm> Realm { get; set; }
        public virtual ICollection<TaskAuctionScan> TaskAuctionScan { get; set; }
    }
}
