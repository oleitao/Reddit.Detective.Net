using Neo4j.Driver;
using reddit.detective.net.Model;
using reddit.detective.net.Model.Metadatas;
using reddit.detective.net.Serializer;
using reddit.detective.net.Settings;
using Reddit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reddit.detective.net
{
    public class Neo4JClient : IDisposable
    {
        private readonly IDriver driver;
        private RedditClient _client;

        public Neo4JClient(IConnectionSettings settings, RedditClient client)
        {
            this.driver = GraphDatabase.Driver(settings.Uri, settings.AuthToken);
            this._client = client;
        }

        public RedditClient Client { get => _client; set => _client = value; }

        public async Task ClearDBContent()
        {
            using (var session = driver.AsyncSession())
            {
                await session.RunAsync("MATCH (n) DETACH DELETE n");
            }
        }
        public async Task DropIndexes()
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

        public async Task CreateIndices()
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

        public async Task SearchRedditor(RedditClient api, string name, int limit = 10)
        {
            Reddit.Controllers.User user = api.User(name);
            IList<Reddit.Controllers.Post> redditorPosts = user.PostHistory;
            IList<Reddit.Controllers.Comment> redditorComments = user.CommentHistory;
            
            IList<Post> posts = (from n in redditorPosts select new Post(n.Id, n.Title)).ToList<Post>();
            IList<Comment> comments = (from n in redditorComments select new Comment(n.Id, n.Subreddit)).ToList<Comment>();

            RedditorMeta redditor = new RedditorMeta(user, posts, comments);
            var relationships = ToIList<RedditorMeta>(new List<RedditorMeta>()
            {
                new RedditorMeta(redditor.Redditor, redditor.Posts, redditor.Comments)
            });

            await MappingRedditorMeta(redditor);
            await CreateRedditorPostsRelationships(relationships);
        }

        public async Task CreateRedditorPostsRelationships(IList<RedditorMeta> metadatas)
        {
            string cypher = new StringBuilder()
                .AppendLine("UNWIND {metadatas} AS metadata")
                 // Find the Movie:
                 .AppendLine("MATCH (m:Redditor { name: metadata.redditor.name })")
                // Create Post Relationships:
                .AppendLine("UNWIND metadata.posts AS post")
                .AppendLine("MATCH (a:Post { title: post.title })")
                .AppendLine("MERGE (a)-[r:POSTED_IN]->(m)")
                // Create Comment Relationship:
                .AppendLine("WITH metadata, m")
                .AppendLine("MATCH (d:Comment { name: metadata.comments.name })")
                .AppendLine("MERGE (d)-[r:COMMENTED]->(m)")
                .ToString();

            using (var session = driver.AsyncSession())
            {
                await session.RunAsync(cypher, new Dictionary<string, object>() { { "metadatas", ParameterSerializer.ToDictionary(metadatas) } });
            }
        }

        public async Task MappingRedditors(IList<Redditor> redditors)
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

        public async Task MappingPosts(IList<Post> posts)
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

        public async Task MappingComments(IList<Comment> comments)
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

        public async Task MappingRedditorMeta(RedditorMeta redditorMeta)
        {
            var r = ToIList<Redditor>(new List<Redditor>() 
            { 
                new Redditor((new Random().Next().ToString()), redditorMeta.Redditor.Name)
            });

            await MappingRedditors(r);
            await MappingPosts(redditorMeta.Posts);
            await MappingComments(redditorMeta.Comments);
        }

        IList<T> ToIList<T>(List<T> t)
        {
            return t;
        }

        public void Dispose()
        {
            driver?.Dispose();
        }
    }
}
