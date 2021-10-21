using Microsoft.AspNetCore.Identity;
using PrivateMessenger.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateMessenger.Models.Interface
{
    /// <summary>
    /// We inject this interface in our component to have an access to our ChatMessage table
    /// </summary>
    public interface ChatMessageInterface
    {
        // Save all Messages 
        Task<ChatMessage> SaveMessage(ChatMessage chatMessage);

        // Update seen field 
        Task<ChatMessage> UpdateMessage(ChatMessage chatMessage);

        // Retrieve all users
        Task<List<UserProfile>> AllUsers(string userId);

        // Retrieve all users by their email
        Task<List<UserProfile>> AllUsersByTheirEmail(string Email, string MyEmail);

        // Retrieve a chat by a user who send a message
        Task<IEnumerable<ChatMessage>> GetByFromUserId(string FromUserId);
        
        // Retieve a chat by a user who recieve a message
        Task<IEnumerable<ChatMessage>> GetByRecieverUserId(string RecieverUserId);

        // Retrieve all chat between 2 users
        Task<IEnumerable<ChatMessage>> GetAllRecieverMessageFromSpecificUser(string From, string To);

        // Retrieve all messages between 2 users by number of message which it sets to 10 from our component
        Task<IEnumerable<ChatMessage>> LoadMore(string From, string To, int numberofMessage);

        // Retrieve last 10 messages to show on a component when it initialized
        Task<IEnumerable<ChatMessage>> Get10LastMessage(string From, string To);

        // Notify to a user which have some messages (The seen value is false)
        Task<IEnumerable<ChatMessage>> NotifyForMe( string ToUserId);
    }
}
