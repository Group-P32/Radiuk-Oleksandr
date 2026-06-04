using System;
using System.Collections.Generic;

namespace AJAXChat.Models
{
    public class ChatModel
    {
        public List<ChatUser> Users;       // Всі користувачі чату
        public List<ChatMessage> Messages; // Повідомлення від користувачів

        public ChatModel()
        {
            Users = new List<ChatUser>();
            Messages = new List<ChatMessage>();

            Messages.Add(new ChatMessage()
            {
                Text = "Чат розпочато " + DateTime.Now
            });
        }
    }
}
