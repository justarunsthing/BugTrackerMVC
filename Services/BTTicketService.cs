using BugTrackerMVC.Data;
using BugTrackerMVC.Models;
using BugTrackerMVC.Interfaces;
using Microsoft.EntityFrameworkCore;
using BugTrackerMVC.Enums;

namespace BugTrackerMVC.Services
{
    public class BTTicketService : IBTTicketService
    {
        private readonly ApplicationDbContext _context;
        private readonly IBTRolesService _rolesService;
        private readonly IBTProjectService _projectService;

        public BTTicketService(ApplicationDbContext context, IBTRolesService rolesService, IBTProjectService projectService)
        {
            _context = context;
            _rolesService = rolesService;
            _projectService = projectService;
        }

        public async Task AddNewTicketAsync(Ticket ticket)
        {
            _context.Add(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task ArchiveTicketAsync(Ticket ticket)
        {
            ticket.IsArchived = true;
            _context.Update(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task AssignTicketAsync(int ticketId, string userId)
        {
            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == ticketId);

            try
            {
                if (ticket != null)
                {
                    try
                    {
                        ticket.DeveloperUserId = userId;
                        // Revisit this code when assigning Tickets
                        ticket.TicketStatusId = (await LookupTicketStatusIdAsync("Development")).Value;
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Ticket>> GetAllTicketsByCompanyAsync(int companyId)
        {
            try
            {
                var tickets = await _context.Projects
                                            .Where(p => p.CompanyId == companyId)
                                            .SelectMany(p => p.Tickets)
                                                .Include(t => t.TicketAttachments)
                                                .Include(t => t.TicketComments)
                                                .Include(t => t.History)
                                                .Include(t => t.TicketPriority)
                                                .Include(t => t.TicketStatus)
                                                .Include(t => t.TicketType)
                                                .Include(t => t.Project)
                                            .ToListAsync();

                return tickets;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Ticket>> GetAllTicketsByPriorityAsync(int companyId, string priorityName)
        {
            var priorityId = (await LookupTicketPriorityIdAsync(priorityName)).Value; // Allow thread to complete

            try
            {
                var tickets = await _context.Projects
                                            .Where(p => p.CompanyId == companyId)
                                            .SelectMany(p => p.Tickets)
                                                .Include(t => t.TicketAttachments)
                                                .Include(t => t.TicketComments)
                                                .Include(t => t.DeveloperUser)
                                                .Include(t => t.OwnerUser)
                                                .Include(t => t.TicketPriority)
                                                .Include(t => t.TicketStatus)
                                                .Include(t => t.TicketType)
                                                .Include(t => t.Project)
                                            .Where(t => t.TicketPriorityId == priorityId)
                                            .ToListAsync();

                return tickets;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Ticket>> GetAllTicketsByStatusAsync(int companyId, string statusName)
        {
            var statusId = (await LookupTicketStatusIdAsync(statusName)).Value;

            try
            {
                var tickets = await _context.Projects
                                            .Where(p => p.CompanyId == companyId)
                                            .SelectMany(p => p.Tickets)
                                                .Include(t => t.TicketAttachments)
                                                .Include(t => t.TicketComments)
                                                .Include(t => t.DeveloperUser)
                                                .Include(t => t.OwnerUser)
                                                .Include(t => t.TicketPriority)
                                                .Include(t => t.TicketStatus)
                                                .Include(t => t.TicketType)
                                                .Include(t => t.Project)
                                            .Where(t => t.TicketStatusId == statusId)
                                            .ToListAsync();
                return tickets;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Ticket>> GetAllTicketsByTypeAsync(int companyId, string typeName)
        {
            var typeId = (await LookupTicketTypeIdAsync(typeName)).Value;

            try
            {
                var tickets = await _context.Projects
                                            .Where(p => p.CompanyId == companyId)
                                            .SelectMany(p => p.Tickets)
                                                .Include(t => t.TicketAttachments)
                                                .Include(t => t.TicketComments)
                                                .Include(t => t.DeveloperUser)
                                                .Include(t => t.OwnerUser)
                                                .Include(t => t.TicketPriority)
                                                .Include(t => t.TicketStatus)
                                                .Include(t => t.TicketType)
                                                .Include(t => t.Project)
                                            .Where(t => t.TicketTypeId == typeId)
                                            .ToListAsync();
                return tickets;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Ticket>> GetArchivedTicketsAsync(int companyId)
        {
            try
            {
                var tickets = (await GetAllTicketsByCompanyAsync(companyId))
                                     .Where(t => t.IsArchived == true)
                                     .ToList();

                return tickets;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<Ticket>> GetProjectTicketsByPriorityAsync(string priorityName, int companyId, int projectId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Ticket>> GetProjectTicketsByRoleAsync(string role, string userId, int projectId, int companyId)
        {
            var tickets = new List<Ticket>();

            try
            {
                tickets = (await GetTicketsByRoleAsync(role, userId, companyId))
                                     .Where(t => t.ProjectId == projectId)
                                     .ToList();

                return tickets;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Ticket>> GetProjectTicketsByStatusAsync(string statusName, int companyId, int projectId)
        {
            var tickets = new List<Ticket>();

            try
            {
                tickets = (await GetAllTicketsByStatusAsync(companyId, statusName))
                                     .Where(t => t.ProjectId == projectId)
                                     .ToList();

                return tickets;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<Ticket>> GetProjectTicketsByTypeAsync(string typeName, int companyId, int projectId)
        {
            throw new NotImplementedException();
        }

        public async Task<Ticket> GetTicketByIdAsync(int ticketId)
        {
            return await _context.Tickets.FirstOrDefaultAsync(t => t.Id == ticketId);
        }

        public async Task<BTUser> GetTicketDeveloperAsync(int ticketId, int companyId)
        {
            var developer = new BTUser();

            try
            {
                var ticket = (await GetAllTicketsByCompanyAsync(companyId))
                                     .FirstOrDefault(t => t.Id == ticketId);

                if (ticket?.DeveloperUserId != null)
                {
                    developer = ticket.DeveloperUser;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return developer;
        }

        public async Task<List<Ticket>> GetTicketsByRoleAsync(string role, string userId, int companyId)
        {
            var tickets = new List<Ticket>();

            try
            {
                if (role == Roles.Admin.ToString())
                {
                    tickets = await GetAllTicketsByCompanyAsync(companyId); // Admin can see all tickets
                }
                else if (role == Roles.Developer.ToString())
                {
                    tickets = (await GetAllTicketsByCompanyAsync(companyId))
                                     .Where(t => t.DeveloperUserId == userId)
                                     .ToList(); // Developer can see tickets assigned to them
                }
                else if (role == Roles.Submitter.ToString())
                {
                    tickets = (await GetAllTicketsByCompanyAsync(companyId))
                                     .Where(t => t.OwnerUserId == userId)
                                     .ToList(); // Submitter can see tickets they created
                }
                else if (role == Roles.ProjectManager.ToString())
                {
                    tickets = await GetTicketsByUserIdAsync(userId, companyId); // Project manager can see tickets for projects they manage
                }

                return tickets;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Ticket>> GetTicketsByUserIdAsync(string userId, int companyId)
        {
            var btUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var tickets = new List<Ticket>();

            try
            {
                if (await _rolesService.IsUserInRoleAsync(btUser, Roles.Admin.ToString()))
                {
                    tickets = (await _projectService.GetAllProjectsByCompanyAsync(companyId))
                                                    .SelectMany(p => p.Tickets)
                                                    .ToList();
                }
                else if (await _rolesService.IsUserInRoleAsync(btUser, Roles.Developer.ToString()))
                {
                    tickets = (await _projectService.GetAllProjectsByCompanyAsync(companyId))
                                                    .SelectMany(p => p.Tickets)
                                                    .Where(t => t.DeveloperUserId == userId)
                                                    .ToList();
                }
                else if (await _rolesService.IsUserInRoleAsync(btUser, Roles.Submitter.ToString()))
                {
                    tickets = (await _projectService.GetAllProjectsByCompanyAsync(companyId))
                                                    .SelectMany(p => p.Tickets)
                                                    .Where(t => t.OwnerUserId == userId)
                                                    .ToList();
                }
                else if (await _rolesService.IsUserInRoleAsync(btUser, Roles.ProjectManager.ToString()))
                {
                    tickets = (await _projectService.GetUserProjectsAsync(userId))
                                                    .SelectMany(p => p.Tickets)
                                                    .ToList();
                }

                return tickets;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int?> LookupTicketPriorityIdAsync(string priorityName)
        {
            try
            {
                var priority = await _context.TicketPriorities
                                             .FirstOrDefaultAsync(p => p.Name == priorityName);

                return priority?.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int?> LookupTicketStatusIdAsync(string statusName)
        {
            try
            {
                var status = await _context.TicketStatuses
                                           .FirstOrDefaultAsync(s => s.Name == statusName);

                return status?.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int?> LookupTicketTypeIdAsync(string typeName)
        {
            try
            {
                var type = await _context.TicketTypes
                                         .FirstOrDefaultAsync(t => t.Name == typeName);

                return type?.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateTicketAsync(Ticket ticket)
        {
            _context.Update(ticket);
            await _context.SaveChangesAsync();
        }
    }
}