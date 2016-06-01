using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeamViewerNet.Models.Response
{
    public class PostSessionsResponse : BaseResponseModel
    {
        public string code { get; set; }
        public string state { get; set; }
        public string groupid { get; set; }
        public string waiting_message { get; set; }
        public string description { get; set; }
        public Objects.SessionEndCustomer end_customer { get; set; }
        public string assigned_userid { get; set; }
        public string assigned_at { get; set; }
        public string end_customer_link { get; set; }
        public string supporter_link { get; set; }
        public string custom_api { get; set; }
        public DateTime created_at { get; set; }
        public DateTime valid_until { get; set; }
    }
}
