﻿using BugTrackerMVC.Models;
using Microsoft.AspNetCore.Identity;

namespace BugTrackerMVC.Interfaces
{
    public interface IBTRolesService
    {
        Task<bool> IsUserInRoleAsync(BTUser user, string roleName);
        Task<IEnumerable<string>> GetUserRolesAsync(BTUser user);
        Task<bool> AddUserToRoleAsync(BTUser user, string roleName);
        Task<bool> RemoveUserFromRoleAsync(BTUser user, string roleName);
        Task<bool> RemoveUserFromRolesAsync(BTUser user, IEnumerable<string> roles);
        Task<List<BTUser>> GetUsersInRoleAsync(string roleName, int companyId);
        Task<List<BTUser>> GetUsersNotInRoleAsync(string roleName, int companyId);
        Task<string> GetRoleNameByIdAsync(string roleId);
        Task<List<IdentityRole>> GetRolesAsync();
    }
}