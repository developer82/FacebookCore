using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace FacebookCore.APIs
{
    /// <summary>
    /// Facebook APIs for requesting user information
    /// </summary>
    public class FacebookUserApi : ApiBaseClass
    {
        private readonly string _authToken;

        /// <summary>
        /// Facebook APIs for requesting user information
        /// </summary>
        /// <param name="client">Facebook client instance</param>
        /// <param name="authToken">User auth token</param>
        public FacebookUserApi(FacebookClient client, string authToken) : base(client)
        {
            _authToken = authToken;
        }

        /// <summary>
        /// Request specific user information
        /// </summary>
        /// <param name="fields">Requested fields</param>
        /// <returns>JObject with user information</returns>
        public async Task<JObject> RequestInformationAsync(string[] fields = null)
        {
            string fieldsStr = string.Empty;
            if (fields != null)
            {
                fieldsStr = string.Join(",", fields);
            }
            var response = await FacebookClient.GetAsync($"/me?fields={fieldsStr}", _authToken);
            return response;
        }

        /// <summary>
        /// User meta-data
        /// </summary>
        /// <returns>JObject with user information</returns>
        public async Task<JObject> RequestMetaDataAsync()
        {
            var response = await FacebookClient.GetAsync($"/{FacebookClient.GraphApiVersion}/me?metadata=1", _authToken);
            return response;
        }
    }
}
