namespace TeamViewerNet.Models.Response
{
    public class OAuthGrantAccessResponse : BaseResponseModel
    {
        public OAuthGrantAccessDataResponse d { get; set; }
        // no fuckin idea what these are
        public int s { get; set; }
        public int e { get; set; }
    }
}
