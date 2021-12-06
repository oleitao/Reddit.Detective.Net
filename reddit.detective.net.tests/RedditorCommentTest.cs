using reddit.detective.net.Services;
using reddit.detective.net.Settings;
using reddit.detective.net.tests.Mapping;
using Reddit;
using Xunit;

namespace reddit.detective.net.tests
{
    public class RedditorCommentTest
    {
        [Fact]
        public void SearchRedditorComments()
        {
            var service = new RedditDataService();
            var settings = ConnectionSettings.CreateBasicAuth(Credentials.uri, Credentials.neo4jUserName, Credentials.neo4jPassword);
            var api = new RedditClient(Credentials.appId, Credentials.refreshToken, Credentials.appSecret, Credentials.accessToken, Credentials.userAgent);            
            string redditorName = "PortugalLivre";

            RedditDataMapping.GetRedditorComments(settings, api, service, redditorName).GetAwaiter().GetResult();
        }
    }
}
