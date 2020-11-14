using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using WebService_Lib.Attributes;

namespace WebService.Components
{
    [Component]
    public class MessageHandler
    {
        private ConcurrentDictionary<string, string> messages = new ConcurrentDictionary<string, string>();
        private int counter = 0;

        public string AddMessage(string message)
        {
            // Generate  id
            var id = Interlocked.Increment(ref counter).ToString();
            messages[id] = message;
            return id;
        }
        
        public void UpdateMessage(string id, string message)
        {
            messages[id] = message;
        }

        public void DeleteMessage(string id)
        {
            while (true)
            {
                var deleted = messages.TryRemove(id, out _);
                if (deleted) break;
                Thread.Sleep(100);
            }
        }

        public string GetMessage(string id)
        {
            return messages.ContainsKey(id) ? messages[id] : "";
        }

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