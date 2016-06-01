using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeamViewerNet.Models.Response
{
    public class GetGroupResponse : BaseResponseModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public Objects.GroupSharedWith shared_with { get; set; }
        public Objects.GroupOwner owner { get; set; }
        public string permissions { get; set; }
    }
}
