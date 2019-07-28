using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading.Tasks;
using FacebookCore.APIs;
using FluentAssertions;
using FacebookCore.Collections;

namespace FacebookCore.Tests
{
    [TestClass]
    public class FacebookCoreTest
    {
        private readonly FacebookClient _client;

        public FacebookCoreTest()
        {
            string basePath = Directory.GetCurrentDirectory();

            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);

            IConfigurationRoot configurationRoot = builder.Build();

            var clientId = configurationRoot["client_id"];
            var clientSecret = configurationRoot["client_secret"];

            _client = new FacebookClient(clientId, clientSecret);
        }

        [TestMethod]
        public async Task ShouldGetApplicationId()
        {
            string appId = await _client.App.GetAppIdAsync();
            appId.Should().NotBeNullOrEmpty();
        }

        [TestMethod]
        public async Task ShouldBeAbleToGetAccessToken()
        {
            string token = await _client.App.GetAccessTokenAsync();
            token.Should().NotBeNullOrEmpty();
        }

        [TestMethod]
        public async Task ShouldBeAbleToGetTestUsersList()
        {
            AppTestUsersCollection users = await _client.App.GetTestUsersAsync();
            users.Should().NotBeNull();
            users.Count.Should().BeGreaterThan(0);
        }

        [TestMethod]
        public async Task ShouldBeAbleToGetPlaces()
        {
            string token = await _client.App.GetAccessTokenAsync();
            FacebookPlacesApi placesApi = _client.GetPlacesApi(token);
            PlacesCollection places = await placesApi.PlacesSearchAsync("-73.9921", "40.7304");

            places.Should().NotBeNull();
            places.Count.Should().Be(50,
                "Default limitation is 50 and we're querying a popular place so we should get such result.");

            //PlacesCollection placesBefore = places.After();
            //placesApi.PlacesSearch("-73.9921", "40.7304", 1000, null, null, null, 0);
        }

        [TestMethod]
        public async Task ShouldBeAbleToLoadMorePlaces()
        {
            string token = await _client.App.GetAccessTokenAsync();
            FacebookPlacesApi placesApi = _client.GetPlacesApi(token);
            PlacesCollection places = await placesApi.PlacesSearchAsync("-73.9921", "40.7304");
            bool loadMoreResult = await places.Load();

            places.Should().NotBeNull();
            loadMoreResult.Should().Be(true, "The load more operation should be successful");
            places.Count.Should().Be(100, "It loaded more results");
        }

        [TestMethod]
        public async Task ShouldBeAbleToGetPlacesAfterAndBefore()
        {
            string token = await _client.App.GetAccessTokenAsync();
            FacebookPlacesApi placesApi = _client.GetPlacesApi(token);
            PlacesCollection places = await placesApi.PlacesSearchAsync("-73.9921", "40.7304");

            PlacesCollection after = await places.AfterAsync();
            PlacesCollection before = await after.BeforeAsync();

            places.Should().NotBeNull();
            after.Should().NotBeNull();
            before.Should().NotBeNull();

            places.Count.Should().Be(50);
            after.Count.Should().Be(50);
            before.Count.Should().Be(50);

            before[10].Id.Should().Be(places[10].Id, "Before is taken from after and therefor should match the original `places` requester");
        }
    }
}
