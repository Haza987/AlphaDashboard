using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class MemberCreationViewModel
{
    [Required]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = null!;

    [Required]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = null!;

    [Required]
    [Display(Name = "Email")]
    public string Email { get; set; } = null!;

    [Required]
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; } = null!;

    [Required]
    [Display(Name = "Job Title")]
    public string JobTitle { get; set; } = null!;

    [Required]
    [Display(Name = "Address")]
    public string Address { get; set; } = null!;

    [Display(Name = "Date of Birth")]
    public DateOnly DateOfBirth { get; set; }

    [Display(Name = "Projects")]
    public List<string> ProjectNames { get; set; } = [];
}
