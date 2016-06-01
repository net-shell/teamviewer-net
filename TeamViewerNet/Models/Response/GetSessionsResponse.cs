using System;
using System.Collections.Generic;

namespace TeamViewerNet.Models.Response
{
    public class GetSessionsResponse : BaseResponseModel
    {
        public List<Objects.GetSessionsSession> sessions { get; set; }
        public int sessions_remaining { get; set; }
        public string next_offset { get; set; }
        // full
        public string description { get; set; }
        public string end_customer { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string assigned_userid { get; set; }
        public DateTime assigned_at { get; set; }
        public List<Objects.SessionEndCustomer> end_customer_link { get; set; }
        public string supporter_link { get; set; }
        public string custom_api { get; set; }
        public DateTime created_at { get; set; }
        public DateTime valid_until { get; set; }
        public DateTime closed_at { get; set; }
    }
}
