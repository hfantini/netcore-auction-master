using System;
using System.Collections.Generic;

namespace AuctionMaster.App.Model
{
    public partial class ConnectedRealmRegion
    {
        public ConnectedRealmRegion()
        {
            ConnectedRealm = new HashSet<ConnectedRealm>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public virtual ICollection<ConnectedRealm> ConnectedRealm { get; set; }
    }
}
