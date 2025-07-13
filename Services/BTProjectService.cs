using BugTrackerMVC.Data;
using BugTrackerMVC.Models;
using BugTrackerMVC.Interfaces;
using Microsoft.EntityFrameworkCore;
using BugTrackerMVC.Enums;

namespace BugTrackerMVC.Services
{
    public class BTProjectService : IBTProjectService
    {
        private readonly ApplicationDbContext _context;
        private readonly IBTRolesService _rolesService;

        public BTProjectService(ApplicationDbContext context, IBTRolesService rolesService)
        {
            _context = context;
            _rolesService = rolesService;
        }

        public async Task AddNewProjectAsync(Project project)
        {
            _context.Add(project);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AddProjectManagerAsync(string userId, int projectId)
        {
            var currentPm = await GetProjectManagerAsync(projectId);

            if (currentPm != null)
            {
                try
                {
                    await RemoveProjectManagerAsync(projectId);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error removing current project manager: {ex.Message}");

                    return false;
                }
            }

            try
            {
                await AddUserToProjectAsync(userId, projectId);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding new project manager: {ex.Message}");

                return false;
            }
        }

        public async Task<bool> AddUserToProjectAsync(string userId, int projectId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user != null)
            {
                var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);

                if (!await IsUserOnProjectAsync(userId, projectId))
                {
                    try
                    {
                        project.Members.Add(user);
                        await _context.SaveChangesAsync();

                        return true;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }

                return false;
            }
            else
            {
                return false;
            }
        }

        public async Task ArchiveProjectAsync(Project project)
        {
            project.IsArchived = true;
            _context.Update(project);
            await _context.SaveChangesAsync();
        }

        public async Task<List<BTUser>> GetAllProjectMembersExceptPMAsync(int projectId)
        {
            var developers = await GetProjectMembersByRoleAsync(projectId, Roles.Developer.ToString());
            var submitters = await GetProjectMembersByRoleAsync(projectId, Roles.Submitter.ToString());
            var admins = await GetProjectMembersByRoleAsync(projectId, Roles.Admin.ToString());
            var teamMembers = developers.Concat(submitters).Concat(admins).ToList();

            return teamMembers;
        }

        public async Task<List<Project>> GetAllProjectsByCompanyAsync(int companyId)
        {
            var result = await _context.Projects.Where(p => p.CompanyId == companyId && p.IsArchived == false)
                                                .Include(p => p.Members)
                                                .Include(p => p.Tickets)
                                                    // Include tickets comments
                                                    .ThenInclude(t => t.TicketComments)
                                                .Include(p => p.Tickets)
                                                    // Include ticket attachments
                                                    .ThenInclude(t => t.TicketAttachments)
                                                .Include(p => p.Tickets)
                                                    // Include ticket history
                                                    .ThenInclude(t => t.History)
                                                .Include(p => p.Tickets)
                                                    // Include ticket notifications
                                                    .ThenInclude(t => t.Notifications)
                                                .Include(p => p.Tickets)
                                                    // Include developer
                                                    .ThenInclude(t => t.DeveloperUser)
                                                .Include(p => p.Tickets)
                                                    // Include ticket owner
                                                    .ThenInclude(t => t.OwnerUser)
                                                .Include(p => p.Tickets)
                                                    // Include ticket status
                                                    .ThenInclude(t => t.TicketStatus)
                                                .Include(p => p.Tickets)
                                                    // Include ticket priority
                                                    .ThenInclude(t => t.TicketPriority)
                                                .Include(p => p.Tickets)
                                                    // Include ticket type
                                                    .ThenInclude(t => t.TicketType)
                                                .Include(p => p.ProjectPriority)
                                                .ToListAsync();

            return result;
        }

        public async Task<List<Project>> GetAllProjectsByPriorityAsync(int companyId, string priorityName)
        {
            var projects = await GetAllProjectsByCompanyAsync(companyId);
            var priorityId = await LookupProjectPriorityIdAsync(priorityName);

            return projects.Where(p => p.ProjectPriorityId == priorityId).ToList();
        }

        public async Task<List<Project>> GetArchivedProjectsByCompanyAsync(int companyId)
        {
            var projects = await GetAllProjectsByCompanyAsync(companyId);

            return projects.Where(p => p.IsArchived == true).ToList();
        }

        public Task<List<BTUser>> GetDevelopersOnProjectAsync(int projectId)
        {
            throw new NotImplementedException();
        }

        public async Task<Project> GetProjectByIdAsync(int projectId, int companyId)
        {
            var project = await _context.Projects
                                        .Include(p => p.Tickets)
                                        .Include(p => p.Members)
                                        .Include(p => p.ProjectPriority)
                                        .FirstOrDefaultAsync(p => p.Id == projectId && p.CompanyId == companyId);

            return project;
        }

        public async Task<BTUser> GetProjectManagerAsync(int projectId)
        {
            var project = await _context.Projects
                                        .Include(p => p.Members)
                                        .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project != null)
            {
                foreach (var member in project.Members)
                {
                    if (await _rolesService.IsUserInRoleAsync(member, Roles.ProjectManager.ToString()))
                    {
                        return member;
                    }
                }
            }

            return null;
        }

        public async Task<List<BTUser>> GetProjectMembersByRoleAsync(int projectId, string role)
        {
            var project = await _context.Projects
                            .Include(p => p.Members)
                            .FirstOrDefaultAsync(p => p.Id == projectId);

            List<BTUser> members = new();

            foreach (var user in project.Members)
            {
                if (await _rolesService.IsUserInRoleAsync(user, role))
                {
                    members.Add(user);
                }
            }

            return members;
        }

        public Task<List<BTUser>> GetSubmittersOnProjectAsync(int projectId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Project>> GetUserProjectsAsync(string userId)
        {
            try
            {
                var userProjects = (await _context.Users
                                                 .Include(u => u.Projects)
                                                    .ThenInclude(p => p.Company)
                                                 .Include(u => u.Projects)
                                                    .ThenInclude(p => p.Members)
                                                 .Include(u => u.Projects)
                                                    .ThenInclude(p => p.Tickets)
                                                        .ThenInclude(t => t.DeveloperUser)
                                                 .Include(u => u.Projects)
                                                    .ThenInclude(p => p.Tickets)
                                                        .ThenInclude(t => t.TicketPriority)
                                                .Include(u => u.Projects)
                                                    .ThenInclude(t => t.Tickets)
                                                        .ThenInclude(t => t.TicketStatus)
                                                .Include(u => u.Projects)
                                                    .ThenInclude(t => t.Tickets)
                                                        .ThenInclude(t => t.TicketType)
                                                .FirstOrDefaultAsync(u => u.Id == userId)).Projects.ToList();

                return userProjects;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving user projects: {ex.Message}");
                throw;
            }
        }

        public async Task<List<BTUser>> GetUsersNotOnProjectAsync(int projectId, int companyId)
        {
            var users = await _context.Users.Where(u => u.Projects.All(p => p.Id != projectId)).ToListAsync();

            return users.Where(u => u.CompanyId == companyId).ToList();
        }

        public async Task<bool> IsUserOnProjectAsync(string userId, int projectId)
        {
            var result = false;
            var project = await _context.Projects
                                        .Include(p => p.Members)
                                        .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project != null)
            {
                result = project.Members.Any(m => m.Id == userId);
            }

            return result;
        }

        public async Task<int> LookupProjectPriorityIdAsync(string priorityName)
        {
            var priorityId = (await _context.ProjectPriorities.FirstOrDefaultAsync(p => p.Name == priorityName)).Id;

            return priorityId;
        }

        public async Task RemoveProjectManagerAsync(int projectId)
        {
            var project = await _context.Projects
                                        .Include(p => p.Members)
                                        .FirstOrDefaultAsync(p => p.Id == projectId);

            try
            {
                foreach (var member in project.Members)
                {
                    if (await _rolesService.IsUserInRoleAsync(member, Roles.ProjectManager.ToString()))
                    {
                        await RemoveUserFromProjectAsync(member.Id, projectId);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing project manager: {ex.Message}");
                throw;
            }
        }

        public async Task RemoveUserFromProjectAsync(string userId, int projectId)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);

                try
                {
                    if (await IsUserOnProjectAsync(userId, projectId))
                    {
                        project.Members.Remove(user);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing user from project: {ex.Message}");
            }
        }

        public async Task RemoveUsersFromProjectByRoleAsync(string role, int projectId)
        {
            try
            {
                var members = await GetProjectMembersByRoleAsync(projectId, role);
                var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);

                foreach (var btUser in members)
                {
                    try
                    {
                        project.Members.Remove(btUser);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing users from project by role: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateProjectAsync(Project project)
        {
            _context.Update(project);
            await _context.SaveChangesAsync();
        }
    }
}