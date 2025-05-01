using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class ProjectCreationViewModel
{
    [Required]
    [Display (Name = "Project Name")]
    public string ProjectName { get; set; } = null!;

    [Required]
    [Display(Name = "Client Name")]
    public string ClientName { get; set; } = null!;

    [Required]
    [Display(Name = "Project Description")]
    public string ProjectDescription { get; set; } = null!;

    [Display(Name = "Start Date")]
    public DateTime StartDate { get; set; } = DateTime.Now;

    [Display(Name = "End Date")]
    public DateTime EndDate { get; set; } = DateTime.Now.AddYears(1);

    [Display(Name = "Budget")]
    public decimal Budget { get; set; }
    
    public bool IsCompleted { get; set; }

    [Display(Name = "Members")]
    public List<int> Members { get; set; } = [];
}
