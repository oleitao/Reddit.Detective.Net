using Newtonsoft.Json;
using System;

namespace Reddit.Detective.Net.Model
{
    public class Comment
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("post_id")]
        public string PostId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        public Comment(string _id, string _name)
        {
            this.Id = _id;
            this.Name = _name;
        }

        public Comment(string _id, string _name, string _body, string _postid)
        {
            this.Id = _id;
            this.Name = _name;
            this.Body = _body;
            this.PostId = _postid;
        }
    }
}
