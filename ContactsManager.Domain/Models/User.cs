using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManager.Domain.Models
{
    public class User: IdentityUser
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(128, ErrorMessage = "")]
        //[Column(TypeName = "nvarchar(128)")]
        public string FirstName { get; set; } 

        [Required]
        [MaxLength(128, ErrorMessage ="")]
        //[Column(TypeName = "nvarchar(128)")]
        public string LastName { get; set; } 

        [Required]
        [MaxLength(60, ErrorMessage = "")]
        //[Column(TypeName = "nvarchar(60)")]
        public string Username { get; set; } 

        [Required]
        [MaxLength(256, ErrorMessage = "")]
        //[Column(TypeName = "nvarchar(256)")]
        public string Password { get; set; } 
    }
}
