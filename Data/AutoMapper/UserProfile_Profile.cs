using AutoMapper;
using PrivateMessenger.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateMessenger.Data.AutoMapper
{
    public class UserProfile_Profile : Profile
    {
        public UserProfile_Profile()
        {
            CreateMap<UserProfile,UserProfileForm>();
            CreateMap<UserProfileForm, UserProfile>();
        }
    }
}
