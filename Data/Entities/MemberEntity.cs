using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class MemberEntity
{
    [Key]
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string JobTitle { get; set; } = null!;
    public string Address { get; set; } = null!;
    public DateOnly DateOfBirth { get; set; }
    public string? MemberImageFilePath { get; set; }

    public ICollection<ProjectEntity>? Projects { get; set; } = [];
}
