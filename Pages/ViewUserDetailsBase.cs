using AutoMapper;
using Microsoft.AspNetCore.Components;
using PrivateMessenger.Data;
using PrivateMessenger.Models.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateMessenger.Pages
{
    public class ViewUserDetailsBase:ComponentBase
    {
        /// <summary>
        /// View user details from other users
        /// For example, a user with id 123 wants to see the details of a user with id 456
        /// </summary>
        [Parameter]
        public string UserId { get; set; }
        // it returns information about user from userManager
        [Inject]
        public IMapper Mapper { get; set; }
        // it returns information about user from userManager
        [Inject]
        protected UserInformation userInformation { get; set; }

        // User Profile
        public Data.UserProfile UserProfile { get; set; } = new Data.UserProfile();
        // User Profile interface
        [Inject]
        public UserProfileInterface _userProfileInterface { get; set; }
        // Use navigation manager to navigate a user between other components
        [Inject]
        protected NavigationManager NavigationManager { get; set; }
        public string Message { get; set; }
        
        protected async override Task OnInitializedAsync()
        {
            if(UserId == null || UserId == "")
            {
                Message = "There is an error";
            }
            else
            {
                UserProfile = await _userProfileInterface.GetProfileByUserId(UserId);
                if (UserProfile == null)
                {
                    Message = "User Not Found";
                }
                if(UserProfile.Private==true)
                {
                    Message = UserProfile.User.UserName+"'s"+" "+ "is private.";
                }
            }
            
        }
    }
}
