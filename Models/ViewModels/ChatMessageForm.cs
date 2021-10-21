using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateMessenger.Models.ViewModels
{
    /// <summary>
    /// This is a model which is used in Chat section.
    /// </summary>
    public class ChatMessageForm
    {
        public long Id { get; set; }
        public string FromUserId { get; set; }
        public string ToUserId { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public bool Seen { get; set; }
        public virtual IdentityUser FromUser { get; set; }
        public virtual IdentityUser ToUser { get; set; }
    }
}
