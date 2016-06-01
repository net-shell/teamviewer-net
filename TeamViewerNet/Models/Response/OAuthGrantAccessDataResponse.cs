namespace TeamViewerNet.Models.Response
{
    public class OAuthGrantAccessDataResponse : BaseResponseModel
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }
}
