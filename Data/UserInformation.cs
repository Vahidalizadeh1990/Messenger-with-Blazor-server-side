using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateMessenger.Data
{
    /// <summary>
    /// We use this class to get all information about any users.
    /// In this class we get information of users by their emails or their id
    /// </summary>
    public class UserInformation
    {
        private readonly UserManager<IdentityUser> userManager;

        public UserInformation(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<IdentityUser> userInformation(string email)
        {
            
            if (email == null)
            {
                return null;
            }
            else
            {
                return await userManager.FindByEmailAsync(email);
            }
        }
        public async Task<IdentityUser> userInformationById(string id)
        {

            if (id == null)
            {
                return null;
            }
            else
            {
                return await userManager.FindByIdAsync(id);
            }
        }
    }
}
