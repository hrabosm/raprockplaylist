using System;
using System.Collections.Generic;

namespace RaprockPlaylist.Models
{
    public partial class UserHasVisitor
    {
        public int UserIdUser { get; set; }
        public int VisitorIdVisitor { get; set; }

        public virtual User UserIdUserNavigation { get; set; }
        public virtual Visitor VisitorIdVisitorNavigation { get; set; }
    }
}
