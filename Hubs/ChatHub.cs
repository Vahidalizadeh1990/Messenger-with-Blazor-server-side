using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using PrivateMessenger.Areas.Identity.Data;
using PrivateMessenger.Data;
using PrivateMessenger.Models.Interface;

namespace PrivateMessenger.Hubs
{
    /// <summary>
    /// This section is the most important part of this application
    /// After we've added our hub to the project, we should create a new hub to manage all the data that comming from any users
    /// </summary>
    public class ChatHub : Hub
    {
        private readonly AppDbContext db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ConnectionInterface connectionInterface;
        private readonly ChatMessageInterface _chatMessageInterface;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ChatHub(ChatMessageInterface chatMessageInterface, AppDbContext db, UserManager<IdentityUser> userManager, ConnectionInterface connectionInterface, IHttpContextAccessor httpContextAccessor)
        {
            this.db = db;
            _userManager = userManager;
            _chatMessageInterface = chatMessageInterface;
            this.connectionInterface = connectionInterface;
            this.httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// This section is a part of hub that is used for send a new message to specific user.
        /// It creates a real time chat system which both users (SenderUser, RecieverUser) can see the message at the same time.
        /// We use connection id to send a message to a specific users
        /// The connection id is stored in our database
        /// Each time that a user wants a new chat, we add connection id for that user
        /// we check which one of those users are online. For instance,if sender user is online and reciever user is offline we update 
        /// connection id of sender and etc.
        /// </summary>
        /// <param name="SenderUser"></param>
        /// <param name="message"></param>
        /// <param name="RecieverUser"></param>
        /// <returns></returns>
        public async Task SendMessage(string SenderUser, string message, string RecieverUser)
        {
            var SenderInfo = await _userManager.FindByIdAsync(SenderUser);
            var RecieverInfo = db.Users.Find(RecieverUser);
            Connection connection = new Connection();
            ChatMessage chatMessage = new ChatMessage();
            var connectionId = Context.ConnectionId;
            var seen =false;
            var existConnection_SenderWay = connectionInterface.GetBySenderAndRecieverId(SenderUser, RecieverUser);
            var existConnection_Reciever = connectionInterface.GetBySenderAndRecieverId(RecieverUser, SenderUser);

            // we use this section to update connection if both way senderUserId and recieverUserId were opposite of null
            if (existConnection_SenderWay != null)
            {
                // check if a reciever userId was equal with From user, it will updated values which related with receiver UserId, for instance
                // RecieverUserId, RecieverConnected, etc.

                if(existConnection_SenderWay.SenderUserConnected==true && existConnection_SenderWay.RecieverUserConnected==false)
                {
                    seen = false;
                    chatMessage.FromUserId = SenderInfo.Id;
                    chatMessage.ToUserId = RecieverInfo.Id;
                    chatMessage.Message = message;
                    chatMessage.Date = DateTime.Now;
                    chatMessage.Seen = false;
                    await _chatMessageInterface.SaveMessage(chatMessage);
                }
                else if (existConnection_SenderWay.SenderUserConnected == true && existConnection_SenderWay.RecieverUserConnected == true)
                {
                    seen = true;
                    chatMessage.FromUserId = SenderInfo.Id;
                    chatMessage.ToUserId = RecieverInfo.Id;
                    chatMessage.Message = message;
                    chatMessage.Date = DateTime.Now;
                    chatMessage.Seen = true;
                    await _chatMessageInterface.SaveMessage(chatMessage);
                }

                if (existConnection_SenderWay.RecieverUserId == SenderUser)
                {
                    existConnection_SenderWay.RecieverUserConnectionId = connectionId;
                    existConnection_SenderWay.RecieverUserConnected = true;
                    existConnection_SenderWay.RecieverUserConnectDate = DateTime.Now;
                }
                existConnection_SenderWay.RecieverUserConnectionId = existConnection_SenderWay.RecieverUserConnectionId;
                existConnection_SenderWay.SenderUserConnectionId = connectionId;
                existConnection_SenderWay.SenderUserConnectDate = DateTime.Now;
                await connectionInterface.Update(existConnection_SenderWay);
            }

            if (existConnection_Reciever != null)
            {
                // check if a reciever userId was equal with From user, it will updated values which related with receiver UserId, for instance
                // RecieverUserId, RecieverConnected, etc.
                if (existConnection_Reciever.SenderUserConnected == false && existConnection_Reciever.RecieverUserConnected == true)
                {
                    seen = false;
                    chatMessage.FromUserId = SenderInfo.Id;
                    chatMessage.ToUserId = RecieverInfo.Id;
                    chatMessage.Message = message;
                    chatMessage.Date = DateTime.Now;
                    chatMessage.Seen = false;
                    await _chatMessageInterface.SaveMessage(chatMessage);
                }
                else if(existConnection_Reciever.SenderUserConnected == true && existConnection_Reciever.RecieverUserConnected == true)
                {
                    seen = true;
                    chatMessage.FromUserId = SenderInfo.Id;
                    chatMessage.ToUserId = RecieverInfo.Id;
                    chatMessage.Message = message;
                    chatMessage.Date = DateTime.Now;
                    chatMessage.Seen = true;
                    await _chatMessageInterface.SaveMessage(chatMessage);
                }
                if (existConnection_Reciever.RecieverUserId == SenderUser)
                {
                    existConnection_Reciever.RecieverUserConnectionId = connectionId;
                    existConnection_Reciever.SenderUserConnectionId = existConnection_Reciever.SenderUserConnectionId;
                    existConnection_Reciever.RecieverUserConnected = true;
                    existConnection_Reciever.RecieverUserConnectDate = DateTime.Now;
                }
                existConnection_Reciever.SenderUserConnectDate = DateTime.Now;
                await connectionInterface.Update(existConnection_Reciever);
            }
            if (existConnection_Reciever != null)
            {
                if (existConnection_Reciever.SenderUserConnected=true && existConnection_Reciever.RecieverUserConnected==false)
                {
                    await Clients.Clients(existConnection_Reciever.SenderUserConnectionId).SendAsync("RecieveMessage", SenderInfo.UserName, message,seen,chatMessage.Date.ToString("dddd, dd MMMM yyyy HH:mm"));
                }
                if (existConnection_Reciever.SenderUserConnected = false && existConnection_Reciever.RecieverUserConnected == true)
                {
                    await Clients.Clients(existConnection_Reciever.RecieverUserConnectionId).SendAsync("RecieveMessage", SenderInfo.UserName, message, seen, chatMessage.Date.ToString("dddd, dd MMMM yyyy HH:mm"));
                }
                if (existConnection_Reciever.SenderUserConnected = true && existConnection_Reciever.RecieverUserConnected == true)
                {
                    await Clients.Clients(existConnection_Reciever.SenderUserConnectionId).SendAsync("RecieveMessage", SenderInfo.UserName, message, seen, chatMessage.Date.ToString("dddd, dd MMMM yyyy HH:mm"));
                    await Clients.Clients(existConnection_Reciever.RecieverUserConnectionId).SendAsync("RecieveMessage", SenderInfo.UserName, message, seen, chatMessage.Date.ToString("dddd, dd MMMM yyyy HH:mm"));
                }
            }
            if (existConnection_SenderWay != null)
            {
                if (existConnection_SenderWay.SenderUserConnected = true && existConnection_SenderWay.RecieverUserConnected == false)
                {
                    await Clients.Clients(existConnection_SenderWay.SenderUserConnectionId).SendAsync("RecieveMessage", SenderInfo.UserName, message, seen, chatMessage.Date.ToString("dddd, dd MMMM yyyy HH:mm"));
                }

                if (existConnection_SenderWay.SenderUserConnected = false && existConnection_SenderWay.RecieverUserConnected == true)
                {
                    await Clients.Clients(existConnection_SenderWay.RecieverUserConnectionId).SendAsync("RecieveMessage", SenderInfo.UserName, message, seen, chatMessage.Date.ToString("dddd, dd MMMM yyyy HH:mm"));
                }
                if (existConnection_SenderWay.SenderUserConnected = true && existConnection_SenderWay.RecieverUserConnected == true)
                {
                    await Clients.Clients(existConnection_SenderWay.SenderUserConnectionId).SendAsync("RecieveMessage", SenderInfo.UserName, message, seen, chatMessage.Date.ToString("dddd, dd MMMM yyyy HH:mm"));
                    await Clients.Clients(existConnection_SenderWay.RecieverUserConnectionId).SendAsync("RecieveMessage", SenderInfo.UserName, message, seen, chatMessage.Date.ToString("dddd, dd MMMM yyyy HH:mm"));
                }
            }
        }

        /// <summary>
        /// We use this section to show the status of user
        /// In this section we use the same way in above section to show the status of users and then send it to specific user
        /// to figure out that user is online or offline
        /// </summary>
        /// <param name="SenderUser"></param>
        /// <param name="Online"></param>
        /// <param name="RecieverUser"></param>
        /// <returns></returns>

        public async Task OnlineSender(string SenderUser, bool Online, string RecieverUser)
        {
            var SenderInfo = await _userManager.FindByIdAsync(SenderUser);
            var RecieverInfo = db.Users.Find(RecieverUser);
            Connection connection = new Connection();
            var connectionId = Context.ConnectionId;
            var existConnection_SenderWay = connectionInterface.GetBySenderAndRecieverId(SenderUser, RecieverUser);
            var existConnection_Reciever = connectionInterface.GetBySenderAndRecieverId(RecieverUser, SenderUser);
            if (existConnection_SenderWay == null && existConnection_Reciever == null)
            {
                connection.SenderUserId = SenderUser;
                connection.RecieverUserId = RecieverUser;
                connection.SenderUserConnected = true;
                connection.RecieverUserConnected = false;
                connection.SenderUserConnectDate = DateTime.Now;
                connection.SenderUserConnectionId = connectionId;
                await connectionInterface.Add(connection);
            }
            else
            {
                // we use this section to update connection if both way senderUserId and recieverUserId were opposite of null
                if (existConnection_SenderWay != null)
                {
                    if (existConnection_SenderWay.SenderUserConnected == true && existConnection_SenderWay.RecieverUserConnected == true)
                    {
                        existConnection_SenderWay.RecieverUserConnectionId = existConnection_SenderWay.RecieverUserConnectionId;
                        existConnection_SenderWay.SenderUserConnected = Online;
                        existConnection_SenderWay.SenderUserConnectionId = connectionId;
                        existConnection_SenderWay.SenderUserConnectDate = DateTime.Now;
                    }
                    else if (existConnection_SenderWay.SenderUserConnected == true && existConnection_SenderWay.RecieverUserConnected == false)
                    {
                        existConnection_SenderWay.SenderUserConnectionId = connectionId;
                        existConnection_SenderWay.SenderUserConnected = Online;
                        existConnection_SenderWay.SenderUserConnectDate = DateTime.Now;
                    }
                    else if (existConnection_SenderWay.SenderUserConnected == false)
                    {
                        existConnection_SenderWay.SenderUserConnectionId = connectionId;
                        existConnection_SenderWay.RecieverUserConnectionId = existConnection_SenderWay.RecieverUserConnectionId;
                        existConnection_SenderWay.SenderUserConnected = Online;
                        existConnection_SenderWay.SenderUserConnectDate = DateTime.Now;
                    }
                    await connectionInterface.Update(existConnection_SenderWay);
                }
                // check if a reciever userId was equal with From user, it will updated values which related with receiver UserId, for instance
                // RecieverUserId, RecieverConnected, etc.
               
                //await _hubConnectionMessage.SendAsync("OnlineSender",FromUserId, existConnection_SenderWay.SenderUserConnected,toUserId);


                if (existConnection_Reciever != null)
                {
                    if (existConnection_Reciever.RecieverUserConnected == true && existConnection_Reciever.SenderUserConnected == true)
                    {
                        existConnection_Reciever.SenderUserConnectionId = existConnection_Reciever.SenderUserConnectionId;
                        existConnection_Reciever.RecieverUserConnected = Online;
                        existConnection_Reciever.RecieverUserConnectionId = connectionId;
                        existConnection_Reciever.RecieverUserConnectDate = DateTime.Now;
                    }
                    else if (existConnection_Reciever.RecieverUserConnected == true && existConnection_Reciever.SenderUserConnected == false)
                    {
                        existConnection_Reciever.RecieverUserConnectionId = connectionId;
                        existConnection_Reciever.RecieverUserConnected = Online;
                        existConnection_Reciever.RecieverUserConnectDate = DateTime.Now;
                    }
                    // check if a reciever userId was equal with From user, it will updated values which related with receiver UserId, for instance
                    // RecieverUserId, RecieverConnected, etc.
                    else if (existConnection_Reciever.RecieverUserConnected == false)
                    {
                        existConnection_Reciever.RecieverUserConnectDate = DateTime.Now;
                        existConnection_Reciever.RecieverUserConnected = Online;
                        existConnection_Reciever.RecieverUserConnectionId = connectionId;
                        existConnection_Reciever.SenderUserConnectionId = existConnection_Reciever.SenderUserConnectionId;
                    }
                    await connectionInterface.Update(existConnection_Reciever);
                    //await _hubConnectionMessage.SendAsync("OnlineSender", FromUserId, existConnection_Reciever.RecieverUserConnected,toUserId);
                }
            }
            if (existConnection_Reciever == null && existConnection_SenderWay == null)
            {
                await Clients.Clients(connection.SenderUserConnectionId).SendAsync("OnlineUser", RecieverInfo.UserName, connection.RecieverUserConnected);
            }

            if (existConnection_Reciever != null)
            {
                if (existConnection_Reciever.RecieverUserConnected == true && existConnection_Reciever.SenderUserConnected == true)
                {
                    await Clients.Clients(existConnection_Reciever.RecieverUserConnectionId).SendAsync("OnlineUser", RecieverInfo.UserName, existConnection_Reciever.SenderUserConnected);
                    await Clients.Clients(existConnection_Reciever.SenderUserConnectionId).SendAsync("OnlineUser", SenderInfo.UserName, existConnection_Reciever.RecieverUserConnected);
                }
                else if (existConnection_Reciever.RecieverUserConnected == false && existConnection_Reciever.SenderUserConnected == true)
                {
                    await Clients.Clients(existConnection_Reciever.SenderUserConnectionId).SendAsync("OnlineUser", SenderInfo.UserName, existConnection_Reciever.RecieverUserConnected);
                }
                else if (existConnection_Reciever.RecieverUserConnected == true && existConnection_Reciever.SenderUserConnected == false)
                {
                    await Clients.Clients(existConnection_Reciever.RecieverUserConnectionId).SendAsync("OnlineUser", RecieverInfo.UserName, existConnection_Reciever.SenderUserConnected);
                }
            }
            if (existConnection_SenderWay != null)
            {
                if (existConnection_SenderWay.RecieverUserConnected == true && existConnection_SenderWay.SenderUserConnected == true)
                {
                    await Clients.Clients(existConnection_SenderWay.RecieverUserConnectionId).SendAsync("OnlineUser", SenderInfo.UserName, existConnection_SenderWay.SenderUserConnected);
                    await Clients.Clients(existConnection_SenderWay.SenderUserConnectionId).SendAsync("OnlineUser", RecieverInfo.UserName, existConnection_SenderWay.RecieverUserConnected);
                }
                else if (existConnection_SenderWay.SenderUserConnected == false && existConnection_SenderWay.RecieverUserConnected == true)
                {
                    await Clients.Clients(existConnection_SenderWay.RecieverUserConnectionId).SendAsync("OnlineUser", SenderInfo.UserName, Online);
                }
                else if (existConnection_SenderWay.SenderUserConnected == true && existConnection_SenderWay.RecieverUserConnected == false)
                {
                    await Clients.Clients(existConnection_SenderWay.SenderUserConnectionId).SendAsync("OnlineUser", RecieverInfo.UserName, existConnection_SenderWay.RecieverUserConnected);
                }
            }



        }

        
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
    }
}
