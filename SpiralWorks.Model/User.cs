using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SpiralWorks.Model
{
    [Table("User")]
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key()]
        public int UserId { get; set; }
        [Required]
      
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
      
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime DateCreated { get; set; }

    }
}
