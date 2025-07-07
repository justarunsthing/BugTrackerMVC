using BugTrackerMVC.Models;
using BugTrackerMVC.Interfaces;
using BugTrackerMVC.Data;
using Microsoft.EntityFrameworkCore;

namespace BugTrackerMVC.Services
{
    public class BTCompanyInfoService : IBTCompanyInfoService
    {
        private readonly ApplicationDbContext _context;

        public BTCompanyInfoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<BTUser>> GetAllMembersAsync(int companyId)
        {
            var result = await _context.Users.Where(u => u.CompanyId == companyId).ToListAsync();

            return result;
        }

        public async Task<List<Project>> GetAllProjectsAsync(int companyId)
        {
            var result = await _context.Projects.Where(p => p.CompanyId == companyId)
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

        public async Task<List<Ticket>> GetAllTicketsAsync(int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<Company> GetCompanyInfoByIdAsync(int? companyId)
        {
            throw new NotImplementedException();
        }
    }
}