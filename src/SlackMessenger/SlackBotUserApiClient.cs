using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace KNet.SlackMessenger
{
    public class SlackBotUserApiClient
    {
        private readonly string _botApiToken;

        public SlackBotUserApiClient(string botUserApiToken, string baseAddress)
        {
            if (string.IsNullOrWhiteSpace(botUserApiToken))
                throw new ArgumentNullException(nameof(botUserApiToken));

            if (string.IsNullOrWhiteSpace(baseAddress))
                throw new ArgumentNullException(nameof(baseAddress));

            this._botApiToken = botUserApiToken;

            InitHttpClient(baseAddress);
        }

        public HttpClient Client { get; private set; }

        public string BotApiToken => _botApiToken;


        private void InitHttpClient(string baseAddress)
        {
            this.Client = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };

            this.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.BotApiToken);

            this.Client.DefaultRequestHeaders.Accept.Clear();
            this.Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            this.Client.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue("utf-8"));
        }
    }
}
