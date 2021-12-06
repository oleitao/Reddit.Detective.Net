using Newtonsoft.Json;
using Reddit.Controllers;
using System.Collections.Generic;

namespace reddit.detective.net.Model.Metadatas
{
    public class RedditorMeta
    {
        private Redditor redditor;
        private IList<Post> posts;
        private IList<Comment> comments;

        [JsonProperty("redditor")]
        public Redditor Redditor { get => redditor; set => redditor = value; }
        
        [JsonProperty("posts")]
        public IList<Post> Posts { get => posts; set => posts = value; }

        [JsonProperty("comments")]
        public IList<Comment> Comments { get => comments; set => comments = value; }

        public RedditorMeta(Redditor _redditor, IList<Post> _posts, IList<Comment> _comments)
        {
            this.posts = _posts;
            this.redditor = _redditor;
            this.comments = _comments;
        }
    }
}
