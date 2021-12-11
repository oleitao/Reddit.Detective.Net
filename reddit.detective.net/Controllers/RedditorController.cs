using Neo4j.Driver;
using Reddit.Detective.Net.Model;
using Reddit.Detective.Net.Model.Metadatas;
using Reddit.Detective.Net.Serializer;
using Reddit.Detective.Net.Services;
using Reddit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reddit.Inputs.Subreddits;

namespace Reddit.Detective.Net.Controllers
{
    public static class RedditorController
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

        public static async Task SearchRedditor(RedditClient api, IDriver driver, string [] userNames, IRedditDataService service, int limit = 10)
        {
            RedditorMeta redditor = null;
            foreach (var userName in userNames)
            {
                Reddit.Controllers.User user = api.User(userName);
                Redditor redditorUser = new Redditor(user.Name);

                IList<Reddit.Controllers.Post> redditorPosts = user.PostHistory;
                IList<Reddit.Controllers.Comment> redditorComments = user.CommentHistory;

                IList<Post> posts = (from n in redditorPosts select new Post(n.Title)).ToArray<Post>();
                IList<Comment> comments = (from n in redditorComments select new Comment(n.Subreddit)).ToArray<Comment>();

                redditor = new RedditorMeta(redditorUser, posts, comments);
                redditor.Redditor = redditorUser;
                redditor.Posts = posts;
                redditor.Comments = comments;

                service.Metadatas.Add(redditor);

                await MappingRedditorMeta(service, driver);
                await CreateRedditorRelationships(service.Metadatas, driver);
            }
        }

        public static async Task SearchRedditorCommonSubreddits(RedditClient api, IDriver driver, string[] userNames, string subredditSearchName, IRedditDataService service, int limit = 10)
        {
            RedditorMeta redditor = null;
            foreach (var userName in userNames)
            {
                Reddit.Controllers.User user = api.User(userName);
                Redditor redditorUser = new Redditor(user.Name);

                var relatedSubreddits = api.Models.Subreddits.SearchSubreddits(new SubredditsSearchNamesInput(subredditSearchName));

                Things.PostContainer postsHistory = api.Models.Users.PostHistory(userName, "submitted", new Reddit.Inputs.Users.UsersHistoryInput(context: limit));
                Things.CommentContainer commentsHistory = api.Models.Users.CommentHistory(userName, "comments", new Reddit.Inputs.Users.UsersHistoryInput(context: limit));

                var subredditsUnion = (from x in postsHistory.Data.Children select x.Data.Subreddit).Union((from y in commentsHistory.Data.Children select y.Data.Subreddit)).ToList();
                IList<Subreddit> subreddits = (from n in subredditsUnion select new Subreddit(n)).ToArray<Subreddit>();
                redditor = new RedditorMeta(redditorUser, subreddits);

                service.Metadatas.Add(redditor);

                await MappingRedditorMeta(service, driver);                
            }

            await CreateRedditorRelationships(service.Metadatas, driver);
        }

        public static async Task CreateRedditorRelationships(IList<RedditorMeta> metadatas, IDriver driver)
        {
            StringBuilder cypher = new StringBuilder();

            if (metadatas.FirstOrDefault() is RedditorMeta metadata)
            {
                cypher.AppendLine("UNWIND {metadatas} AS metadata");

                if (metadata.Redditor != null)
                {
                    // Find the Redditor:
                    cypher.AppendLine("MATCH (m:Redditor { name: metadata.redditor.name })");
                }

                if (metadata.Posts != null && metadata.Posts.Count > 0)
                {
                    // Create Post Relationships:
                    cypher.AppendLine("WITH metadata, m");
                    cypher.AppendLine("UNWIND metadata.posts AS post");
                    cypher.AppendLine("MATCH (p:Post { title: post.title })");
                    cypher.AppendLine("MERGE (m)-[r:POSTED_IN]->(p)");
                }

                if (metadata.Comments != null && metadata.Comments.Count > 0)
                {
                    // Create Comment Relationship:
                    cypher.AppendLine("WITH metadata, m");
                    cypher.AppendLine("UNWIND metadata.comments AS comment");
                    cypher.AppendLine("MATCH (c:Comment { name: comment.name })");
                    cypher.AppendLine("MERGE (m)-[r:COMMENTED]->(c)");
                }

                if (metadata.Subreddits != null && metadata.Subreddits.Count > 0)
                {
                    //Create Subreddit Relationship:
                    cypher.AppendLine("WITH metadata, m");
                    cypher.AppendLine("UNWIND metadata.subreddits AS subreddit");
                    cypher.AppendLine("MATCH (s:Subreddit { name: subreddit.name })");
                    cypher.AppendLine("MERGE (m)-[r:LINKED]->(s)");
                }
            }

            using (var session = driver.AsyncSession())
            {
                await session.RunAsync(cypher.ToString(), new Dictionary<string, object>() { { "metadatas", ParameterSerializer.ToDictionary(metadatas) } });
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

        public static async Task MappingPosts(IList<Post> posts, IDriver driver)
        {
            string cypher = new StringBuilder()
                .AppendLine("UNWIND {posts} AS post")
                .AppendLine("MERGE (p:Post {title: post.title})")
                .AppendLine("SET p = post")
                .ToString();

            using (var session = driver.AsyncSession())
            {
                await session.RunAsync(cypher, new Dictionary<string, object>() { { "posts", ParameterSerializer.ToDictionary(posts) } });
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

        public static async Task MappingSubreddits(IList<Subreddit> subreddits, IDriver driver)
        {
            string cypher = new StringBuilder()
                .AppendLine("UNWIND {subreddits} AS subreddit")
                .AppendLine("MERGE (p:Subreddit {name: subreddit.name})")
                .AppendLine("SET p = subreddit")
                .ToString();

            using (var session = driver.AsyncSession())
            {
                await session.RunAsync(cypher, new Dictionary<string, object>() { { "subreddits", ParameterSerializer.ToDictionary(subreddits) } });
            }
        }

        public static async Task MappingRedditorMeta(IRedditDataService service, IDriver driver)
        {
            foreach (var redditorMeta in service.Metadatas.ToList())
            {
                var redditors = ToIList<Redditor>(new List<Redditor>()
                {
                    new Redditor(redditorMeta.Redditor.Name)
                });
                
                if(redditors != null && redditors.Count > 0)
                    await MappingRedditors(redditors, driver);

                if(redditorMeta.Posts != null && redditorMeta.Posts.Count > 0)
                    await MappingPosts(redditorMeta.Posts, driver);

                if (redditorMeta.Comments != null && redditorMeta.Comments.Count > 0)
                    await MappingComments(redditorMeta.Comments, driver);

                if (redditorMeta.Subreddits != null && redditorMeta.Subreddits.Count > 0)
                    await MappingSubreddits(redditorMeta.Subreddits, driver);
            }
        }

        static IList<T> ToIList<T>(List<T> t)
        {
            return t;
        }
    }
}
