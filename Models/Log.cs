using System;
using System.Collections.Generic;

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
    }
}
