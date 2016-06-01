namespace TeamViewerNet.Models.Response
{
    public class TokenResponse : BaseResponseModel
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string refresh_token { get; set; }

        public override string ToString()
        {
            return access_token;
        }
    }
}
