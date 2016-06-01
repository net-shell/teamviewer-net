using System;

namespace TeamViewerNet.Models.Response.Objects
{
    public class ReportRecord
    {
        public string id { get; set; }
        public string userid { get; set; }
        public string username { get; set; }
        public string deviceid { get; set; }
        public string devicename { get; set; }
        public string groupid { get; set; }
        public string groupname { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public string fee { get; set; }
        public string currency { get; set; }
        public string billing_state { get; set; }
        public string notes { get; set; }
        // additional
        public string assigned_userid { get; set; }
        public DateTime assigned_at { get; set; }
        public string session_code { get; set; }
        public DateTime session_created_at { get; set; }
        public DateTime valid_until { get; set; }
        public string description { get; set; }
        public string custom_api { get; set; }
    }
}
