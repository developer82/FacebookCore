namespace FacebookCore
{
    public interface FacebookConfig
    {
        string ClientId { get; set; }
        string ClientSecret { get; set; }
        string GraphApiVersion { get; set; }
    }
}
