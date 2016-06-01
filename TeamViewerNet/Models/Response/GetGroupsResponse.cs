using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeamViewerNet.Models.Response
{
    public class GetGroupsResponse : BaseResponseModel
    {
        public List<GetGroupResponse> groups { get; set; }
    }
}
