using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateMessenger.Data
{
    /// <summary>
    /// This model is related to profile table.
    /// We use basic information that we need to show a user in our chat message
    /// All the users that we can find in this application should have a profile.
    /// Oterwise if you search a username which doesn't have a profile, you can not find that user
    /// </summary>
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Display(Name ="First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public int Age { get; set; }
        [MaxLength(10,ErrorMessage ="Maximum length for Gender is 10 characters")]
        public string Gender { get; set; }
        [MaxLength(200,ErrorMessage ="Maximum length for Bio is 200 characters")]
        public string Bio { get; set; }
        public string Image { get; set; }
        public DateTime Date { get; set; }
        public bool Private { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
    }
}
