using Newtonsoft.Json;

namespace reddit.detective.net.Model
{
    public class Post
    {
        private string id;
        private string title;

        public Post(string _id, string _title)
        {
            this.id = _id;
            this.title = _title;
        }

        [JsonProperty("id")]
        public string Id { get => id; set => id = value; }

        [JsonProperty("title")]
        public string Title { get => title; set => title = value; }
    }
}
