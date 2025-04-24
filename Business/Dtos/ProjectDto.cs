namespace Business.Dtos;

public class ProjectDto
{
    public int Id { get; set; }
    public string? ProjectId { get; set; }
    public string ProjectName { get; set; } = null!;
    public string ClientName { get; set; } = null!;
    public string ProjectDescription { get; set; } = null!;
    public DateTime StartDate { get; set; } = DateTime.Now;
    public DateTime EndDate { get; set; } = DateTime.Now.AddYears(1);
    public decimal Budget { get; set; }
    public bool IsCompleted { get; set; }
    public int Members { get; set; }
}
