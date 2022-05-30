using System.ComponentModel.DataAnnotations;

namespace WebAdvert.Web.Models.Accounts;

public class SignupModel
{
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [StringLength(6, ErrorMessage = "password must be at least 6 characters.")]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; }
}
