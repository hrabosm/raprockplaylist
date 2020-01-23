using System;
using System.Collections.Generic;

namespace RaprockPlaylist.Models
{
    public partial class ErrorLog
    {
        public int IdErrorLog { get; set; }
        public int IdVisitor { get; set; }
        public string Source { get; set; }
        public string Message { get; set; }
        public DateTime TsErrorLog { get; set; }

        public virtual Visitor IdVisitorNavigation { get; set; }
    }
}
