namespace TeamViewerNet.Models.Response
{
    public class ErrorResponse : BaseResponseModel
    {
        public string error { get; set; }
        public string error_description { get; set; }
        public string error_code { get; set; }
    }
}
