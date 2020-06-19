using System;
using System.Collections.Generic;

namespace AuctionMaster.App.Model
{
    public partial class Realm
    {
        public int Id { get; set; }
        public int ConnectedRealm { get; set; }
        public string Name { get; set; }
        public string Locale { get; set; }
        public string Timezone { get; set; }
        public string Category { get; set; }

        public virtual ConnectedRealm ConnectedRealmNavigation { get; set; }
    }
}
