using Reddit.Detective.Net.Controllers;
using Reddit.Detective.Net.Services;
using Reddit.Detective.Net.Settings;
using System.Threading.Tasks;

namespace Reddit.Detective.Net.tests.Mapping
{
    public static class RedditDataMapping
    {
        #region Methods

        public static async Task RedditorsPostsWithComments(ConnectionSettings settings, RedditClient api, IRedditDataService service, string [] redditorName)
        {
            using (var client = new Neo4JClient(settings, api))
            {
                await RedditorController.ResetContent(client.Driver);
                //await RedditorController.DropIndexes(client.Driver);
                await RedditorController.CreateIndices(client.Driver);
                await RedditorController.RedditorsPostsWithComments(api, client.Driver, redditorName, service);
            }
        }

        public static async Task GetRedditorComments(ConnectionSettings settings, RedditClient api, IRedditDataService service, string redditorName)
        {
            using (var client = new Neo4JClient(settings, api))
            {
                await RedditorCommentsController.ResetContent(client.Driver);
                //await RedditorCommentsController.DropIndexes(client.Driver);
                await RedditorCommentsController.CreateIndices(client.Driver);
                await RedditorCommentsController.SearchRedditorComments(api, client.Driver, redditorName, service);
            }
        }

        public static async Task GetRedditorCommonSubreddits(ConnectionSettings settings, RedditClient api, IRedditDataService service, string[] redditorName, string subredditSearchName)
        {
            using (var client = new Neo4JClient(settings, api))
            {
                await RedditorController.ResetContent(client.Driver);
                //await RedditorController.DropIndexes(client.Driver);
                await RedditorController.CreateIndices(client.Driver);
                await RedditorController.SearchRedditorCommonSubreddits(api, client.Driver, redditorName, subredditSearchName, service);
            }
        }

        #endregion
    }
}
