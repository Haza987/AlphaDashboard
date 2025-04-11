using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Data.Entities;
using Data.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Business.Services;

public class MemberService(MemberRepository memberRepository) : IMemberService
{
    private readonly MemberRepository _memberRepository = memberRepository;

    public async Task<bool> CreateMemberAsync(MemberDto dto)
    {
        await _memberRepository.BeginTransactionAsync();
        try
        {
            var member = MemberFactory.CreateMemberEntity(dto);
            var result = await _memberRepository.CreateAsync(member);
            if (result)
            {
                await _memberRepository.CommitTransactionAsync();
                return true;
            }
            return false;
        }
        catch
        {
            await _memberRepository.RollbackTransactionAsync();
            throw;
        }
    }
}
