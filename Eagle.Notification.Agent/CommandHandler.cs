using Eagle.Infrastructure.Messaging;
using MassTransit;

namespace Eagle.Notification.Agent
{
    public abstract class CommandHandler<T> : ICommandHandler<T>
        where T : MessageBase, ICommand
    {
        protected IServiceBusPublisherBase ServiceBusPublisherBase;

        protected CommandHandler()
        {
        }

        protected CommandHandler(IServiceBusPublisherBase serviceBusPublisherBase)
        {
            ServiceBusPublisherBase = serviceBusPublisherBase;
        }

        ///// <summary>
        ///// MassTransit will receive message and call command handler
        ///// </summary>
        ///// <param name="message"></param>
        //public void Consume(IConsumeContext<T> message)
        //{
        //    Handle(message.Message);
        //}

        /// <summary>
        /// Child class must override this method for respective command handler
        /// </summary>
        /// <param name="command"></param>
        public virtual void Handle(T command)
        {
            //Busines logic is in inherited class
        }

        public void Retry(ServiceBusQueueType serviceBusQueue, T command)
        {
            ServiceBusPublisherBase.PublishAddRetryMessage(serviceBusQueue, command);
        }

        //protected ClaimsPrincipal GetSystemAdminClaim()
        //{
        //    return new ClaimsPrincipal
        //        (
        //            new[]
        //            {
        //                new ClaimsIdentity
        //                (
        //                    new List<Claim>
        //                    {
        //                        //setup fake MemberId and NetworkId to bypass code
        //                        new Claim(SherpaClaims.MemberId, SherpaClaims.SystemAdminMemberId),
        //                        new Claim(SherpaClaims.NetworkId, SherpaClaims.SystemAdminNetworkId),
        //                        new Claim(SherpaClaims.IsAdmin, SherpaClaims.True)
        //                    }
        //                )
        //        });
        //}
    }
}
