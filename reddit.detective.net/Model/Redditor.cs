using Newtonsoft.Json;
using System;

namespace Reddit.Detective.Net.Model
{
    public class Redditor
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        public Redditor(string _name)
        {
            this.Name = _name;
        }

        public Redditor(string _id, string _name)
        {
            this.Id = Id;
            this.Name = _name;
        }
    }
}
