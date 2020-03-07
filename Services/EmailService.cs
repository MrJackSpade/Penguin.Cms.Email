using Penguin.Cms.Email.Abstractions;
using Penguin.Configuration.Abstractions.Interfaces;
using Penguin.DependencyInjection.Abstractions.Attributes;
using Penguin.DependencyInjection.Abstractions.Enums;
using Penguin.Email.Abstractions.Interfaces;
using Penguin.Email.Services;
using Penguin.Persistence.Abstractions.Interfaces;

namespace Penguin.Cms.Email.Services
{
    /// <summary>
    /// A service used to send/queue/persist new email messages
    /// </summary>
    [Register(ServiceLifetime.Scoped, typeof(ISendMail), typeof(IQueueMail), typeof(IQueueAndSendMail))]
    public class EmailService : MailService, IQueueAndSendMail
    {
        /// <summary>
        /// The IRepository implementation to be used when persisting the message
        /// </summary>
        protected IRepository<EmailMessage> EmailRepository { get; set; }

        /// <summary>
        /// Constructs a new instance of this service
        /// </summary>
        /// <param name="emailRepository">The IRepository implementation to be used when persisting the message</param>
        /// <param name="configurationProvider">A Configuration provider to use to retrieve email message configurations</param>
        /// <param name="messageBus">An optonal message bus to be used for publishing email related messages</param>
        public EmailService(IRepository<EmailMessage> emailRepository, IProvideConfigurations configurationProvider) : base(configurationProvider)
        {
            this.EmailRepository = emailRepository;
        }

        /// <summary>
        /// Queues a new email message by adding it to the IRepository implementation with the intent of being sent later by a worker
        /// </summary>
        /// <param name="message">The message to queue</param>
        public void Queue(EmailMessage message)
        {
            this.EmailRepository.AddOrUpdate(message);
        }

        void IQueueMail.Queue(IEmailMessage message)
        {
            this.Queue(this.EnsureEntity(message));
        }

        /// <summary>
        /// Persists the email message to the IRepository implementation and attempts to send it immediately bypassing any queue
        /// </summary>
        /// <param name="message">The message to send/persist</param>
        public void QueueAndSend(EmailMessage message)
        {
            this.EmailRepository.AddOrUpdate(message);

            this.Send(message);
        }

        void IQueueAndSendMail.QueueAndSend(IEmailMessage message)
        {
            this.QueueAndSend(this.EnsureEntity(message));
        }

        /// <summary>
        /// Calls Queue as the default
        /// </summary>
        /// <param name="message">Queues the message</param>
        public void QueueOrSend(EmailMessage message)
        {
            this.Queue(message);
        }

        void IQueueAndSendMail.QueueOrSend(IEmailMessage message)
        {
            this.QueueOrSend(this.EnsureEntity(message));
        }

        /// <summary>
        /// Copies the provided message and requeues it with an optional new recipient
        /// </summary>
        /// <param name="message">The message to copy</param>
        /// <param name="newRecipient">If not null, the value will replace the former recipient</param>
        public void ReQueue(EmailMessage message, string newRecipient = null)
        {
            this.Queue(this.CopyMessage(message, newRecipient));
        }

        void IQueueMail.ReQueue(IEmailMessage message, string newRecipient = null)
        {
            this.ReQueue(this.EnsureEntity(message), newRecipient);
        }

        /// <summary>
        /// Copies the provided message and requeues it with an optional new recipient, marks it sent, and then immediately sends it
        /// </summary>
        /// <param name="message">The message to copy</param>
        /// <param name="newRecipient">If not null, the value will replace the former recipient</param>
        public void ReQueueAndSend(EmailMessage message, string newRecipient = null)
        {
            this.QueueAndSend(this.CopyMessage(message, newRecipient, EmailMessageState.Success));
        }

        void IQueueAndSendMail.ReQueueAndSend(IEmailMessage message, string newRecipient = null)
        {
            this.ReQueueAndSend(this.EnsureEntity(message), newRecipient);
        }

        private EmailMessage CopyMessage(EmailMessage message, string newRecipient = null, EmailMessageState state = EmailMessageState.Unsent)
        {
            EmailMessage toReturn = this.EmailRepository.ShallowClone(message);

            toReturn.State = state;

            if (newRecipient != null)
            {
                toReturn.Recipients = newRecipient;
            }

            return toReturn;
        }

        private EmailMessage EnsureEntity(IEmailMessage source)
        {
            if (source is EmailMessage e)
            {
                return e;
            }
            else
            {
                return new EmailMessage(source);
            }
        }
    }
}