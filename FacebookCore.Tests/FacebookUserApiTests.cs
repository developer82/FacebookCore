using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading.Tasks;

namespace FacebookCore.Tests
{
    [TestClass]
    public class FacebookUserApiTests
    {
        private readonly FacebookClient _client;
        private readonly string _accessToken;

        public FacebookUserApiTests()
        {
            string basePath = Directory.GetCurrentDirectory();

            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);

            IConfigurationRoot configurationRoot = builder.Build();

            var clientId = configurationRoot["client_id"];
            var clientSecret = configurationRoot["client_secret"];

            _accessToken = configurationRoot["access_token"];
            _client = new FacebookClient(clientId, clientSecret);
        }

        [TestMethod]
        public async Task ShouldExtendUserToken()
        {
            var userApi = _client.GetUserApi(_accessToken);

            var extendResult = await userApi.RequestExtendAccessToken();

            extendResult["access_token"].ToString().Should().NotBeNullOrWhiteSpace();
        }

        [TestMethod]
        public async Task ShouldGetUserInfo()
        {
            var userApi = _client.GetUserApi(_accessToken);

            var extendResult = await userApi.RequestInformationAsync();

            extendResult["id"].ToString().Should().NotBeNullOrWhiteSpace();
        }

        [TestMethod]
        public async Task ShouldGetUserMetaInfo()
        {
            var userApi = _client.GetUserApi(_accessToken);

            var metadata = await userApi.RequestMetaDataAsync();

            metadata["metadata"].ToString().Should().NotBeNullOrWhiteSpace();
        }
    }
}
