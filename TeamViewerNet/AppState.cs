using RestSharp;
using System;

namespace TeamViewerNet
{
    public class AppState
    {
        public string Code { get; set; }

        private Models.Response.TokenResponse token;
        public Models.Response.TokenResponse Token
        {
            get { return token; }
            set
            {
                token = value;
                lastTokenRefresh = DateTime.Now;
            }
        }

        public string uID { get; set; }

        private DateTime? lastTokenRefresh;

        public bool TokenNeedsRefresh()
        {
            if (lastTokenRefresh == null)
                return true;
            return DateTime.Now >= lastTokenRefresh.Value.AddSeconds(Token.expires_in);
        }

        public string Serialize()
        {
            return SimpleJson.SerializeObject(this);
        }

        public static AppState FromJSON(string json)
        {
            return SimpleJson.DeserializeObject<AppState>(json);
        }
    }
}
