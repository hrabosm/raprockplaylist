using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Internal;
using RaprockPlaylist.Context;

namespace RaprockPlaylist.Models
{
    public partial class Log
    {
        public int IdLog { get; set; }
        public int IdVisitor { get; set; }
        public DateTime TsLog { get; set; }
        public string Source { get; set; }
        public string Message { get; set; }
        public virtual Visitor IdVisitorNavigation { get; set; }
        public void LogContent(string source, string message)
        {
            this.Source = source;
            this.Message = message;
        }
        public Visitor Initialize(PlaylistContext context, IHttpContextAccessor accessor)
        {
            Visitor visitor = context.Visitor.Where(v => v.IpAdress == accessor.HttpContext.Connection.RemoteIpAddress.ToString()).FirstOrDefault() ?? new Visitor();
            if(String.IsNullOrEmpty(visitor.IpAdress))
            {
                visitor.GetIpAdress(accessor);
                context.Visitor.Add(visitor);
            }
            IdVisitorNavigation = visitor;
            return visitor;
        }
    }
}
