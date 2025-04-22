namespace Business.Dtos;

public class MemberUpdateDto
{
    public int Id { get; set; }
    public string? FirstName { get; set; } = null!;
    public string? LastName { get; set; } = null!;
    public string? Email { get; set; } = null!;
    public string? PhoneNumber { get; set; } = null!;
    public string? JobTitle { get; set; } = null!;
    public string? Address { get; set; } = null!;
    public DateOnly? DateOfBirth { get; set; }
    public List<string>? ProjectNames { get; set; } = [];
}
