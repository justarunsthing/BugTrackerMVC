﻿using BugTrackerMVC.Models;
using BugTrackerMVC.Interfaces;
using BugTrackerMVC.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BugTrackerMVC.Services
{
    public class BTRolesService : IBTRolesService
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<BTUser> _userManager;

        public BTRolesService(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<BTUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        
        public async Task<bool> IsUserInRoleAsync(BTUser user, string roleName)
        {
            bool result = await _userManager.IsInRoleAsync(user, roleName);

            return result;
        }
        public async Task<IEnumerable<string>> GetUserRolesAsync(BTUser user)
        {
            var result = await _userManager.GetRolesAsync(user);

            return result;
        }
        public async Task<bool> AddUserToRoleAsync(BTUser user, string roleName)
        {
            bool result = (await _userManager.AddToRoleAsync(user, roleName)).Succeeded;

            return result;
        }
        public async Task<bool> RemoveUserFromRoleAsync(BTUser user, string roleName)
        {
            bool result = (await _userManager.RemoveFromRoleAsync(user, roleName)).Succeeded;

            return result;
        }
        public async Task<bool> RemoveUserFromRolesAsync(BTUser user, IEnumerable<string> roles)
        {
            bool result = (await _userManager.RemoveFromRolesAsync(user, roles)).Succeeded;

            return result;
        }
        public async Task<List<BTUser>> GetUsersInRoleAsync(string roleName, int companyId)
        {
            List<BTUser> users = (await _userManager.GetUsersInRoleAsync(roleName)).ToList();
            List<BTUser> result = users.Where(u => u.CompanyId == companyId).ToList();

            return result;
        }
        public async Task<List<BTUser>> GetUsersNotInRoleAsync(string roleName, int companyId)
        {
            List<string> userIds = (await _userManager.GetUsersInRoleAsync(roleName)).Select(u => u.Id).ToList();
            List<BTUser> roleUsers = _context.Users.Where(u => !userIds.Contains(u.Id)).ToList();
            List<BTUser> result = roleUsers.Where(u => u.CompanyId == companyId).ToList();

            return result;
        }

        public async Task<string> GetRoleNameByIdAsync(string roleId)
        {
            var role = _context.Roles.Find(roleId);
            var result = await _roleManager.GetRoleNameAsync(role);

            return result;
        }

        public async Task<List<IdentityRole>> GetRolesAsync()
        {
            try
            {
                var result = await _context.Roles.ToListAsync();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}