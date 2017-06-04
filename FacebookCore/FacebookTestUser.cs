namespace FacebookCore
{
    public class FacebookTestUser
    {
        public string Id { get; private set; }
        public string LoginUrl { get; private set; }
        public string AccessToken { get; private set; }

        public FacebookTestUser(string id, string loginUrl, string accessToken)
        {
            Id = id;
            LoginUrl = loginUrl;
            AccessToken = accessToken;
        }
    }
}
