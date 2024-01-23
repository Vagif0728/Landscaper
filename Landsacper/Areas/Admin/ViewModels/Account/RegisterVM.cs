using System.ComponentModel.DataAnnotations;

namespace Landsacper.Areas.Admin.ViewModels
{
    public class RegisterVM
    {
        [Required]
        [MinLength(5)]
        [MaxLength(25)]
        public string Name { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(25)]
        public string Surname { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(25)]
        public string Username { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(25)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfrimPassword { get; set; }
        
    }
}
