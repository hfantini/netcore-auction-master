using System;
using System.Collections.Generic;

namespace AuctionMaster.App.Model
{
    public partial class ConnectedRealm
    {
        public ConnectedRealm()
        {
            Auction = new HashSet<Auction>();
            Realm = new HashSet<Realm>();
        }

        public int Id { get; set; }
        public int RealmRegion { get; set; }

        public virtual ConnectedRealmRegion RealmRegionNavigation { get; set; }
        public virtual ICollection<Auction> Auction { get; set; }
        public virtual ICollection<Realm> Realm { get; set; }
    }
}
