namespace FacebookCore.Entities
{
    public class Place
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Checkins { get; set; }
        public Picture Picture { get; set; }
    }
}
