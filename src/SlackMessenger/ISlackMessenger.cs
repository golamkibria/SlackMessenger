using System;
using System.Threading.Tasks;

namespace KNet.SlackMessenger
{
    public interface ISlackThreadMessenger : IDisposable
    {
        Task StartThread(SlackMessage slackThreadMessage);

        Task SendReply(SlackMessage slackThreadMessage);

        Task SendBroadcastReply(SlackMessage slackThreadMessage);
    }

    public interface ISlackMessenger
    {
        Task Send(SlackMessage slackMessage, Action<PostMessageResponse> callback = null);
    }
}