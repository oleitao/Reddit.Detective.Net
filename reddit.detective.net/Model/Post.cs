using Newtonsoft.Json;

namespace Reddit.Detective.Net.Model
{
    public class Post
    {
        public Post(string _id, string _title)
        {
            this.Id = _id;
            this.Title = _title;
        }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }
}
