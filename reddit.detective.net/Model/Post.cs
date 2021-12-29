using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Reddit.Detective.Net.Model
{
    public class Post
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        public Post(string _id, string _title)
        {
            Id = _id;
            Title = _title;
        }
    }
}
