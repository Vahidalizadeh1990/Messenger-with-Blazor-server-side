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
    public class UserProfileDetailsBase:ComponentBase
    {
        // it returns information about user from userManager
        [Inject]
        protected UserInformation userInformation { get; set; }
        [CascadingParameter]
        public Data.UserProfile UserProfile { get; set; } = new Data.UserProfile();
        [Inject]
        public UserProfileInterface UserProfileInterface { get; set; }
        [Inject]
        IHttpContextAccessor httpContextAccessor { get; set; }
        public string Message { get; set; }
        protected async override Task OnInitializedAsync()
        {
            var userName = userInformation.userInformation(httpContextAccessor.HttpContext.User.Identity.Name);
            try
            {
                UserProfile = await UserProfileInterface.GetProfileByUserId(userName.Result.Id);
            }
            catch (Exception ex)
            {
                Message = "There is an error";
            }
        }
    }
}
