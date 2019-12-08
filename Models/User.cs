using System;
using System.Collections.Generic;

namespace RaprockPlaylist.Models
{
    public partial class User
    {
        public User()
        {
            SongRequest = new HashSet<SongRequest>();
        }

        public int IdUser { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }

        public virtual ICollection<SongRequest> SongRequest { get; set; }
    }
}
