using System.ComponentModel.DataAnnotations;

namespace Landsacper.Areas.Admin.ViewModels
{
    public class UpdateServiceVM
    {
        [Required]
        [MinLength(5)]
        [MaxLength(255)]
        public string Name { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(255)]
        public string Description { get; set; }

        public string? Image { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
