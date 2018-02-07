using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace KNet.SlackMessenger
{
    public class SlackMessage
    {
        [JsonProperty("channel")]
        public string Channel { get; set; }

        [JsonProperty("username"), JsonIgnore]
        public string Username { get; set; }


        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("attachments")]
        public IEnumerable<IDictionary<string, object>> Attachments { get; set; }

        [JsonProperty("thread_ts"),]
        public string ThreadTs { get; set; }

        [JsonProperty("reply_broadcast")]
        public bool ReplyBroadcast { get; set; }


        [JsonProperty("as_user")]
        public bool AsUser { get; set; }

        public void Validate()
        {
            if(string.IsNullOrEmpty(this.Text) && (this.Attachments == null || !this.Attachments.Any()))
                throw new ApplicationException("SlackMessage requires either Text or Attachments");

            if(string.IsNullOrWhiteSpace(this.Channel))
                throw new ApplicationException("SlackMessage requires a channel");
        }
    }

    public class PostMessageResponse
    {
        public string Ts { get; set; }
    }

}