using Rest.Net.Interfaces;

namespace FacebookCore.APIs
{
    public abstract class ApiBaseClass
    {
        protected readonly FacebookClient FacebookClient;
        protected readonly IRestClient RestClient;

        protected ApiBaseClass(FacebookClient client)
        {
            FacebookClient = client;
            RestClient = client.RestClient;
        }
    }
}
