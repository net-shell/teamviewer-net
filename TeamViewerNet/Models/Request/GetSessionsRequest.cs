namespace TeamViewerNet.Models.Request
{
    public class GetSessionsRequest : BaseRequestModel
    {
        public string groupid { get; set; }
        public string assigned_userid { get; set; }
        public string state { get; set; }
        public bool full_list { get; set; }
        public string offset { get; set; }
    }
}
