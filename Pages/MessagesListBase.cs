using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using PrivateMessenger.Data;
using PrivateMessenger.Models.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateMessenger.Pages
{
    public class MessagesListBase : ComponentBase
    {
        [Inject]
        public ConnectionInterface _connectionInterface { get; set; }
        [Inject]
        protected UserInformation userInformation { get; set; }
        [Inject]
        protected NavigationManager NavigationManager { get; set; }
        // it returns current user name that is loged in
        [Inject]
        public IHttpContextAccessor httpContextAccessor { get; set; }

        public List<Data.UserProfile> UserProfiles { get; set; } = new List<Data.UserProfile>();
        [Inject]
        public UserProfileInterface UserProfileInterface { get; set; }
        public List<Data.ChatMessage> ChatMessages { get; set; } = new List<Data.ChatMessage>();
        [Inject]
        public ChatMessageInterface ChatMessageInterface { get; set; }


        protected async override Task OnInitializedAsync()
        {
            var userName = await userInformation.userInformation(httpContextAccessor.HttpContext.User.Identity.Name);
            var UserInfo= await userInformation.userInformation(userName.Email);

            var recieveProfile =await _connectionInterface.GetByRecieverId(UserInfo.Id);
            var senderProfile = await _connectionInterface.GetBySenderId(UserInfo.Id);

            foreach (var recieve in recieveProfile)
            {
                // a user who has been sent a Message to you
                var RecieveMessageByMe = await UserProfileInterface.GetProfileByUserId(recieve.SenderUser.Id);
                UserProfiles.Add(RecieveMessageByMe);
            }
            foreach (var sender in senderProfile)
            {
                // a user who has been recieved a Message from me
                var SendMessageByMe = await UserProfileInterface.GetProfileByUserId(sender.RecieverUser.Id);
                UserProfiles.Add(SendMessageByMe);
            }
            // Notifty to user if therer is some messages with false seen value
            var notify = await ChatMessageInterface.NotifyForMe(UserInfo.Id);
            ChatMessages = notify.ToList();
        }
    }
}
