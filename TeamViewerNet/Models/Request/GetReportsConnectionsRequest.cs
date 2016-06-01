using System;

namespace TeamViewerNet.Models.Request
{
    public class GetReportsConnectionsRequest : BaseRequestModel
    {
        public string username { get; set; }
        public string userid { get; set; }
        public string groupid { get; set; }
        public string devicename { get; set; }
        public string deviceid { get; set; }
        public DateTime from_date { get; set; }
        public DateTime to_date { get; set; }
        public string offset_id { get; set; }
        public string has_code { get; set; }
        public string session_code { get; set; }
    }
}