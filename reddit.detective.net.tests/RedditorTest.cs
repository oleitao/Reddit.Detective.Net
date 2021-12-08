using Reddit.Detective.Net.Services;
using Reddit.Detective.Net.Settings;
using Reddit.Detective.Net.tests.Mapping;
using Reddit.Detective.Net.Tests;
using Xunit;

namespace Reddit.Detective.Net.tests
{
    public class RedditorTest
    {
        [Fact]
        public void SearchRedditor()
        {
            var service = new RedditDataService();
            var settings = ConnectionSettings.CreateBasicAuth(Credentials.uri, Credentials.neo4jUserName, Credentials.neo4jPassword);
            var api = new RedditClient(Credentials.appId, Credentials.refreshToken, Credentials.appSecret, Credentials.accessToken, Credentials.userAgent);
            string [] redditorName = { "pmol87", "PortugalLivre" };

            RedditDataMapping.GetRedditor(settings, api, service, redditorName).GetAwaiter().GetResult();
        }
    }
}
