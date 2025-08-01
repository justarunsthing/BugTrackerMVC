using BugTrackerMVC.Models;

namespace BugTrackerMVC.Interfaces
{
    public interface IBTLookupService
    {
        Task<List<TicketPriority>> GetTicketPrioritiesAsync();
        Task<List<TicketStatus>> GetTicketStatusesAsync();
        Task<List<TicketType>> GetTicketTypesAsync();
        Task<List<ProjectPriority>> GetProjectPrioritiesAsync();
    }
}