using Neo4j.Driver;

namespace reddit.detective.net.Settings
{
    public interface IConnectionSettings
    {
        string Uri { get; }

        IAuthToken AuthToken { get; }

        string AppId { get; set; }
        string RefreshToken { get; set; }
        string AppSecret { get; set; }
        string AccessToken { get; set; }
        string UserAgent { get; set; }
    }
}
