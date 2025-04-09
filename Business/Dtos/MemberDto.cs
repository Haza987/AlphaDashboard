namespace Business.Dtos;

public class MemberDto
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Role { get; set; } = null!;
    public List<ProjectDto> Projects { get; set; } = [];
}
