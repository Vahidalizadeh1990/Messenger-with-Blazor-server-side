using AutoMapper;
using PrivateMessenger.Data;
using PrivateMessenger.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerApp.Models
{
    public class ReportProfile:Profile
    {
        public ReportProfile()
        {
            CreateMap<Reports, ReportsForm>();
            CreateMap<ReportsForm, Reports>();
        }
    }
}
