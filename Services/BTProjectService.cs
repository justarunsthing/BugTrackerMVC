using BugTrackerMVC.Data;
using BugTrackerMVC.Models;
using BugTrackerMVC.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BugTrackerMVC.Services
{
    public class BTProjectService : IBTProjectService
    {
        private readonly ApplicationDbContext _context;

        public BTProjectService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddNewProjectAsync(Project project)
        {
            _context.Add(project);
            await _context.SaveChangesAsync();
        }

        public Task<bool> AddProjectManagerAsync(string userId, int projectId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddUserToProjectAsync(string userId, int projectId)
        {
            throw new NotImplementedException();
        }

        public async Task ArchiveProjectAsync(Project project)
        {
            project.IsArchived = true;
            _context.Update(project);
            await _context.SaveChangesAsync();
        }

        public Task<List<BTUser>> GetAllProjectMembersExceptPMAsync(int projectId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Project>> GetAllProjectsByCompany(int companyId)
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

        public async Task<List<Project>> GetAllProjectsByPriority(int companyId, string priorityName)
        {
            var projects = await GetAllProjectsByCompany(companyId);
            var priorityId = await LookupProjectPriorityId(priorityName);

            return projects.Where(p => p.ProjectPriorityId == priorityId).ToList();
        }

        public async Task<List<Project>> GetArchivedProjectsByCompany(int companyId)
        {
            var projects = await GetAllProjectsByCompany(companyId);

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

        public Task<BTUser> GetProjectManagerAsync(int projectId)
        {
            throw new NotImplementedException();
        }

        public Task<List<BTUser>> GetProjectMembersByRoleAsync(int projectId, string role)
        {
            throw new NotImplementedException();
        }

        public Task<List<BTUser>> GetSubmittersOnProjectAsync(int projectId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Project>> GetUserProjectsAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<BTUser>> GetUsersNotOnProjectAsync(int projectId, int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsUserOnProject(string userId, int projectId)
        {
            throw new NotImplementedException();
        }

        public async Task<int> LookupProjectPriorityId(string priorityName)
        {
            var priorityId = (await _context.ProjectPriorities.FirstOrDefaultAsync(p => p.Name == priorityName)).Id;

            return priorityId;
        }

        public Task RemoveProjectManagerAsync(int projectId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveUserFromProjectAsync(string userId, int projectId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveUsersFromProjectByRoleAsync(string role, int projectId)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateProjectAsync(Project project)
        {
            _context.Update(project);
            await _context.SaveChangesAsync();
        }
    }
}