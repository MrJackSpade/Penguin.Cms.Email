using Penguin.Cms.Abstractions.Attributes;
using Penguin.Cms.Email.Abstractions;
using Penguin.Cms.Email.Abstractions.Extensions;
using Penguin.Cms.Email.Abstractions.Interfaces;
using Penguin.Cms.Entities;
using Penguin.Cms.Files;
using Penguin.Email.Abstractions.Interfaces;
using Penguin.Files.Abstractions;
using Penguin.Persistence.Abstractions.Attributes.Control;
using Penguin.Persistence.Abstractions.Attributes.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Penguin.Cms.Email
{
    /// <summary>
    /// Provides a database persistable representation of an email object
    /// </summary>
    public class EmailMessage : Entity, IPersistableEmailMessage
    {
        /// <summary>
        /// A list of Database Files attached to this message
        /// </summary>
        [EagerLoad(1)]
        [DontAllow(DisplayContexts.Any)]
        public List<DatabaseFile> Attachments { get; } = new List<DatabaseFile>();

        IEnumerable<IFile> IEmailMessage.Attachments => this.Attachments;

        IEnumerable<string> IEmailMessage.BCCRecipients => this.GetBCCRecipients();

        /// <summary>
        /// A ; delimited list of BCC recipients
        /// </summary>
        [Display(Order = -600)]
        public string BCCRecipients { get; set; } = string.Empty;

        /// <summary>
        /// The HTML body of the email message
        /// </summary>
        [Display(Order = -400)]
        [DisplayType("System.String.Html")]
        [DontAllow(DisplayContexts.List)]
        public virtual string Body { get; set; } = string.Empty;

        IEnumerable<string> IEmailMessage.CCRecipients => this.GetCCRecipients();

        /// <summary>
        /// A ; delimited list of CC recipients
        /// </summary>
        [Display(Order = -700)]
        public string CCRecipients { get; set; } = string.Empty;

        /// <summary>
        /// A string representing the account the email should be sent from, used when gathering connection information. Blank sends from default
        /// </summary>
        [Display(Order = -900)]
        [DontAllow(DisplayContexts.List)]
        public string From { get; set; } = string.Empty;

        /// <summary>
        /// All messages are HTML by default. Set to false to override
        /// </summary>
        public bool IsHtml { get; set; } = true;

        /// <summary>
        /// An internal label for associating emails with the process that created them
        /// </summary>
        public string Label { get; set; } = string.Empty;

        IEnumerable<string> IEmailMessage.Recipients => this.GetRecipients();

        /// <summary>
        /// A ; delimited list of Message recipients
        /// </summary>
        [Display(Order = -800)]
        public string Recipients { get; set; } = string.Empty;

        /// <summary>
        /// The date the email message should be sent on. Defaults to the current time
        /// </summary>
        [DontAllow(DisplayContexts.Edit)]
        public DateTime SendDate { get; set; } = DateTime.Now;

        /// <summary>
        /// The sent state of the message as persisted
        /// </summary>
        public EmailMessageState State { get; set; }

        /// <summary>
        /// The subject to send the message with
        /// </summary>
        [Display(Order = -500)]
        public string Subject { get; set; } = string.Empty;

        public EmailMessage()
        {
        }

        public EmailMessage(IEmailMessage message)
        {
            this.Attachments = message.Attachments.OfType<DatabaseFile>().ToList();

            this.AddBCCRecipients(message.BCCRecipients.ToArray());
            this.AddCCRecipients(message.CCRecipients.ToArray());
            this.AddRecipients(message.Recipients.ToArray());

            this.Body = message.Body;
            this.From = message.From;
            this.IsHtml = message.IsHtml;
            this.Subject = message.Subject;
        }
    }
}