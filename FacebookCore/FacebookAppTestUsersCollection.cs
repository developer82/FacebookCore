using System.Collections.Generic;
using System.Linq;

namespace FacebookCore
{
    public class FacebookAppTestUsersCollection : List<FacebookTestUser>
    {
        private FacebookClient _fbClient;
        private string _appAccessToken;
        private string _appId;

        public FacebookCursor Cursor { get; internal set; }

        public FacebookAppTestUsersCollection(FacebookClient client)
        {
            _fbClient = client;
            _appAccessToken = _fbClient.App.GetAccessToken();
            _appId = _fbClient.App.GetAppId(_appAccessToken);
            Next();
        }

        public bool Next()
        {
            var response = _fbClient.Get($"/{_appId}/accounts/test-users", _appAccessToken);
            var users = response["data"];

            if (!users.Any())
            {
                return false;
            }

            foreach (var user in users)
            {
                string id = user["id"].ToString();
                string loginUrl = user["login_url"].ToString();
                string accessToken = user["access_token"].ToString();
                Add(new FacebookTestUser(id, loginUrl, accessToken));
            }
            
            var cursor = response["paging"]["cursors"];
            string before = cursor["before"].ToString();
            string after = cursor["after"].ToString();
            Cursor = new FacebookCursor(before, after);

            return true;
        }
    }
}
