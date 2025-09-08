using System.ComponentModel.DataAnnotations;
using UPTrain.Models;

namespace UPTrain.ViewModels
{
    public class RegisterViewModel
    {
        [Key]
        public int RegisterId { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(40, MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm Password is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password does not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Role is required")]
        [Display(Name = "User Role")]
        public UserRole Role { get; set; }
    }
}
