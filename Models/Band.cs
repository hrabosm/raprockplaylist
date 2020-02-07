using System;
using System.Collections.Generic;

namespace RaprockPlaylist.Models
{
    public partial class Band
    {
        public Band()
        {
            BandHasUser = new HashSet<BandHasUser>();
            Song = new HashSet<Song>();
        }

        public int IdBand { get; set; }
        public string BandName { get; set; }
        public string BandLocation { get; set; }
        public byte[] InPlaylist { get; set; }

        public virtual ICollection<BandHasUser> BandHasUser { get; set; }
        public virtual ICollection<Song> Song { get; set; }
    }
}
