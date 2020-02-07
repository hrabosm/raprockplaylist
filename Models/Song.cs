using System;
using System.Collections.Generic;

namespace RaprockPlaylist.Models
{
    public partial class Song
    {
        public int IdSong { get; set; }
        public string SongUrl { get; set; }
        public int IdSongRequest { get; set; }
        public int IdBand { get; set; }

        public virtual Band IdBandNavigation { get; set; }
        public virtual SongRequest IdSongRequestNavigation { get; set; }
    }
}
