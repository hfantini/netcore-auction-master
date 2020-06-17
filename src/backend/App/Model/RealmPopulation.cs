using System;
using System.Collections.Generic;

namespace AuctionMaster.App.Model
{
    public partial class RealmPopulation
    {
        public RealmPopulation()
        {
            ConnectedRealm = new HashSet<ConnectedRealm>();
        }

        public int RpopId { get; set; }
        public string RpopName { get; set; }

        public virtual ICollection<ConnectedRealm> ConnectedRealm { get; set; }
    }
}
