using PrivateMessenger.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateMessenger.Models.Interface
{
    /// <summary>
    ///  We inject this interface in our component to Add, Update and Retrieve a profile from database
    /// </summary>
    public interface UserProfileInterface
    {
        // Add a new profile if it doesn't exist
        Task<UserProfile> Add(UserProfile profile);
        // Update a profile if it exists
        Task<UserProfile> Edit(UserProfile profile);
        // Retrieve a profile by user id
        Task<UserProfile> GetProfileByUserId(string UserId);
    }
}
