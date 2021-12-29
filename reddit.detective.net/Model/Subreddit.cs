using Newtonsoft.Json;
using System;

namespace Reddit.Detective.Net.Model
{
    public class Subreddit
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        public Subreddit(string _name)
        {
            Name = _name;
        }
        public Subreddit(string _id, string _name)
        {
            Id = _id;
            Name = _name;
        }
    }
}
