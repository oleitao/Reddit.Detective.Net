using Newtonsoft.Json;
using System;

namespace Reddit.Detective.Net.Model
{
    public class Comment
    {
        [JsonProperty("id")]
        public string Id { get { return new Random().Next().ToString(); } }

        [JsonProperty("name")]
        public string Name { get; set; }

        public Comment(string _name)
        {
            this.Name = _name;
        }
    }
}
