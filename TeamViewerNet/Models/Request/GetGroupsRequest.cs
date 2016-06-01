using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeamViewerNet.Models.Request
{
    public class GetGroupsRequest : BaseRequestModel
    {
        public string name { get; set; }
        public bool shared { get; set; }
    }
}
