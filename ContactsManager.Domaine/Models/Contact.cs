using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManager.Domaine.Models
{
    public class Contact
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(128)")]
        public string FirstName { get; set; } 

        [Column(TypeName = "nvarchar(128)")]
        public string LastName { get; set; }  

        [Required]
        [Column(TypeName = "nvarchar(128)")]
        public string Email { get; set; } 

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(20)")]
        public string Phone { get; set; } 
        public Guid Owner { get; set; }

    }
}
