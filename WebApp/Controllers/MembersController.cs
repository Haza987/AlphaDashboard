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
            return PartialView("MemberPartials/_MemberCreation", new MemberDto());
        }

        [HttpPost]
        public async Task<IActionResult> CreateMember(MemberDto form)
        {
            if (ModelState.IsValid)
            {
                var result = await _memberService.CreateMemberAsync(form);
                if (result)
                {
                    return RedirectToAction("Members");
                }
            }
            return View(form);
        }

        [HttpGet]
        public IActionResult UpdateMember()
        {
            return PartialView("MemberPartials/_MemberUpdate", new MemberUpdateDto());
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
            return View("MemberPartials/_MemberUpdate", form);
        }

        [HttpGet]
        public async Task<IActionResult> GetMemberById(int id)
        {
            var member = await _memberService.GetMemberByIdAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            Debug.WriteLine($"Member Data: {System.Text.Json.JsonSerializer.Serialize(member)}");


            return Json(member);
        }
    }
}
