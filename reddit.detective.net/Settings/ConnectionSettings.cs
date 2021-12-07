using Neo4j.Driver;

namespace Reddit.Detective.Net.Settings
{
    public class ConnectionSettings : IConnectionSettings
    {
        public string Uri { get; private set; }

        public IAuthToken AuthToken { get; private set; }

        public string AppId { get; set; }
        public string RefreshToken { get; set; }
        public string AppSecret { get; set; }
        public string AccessToken { get; set; }
        public string UserAgent { get; set; }

        public ConnectionSettings(string uri, IAuthToken authToken)
        {
            Uri = uri;
            AuthToken = authToken;
        }

        public static ConnectionSettings CreateBasicAuth(string uri, string username, string password)
        {
            return new ConnectionSettings(uri, AuthTokens.Basic(username, password));
        }
    }
}