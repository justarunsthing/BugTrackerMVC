using BugTrackerMVC.Data;
using BugTrackerMVC.Models;
using BugTrackerMVC.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BugTrackerMVC.Services
{
    public class BTTicketService : IBTTicketService
    {
        private readonly ApplicationDbContext _context;

        public BTTicketService(ApplicationDbContext context)
        {
            _context = context;
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

        public Task AssignTicketAsync(int ticketId, string userId)
        {
            throw new NotImplementedException();
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

        public Task<List<Ticket>> GetAllTicketsByTypeAsync(int companyId, string typeName)
        {
            throw new NotImplementedException();
        }

        public Task<List<Ticket>> GetArchivedTicketsAsync(int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Ticket>> GetProjectTicketsByPriorityAsync(string priorityName, int companyId, int projectId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Ticket>> GetProjectTicketsByRoleAsync(string role, string userId, int projectId, int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Ticket>> GetProjectTicketsByStatusAsync(string statusName, int companyId, int projectId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Ticket>> GetProjectTicketsByTypeAsync(string typeName, int companyId, int projectId)
        {
            throw new NotImplementedException();
        }

        public async Task<Ticket> GetTicketByIdAsync(int ticketId)
        {
            return await _context.Tickets.FirstOrDefaultAsync(t => t.Id == ticketId);
        }

        public Task<BTUser> GetTicketDeveloperAsync(int ticketId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Ticket>> GetTicketsByRoleAsync(string role, string userId, int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Ticket>> GetTicketsByUserIdAsync(string userId, int companyId)
        {
            throw new NotImplementedException();
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