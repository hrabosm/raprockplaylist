using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace RaprockPlaylist.Models
{
    public partial class Visitor
    {
        public Visitor()
        {
            ErrorLog = new HashSet<ErrorLog>();
            Log = new HashSet<Log>();
            SongRequest = new HashSet<SongRequest>();
        }
        public void GetIpAdress(IHttpContextAccessor accessor)
        {
            this.IpAdress = accessor.HttpContext.Connection.RemoteIpAddress.ToString();
        }
        public int IdVisitor { get; set; }
        public string IpAdress { get; set; }

        public virtual ICollection<ErrorLog> ErrorLog { get; set; }
        public virtual ICollection<Log> Log { get; set; }
        public virtual ICollection<SongRequest> SongRequest { get; set; }
    }
}
