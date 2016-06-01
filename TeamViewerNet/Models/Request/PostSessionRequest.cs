namespace TeamViewerNet.Models.Request
{
    public class PostSessionRequest : BaseRequestModel
    {
        public string groupid { get; set; }
        public string groupname { get; set; }
        public string waiting_message { get; set; }
        public string description { get; set; }
        public Response.Objects.SessionEndCustomer end_customer { get; set; }
        public string assigned_userid { get; set; }
        public string custom_api { get; set; }
    }
}
