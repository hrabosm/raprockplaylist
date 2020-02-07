using System;
using System.Collections.Generic;

namespace RaprockPlaylist.Models
{
    public partial class User
    {
        public User()
        {
            BandHasUser = new HashSet<BandHasUser>();
            SongRequest = new HashSet<SongRequest>();
            UserCredentials = new HashSet<UserCredentials>();
            UserHasVisitor = new HashSet<UserHasVisitor>();
        }

        public string Email { get; set; }
        public DateTime? TsCreate { get; set; }
        public int IdUser { get; set; }
        public bool ConsentGdpr { get; set; }

        public virtual ICollection<BandHasUser> BandHasUser { get; set; }
        public virtual ICollection<SongRequest> SongRequest { get; set; }
        public virtual ICollection<UserCredentials> UserCredentials { get; set; }
        public virtual ICollection<UserHasVisitor> UserHasVisitor { get; set; }
    }
}
