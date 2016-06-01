namespace TeamViewerNet.Models.Response
{
    public class AccountResponse : BaseResponseModel
    {
        public string name { get; set; }
        public string email { get; set; }
        public string userid { get; set; }
        public string company_name { get; set; }
    }
}
