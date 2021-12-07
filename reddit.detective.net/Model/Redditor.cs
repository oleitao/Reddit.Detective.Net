using Newtonsoft.Json;
using System;

namespace Reddit.Detective.Net.Model
{
    public class Redditor
    {
        [JsonProperty("id")]
        public string Id { get { return new Random().Next().ToString(); } }

        [JsonProperty("name")]
        public string Name { get; set; }

        public Redditor(string _name)
        {
            this.Name = _name;
        }
    }
}
