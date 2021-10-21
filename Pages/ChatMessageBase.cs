using AutoMapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using PrivateMessenger.Data;
using PrivateMessenger.Models.Interface;
using PrivateMessenger.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PrivateMessenger.Pages
{
    public class ChatMessageBase:ComponentBase
    {
        // Use IMapper to map a model to another model
        [Inject]
        public IMapper Mapper { get; set; }
        // it returns information about user from userManager
        [Inject]
        protected UserInformation userInformation { get; set; }

        
        // Retrieve list of user profiles
        protected List<Data.UserProfile> identityUser{ get; set; } = new List<Data.UserProfile>();
        // We use this interface to have an access to chat message table to show all users that we search them
        [Inject]
        public ChatMessageInterface _chatMessageInterface{ get; set; }
        // Use navigation manager to navigate a user between other components
        [Inject]
        protected NavigationManager NavigationManager { get; set; }
        // This property is used to show some messages to user
        public string Message { get; set; }
        // This property is used for search a profile based on email
        public string EmailForSearch { get; set; }
        // This property store my email
        protected string MyEmail { get; set; }
        protected bool Loading { get; set; } = false;

        // it returns current user name that is loged in
        [Inject]
        public IHttpContextAccessor httpContextAccessor { get; set; }
        protected async override Task OnInitializedAsync()
        {
            var userName = await userInformation.userInformation(httpContextAccessor.HttpContext.User.Identity.Name);
            string UserProfileObject = userName.Id;
            MyEmail = userName.Email;
            
        }

        // We use search to find any profiles based on their emails
        public async Task Search()
        {
            
            if (EmailForSearch==null || EmailForSearch=="" || String.IsNullOrEmpty(EmailForSearch))
            {
                Loading = true;
                Message = "Search a profile by email";
                Thread.Sleep(1000);
            }
            else
            {
                Loading = true;
                identityUser = await _chatMessageInterface.AllUsersByTheirEmail(EmailForSearch, MyEmail);
                Thread.Sleep(1000);
                
                Message = null;
                if(identityUser.Count==0)
                {
                    Message = "User not found";
                }
            }
            Loading = false;
        }
    }
}
