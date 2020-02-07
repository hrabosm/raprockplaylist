using System;
using System.Collections.Generic;

namespace RaprockPlaylist.Models
{
    public partial class SongRequest
    {
        public SongRequest()
        {
            Song = new HashSet<Song>();
        }

        public int IdSongRequest { get; set; }
        public string SongRequest1 { get; set; }
        public int IdUser { get; set; }

        public virtual User IdUserNavigation { get; set; }
        public virtual ICollection<Song> Song { get; set; }
    }
}
