using Neo4j.Driver;
using Reddit.Detective.Net.Settings;
using Reddit;
using System;
using System.Threading.Tasks;
using Reddit.Detective.Net.Serializer;
using Reddit.Detective.Net.Model;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Reddit.Detective.Net.Model.Metadatas;

namespace Reddit.Detective.Net
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
