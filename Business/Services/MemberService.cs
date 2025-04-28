using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Interfaces;

namespace Business.Services;

public class MemberService(IMemberRepository memberRepository, IProjectRepository projectRepository) : IMemberService
{
    private readonly IMemberRepository _memberRepository = memberRepository;
    private readonly IProjectRepository _projectRepository = projectRepository;

    public async Task<bool> CreateMemberAsync(MemberDto dto)
    {
        await _memberRepository.BeginTransactionAsync();
        try
        {
            var projects = (await _projectRepository.GetAllAsync())?.ToList() ?? [];
            var member = MemberFactory.CreateMemberEntity(dto, projects);
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

    public async Task<IEnumerable<Member>?> GetAllMembersAsync()
    {
        var memberEntities = await _memberRepository.GetAllAsync();
        var members = memberEntities?.Select(MemberFactory.CreateMemberModel);
        return members;
    }

    public async Task<Member?> GetMemberByIdAsync(int id)
    {
        var memberEntity = await _memberRepository.GetAsync(x => x.Id == id);
        if (memberEntity == null)
        {
            return null;
        }
        var member = MemberFactory.CreateMemberModel(memberEntity);
        return member;
    }

    public async Task<bool> UpdateMemberAsync(int id, MemberUpdateDto updateDto)
    {
        await _memberRepository.BeginTransactionAsync();

        var memberEntity = await _memberRepository.GetAsync(x => x.Id == id);
        if (memberEntity == null)
        {
            return false;
        }

        try
        {
            memberEntity = MemberFactory.UpdateMember(memberEntity, updateDto);
            var result = await _memberRepository.UpdateAsync(memberEntity);
            await _memberRepository.CommitTransactionAsync();
            return result;
        }
        catch
        {
            await _memberRepository.RollbackTransactionAsync();
            return false;
        }
    }

    public async Task<bool> DeleteMemberAsync(int id)
    {
        await _memberRepository.BeginTransactionAsync();
        var memberEntity = await _memberRepository.GetAsync(x => x.Id == id);

        if (memberEntity == null)
        {
            return false;
        }

        try
        {
            var result = await _memberRepository.DeleteAsync(memberEntity);
            if (result)
            {
                await _memberRepository.CommitTransactionAsync();
            }

            return true;
        }
        catch
        {
            await _memberRepository.RollbackTransactionAsync();
            return false;
        }
    }
}
