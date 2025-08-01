﻿using BugTrackerMVC.Models;

namespace BugTrackerMVC.Interfaces
{
    public interface IBTTicketHistoryService
    {
        Task AddHistoryAsync(Ticket oldTicket, Ticket newTicket, string userId);
        Task<List<TicketHistory>> GetProjectTicketsHistoriesAsync(int projectId, int companyId);
        Task<List<TicketHistory>> GetCompanyTicketHistoriesAsync(int companyId);
    }
}