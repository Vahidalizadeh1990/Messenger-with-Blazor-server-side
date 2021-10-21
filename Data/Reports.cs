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
    /// This model is related with reports table
    /// We store all reports that users should send for us
    /// </summary>
    public class Reports
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage ="Report is required")]
        public string Report { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
        public DateTime Date { get; set; }
    }
}
