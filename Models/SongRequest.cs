using System;
using System.Collections.Generic;

namespace RaprockPlaylist.Models
{
    public partial class SongRequest
    {
        public int IdSongRequest { get; set; }
        public string SongRequest1 { get; set; }
        public int IdVisitor { get; set; }
        public string Email { get; set; }

        public virtual Visitor IdVisitorNavigation { get; set; }
    }
}
