using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using WebService_Lib.Attributes;

namespace WebService.Components
{
    /// <summary>
    /// <c>Component</c> class that handles message management & storage.
    /// </summary>
    [Component]
    public class MessageHandler
    {
        private readonly ConcurrentDictionary<string, string> messages = new ConcurrentDictionary<string, string>();
        private int counter = 0;

        /// <summary>
        /// Adds a new message.
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Id of new message</returns>
        public string AddMessage(string message)
        {
            // Generate  id
            var id = Interlocked.Increment(ref counter).ToString();
            messages[id] = message;
            return id;
        }
        
        /// <summary>
        /// Updates a message.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="message"></param>
        public void UpdateMessage(string id, string message)
        {
            if (messages.ContainsKey(id)) messages[id] = message;
        }

        /// <summary>
        /// Deletes a message.
        /// </summary>
        /// <param name="id"></param>
        public void DeleteMessage(string id)
        {
            if (!messages.ContainsKey(id)) return;
            while (true)
            {
                var deleted = messages.TryRemove(id, out _);
                if (deleted) break;
                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// Returns a message from a specific id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// If the id exists, the message will be returned.
        /// Else an empty string will be returned.
        /// </returns>
        public string GetMessage(string id)
        {
            return messages.ContainsKey(id) ? messages[id] : "";
        }

        /// <summary>
        /// Returns all messages.
        /// </summary>
        /// <returns>
        /// Returns all messages as one string.
        /// Is empty when no messages are found.
        /// </returns>
        public string GetAllMessages()
        {
            var allMessages = new StringBuilder();
            foreach (KeyValuePair<string, string> message in messages)
            {
                allMessages.AppendLine($"Message {message.Key}:");
                allMessages.AppendLine(message.Value);
                allMessages.AppendLine();
            }

            return allMessages.ToString();
        }
    }
}