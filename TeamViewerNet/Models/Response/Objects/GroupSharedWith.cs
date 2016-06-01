using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeamViewerNet.Models.Response.Objects
{
    public class GroupSharedWith
    {
        public string userid { get; set; }
        public string name { get; set; }
        public string permissions { get; set; }
        public bool pending { get; set; }
    }
}
