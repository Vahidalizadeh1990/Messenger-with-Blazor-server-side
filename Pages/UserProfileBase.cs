using AutoMapper;
using BlazorInputFile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using PrivateMessenger.Data;
using PrivateMessenger.Models.Interface;
using PrivateMessenger.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateMessenger.Pages
{
    
    public class UserProfileBase : ComponentBase
    {
        // Use IMapper to map a model to another model
        [Inject]
        public IMapper Mapper { get; set; }
        // it returns information about user from userManager
        [Inject]
        protected UserInformation userInformation { get; set; }
        // UserProfileForm 

        public UserProfileForm UserProfileForm { get; set; } = new UserProfileForm();

        // User Profile

        public Data.UserProfile UserProfile { get; set; } = new Data.UserProfile();
        // User Profile interface
        [Inject]
        public UserProfileInterface _userProfileInterface { get; set; }
        // Use navigation manager to navigate a user between other components
        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        // it returns current user name that is loged in
        [Inject]
        public IHttpContextAccessor httpContextAccessor { get; set; }

        public string Message { get; set; }
        public string ErrorMessage { get; set; }
        protected bool Toast { get; set; }
        public bool UserDetailsExist { get; set; }
        // File Upload
        [Inject]
        public IFileUpload fileUpload { get; set; }

        public IFileListEntry file { get; set; }
        public string filename { get; set; } = "NoImage.png";
        public void HandleFileSelected(IFileListEntry[] files)
        {
            file = files.FirstOrDefault();
            if (file != null)
            {
                filename = file.Name;
                //await fileUpload.Upload(file);
            }
        }
        protected async override Task OnInitializedAsync()
        {
            var userName = userInformation.userInformation(httpContextAccessor.HttpContext.User.Identity.Name);
            var exist = await _userProfileInterface.GetProfileByUserId(userName.Result.Id);
            if (exist != null)
            {
                UserDetailsExist = true;
                UserProfile = exist;
            }
            else
            {
                if (userName.Result == null)
                {
                    Toast = true;
                    Message = "Please sign in first";
                }
                else
                {
                    UserProfile = new Data.UserProfile
                    {
                        Date = DateTime.Now,
                        UserId = userName.Result.Id,
                        Id = 0
                    };
                    Mapper.Map(UserProfile, UserProfileForm);
                }
            }
        }

        protected async void HandleValidSubmit()
        {
            Mapper.Map(UserProfileForm, UserProfile);
            Data.UserProfile result = null;
            if (file != null)
            {
                UserProfile.Image = filename;
                await fileUpload.Upload(file);
            }
            UserProfile.Image = filename;

            Data.UserProfile report = await _userProfileInterface.Add(UserProfile);
            result = report;
            if (result != null)
            {
                NavigationManager.NavigateTo("/");
            }

        }
    }
}
