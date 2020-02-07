using System;
using System.Collections.Generic;

namespace RaprockPlaylist.Models
{
    public partial class BandHasUser
    {
        public int BandIdBand { get; set; }
        public int UserIdUser { get; set; }

        public virtual Band BandIdBandNavigation { get; set; }
        public virtual User UserIdUserNavigation { get; set; }
    }
}
