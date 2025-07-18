using BugTrackerMVC.Data;
using BugTrackerMVC.Models;
using BugTrackerMVC.Interfaces;

namespace BugTrackerMVC.Services
{
    public class BTTicketHistoryService : IBTTicketHistoryService
    {
        private readonly ApplicationDbContext _context;

        public BTTicketHistoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task AddHistoryAsync(Ticket oldTicket, Ticket newTicket, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<TicketHistory>> GetCompanyTicketHistoriesAsync(int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<List<TicketHistory>> GetProjectTicketsHistoriesAsync(int projectId, int companyId)
        {
            throw new NotImplementedException();
        }
    }
}