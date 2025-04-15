namespace Business.Dtos;

public class ProjectUpdateDto
{
    public string? ProjectId { get; set; }
    public string? ProjectName { get; set; }
    public string? ClientName { get; set; }
    public string? ProjectDescription { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal? Budget { get; set; }
    public List<string>? Members { get; set; } = [];
}
