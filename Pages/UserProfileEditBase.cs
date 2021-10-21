using AutoMapper;
using BlazorInputFile;
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
    public class UserProfileEditBase : ComponentBase
    {
        // it returns information about user from userManager
        [Inject]
        public IMapper Mapper { get; set; }
        // it returns information about user from userManager
        [Inject]
        protected UserInformation userInformation { get; set; }
        // User Profile
        public UserProfileForm UserProfileForm { get; set; } = new UserProfileForm();
        // User Profile interface
        public Data.UserProfile UserProfile { get; set; } = new Data.UserProfile();
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
        // Handle selected file
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
            if (exist == null)
            {
                Toast = true;
                Message = "Please sign in first";
            }
            else
            {
                UserProfile = exist;
                Mapper.Map(UserProfile, UserProfileForm);
            }
        }

        protected async void HandleValidSubmit()
        {
            Mapper.Map(UserProfileForm, UserProfile);
            Data.UserProfile result = null;
            var uerprofileImage =await _userProfileInterface.GetProfileByUserId(UserProfile.UserId);
            
            if (file != null)
            {
                fileUpload.Remove(uerprofileImage.Image);
                UserProfile.Image = filename;
                await fileUpload.Upload(file);
                UserProfile.Image = filename;
            }
            else
            {
                UserProfile.Image = UserProfile.Image;
            }
            UserProfile.Date = DateTime.Now;

            Data.UserProfile report = await _userProfileInterface.Edit(UserProfile);
            result = report;
            if (result != null)
            {
                NavigationManager.NavigateTo("/userprofile");
            }

        }
    }
}
