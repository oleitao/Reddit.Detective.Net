using Newtonsoft.Json;
using System;

namespace reddit.detective.net.Model
{
    public class Subreddit
    {
        [JsonProperty("id")]
        public string Id { get { return new Random().Next().ToString(); } }


        private string name;

        [JsonProperty("name")]
        public string Name { get => name; set => name = value; }

        public Subreddit(string name)
        {
            Name = name;
        }
    }
}
