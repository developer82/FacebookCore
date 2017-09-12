using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rest.Net;
using Rest.Net.Interfaces;
using System.Threading.Tasks;

namespace FacebookCore
{
    public class FacebookAppApi : ApiBaseClass
    {
        public FacebookAppApi(FacebookClient client) : base(client)
        {
        }

        public string GetAccessToken()
        {
            try
            {
                IRestRequest request = CreateTokenRequest();
                IRestResponse<string> result = FacebookClient.RestClient.Execute(request);

                string accessToken = ExtractToken(result);
                return accessToken;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string> GetAccessTokenAsync()
        {
            try
            {
                IRestRequest request = CreateTokenRequest();
                IRestResponse<string> result = await FacebookClient.RestClient.ExecuteAsync(request);

                string accessToken = ExtractToken(result);
                return accessToken;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string GetAppId()
        {
            string accessToken = GetAccessToken();
            return GetAppId(accessToken);
        }

        public string GetAppId(string accessToken)
        {
            try
            {
                string appId = accessToken.Split(new char[] { '|' })[0];
                return appId;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public FacebookAppTestUsersCollection GetTestUsers()
        {
            FacebookAppTestUsersCollection testUsers = new FacebookAppTestUsersCollection(FacebookClient);
            return testUsers;
        }

        private IRestRequest CreateTokenRequest()
        {
            IRestRequest request = new RestRequest("/oauth/access_token", Http.Method.GET);
            request.AddParameter("client_id", FacebookClient.ClientId);
            request.AddParameter("client_secret", FacebookClient.ClientSecret);
            request.AddParameter("grant_type", "client_credentials");
            return request;
        }

        private string ExtractToken(IRestResponse<string> response)
        {
            var jsreader = new JsonTextReader(new StringReader(response.RawData.ToString()));
            var json = (JObject)new JsonSerializer().Deserialize(jsreader);

            string accessToken = json["access_token"].ToString(); //result.RawData.ToString().Replace("access_token=", string.Empty);
            return accessToken;
        }
    }
}
