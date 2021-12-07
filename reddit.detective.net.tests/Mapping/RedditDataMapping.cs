using Reddit.Detective.Net.Controllers;
using Reddit.Detective.Net.Services;
using Reddit.Detective.Net.Settings;
using System.Threading.Tasks;

namespace Reddit.Detective.Net.tests.Mapping
{
    public static class RedditDataMapping
    {
        #region Methods

        public static async Task GetRedditor(ConnectionSettings settings, RedditClient api, IRedditDataService service, string redditorName)
        {
            using (var client = new Neo4JClient(settings, api))
            {
                await RedditorController.ResetContent(client.Driver);
                await RedditorController.DropIndexes(client.Driver);
                await RedditorController.CreateIndices(client.Driver);
                await RedditorController.SearchRedditor(api, client.Driver, redditorName, service);
            }
        }

        public static async Task GetRedditorComments(ConnectionSettings settings, RedditClient api, IRedditDataService service, string redditorName)
        {
            using (var client = new Neo4JClient(settings, api))
            {
                await RedditorCommentsController.ResetContent(client.Driver);
                await RedditorCommentsController.DropIndexes(client.Driver);
                await RedditorCommentsController.CreateIndices(client.Driver);
                await RedditorCommentsController.SearchRedditorComments(api, client.Driver, redditorName, service);
            }
        }

        #endregion
    }
}
