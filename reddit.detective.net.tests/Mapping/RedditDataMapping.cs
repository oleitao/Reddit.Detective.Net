using reddit.detective.net.Settings;
using reddit.detective.net.tests.Services;
using Reddit;
using System.Threading.Tasks;

namespace reddit.detective.net.tests.Mapping
{
    public static class RedditDataMapping
    {
        #region Methods

        public static async Task GetRedditor(ConnectionSettings settings, RedditClient api, IRedditDataService service, string redditorName)
        {
            using (var client = new Neo4JClient(settings, api))
            {
                await client.ClearDBContent();
                //await client.DropIndexes();
                await client.CreateIndices();
                await client.SearchRedditor(api, redditorName);
            }
        }

        #endregion
    }
}
