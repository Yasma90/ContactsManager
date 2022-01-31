using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace ContactsManager.Domain.Models
{
    public class User: IdentityUser
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(128)]
        public string FirstName { get; set; } 

        [Required]
        [MaxLength(128)]
        public string LastName { get; set; } 

        [Required]
        [MaxLength(60)]
        public string Username { get; set; } 

        [Required]
        [MaxLength(256)]
        public string Password { get; set; } 
    }
}
