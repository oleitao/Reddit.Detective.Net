using Newtonsoft.Json;

namespace Reddit.Detective.Net.Model
{
    public class Comment
    {
        public Comment(string _id, string _name)
        {
            this.Id = _id;
            this.Name = _name;
        }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
