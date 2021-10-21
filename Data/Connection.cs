using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateMessenger.Data
{
    /// <summary>
    /// This model represent all the informations that we need to know about a signal r connection
    /// For instance, we need userId and connectionId to create a connection between these users.
    /// Also we use this section to figure out which user is online or offline, and we know which user start a chat.
    /// </summary>
    public class Connection
    {
        public int Id { get; set; }
        // This user start a chat with another user when open Chat component. we store connection id for this user
        public string SenderUserConnectionId { get; set; }
        // This user recieve a chat from sender user. we store new connection id for this user and keep the sender connection id.
        public string RecieverUserConnectionId { get; set; }
        
        // This property belongs to sender user 
        public string SenderUserId { get; set; }

        // This property shows the status of sender (connected/disconnected)
        public bool SenderUserConnected { get; set; }

        // This property shows the status of reciever (connected/disconnected)
        public bool RecieverUserConnected { get; set; }

        // This property belongs to reciever user 
        public string RecieverUserId { get; set; }
        public virtual IdentityUser SenderUser{ get; set; }
        public virtual IdentityUser RecieverUser { get; set; }
        
        // It shows the time when a sender connected
        public DateTime SenderUserConnectDate { get; set; }

        // It shows the time when a reciever connected
        public DateTime RecieverUserConnectDate { get; set; }
    }
}
