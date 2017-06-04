using Newtonsoft.Json.Linq;

namespace FacebookCore
{
    public class FacebookUserApi : ApiBaseClass
    {
        private readonly string _authToken;

        public FacebookUserApi(FacebookClient client, string authToken) : base(client)
        {
            _authToken = authToken;
        }
        
        public JObject RequestInformation(string[] fields = null)
        {
            string fieldsStr = string.Empty;
            if (fields != null)
            {
                fieldsStr = string.Join(",", fields);
            }
            var response = FacebookClient.Get($"/me?fields={fieldsStr}", _authToken);
            return response;
        }

        public JObject RequestMetaData()
        {
            var response = FacebookClient.Get($"/{FacebookClient.GraphApiVersion}/me?metadata=1", _authToken);
            return response;
        }
    }
}
