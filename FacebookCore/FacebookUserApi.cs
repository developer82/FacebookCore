using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

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
            string path = GetPath(fields);
            var response = FacebookClient.Get(path, _authToken);
            return response;
        }

        public async Task<JObject> RequestInformationAsync(string[] fields = null)
        {
            string path = GetPath(fields);
            var response = await FacebookClient.GetAsync(path, _authToken);
            return response;
        }

        public JObject RequestMetaData()
        {
            var response = FacebookClient.Get($"/{FacebookClient.GraphApiVersion}/me?metadata=1", _authToken);
            return response;
        }

        public async Task<JObject> RequestMetaDataAsync()
        {
            var response = await FacebookClient.GetAsync($"/{FacebookClient.GraphApiVersion}/me?metadata=1", _authToken);
            return response;
        }

        private string GetPath(string[] fields)
        {
            string fieldsStr = string.Empty;
            if (fields != null)
            {
                fieldsStr = string.Join(",", fields);
            }
            string path = $"/me?fields={fieldsStr}";
            return path;
        }
    }
}
