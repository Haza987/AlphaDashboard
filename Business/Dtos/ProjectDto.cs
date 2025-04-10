﻿namespace Business.Dtos;

public class ProjectDto
{
    public string ProjectId { get; set; } = null!;
    public string ProjectName { get; set; } = null!;
    public string ClientName { get; set; } = null!;
    public string ProjectDescription { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Budget { get; set; }
    public List<MemberDto> Members { get; set; } = [];
}
