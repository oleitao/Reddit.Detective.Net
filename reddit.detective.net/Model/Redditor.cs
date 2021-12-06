using Newtonsoft.Json;
using System;

namespace reddit.detective.net.Model
{
    public class Redditor
    {
        private string name;

        [JsonProperty("id")]
        public string Id { get { return new Random().Next().ToString(); } }

        [JsonProperty("name")]
        public string Name { get => name; set => name = value; }

        public Redditor(string _name)
        {
            this.name = _name;
        }
    }
}
