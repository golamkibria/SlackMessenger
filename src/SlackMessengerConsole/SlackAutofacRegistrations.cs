using Autofac;
using System.Configuration;

namespace KNet.SlackMessenger
{
    public static class SlackAutofacRegistrations
    {
        public static void RegisterSlackMessenger(this ContainerBuilder builder)
        {
            builder.RegisterType<SlackBotUserApiClient>()
                     .WithParameter("botUserApiToken", ConfigurationManager.AppSettings["slack.botUserApiToken"])
                     .WithParameter("baseAddress", ConfigurationManager.AppSettings["slack.apiEndpoint"])
                     .SingleInstance();

            builder.RegisterType<SlackMessenger>()
                    .AsImplementedInterfaces();
        }
    }
}
