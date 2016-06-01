namespace TeamViewerNet.Models.Response
{
    public class GetUserResponse : BaseResponseModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string permissions { get; set; }
        public bool active { get; set; }
    }
}
