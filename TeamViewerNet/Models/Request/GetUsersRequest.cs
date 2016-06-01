namespace TeamViewerNet.Models.Request
{
    public class GetUsersRequest : BaseRequestModel
    {
        public string email { get; set; }
        public string name { get; set; }
        public string permissions { get; set; }
        public bool full_list { get; set; }
    }
}
