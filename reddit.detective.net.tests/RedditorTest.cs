using reddit.detective.net.Settings;
using reddit.detective.net.tests.Mapping;
using reddit.detective.net.tests.Services;
using Reddit;
using Xunit;

namespace reddit.detective.net.tests
{
    public class RedditorTest
    {
        [Fact]
        public void SearchRedditorPosts()
        {
            var service = new RedditDataService();
            var settings = ConnectionSettings.CreateBasicAuth(Credentials.uri, Credentials.neo4jUserName, Credentials.neo4jPassword);
            var api = new RedditClient(Credentials.appId, Credentials.refreshToken, Credentials.appSecret, Credentials.accessToken, Credentials.userAgent);
            string redditorName = "pmol87";

            RedditDataMapping.GetRedditor(settings, api, service, redditorName).GetAwaiter().GetResult();
        }
    }
}
