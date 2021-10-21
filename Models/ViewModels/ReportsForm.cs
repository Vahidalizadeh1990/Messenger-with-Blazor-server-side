using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateMessenger.Models.ViewModels
{
    /// <summary>
    /// This form is used in Reports component
    /// </summary>
    public class ReportsForm
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Report is required")]
        public string Report { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
        public DateTime Date { get; set; }
    }
}
