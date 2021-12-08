using Newtonsoft.Json;
using System;

namespace Reddit.Detective.Net.Model
{
    public class Post
    {
        [JsonProperty("id")]
        public string Id { get { return new Random().Next().ToString(); } }

        [JsonProperty("title")]
        public string Title { get; set; }

        public Post(string _title)
        {
            this.Title = _title;
        }
    }
}
