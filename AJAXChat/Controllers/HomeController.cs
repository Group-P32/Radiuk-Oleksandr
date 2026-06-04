using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AJAXChat.Models;

namespace AJAXChat.Controllers
{
    public class HomeController : Controller
    {
        static ChatModel chatModel;

        public ActionResult Index(string user, bool? logOn, bool? logOff, string chatMessage)
        {
            try
            {
                if (chatModel == null)
                    chatModel = new ChatModel();

                // Залишаємо тільки останні 10 повідомлень (не більше 100)
                if (chatModel.Messages.Count > 100)
                    chatModel.Messages.RemoveRange(0, 90);

                // Якщо звичайний запит — повертаємо повне представлення
                if (!Request.IsAjaxRequest())
                {
                    return View(chatModel);
                }
                // Якщо переданий параметр logOn
                else if (logOn != null && (bool)logOn)
                {
                    // Перевіряємо унікальність імені користувача
                    if (chatModel.Users.FirstOrDefault(u => u.Name == user) != null)
                    {
                        throw new Exception("Такий користувач вже існує");
                    }
                    else if (chatModel.Users.Count > 10)
                    {
                        throw new Exception("Чат заповнений");
                    }
                    else
                    {
                        // Додаємо нового користувача
                        chatModel.Users.Add(new ChatUser()
                        {
                            Name = user,
                            LoginTime = DateTime.Now,
                            LastPing = DateTime.Now
                        });

                        // Системне повідомлення про вхід
                        chatModel.Messages.Add(new ChatMessage()
                        {
                            Text = user + " увійшов у чат",
                            Date = DateTime.Now
                        });
                    }

                    return PartialView("ChatRoom", chatModel);
                }
                // Якщо переданий параметр logOff
                else if (logOff != null && (bool)logOff)
                {
                    LogOff(chatModel.Users.FirstOrDefault(u => u.Name == user));
                    return PartialView("ChatRoom", chatModel);
                }
                else
                {
                    ChatUser currentUser = chatModel.Users.FirstOrDefault(u => u.Name == user);

                    if (currentUser == null)
                    {
                        throw new Exception("Сесія завершена. Оновіть сторінку.");
                    }

                    // Оновлюємо час останнього пінгу
                    currentUser.LastPing = DateTime.Now;

                    // Видаляємо неактивних користувачів (більше 15 секунд без пінгу)
                    List<ChatUser> toRemove = new List<ChatUser>();
                    foreach (ChatUser usr in chatModel.Users)
                    {
                        TimeSpan span = DateTime.Now - usr.LastPing;
                        if (span.TotalSeconds > 15)
                            toRemove.Add(usr);
                    }
                    foreach (ChatUser u in toRemove)
                    {
                        LogOff(u);
                    }

                    // Додаємо нове повідомлення від користувача
                    if (!string.IsNullOrEmpty(chatMessage))
                    {
                        chatModel.Messages.Add(new ChatMessage()
                        {
                            User = currentUser,
                            Text = chatMessage,
                            Date = DateTime.Now
                        });
                    }

                    return PartialView("History", chatModel);
                }
            }
            catch (Exception ex)
            {
                // При помилці повертаємо статус 500
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        // При виході користувача — видаляємо зі списку і додаємо системне повідомлення
        public void LogOff(ChatUser user)
        {
            if (user == null) return;
            chatModel.Users.Remove(user);
            chatModel.Messages.Add(new ChatMessage()
            {
                Text = user.Name + " покинув чат.",
                Date = DateTime.Now
            });
        }
    }
}
