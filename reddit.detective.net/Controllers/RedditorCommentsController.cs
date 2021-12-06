using Neo4j.Driver;
using reddit.detective.net.Model;
using reddit.detective.net.Model.Metadatas;
using reddit.detective.net.Serializer;
using reddit.detective.net.Services;
using Reddit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reddit.detective.net.Controllers
{
    public static class RedditorCommentsController
    {
        public static async Task ResetContent(IDriver driver)
        {
            using (var session = driver.AsyncSession())
            {
                await session.RunAsync("MATCH (n) DETACH DELETE n");
            }
        }

        public static async Task DropIndexes(IDriver driver)
        {
            string[] queries = {
                "DROP INDEX ON :Subreddit (id)",
                "DROP INDEX ON :Subreddit (name)",
                "DROP INDEX ON :Redditor (id)",
                "DROP INDEX ON :Redditor (name)",
                "DROP INDEX ON :Comment (id)",
                "DROP INDEX ON :Comment (name)",
                "DROP INDEX ON :Post (id)",
                "DROP INDEX ON :Post (title)"
            };

            using (var session = driver.AsyncSession())
            {
                foreach (var query in queries)
                {
                    await session.RunAsync(query);
                }
            }
        }

        public static async Task CreateIndices(IDriver driver)
        {
            string[] queries = {
                "CREATE INDEX ON :Subreddit (id)",
                "CREATE INDEX ON :Subreddit (name)",
                "CREATE INDEX ON :Redditor (id)",
                "CREATE INDEX ON :Redditor (name)",
                "CREATE INDEX ON :Comment (id)",
                "CREATE INDEX ON :Comment (name)",
                "CREATE INDEX ON :Post (id)",
                "CREATE INDEX ON :Post (title)"
            };

            using (var session = driver.AsyncSession())
            {
                foreach (var query in queries)
                {
                    await session.RunAsync(query);
                }
            }
        }

        public static async Task SearchRedditorComments(RedditClient api, IDriver driver, string name, IRedditDataService service, int limit = 10)
        {
            Reddit.Controllers.User user = api.User(name);
            Redditor redditorUser = new Redditor(user.Name);

            IList<Reddit.Controllers.Post> redditorPosts = user.PostHistory;
            IList<Reddit.Controllers.Comment> redditorComments = user.CommentHistory;


            IList<Post> posts = (from n in redditorPosts select new Post(n.Id, n.Title)).ToList<Post>();
            IList<Comment> comments = (from n in redditorComments select new Comment(n.Id, n.Subreddit)).ToList<Comment>();

            RedditorMeta redditor = new RedditorMeta(redditorUser, posts, comments);
            redditor.Redditor = redditorUser;
            redditor.Comments = comments;

            service.Metadatas.Add(redditor);

            await MappingRedditorMeta(redditor, driver);
            await CreateRedditorCommentsRelationships(service.Metadatas, driver);
        }

        public static async Task CreateRedditorCommentsRelationships(IList<RedditorMeta> metadatas, IDriver driver)
        {
            string cypher = new StringBuilder()
                .AppendLine("UNWIND {metadatas} AS metadata")
                 .AppendLine("MATCH (m:Redditor { name: metadata.redditor.name })")
                // Create Comment Relationship:
                .AppendLine("WITH metadata, m")
                .AppendLine("UNWIND metadata.comments AS comment")
                .AppendLine("MATCH (c:Comment { name: comment.name })")
                .AppendLine("MERGE (m)-[r:COMMENTED]->(c)")
                .ToString();

            using (var session = driver.AsyncSession())
            {
                await session.RunAsync(cypher, new Dictionary<string, object>() { { "metadatas", ParameterSerializer.ToDictionary(metadatas) } });
            }
        }

        public static async Task MappingRedditors(IList<Redditor> redditors, IDriver driver)
        {
            string cypher = new StringBuilder()
                .AppendLine("UNWIND {redditors} AS redditor")
                .AppendLine("MERGE (p:Redditor {name: redditor.name})")
                .AppendLine("SET p = redditor")
                .ToString();

            using (var session = driver.AsyncSession())
            {
                await session.RunAsync(cypher, new Dictionary<string, object>() { { "redditors", ParameterSerializer.ToDictionary(redditors) } });
            }
        }

        public static async Task MappingComments(IList<Comment> comments, IDriver driver)
        {
            string cypher = new StringBuilder()
                .AppendLine("UNWIND {comments} AS comment")
                .AppendLine("MERGE (p:Comment {name: comment.name})")
                .AppendLine("SET p = comment")
                .ToString();

            using (var session = driver.AsyncSession())
            {
                await session.RunAsync(cypher, new Dictionary<string, object>() { { "comments", ParameterSerializer.ToDictionary(comments) } });
            }
        }

        public static async Task MappingRedditorMeta(RedditorMeta redditorMeta, IDriver driver)
        {
            var r = ToIList<Redditor>(new List<Redditor>()
            {
                new Redditor(redditorMeta.Redditor.Name)
            });

            await MappingRedditors(r, driver);
            await MappingComments(redditorMeta.Comments, driver);
        }

        static IList<T> ToIList<T>(List<T> t)
        {
            return t;
        }
    }
}
