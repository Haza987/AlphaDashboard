using Business.Dtos;
using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class MembersController(IMemberService memberService) : Controller
    {
        private readonly IMemberService _memberService = memberService;

        [Route("members")]
        public async Task<IActionResult> Members()
        {
            var members = await _memberService.GetAllMembersAsync();

            var viewModel = new MemberViewModel
            {
                Members = members?.ToList() ?? new List<Member>(),
                MemberDto = new MemberDto()
            };
            Debug.WriteLine($"Members count: {members?.Count()}");
            ViewBag.Title = "Members";
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult CreateMember()
        {
            return PartialView("_MemberCreation", new MemberDto());
        }

        [HttpPost]
        public async Task<IActionResult> CreateMember(MemberDto form)
        {
            if (ModelState.IsValid)
            {
                var member = new MemberDto
                {
                    FirstName = form.FirstName,
                    LastName = form.LastName,
                    Email = form.Email,
                    PhoneNumber = form.PhoneNumber,
                    JobTitle = form.JobTitle,
                    Address = form.Address,
                    DateOfBirth = form.DateOfBirth,
                    ProjectNames = form.ProjectNames
                };

                var result = await _memberService.CreateMemberAsync(member);
                if (result)
                {
                    return RedirectToAction("Members");
                }
            }
            return View(form);
        }
    }
}
