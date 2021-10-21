using AutoMapper;
using Microsoft.AspNetCore.Components;
using PrivateMessenger.Models.Interface;
using PrivateMessenger.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrivateMessenger.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace PrivateMessenger.Pages
{
    public class ReportsBase : ComponentBase
    {
        // it returns information about user from userManager
        [Inject]
        protected UserInformation userInformation { get; set; }

        public Models.ViewModels.ReportsForm ReportsForm { get; set; } = new Models.ViewModels.ReportsForm();
        [Inject]
        public Models.Interface.ReportsInterface _reportsInterface { get; set; }
        protected string Message { get; set; }
        protected bool Toast { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }
        [Inject]
        public IMapper Mapper { get; set; }
        [Inject]
        IHttpContextAccessor httpContextAccessor { get; set; }
        public PrivateMessenger.Data.Reports Reports { get; set; } = new PrivateMessenger.Data.Reports();

        protected async override Task OnInitializedAsync()
        {

            var userName = userInformation.userInformation(httpContextAccessor.HttpContext.User.Identity.Name);
            if (userName.Result == null)
            {
                Toast = true;
                Message = "Please sign in first";
            }
            else
            {
                Reports = new Data.Reports
                {
                    Date = DateTime.Now,
                    UserId = userName.Result.Id,
                    Id = 0
                };
                Mapper.Map(Reports, ReportsForm);
            }
        }

        protected async void HandleValidSubmit()
        {
            Mapper.Map(ReportsForm, Reports);
            Data.Reports result = null;
            Data.Reports report = await _reportsInterface.Add(Reports);
            result = report;
            if (result != null)
            {
                NavigationManager.NavigateTo("/");
            }
        }
    }
}
