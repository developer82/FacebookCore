using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacebookCore.Collections
{
    public class FacebookCollection<T> : List<T>
    {
        private FacebookClient _fbClient;
        private string _query, _token;

        internal Func<JToken, T> Mapper { get; set; }

        public FacebookCursor Cursor { get; internal set; }

        public FacebookCollection(FacebookClient client, string query, string token, Func<JToken, T> mapper, FacebookCursor cursor = null)
        {
            _fbClient = client;
            _query = query;
            _token = token;
            Mapper = mapper;
        }

        public async Task<bool> Load()
        {
            return await LoadToCollectionAsync();
        }

        public virtual async Task<FacebookCollection<T>> BeforeAsync()
        {
            return await CreateNewCollectionAsync(FacebookCursor.Direction.Before);
        }

        public async Task<FacebookCollection<T>> AfterAsync()
        {
            return await CreateNewCollectionAsync(FacebookCursor.Direction.After);
        }
        
        private async Task<FacebookCollection<T>> CreateNewCollectionAsync(FacebookCursor.Direction direction)
        {
            Type collectionType = GetType();
            FacebookCollection<T> collection = (FacebookCollection<T>)Activator.CreateInstance(collectionType, _fbClient, _query, _token, Cursor);
            await collection.LoadToCollectionAsync(Cursor, direction);
            return collection;
        }

        private async Task<bool> LoadToCollectionAsync(FacebookCursor cursor = null, FacebookCursor.Direction direction = FacebookCursor.Direction.None)
        {
            try
            {
                var response = await _fbClient.GetAsync(_query, _token, cursor, direction);
                var results = response["data"];

                if (!results.Any())
                {
                    return false;
                }

                foreach (var result in results)
                {
                    T mappedResult = Mapper(result);
                    Add(mappedResult);
                }

                var newCursor = response["paging"]["cursors"];
                string before = newCursor["before"]?.ToString();
                string after = newCursor["after"]?.ToString();
                Cursor = new FacebookCursor(before, after);

                return true;
            }
            catch (Exception ex)
            {
                throw new FacebookCollectionException($"There was a problem loading additional items to Facebook collection {typeof(T)}", ex);
            }
        }
    }
}
