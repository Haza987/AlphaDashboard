using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class ProjectCreationViewModel
{
    [Required]
    public string ProjectName { get; set; } = null!;

    [Required]
    public string ClientName { get; set; } = null!;

    [Required]
    public string ProjectDescription { get; set; } = null!;
    public DateTime StartDate { get; set; } = DateTime.Now;
    public DateTime EndDate { get; set; } = DateTime.Now.AddYears(1);
    public decimal Budget { get; set; }
    public bool IsCompleted { get; set; }
    public List<int> Members { get; set; } = [];
}
