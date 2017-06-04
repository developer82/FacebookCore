using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rest.Net;
using Rest.Net.Interfaces;

namespace FacebookCore
{
    public class FacebookClient
    {
        private FacebookAppApi _app;
        
        internal string ClientId { get; private set; }

        internal string ClientSecret { get; private set; }

        internal IRestClient RestClient { get; private set; }

        public const string GraphApiVersion = "v2.8";

        public FacebookAppApi App => _app ?? (_app = new FacebookAppApi(this));
        
        public FacebookClient(string clientId, string clientSecret)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            RestClient = new RestClient("https://graph.facebook.com/");
        }

        public FacebookUserApi CreateUserApi(string authToken)
        {
            FacebookUserApi userApi = new FacebookUserApi(this, authToken);
            return userApi;
        }
        
        public JObject Get(string path, string accessToken = null)
        {
            if (!path.StartsWith("/"))
            {
                path = "/" + path;
            }

            if (accessToken == null)
            {
                accessToken = string.Empty;
            }
            else
            {
                accessToken = (path.Contains("?") ? "&" : "?") + "access_token=" + accessToken;
            }

            var response = RestClient.Get($"/{GraphApiVersion}{path}{accessToken}");
            var serializedResponse = SerializeResponse(response);
            return serializedResponse;
        }

        internal JObject SerializeResponse(IRestResponse<string> response)
        {
            try
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var jsreader = new JsonTextReader(new StringReader(response.RawData.ToString()));
                    var json = (JObject)new JsonSerializer().Deserialize(jsreader);
                    return json;
                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}