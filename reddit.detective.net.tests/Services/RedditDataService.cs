using reddit.detective.net.Model.Metadatas;
using System.Collections.Generic;

namespace reddit.detective.net.tests.Services
{
    public interface IRedditDataService
    {
        IList<Model.Redditor> Redditors { get; }
        IList<Model.Comment> Comments { get; }
        IList<Model.Subreddit> Subreddits { get; }

        IList<RedditorMeta> RedditorMetas { get; }
    }

    public class RedditDataService : IRedditDataService
    {
        private IList<Model.Redditor> redditors;
        private IList<Model.Comment> comments;
        private IList<Model.Subreddit> subreddits;

        private IList<RedditorMeta> redditorMetas;

        public IList<Model.Redditor> Redditors { get => redditors; set => redditors = value; }
        public IList<Model.Comment> Comments { get => comments; set => comments = value; }
        public IList<Model.Subreddit> Subreddits { get => subreddits; set => subreddits = value; }

        public IList<RedditorMeta> RedditorMetas { get => redditorMetas; set => redditorMetas = value; }

    }
}
