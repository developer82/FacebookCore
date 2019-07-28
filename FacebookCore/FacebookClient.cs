using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using FacebookCore.APIs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rest.Net;
using Rest.Net.Interfaces;
using static FacebookCore.FacebookCursor;

namespace FacebookCore
{
    /// <summary>
    /// FacebookClient is in-charge of the making the actual API calls to the Facebook http API
    /// </summary>
    public class FacebookClient
    {
        private FacebookAppApi _app;
        
        internal string ClientId { get; private set; }

        internal string ClientSecret { get; private set; }

        internal IRestClient RestClient { get; private set; }

        public string GraphApiVersion { get; set; } = "v3.2";

        /// <summary>
        /// Application API
        /// </summary>
        public FacebookAppApi App => _app ?? (_app = new FacebookAppApi(this));
        
        public FacebookClient(string clientId, string clientSecret)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            RestClient = new RestClient("https://graph.facebook.com/");
        }

        /// <summary>
        /// Create a new instance of the Users API
        /// </summary>
        /// <param name="authToken">Authentication token</param>
        /// <returns>New instance for interacting with the users API</returns>
        public FacebookUserApi GetUserApi(string authToken)
        {
            return new FacebookUserApi(this, authToken);
        }

        /// <summary>
        /// Create a new instance of the Places API
        /// </summary>
        /// <param name="authToken">Authentication token</param>
        /// <returns>New instance for interacting with the places API</returns>
        public FacebookPlacesApi GetPlacesApi(string authToken)
        {
            return new FacebookPlacesApi(this, authToken);
        }

        /// <summary>
        /// Makes a request to the Facebook http API
        /// </summary>
        /// <param name="path">API endpoint</param>
        /// <param name="accessToken">Authentication token</param>
        /// <param name="cursor">Cursor of current available data (if exists)</param>
        /// <param name="cursorDirection">What set of results should be taken (after or before the current cursor)</param>
        /// <returns></returns>
        public async Task<JObject> GetAsync(string path, string accessToken = null, FacebookCursor cursor = null, Direction cursorDirection = Direction.None)
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

            string cursorStr = string.Empty;
            if (cursor != null && cursorDirection != Direction.None)
            {
                if ((cursorDirection == Direction.After || cursorDirection == Direction.Next) && !string.IsNullOrWhiteSpace(cursor.After))
                {
                    cursorStr = (path.Contains("?") ? "&" : "?") + "after=" + cursor.After;
                }
                else if (!string.IsNullOrWhiteSpace(cursor.Before))
                {
                    cursorStr = (path.Contains("?") ? "&" : "?") + "before=" + cursor.Before;
                }
            }

            var response = await RestClient.GetAsync($"/{GraphApiVersion}{path}{accessToken}{cursorStr}");
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