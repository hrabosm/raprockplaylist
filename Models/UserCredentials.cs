using System;
using System.Collections.Generic;

namespace RaprockPlaylist.Models
{
    public partial class UserCredentials
    {
        public int IdUserCredentials { get; set; }
        public int IdUser { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime? TsCreate { get; set; }

        public virtual User IdUserNavigation { get; set; }
    }
}
