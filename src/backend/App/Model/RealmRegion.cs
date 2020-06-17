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

        public int RreId { get; set; }
        public string RreName { get; set; }

        public virtual ICollection<ConnectedRealm> ConnectedRealm { get; set; }
    }
}
