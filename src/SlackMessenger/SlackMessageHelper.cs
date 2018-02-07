using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace KNet.SlackMessenger
{
    public static class SlackMessageHelper
    {
        public static IDictionary<string, string> ToKeyValue(this SlackMessage slackMessage)
        {
            if (slackMessage == null)
            {
                return null;
            }

            var token = JObject.FromObject(slackMessage);
            if (token == null)
            {
                return null;
            }

            var contentData = new Dictionary<string, string>();
            foreach (var property in token.Properties())
            {
                var value = property.Value.Type != JTokenType.Object ? property.Value.ToString() : property.Value.ToClientJson();
                contentData.Add(property.Name, value);
            }

            return contentData;
        }

        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long ConvertToUnixTime(DateTime datetime)
        {
            return (long)(datetime - Epoch).TotalSeconds;
        }

        public static IEnumerable<IDictionary<string, object>> GetDefaultAttatchments()
        {
            return new[]
            {
                new Dictionary<string, object>
                {
                    {"color", "#36a64f"},
                    {
                        "fields", new[]
                        {
                            CreateAttachmentField("MachineName", Environment.MachineName)
                        }
                    },
                    {"ts", ConvertToUnixTime(DateTime.UtcNow)},
                    {"footer", "Slack API"},
                }
            };
        }

        public static object CreateAttachmentField(string title, string value, bool @short = true)
        {
            return new { title, value, @short };
        }

        public static SlackMessage CreateMessage(string channel,
                                                 string messageText,
                                                 IEnumerable<IDictionary<string, object>> attachments)
        {
            var slackMessage = new SlackMessage
            {
                Channel = channel,

                Text = messageText,
                Attachments = attachments
            };

            return slackMessage;
        }
    }
}