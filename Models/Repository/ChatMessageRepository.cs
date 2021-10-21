using Microsoft.AspNetCore.Identity;
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
    public class ChatMessageRepository : ChatMessageInterface
    {
        private readonly AppDbContext db;
        public ChatMessageRepository(AppDbContext db)
        {
            this.db = db;
        }

        // Retrieve list of user profiles
        public async Task<List<UserProfile>> AllUsers(string userId)
        {
            return await db.UserProfiles.Include(x=>x.User).Where(x=>x.UserId!=userId).ToListAsync();
        }

        // Retrieve list of user profiles based on their emails
        public async Task<List<UserProfile>> AllUsersByTheirEmail(string Email,string MyEmail)
        {
            return await db.UserProfiles.Include(x => x.User).Where(x => x.User.Email.StartsWith(Email) && x.User.Email!=MyEmail).ToListAsync();
        }

        // Retrieve all messages for a user
        public async Task<IEnumerable<ChatMessage>> GetAllRecieverMessageFromSpecificUser(string From, string To)
        {
            if (From != null && To!=null)
            {
                return await db.ChatMessage.Include(x => x.FromUser).Include(x=>x.ToUser).Where(x => x.FromUserId == From && x.ToUserId==To|| x.FromUserId==To && x.ToUserId==From).ToListAsync();
            }
            return null;
        }

        // Retrieve all messages with number of messages that we need, because we don't wanna show all messages in a current time
        public async Task<IEnumerable<ChatMessage>> LoadMore(string From, string To, int numberofMessage)
        {
            if (From != null && To != null)
            {
                return await db.ChatMessage.OrderByDescending(x=>x.Id).Include(x => x.FromUser).Include(x => x.ToUser).Where(x => x.FromUserId == From && x.ToUserId == To || x.FromUserId == To && x.ToUserId == From).Take(numberofMessage).ToListAsync();
            }
            return null;
        }

        // Retrieve 10 last messages that we need when chat component runs
        public async Task<IEnumerable<ChatMessage>> Get10LastMessage(string From, string To)
        {
            if (From != null && To != null)
            {
                return await db.ChatMessage.OrderByDescending(x=>x.Id).Include(x => x.FromUser).Include(x => x.ToUser).Where(x => x.FromUserId == From && x.ToUserId == To || x.FromUserId == To && x.ToUserId == From).Take(10).ToListAsync();
            }
            return null;
        }

        // Retrieve list of chat messages by sender user id
        public async Task<IEnumerable<ChatMessage>> GetByFromUserId(string FromUserId)
        {
            if (FromUserId != null)
            {
                return await db.ChatMessage.Include(x => x.FromUser).Where(x => x.FromUserId == FromUserId).ToListAsync();
            }
            return null;
        }

        // Retrieve list of chat messages by reciever user id
        public async Task<IEnumerable<ChatMessage>> GetByRecieverUserId(string RecieverUserId)
        {
            if (RecieverUserId != null)
            {
                return await db.ChatMessage.Include(x => x.ToUser).Where(x => x.ToUserId == RecieverUserId).ToListAsync();
            }
            return null;
        }

        // Save a chat message in database
        public async Task<ChatMessage> SaveMessage(ChatMessage chatMessage)
        {
            await db.ChatMessage.AddAsync(chatMessage);
            await db.SaveChangesAsync();
            return chatMessage;
        }

        // Update a chat message in database
        public async Task<ChatMessage> UpdateMessage(ChatMessage chatMessage)
        {
            var chat = db.ChatMessage.Attach(chatMessage);
            if (chat != null)
            {
                chat.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                await db.SaveChangesAsync();
                return chatMessage;
            }
            return null;
        }

        // Notify to a user when there are some message with false seen value
        public async Task<IEnumerable<ChatMessage>> NotifyForMe(string ToUserId)
        {
            if (ToUserId != null)
            {
                return await db.ChatMessage.Include(x => x.FromUser).Include(x=>x.ToUser).Where(x => x.ToUserId==ToUserId && x.Seen==false).ToListAsync();
            }
            return null;
        }
    }
}
