using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace KNet.SlackMessenger
{
    public static class JsonHelper
    {
        private static readonly JsonSerializerSettings ClientJsonSettings = CreateClientJsonSettings();
        private static readonly JsonSerializerSettings ClientJsonWithIgnoreNullAndDefaultValueSettings = CreateClientJsonWithIgnoreNullAndDefaultValueSettings();

        public static string ToClientJson(this object instance)
        {
            return instance == null
                       ? null
                       : JsonConvert.SerializeObject(instance, ClientJsonSettings);
        }

        public static string ToClientJson(this object instance, bool ignoreNullAndDefaultValueProperties)
        {
            return instance == null
                       ? null
                       : JsonConvert.SerializeObject(instance, ignoreNullAndDefaultValueProperties ? ClientJsonWithIgnoreNullAndDefaultValueSettings : ClientJsonSettings);
        }

        private static JsonSerializerSettings CreateClientJsonSettings()
        {
            var settings = new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            return settings;
        }


        private static JsonSerializerSettings CreateClientJsonWithIgnoreNullAndDefaultValueSettings()
        {
            var settings = CreateClientJsonSettings();

            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.DefaultValueHandling = DefaultValueHandling.Ignore;

            return settings;
        }
    }
}
