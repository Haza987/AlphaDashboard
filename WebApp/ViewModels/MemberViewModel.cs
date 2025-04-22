using Business.Dtos;
using Business.Models;

namespace WebApp.ViewModels;

public class MemberViewModel
{
    public List<Member> Members { get; set; } = [];
    public MemberDto MemberDto { get; set; } = new();
}
