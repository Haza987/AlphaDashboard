using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class SignInViewModel
{
    [Required(ErrorMessage = "Please enter your email address.")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Email or password is incorrect.")]
    [Display(Name = "Email", Prompt = "Enter your email address")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Please enter your password.")]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{7,}$", ErrorMessage = "Email or password is incorrect.")]
    [Display(Name = "Password", Prompt = "Enter your password")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    public bool RememberMe { get; set; }
}
