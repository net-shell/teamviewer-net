using RestSharp;
using RestSharp.Contrib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TeamViewerNet
{
    public class API
    {
        private RestClient restClient;

        public AppState State { get; set; }

        private CookieContainer cookieContainer;
        const string loginBaseUrl = "https://login.teamviewer.com/";
        const string loginUrl = loginBaseUrl + "Account/LogOn?returnUrl=";


        public API(AppState state = null)
        {
            State = state ?? new AppState();
            restClient = new RestClient(Config.ApiUrl);
            cookieContainer = new CookieContainer();
        }

        #region Authorization

        private bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        private bool WebLogin(Models.AuthCredentials authForm)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(loginUrl);
                request.CookieContainer = cookieContainer;
                using (request.GetResponse()) { }
            }
            catch (Exception)
            {
                return false;
            }

            // Login
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(loginUrl);

                string postData = string.Format("UserName={0}&Password={1}&RememberMe=false", authForm.Username, HttpUtility.UrlEncode(authForm.Password));

                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
                byte[] data = Encoding.ASCII.GetBytes(postData);
                request.AllowAutoRedirect = true;
                request.ContentLength = data.Length;
                request.ContentType = "application/x-www-form-urlencoded";
                request.CookieContainer = cookieContainer;
                request.Method = "POST";
                request.Referer = loginUrl;

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                using (request.GetResponse()) { }
            }
            catch (Exception)
            {
                return false;
            }

            // Auth
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(loginBaseUrl + "ManageApi/OAuthGrantAccess");

                string postData = '{' + string.Format("\"client_id\":\"{0}\",\"isImplicitGrantFlow\":false", Config.ClientID) + '}';

                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
                byte[] data = Encoding.ASCII.GetBytes(postData);
                request.ContentLength = data.Length;
                request.ContentType = "application/json";
                request.CookieContainer = cookieContainer;
                request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                request.Method = "POST";

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    string responseString = new StreamReader(responseStream).ReadToEnd();
                    Models.Response.OAuthGrantAccessResponse resp = SimpleJson.DeserializeObject<Models.Response.OAuthGrantAccessResponse>(responseString);
                    State.Code = resp.d.Code;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public bool Authenticate(Models.AuthCredentials authForm)
        {
            if (!WebLogin(authForm))
                return false;

            return getAuthToken();
        }

        private bool getAuthToken(bool refresh = false)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Config.ApiUrl + string.Format(Config.ApiResource, "oauth2/token"));

                string postData;

                if (refresh)
                    postData = string.Format("grant_type={0}&refresh_token={1}&client_id={2}&client_secret={3}", "refresh_token", State.Token, Config.ClientID, Config.ClientSecret);
                else
                    postData = string.Format("grant_type={0}&code={1}&client_id={2}&client_secret={3}", "authorization_code", State.Code, Config.ClientID, Config.ClientSecret);

                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
                byte[] data = Encoding.ASCII.GetBytes(postData);
                request.ContentLength = data.Length;
                request.ContentType = "application/x-www-form-urlencoded";
                request.Method = "POST";

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    string responseString = new StreamReader(responseStream).ReadToEnd();
                    State.Token = SimpleJson.DeserializeObject<Models.Response.TokenResponse>(responseString);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region API Methods

        #region 3.2 Ping

        /// <summary>
        /// This function can be used to check if the API is available. It can also be used to verify if the token is valid.
        /// </summary>
        public Models.Response.PingResponse Ping()
        {
            return getResponse<Models.Response.PingResponse>(
                makeRequest("ping", Method.GET, new { token = State.Token.access_token })
                );
        }

        #endregion

        #region 3.3 Account Management

        /// <summary>
        /// Retrieves account information of the account associated with the access token
        /// </summary>
        public Models.Response.AccountResponse GetAccount()
        {
            return getResponse<Models.Response.AccountResponse>(
                makeRequest("account")
                );
        }

        #endregion

        #region 3.4 User Management

        /// <summary>
        /// Lists all users in a company. The list can be filtered with additional parameters. The function can also r eturn 
        /// a list containing all information about the users. This data is the same as when using GET /users/uID for 
        /// each of these users.
        /// </summary>
        public Models.Response.GetUsersResponse GetUsers(Models.Request.GetUsersRequest param = null)
        {
            return getResponse<Models.Response.GetUsersResponse>(
                makeRequest("users", Method.GET, param)
            );
        }

        /// <summary>
        /// Creates a new user for the company. The data for the new user will be returned as response to the POST. 
        /// This should be the same as GET /users/uID, except that it will include the id as well. You will need to have 
        /// the scope Users.CreateAdministrators to set the permissions ManageUsers or ManageAdmins.
        /// </summary>
        public Models.Response.GetUserResponse PostUsers(Models.Request.PostUsersRequest param)
        {
            return getResponse<Models.Response.GetUserResponse>(makeRequest("users", Method.GET, param));
        }

        /// <summary>
        /// Returns the information for a single user. The information is the same as when using GET /users?full_list=1
        /// </summary>
        public Models.Response.GetUserResponse GetUser(string id)
        {
            return getResponse<Models.Response.GetUserResponse>(
                makeRequest(string.Format("users/{0}", id))
            );
        }

        /// <summary>
        /// Changes information for a selected user. Only the parts that need to be changed are needed in the request body.
        /// Security-Warning: An attacker can gain access to a user account either by changing the email (+password reset) or by changing the password if he can steal the company access token. This makes the company access token equivalent to email and password for ALL company accounts with which an attacker can get the full Computer & Contacts list (and not just what is available over the API).
        /// </summary>
        public bool PutUser(string id, Models.Request.PutUserRequest param)
        {
            RestRequest request = makeRequest(string.Format("users/{0}", id), Method.GET, param);
            IRestResponse response = restClient.Execute(request);
            return response.StatusCode == HttpStatusCode.NoContent;
        }

        #endregion

        #region 3.5 Group Management

        /*
         * Accessing groups from different users in a company
         * Important note: If you are using a company access token, you can use all the functions below but have to 
         * prefix them with a user-location. So "GET /groups" for example becomes "GET /users/<uID>/groups".
         */

        /// <summary>
        /// Returns a list of groups.
        /// </summary>
        public Models.Response.GetGroupsResponse GetGroups(Models.Request.GetGroupsRequest param = null)
        {
            return getResponse<Models.Response.GetGroupsResponse>(
                makeUidRequest("groups", Method.GET, param)
                );
        }

        /// <summary>
        /// Creates a new group and returns its info.
        /// </summary>
        public Models.Response.GetGroupResponse PostGroups(string name)
        {
            return getResponse<Models.Response.GetGroupResponse>(
                makeUidRequest("groups", Method.POST, new { name = name })
                );
        }

        /// <summary>
        /// Returns info for one group.
        /// </summary>
        public Models.Response.GetGroupResponse GetGroup(string groupId)
        {
            return getResponse<Models.Response.GetGroupResponse>(
                makeUidRequest(string.Format("groups/{0}", groupId))
                );
        }

        /// <summary>
        /// Changes a group. Right now only the name can be changed
        /// </summary>
        public bool PutGroup(string groupId, string name)
        {
            return restClient.Execute(
                makeUidRequest(string.Format("groups/{0}", groupId, new { name = name }), Method.PUT)
                ).StatusCode == HttpStatusCode.NoContent;
        }

        /// <summary>
        /// Deletes an existing group. If the group is not owned, but only shared with the user's account it will just be unshared.
        /// </summary>
        public bool DeleteGroup(string groupId)
        {
            return restClient.Execute(
                makeUidRequest(string.Format("groups/{0}", groupId), Method.DELETE)
                ).StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// Shares a group with the given users. Will not change the share state with other users, but it is possible to overwrite the permissions for existing shares
        /// </summary>
        public bool ShareGroup(string groupId, List<Models.Request.ShareGroupUser> withUsers)
        {
            return restClient.Execute(
                makeUidRequest(string.Format("groups/{0}/share_group", groupId), Method.POST, new { users = withUsers })
                ).StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// Unshares a group from certain users.
        /// </summary>
        public bool UnshareGroup(string groupId, List<string> userIds)
        {
            return restClient.Execute(
                makeUidRequest(string.Format("groups/{0}/unshare_group", groupId), Method.POST, new { users = userIds })
                ).StatusCode == HttpStatusCode.OK;
        }

        #endregion

        #region 3.6 Session Management

        /// <summary>
        /// Lists sessions. If no filters are given it will list all sessions in the active account (user access token) or all sessions from all accounts (company access token).
        /// A single request will return a maximum of 1000 session codes. To get the next 1000 session codes, repeat the same request with the offset parameter set to the value from next_offset. Session codes will be sorted by the created_at date from new to old.
        /// </summary>
        public Models.Response.GetSessionsResponse GetSessions(Models.Request.GetSessionsRequest param)
        {
            return getResponse<Models.Response.GetSessionsResponse>(
                makeRequest("sessions", Method.GET, param)
                );
        }

        /// <summary>
        /// Creates a new session code. A session code will always be stored in a group, so either the groupid or groupname parameter must be set. Session codes will expire after 24h if no valid_until date is set.
        /// </summary>
        public Models.Response.PostSessionsResponse PostSessions(Models.Request.PostSessionsRequest param)
        {
            return getResponse<Models.Response.PostSessionsResponse>(
                makeRequest("sessions", Method.POST, param)
                );
        }

        /// <summary>
        /// Returns information for one session code. It will return exactly the same data that a POST to /sessions would return except that some of the fields may have changed values.
        /// </summary>
        public Models.Response.PostSessionsResponse GetSession(string sessionCode)
        {
            return getResponse<Models.Response.PostSessionsResponse>(
                makeRequest(string.Format("sessions/{0}", sessionCode))
                );
        }

        /// <summary>
        /// Modifies an existing session code
        /// </summary>
        public bool PutSession(string sessionCode, Models.Request.PostSessionRequest param)
        {
            return restClient.Execute(
                makeRequest(string.Format("sessions/{0}", sessionCode), Method.PUT, param)
                ).StatusCode == HttpStatusCode.NoContent;
        }

        #endregion

        #region 3.7 Connection Reporting

        /// <summary>
        /// Returns a list of connection reports. The list is limited to 1000 reports per request.
        /// If there are more reports the reports_remaining field will tell you how many. 
        /// The next_offset field will contain the offset ID to get the next 1000 (or less) reports and should be used as offset_id parameter for the next request.
        /// If you want to get connections for a single day or multiple days, use the first day at 0:00 for the from_date parameter and the day after the last day at 0:00 for the to_date parameter.
        /// </summary>
        public Models.Response.GetReportConnectionsResponse GetReportConnections(Models.Request.GetReportsConnectionsRequest param)
        {
            return getResponse<Models.Response.GetReportConnectionsResponse>(
                makeRequest("reports/connections")
                );
        }

        /// <summary>
        /// Changes a field in the connection report.
        /// </summary>
        public bool PutReportConnections(Models.Request.PutReportConnectionsRequest param)
        {
            return restClient.Execute(
                makeRequest(string.Format("reports/connections/{0}", param.rID), Method.PUT, param)
                ).StatusCode == HttpStatusCode.NoContent;
        }

        /// <summary>
        /// Deletes a connection report.
        /// </summary>
        public bool DeleteReportConnections(string rID)
        {
            return restClient.Execute(
                makeRequest(string.Format("reports/connections/{0}", rID), Method.DELETE)
                ).StatusCode == HttpStatusCode.OK;
        }

        #endregion

        #endregion;

        #region Helper Methods

        private T getResponse<T>(RestRequest request) where T : new()
        {
            return restClient.Execute<T>(request).Data;
        }

        private RestRequest makeRequest(string resource, Method method = Method.GET, object param = null)
        {
            RestRequest request = new RestRequest(string.Format(Config.ApiResource, resource));
            request.Method = method;

            if (param != null)
            {
                request.AddObject(param);
            }

            if (State.TokenNeedsRefresh())
                getAuthToken(true);
            {
                string ttype = State.Token.token_type;
                if (!string.IsNullOrEmpty(ttype))
                    ttype = char.ToUpper(ttype[0]) + ttype.Substring(1);
                request.AddHeader("Authorization", string.Format("{0} {1}", ttype, State.Token.access_token));
            }

            return request;
        }

        private RestRequest makeUidRequest(string resource, Method method = Method.GET, object param = null)
        {
            if (string.IsNullOrEmpty(State.uID))
                State.uID = GetUsers().users[0].id;
            resource = string.Format("users/{0}/{1}", State.uID, resource);
            return makeRequest(resource, method, param);
        }

        #endregion
    }
}
