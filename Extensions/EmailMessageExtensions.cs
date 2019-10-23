using Penguin.Cms.Files;

namespace Penguin.Cms.Email.Extensions
{
    public static class EmailMessageExtensions
    {
        /// <summary>
        /// Adds a file as an attachment to the message, persists it as a database file
        /// </summary>
        /// <param name="filePath">The path of the file to add</param>
        /// <param name="fileName">The name to give the attachment</param>
        public static void AddAttachment(this EmailMessage source, string filePath, string fileName = "") => source.Attachments.Add(new DatabaseFile(filePath, fileName));

        /// <summary>
        /// Adds a file as an attachment to the message, persists it as a database file
        /// </summary>
        /// <param name="bytes">A byte[] representing the file contents</param>
        /// <param name="fileName">The name to give the attachment</param>
        public static void AddAttachment(this EmailMessage source, byte[] bytes, string fileName) => source.Attachments.Add(new DatabaseFile(bytes, fileName));

        /// <summary>
        /// Adds an existing database file as an attachment to the message
        /// </summary>
        /// <param name="db">The database file to add as an attachment</param>
        public static void AddAttachment(this EmailMessage source, DatabaseFile db) => source.Attachments.Add(db);
    }
}