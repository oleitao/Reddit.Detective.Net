using Newtonsoft.Json;

namespace reddit.detective.net.Model
{
    public class Redditor
    {
        private string id;
        private string name;
        
        [JsonProperty("id")]
        public string Id { get => id; set => id = value; }

        [JsonProperty("name")]
        public string Name { get => name; set => name = value; }

        public Redditor(string _id, string _name)
        {
            this.id = _id;
            this.name = _name;
        }
    }
}
