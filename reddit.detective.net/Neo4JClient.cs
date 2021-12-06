using Neo4j.Driver;
using reddit.detective.net.Settings;
using Reddit;
using System;
using System.Threading.Tasks;
using reddit.detective.net.Serializer;
using reddit.detective.net.Model;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using reddit.detective.net.Model.Metadatas;

namespace reddit.detective.net
{
    public class Neo4JClient : IDisposable
    {
        private IDriver _driver;
        private RedditClient _client;

        public Neo4JClient(IConnectionSettings settings, RedditClient client)
        {
            this._driver = GraphDatabase.Driver(settings.Uri, settings.AuthToken);
            this._client = client;
        }

        public IDriver Driver { get => _driver; set => _driver = value; }
        public RedditClient Client { get => _client; set => _client = value; }

        public void Dispose()
        {
            _driver?.Dispose();
        }
    }
}
