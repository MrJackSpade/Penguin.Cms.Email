using Penguin.Cms.Files;

namespace Penguin.Cms.Email.Extensions
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    public static class EmailMessageExtensions
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        /// <summary>
        /// Adds a file as an attachment to the message, persists it as a database file
        /// </summary>
        /// <param name="source">The target email message</param>
        /// <param name="filePath">The path of the file to add</param>
        /// <param name="fileName">The name to give the attachment</param>
        public static void AddAttachment(this EmailMessage source, string filePath, string fileName = "")
        {
            if (source is null)
            {
                throw new System.ArgumentNullException(nameof(source));
            }

            source.Attachments.Add(new DatabaseFile(filePath, fileName));
        }

        /// <summary>
        /// Adds a file as an attachment to the message, persists it as a database file
        /// </summary>
        /// <param name="source">The target email message</param>
        /// <param name="bytes">A byte[] representing the file contents</param>
        /// <param name="fileName">The name to give the attachment</param>
        public static void AddAttachment(this EmailMessage source, byte[] bytes, string fileName)
        {
            if (source is null)
            {
                throw new System.ArgumentNullException(nameof(source));
            }

            source.Attachments.Add(new DatabaseFile(bytes, fileName));
        }

        /// <summary>
        /// Adds an existing database file as an attachment to the message
        /// </summary>
        /// <param name="source">The target email message</param>
        /// <param name="db">The database file to add as an attachment</param>
        public static void AddAttachment(this EmailMessage source, DatabaseFile db)
        {
            if (source is null)
            {
                throw new System.ArgumentNullException(nameof(source));
            }

            source.Attachments.Add(db);
        }
    }
}