using Newtonsoft.Json;
using System;

namespace Reddit.Detective.Net.Model
{
    public class Subreddit
    {
        [JsonProperty("id")]
        public string Id { get { return new Random().Next().ToString(); } }

        [JsonProperty("name")]
        public string Name { get; set; }

        public Subreddit(string name)
        {
            Name = name;
        }
    }
}
