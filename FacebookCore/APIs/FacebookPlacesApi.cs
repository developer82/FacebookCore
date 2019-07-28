using System.Threading.Tasks;
using FacebookCore.Collections;

namespace FacebookCore.APIs
{
    /// <summary>
    /// Facebook places API class allows making calls to query the Facebook places API and retrieving and searching content related to locations and checkins
    /// </summary>
    public class FacebookPlacesApi : ApiBaseClass
    {
        private readonly FacebookClient _fbClient;
        private readonly string _authToken;

        /// <summary>
        /// Facebook places API class allows making calls to query the Facebook places API and retrieving and searching content related to locations and checkins
        /// </summary>
        /// <param name="client">Facebook client instance</param>
        /// <param name="authToken">Application auth token for making the call</param>
        public FacebookPlacesApi(FacebookClient client, string authToken) : base(client)
        {
            _fbClient = client;
            _authToken = authToken;
        }

        /// <summary>
        /// Search for places
        /// </summary>
        /// <param name="lon">Longitude</param>
        /// <param name="lat">Latitude</param>
        /// <returns>Collection of places</returns>
        public async Task<PlacesCollection> PlacesSearchAsync(string lon, string lat)
        {
            return await PlacesSearchAsync(lon, lat, 100);
        }

        /// <summary>
        /// Search for places
        /// </summary>
        /// <param name="lon">Longitude</param>
        /// <param name="lat">Latitude</param>
        /// <param name="distance">Distance in meters from the center</param>
        /// <returns>Collection of places</returns>
        public async Task<PlacesCollection> PlacesSearchAsync(string lon, string lat, int distance)
        {
            return await PlacesSearchAsync(lon, lat, distance, string.Empty);
        }

        /// <summary>
        /// Search for places by name
        /// </summary>
        /// <param name="name">Name of place</param>
        /// <returns>Collection of places that match the given name</returns>
        public async Task<PlacesCollection> PlacesSearchAsync(string name)
        {
            return await PlacesSearchAsync(string.Empty, string.Empty, 100, string.Empty, "name,checkins,picture", name, 50);
        }

        /// <summary>
        /// Search for places
        /// </summary>
        /// <param name="lon">Longitude</param>
        /// <param name="lat">Latitude</param>
        /// <param name="distance">Distance in meters from the center</param>
        /// <param name="categories">Places that match one or more categories (consult Facebook API for available options)</param>
        /// <returns></returns>
        public async Task<PlacesCollection> PlacesSearchAsync(string lon, string lat, int distance, string categories)
        {
            return await PlacesSearchAsync(lon, lat, distance, categories, "name,checkins,picture");
        }

        /// <summary>
        /// Search for places
        /// </summary>
        /// <param name="lon">Longitude</param>
        /// <param name="lat">Latitude</param>
        /// <param name="distance">Distance in meters from the center</param>
        /// <param name="categories">Places that match one or more categories (consult Facebook API for available options)</param>
        /// <param name="fields">Place information fields you want returned (consult Facebook API for available options)</param>
        /// <returns></returns>
        public async Task<PlacesCollection> PlacesSearchAsync(string lon, string lat, int distance, string categories, string fields)
        {
            return await PlacesSearchAsync(lon, lat, distance, categories, fields, string.Empty, 50);
        }

        /// <summary>
        /// Search for places
        /// </summary>
        /// <param name="lon">Longitude</param>
        /// <param name="lat">Latitude</param>
        /// <param name="distance">Distance in meters from the center</param>
        /// <param name="categories">Places that match one or more categories (consult Facebook API for available options)</param>
        /// <param name="fields">Place information fields you want returned (consult Facebook API for available options)</param>
        /// <param name="name">Name of place</param>
        /// <param name="limit">Limit the number of returned results</param>
        /// <returns></returns>
        public async Task<PlacesCollection> PlacesSearchAsync(string lon, string lat, int distance, string categories, string fields, string name, int limit)
        {
            //https://graph.facebook.com/v3.2/search?type=place&center=40.7304,-73.9921&distance=1000&q=cafe&fields=name,checkins,picture&limit=3&access_token={access-token}'

            string center = string.Empty;
            if (!string.IsNullOrWhiteSpace(lon) && !string.IsNullOrWhiteSpace(lat))
            {
                center = "&center=" + lat + "," + lon;
            }

            string distanceStr = string.Empty;
            if (distance > 0)
            {
                distanceStr = "&distance=" + distance.ToString();
            }

            string query = string.Empty;
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = "&q=" + name;
            }

            if (string.IsNullOrWhiteSpace(fields))
            {
                fields = "name,checkins,picture";
            }
            fields = "&fields=" + fields;

            string limitStr = string.Empty;
            if (limit > 0)
            {
                limitStr = "&limit=" + limit.ToString();
            }

            //var response = FacebookClient.Get($"/search?type=place{center}{distance}{query}{fields}{limit}", _authToken);
            string apiQuery = $"/search?type=place{center}{distance}{query}{fields}{limitStr}";

            PlacesCollection collection = new PlacesCollection(_fbClient, apiQuery, _authToken, null);
            await collection.Load();

            return collection;
        }
    }
}
