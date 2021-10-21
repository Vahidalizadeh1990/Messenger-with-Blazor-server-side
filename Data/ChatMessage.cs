using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateMessenger.Data
{
    /// <summary>
    /// This model represent the chat properties which it will be needed to store all message from sender and reciever users.
    /// We use Identity User table that is a foreign key for this model.
    /// </summary>
    public class ChatMessage
    {
        public long Id { get; set; }
        public string FromUserId { get; set; }
        public string ToUserId { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public bool Seen { get; set; }
        
        /// <summary>
        /// We use this section for show all the user informations which are related with messages
        /// Each of these virtual properties are used for individual user.
        /// </summary>
        public virtual IdentityUser FromUser { get; set; }
        public virtual IdentityUser ToUser { get; set; }
    }
}
