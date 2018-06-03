using System;
using System.ComponentModel.DataAnnotations;

namespace SpiralWorks.Web.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        [Required]
        [Display(Name = "LastName")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Firstname")]
        public string FirstName { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "BirthDate")]
        public DateTime BirthDate { get; set; }
    }
}
