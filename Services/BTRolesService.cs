using BugTrackerMVC.Models;
using BugTrackerMVC.Interfaces;
using BugTrackerMVC.Data;
using Microsoft.AspNetCore.Identity;

namespace BugTrackerMVC.Services
{
    public class BTRolesService : IBTRolesService
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public BTRolesService(ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }
        
        public Task<bool> IsUserInRoleAsync(BTUser user, string roleName)
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<string>> GetUserRolesAsync(BTUser user)
        {
            throw new NotImplementedException();
        }
        public Task<bool> AddUserToRoleAsync(BTUser user, string roleName)
        {
            throw new NotImplementedException();
        }
        public Task<bool> RemoveUserFromRoleAsync(BTUser user, string roleName)
        {
            throw new NotImplementedException();
        }
        public Task<bool> RemoveUserFromRolesAsync(BTUser user, IEnumerable<string> roles)
        {
            throw new NotImplementedException();
        }
        public Task<List<BTUser>> GetUsersInRoleAsync(string roleName, int companyId)
        {
            throw new NotImplementedException();
        }
        public Task<List<BTUser>> GetUsersNotInRoleAsync(string roleName, int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRoleNameByIdAsync(string roleId)
        {
            throw new NotImplementedException();
        }
    }
}