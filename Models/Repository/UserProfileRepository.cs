using Microsoft.EntityFrameworkCore;
using PrivateMessenger.Areas.Identity.Data;
using PrivateMessenger.Data;
using PrivateMessenger.Models.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateMessenger.Models.Repository
{
    public class UserProfileRepository : UserProfileInterface
    {
        private readonly AppDbContext db;
        public UserProfileRepository(AppDbContext db)
        {
            this.db = db;
        }
        // Add a new user profile to userprofile table
        public async Task<UserProfile> Add(UserProfile profile)
        {
            await db.UserProfiles.AddAsync(profile);
            await db.SaveChangesAsync();
            return profile;
        }

        // Update a user profile to userprofile table
        public async Task<UserProfile> Edit(UserProfile profile)
        {
            var userProfiles = db.UserProfiles.Attach(profile);
            if (userProfiles != null)
            {
                userProfiles.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                await db.SaveChangesAsync();
                return profile;
            }
            return null;
        }

        // Retrieve a user from user profile table
        public async Task <UserProfile> GetProfileByUserId(string UserId)
        {
            if (UserId!=null)
            {
                return await db.UserProfiles.Include(y=>y.User).SingleOrDefaultAsync(x => x.UserId == UserId);
            }
            return null;
        }
    }
}
