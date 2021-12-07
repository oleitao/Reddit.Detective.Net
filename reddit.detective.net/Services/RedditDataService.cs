using Reddit.Detective.Net.Model.Metadatas;
using System.Collections.Generic;

namespace Reddit.Detective.Net.Services
{
    public interface IRedditDataService
    {
        IList<RedditorMeta> Metadatas { get; set; }
    }

    public class RedditDataService : IRedditDataService
    {
        private IList<RedditorMeta> metadatas;

        public RedditDataService()
        {
            this.metadatas = new List<RedditorMeta>();
        }

        public IList<RedditorMeta> Metadatas { get => metadatas; set => metadatas = value; }
    }
}
