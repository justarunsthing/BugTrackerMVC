﻿using BugTrackerMVC.Extensions;
using BugTrackerMVC.Interfaces;
using BugTrackerMVC.Models;
using BugTrackerMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BugTrackerMVC.Controllers
{
    [Authorize]
    public class UserRolesController : Controller
    {
        private readonly IBTRolesService _rolesService;
        private readonly IBTCompanyInfoService _companyInfoService;

        public UserRolesController(IBTRolesService rolesService, IBTCompanyInfoService companyInfoService)
        {
            _rolesService = rolesService;
            _companyInfoService = companyInfoService;
        }

        [HttpGet]
        public async Task<IActionResult> ManageUserRoles()
        {
            List<ManageUserRolesViewModel> model = [];
            int companyId = User.Identity.GetCompanyId().Value;
            List<BTUser> users = await _companyInfoService.GetAllMembersAsync(companyId);

            foreach (var user in users)
            {
                ManageUserRolesViewModel viewModel = new()
                {
                    BTUser = user
                };

                var selectedRoles = await _rolesService.GetUserRolesAsync(user);
                var roles = await _rolesService.GetRolesAsync();
                viewModel.Roles = new MultiSelectList(roles, "Name", "Name", selectedRoles);

                model.Add(viewModel);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageUserRoles(ManageUserRolesViewModel member)
        {
            int companyId = User.Identity.GetCompanyId().Value;
            BTUser btUser = (await _companyInfoService.GetAllMembersAsync(companyId)).FirstOrDefault(u => u.Id == member.BTUser.Id);
            IEnumerable<string> roles = await _rolesService.GetUserRolesAsync(btUser);
            string userRole = member.SelectedRoles.FirstOrDefault();

            if (!string.IsNullOrEmpty(userRole))
            {
                if (await _rolesService.RemoveUserFromRolesAsync(btUser, roles))
                {
                    await _rolesService.AddUserToRoleAsync(btUser, userRole);
                }
            }

            return RedirectToAction(nameof(ManageUserRoles));
        }
    }
}