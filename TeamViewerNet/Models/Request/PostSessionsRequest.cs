using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeamViewerNet.Models.Request
{
    public class PostSessionsRequest : BaseRequestModel
    {
        public DateTime valid_until { get; set; }
        public string groupid { get; set; }
        public string groupname { get; set; }
        public string waiting_message { get; set; }
        public string description { get; set; }
        public Response.Objects.SessionEndCustomer end_customer { get; set; }
        public string assigned_userid { get; set; }
        public string custom_api { get; set; }
    }
}
