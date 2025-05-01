using Business.Dtos;
using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class MembersController(IMemberService memberService) : Controller
    {
        private readonly IMemberService _memberService = memberService;

        //[Authorize(Roles = "Admin")]
        [Route("members")]
        public async Task<IActionResult> Members()
        {
            var members = await _memberService.GetAllMembersAsync();

            var viewModel = new MemberViewModel
            {
                Members = members?.ToList() ?? new List<Member>(),
                MemberDto = new MemberDto()
            };

            ViewBag.Title = "Members";
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult CreateMember()
        {
            return PartialView("MemberPartials/_MemberCreation", new MemberCreationViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateMember(MemberCreationViewModel form)
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
                    DateOfBirth = form.DateOfBirth
                };

                var result = await _memberService.CreateMemberAsync(member);
                if (result)
                {
                    return RedirectToAction("Members");
                }
            }
            return View("MemberPartials/_MemberCreation", form);
        }

        [HttpGet]
        public async Task<IActionResult> GetMemberById(int id)
        {
            var member = await _memberService.GetMemberByIdAsync(id);
            if (member == null)
            {
                return NotFound();
            }

            return Json(member);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateMember(int id)
        {
            var member = await _memberService.GetMemberByIdAsync(id);

            var updateDto = new MemberUpdateDto
            {
                FirstName = member!.FirstName,
                LastName = member.LastName,
                Email = member.Email,
                PhoneNumber = member.PhoneNumber,
                JobTitle = member.JobTitle,
                Address = member.Address,
                DateOfBirth = member.DateOfBirth
            };

            return PartialView("MemberPartials/_MemberUpdate", updateDto);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMember(int id, MemberUpdateDto form)
        {
            if (ModelState.IsValid)
            {
                var result = await _memberService.UpdateMemberAsync(id, form);

                if (result)
                {
                    return RedirectToAction("Members");
                }
            }
            return PartialView("MemberPartials/_MemberUpdate", form);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMember(int id)
        {
            var result = await _memberService.DeleteMemberAsync(id);
            if (result)
            {
                return RedirectToAction("Members");
            }
            return BadRequest("Failed to delete member.");
        }
    }
}
