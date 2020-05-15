using System;
using System.IO;
using System.Threading.Tasks;
using FacebookCore.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rest.Net;
using Rest.Net.Interfaces;

namespace FacebookCore.APIs
{
    /// <summary>
    /// Facebook App API allow for making calls to API that retrieve/modify information for the general Facebook application.
    /// Note that the access token required for these type of calls is client_credentials for the app itself.
    /// </summary>
    public class FacebookAppApi : ApiBaseClass
    {
        private string _appId, _appAccessToken;

        /// <summary>
        /// Facebook App API allow for making calls to API that retrieve/modify information for the general Facebook application.
        /// Note that the access token required for these type of calls is client_credentials for the app itself.
        /// </summary>
        /// <param name="client">Facebook client instance</param>
        public FacebookAppApi(FacebookClient client) : base(client)
        {
            _appId = client.ClientId;
        }

        /// <summary>
        /// Get an access token for authenticating with your Facebook app.
        /// </summary>
        /// <returns>Access token string</returns>
        public async Task<string> GetAccessTokenAsync()
        {
            if (!string.IsNullOrWhiteSpace(_appAccessToken))
            {
                return _appAccessToken;
            }

            try
            {
                IRestRequest request = new RestRequest("/oauth/access_token", Http.Method.GET);
                request.AddParameter("client_id", FacebookClient.ClientId);
                request.AddParameter("client_secret", FacebookClient.ClientSecret);
                request.AddParameter("grant_type", "client_credentials");
                IRestResponse<string> result = await FacebookClient.RestClient.ExecuteAsync(request);

                var jsreader = new JsonTextReader(new StringReader(result.RawData.ToString()));
                var json = (JObject)new JsonSerializer().Deserialize(jsreader);

                _appAccessToken = json["access_token"].ToString();
                return _appAccessToken;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Get your Facebook application id without a token (token will be retrieved for you using your Facebook client id and secret)
        /// </summary>
        /// <returns>Application id string</returns>
        public async Task<string> GetAppIdAsync()
        {
            if (!string.IsNullOrWhiteSpace(_appId))
            {
                return _appId;
            }

            string accessToken = await GetAccessTokenAsync();
            return GetAppId(accessToken);
        }

        /// <summary>
        /// Get your Facebook application id
        /// </summary>
        /// <param name="accessToken">Your app token</param>
        /// <returns>Application id string</returns>
        public string GetAppId(string accessToken)
        {
            if (!string.IsNullOrWhiteSpace(_appId))
            {
                return _appId;
            }

            try
            {
                string appId = accessToken.Split(new char[] { '|' })[0];
                return appId;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Get your application defined test users
        /// </summary>
        /// <returns>Application test users collection</returns>
        public async Task<AppTestUsersCollection> GetTestUsersAsync()
        {
            string accessToken = await GetAccessTokenAsync();
            string appId = await GetAppIdAsync();
            string query = $"/{appId}/accounts/test-users";

            AppTestUsersCollection testUsers = new AppTestUsersCollection(FacebookClient, query, accessToken);
            await testUsers.Load();
            return testUsers;
        }
    }
}
