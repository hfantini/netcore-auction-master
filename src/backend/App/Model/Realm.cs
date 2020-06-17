using System;
using System.Collections.Generic;

namespace AuctionMaster.App.Model
{
    public partial class Realm
    {
        public int ReaId { get; set; }
        public int ConnectedRealm { get; set; }
        public string ReaName { get; set; }
        public string ReaLocale { get; set; }
        public string ReaTimezone { get; set; }
        public string ReaCategory { get; set; }

        public virtual ConnectedRealm ConnectedRealmNavigation { get; set; }
    }
}
