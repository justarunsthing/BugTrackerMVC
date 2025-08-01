using BugTrackerMVC.Models;
using BugTrackerMVC.Interfaces;

namespace BugTrackerMVC.Services
{
    public class BTLookupService : IBTLookupService
    {
        public Task<List<ProjectPriority>> GetProjectPrioritiesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<TicketPriority>> GetTicketPrioritiesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<TicketStatus>> GetTicketStatusesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<TicketType>> GetTicketTypesAsync()
        {
            throw new NotImplementedException();
        }
    }
}