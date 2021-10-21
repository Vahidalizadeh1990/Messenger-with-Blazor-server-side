using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateMessenger.Models.ViewModels
{
    /// <summary>
    /// We use this form to create a new profile for each of users who registered in Private Messenger
    /// </summary>
    public class UserProfileForm
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public int Age { get; set; }
        [MaxLength(10, ErrorMessage = "Maximum length for Gender is 10 characters")]
        public string Gender { get; set; }
        [MaxLength(200, ErrorMessage = "Maximum length for Bio is 200 characters")]
        public string Bio { get; set; }
        public string Image { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        public DateTime Date { get; set; }
        public bool Private { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
    }
}
