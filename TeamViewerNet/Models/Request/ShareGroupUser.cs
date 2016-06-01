using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeamViewerNet.Models.Request
{
    public class ShareGroupUser
    {
        public string userid { get; set; }
        public Objects.ShareGroupUserPermissions Permissions { get; set; }
        public string permissions
        {
            get
            {
                return "read" + (Permissions == Objects.ShareGroupUserPermissions.ReadWrite ? "write" : string.Empty);
            }
        }
    }
}
