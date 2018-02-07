using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace KNet.SlackMessenger
{
    public class SlackMessenger : ISlackMessenger, ISlackThreadMessenger
    {
        private readonly SlackBotUserApiClient _apiClient;
        private string _parentThreadTs;

        public SlackMessenger(SlackBotUserApiClient apiClient)
        {
            this._apiClient = apiClient;
        }

        public async Task StartThread(SlackMessage slackThreadMessage)
        {
            slackThreadMessage.ThreadTs = this._parentThreadTs;

            await Send(slackThreadMessage, response =>
            {
                this._parentThreadTs = response.Ts;
            });
        }

        public async Task SendReply(SlackMessage slackThreadMessage)
        {
            slackThreadMessage.ThreadTs = this._parentThreadTs;

            await Send(slackThreadMessage);
        }

        public async Task SendBroadcastReply(SlackMessage slackThreadMessage)
        {
            slackThreadMessage.ThreadTs = this._parentThreadTs;

            slackThreadMessage.ReplyBroadcast = true;

            await Send(slackThreadMessage);
        }

        public async Task Send(SlackMessage slackMessage, Action<PostMessageResponse> callback = null)
        {
            if (slackMessage == null)
                throw new ArgumentNullException(nameof(slackMessage));

            slackMessage.Validate();

            var response = await SendAsQueryString(slackMessage, callback);

            //var response = await SendAsJson(slackMessage, callback);

            var responseContent = await response.Content.ReadAsStringAsync();
            dynamic responseMsg = JsonConvert.DeserializeObject(responseContent);

            if (response.IsSuccessStatusCode && responseMsg.ok == "true")
            {
                var ts = responseMsg.message.ts;

                callback?.Invoke(new PostMessageResponse { Ts = ts });
            }
            else
            {
                throw new ApplicationException($"SlackAPI: Response: {responseMsg.error}");
            }
        }

        protected async Task<HttpResponseMessage> SendAsJson(SlackMessage slackMessage, Action<PostMessageResponse> callback = null)
        {
            var jsonContent = new StringContent(slackMessage.ToClientJson(), Encoding.UTF8, "application/json");

            //var formUrlEncodedContent = new FormUrlEncodedContent(ToKeyValue(slackMessage));

            var requestUri = new Uri("api/chat.postMessage/", UriKind.Relative);

            var response = await this._apiClient.Client.PostAsync(requestUri, jsonContent);

            return response; //response: {"ok":false,"error":"not_authed"}
        }

        protected async Task<HttpResponseMessage> SendAsQueryString(SlackMessage slackMessage, Action<PostMessageResponse> callback = null)
        {
            var queryStringContent = string.Join("&", slackMessage.ToKeyValue().Select(p => $"{p.Key}={Uri.EscapeDataString(p.Value)}"));

            var requestUri = new Uri($"api/chat.postMessage/?token={this._apiClient.BotApiToken}&{queryStringContent}", UriKind.Relative);

            var response = await this._apiClient.Client.PostAsync(requestUri, null);

            return response;
        }

        public void Dispose()
        {
        }
    }
}