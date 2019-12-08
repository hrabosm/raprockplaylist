using System;
using System.Collections.Generic;

namespace RaprockPlaylist.Models
{
    public partial class Band
    {
        public int IdBand { get; set; }
        public string BandName { get; set; }
        public string BandLocation { get; set; }
        public byte[] InPlaylist { get; set; }
    }
}
