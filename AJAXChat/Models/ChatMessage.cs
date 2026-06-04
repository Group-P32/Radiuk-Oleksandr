using System;

namespace AJAXChat.Models
{
    public class ChatMessage
    {
        // Автор повідомлення, якщо null - автор сервер
        public ChatUser User;
        // Час відправленого повідомлення
        public DateTime Date = DateTime.Now;
        // Текст повідомлення
        public string Text = "";
    }
}
