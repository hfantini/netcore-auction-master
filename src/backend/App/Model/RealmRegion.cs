using System;
using System.Collections.Generic;

namespace AuctionMaster.App.Model
{
    public partial class RealmRegion
    {
        public RealmRegion()
        {
            ConnectedRealm = new HashSet<ConnectedRealm>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ConnectedRealm> ConnectedRealm { get; set; }
    }
}
