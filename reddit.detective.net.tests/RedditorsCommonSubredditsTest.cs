using Reddit.Detective.Net.Services;
using Reddit.Detective.Net.Settings;
using Reddit.Detective.Net.tests.Mapping;
using Reddit.Detective.Net.Tests;
using Xunit;

namespace Reddit.Detective.Net.tests
{
    public class RedditorsCommonSubredditsTest
    {
        [Fact]
        public void SearchRedditorsCommonSubreddits()
        {
            var service = new RedditDataService();
            var settings = ConnectionSettings.CreateBasicAuth(Credentials.uri, Credentials.neo4jUserName, Credentials.neo4jPassword);
            var api = new RedditClient(Credentials.appId, Credentials.refreshToken, Credentials.appSecret, Credentials.accessToken, Credentials.userAgent);
            string [] redditorName = { "pmol87", "desculpe_mas" };
            string subredditSearchName = "portugal";

            RedditDataMapping.GetRedditorCommonSubreddits(settings, api, service, redditorName, subredditSearchName).GetAwaiter().GetResult();
        }
    }
}
