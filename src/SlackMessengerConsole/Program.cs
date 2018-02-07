using Autofac;
using System;
using System.Threading.Tasks;
using KNet.SlackMessenger;

namespace KNet.SlackMessengerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //Set 'slack.botUserApiToken' and 'slack.apiEndpoint' values in App.config

                var container = CreateContainer();

                TestThreadMessenger(container).Wait();
            }
            catch (AggregateException ex)
            {
                Console.WriteLine(ex.Flatten().Message);
                Console.WriteLine(ex.Flatten().StackTrace);
            }
        }

        private static async Task TestThreadMessenger(IContainer container)
        {
            var slackMessanger = container.Resolve<ISlackThreadMessenger>();

            var channel = "friends";

            await slackMessanger.StartThread(SlackMessageHelper.CreateMessage(channel, "Start Thread", SlackMessageHelper.GetDefaultAttatchments()));

            for (var i = 1; i <= 5; i++)
            {
                await slackMessanger.SendReply(SlackMessageHelper.CreateMessage(channel, $"Reply: {i}", SlackMessageHelper.GetDefaultAttatchments()));

                await Task.Delay(5 * 1000);
            }

            await slackMessanger.SendBroadcastReply(SlackMessageHelper.CreateMessage(channel, $"Reply: Done!", SlackMessageHelper.GetDefaultAttatchments()));
        }

        private static IContainer CreateContainer()
        {
            Console.WriteLine("Initializing Container...");

            var builder = new ContainerBuilder();

            builder.RegisterSlackMessenger();

            var container = builder.Build();

            Console.WriteLine("Build Container Completed.");
            return container;
        }
    }
}
