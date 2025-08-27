using BugTrackerMVC.Data;
using BugTrackerMVC.Models;
using BugTrackerMVC.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BugTrackerMVC.Services
{
    public class BTTicketHistoryService : IBTTicketHistoryService
    {
        private readonly ApplicationDbContext _context;

        public BTTicketHistoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddHistoryAsync(Ticket oldTicket, Ticket newTicket, string userId)
        {
            // New ticket added
            if (oldTicket == null && newTicket != null)
            {
                var history = new TicketHistory
                {
                    TicketId = newTicket.Id,
                    Property = "",
                    OldValue = "",
                    NewValue = "",
                    Created = DateTimeOffset.UtcNow,
                    UserId = userId,
                    Description = "New ticket created",
                };

                try
                {
                    await _context.TicketHistories.AddAsync(history);
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                // Check ticket title
                if (oldTicket.Title != newTicket.Title)
                {
                    var history = new TicketHistory
                    {
                        TicketId = newTicket.Id,
                        Property = "Title",
                        OldValue = oldTicket.Title,
                        NewValue = newTicket.Title,
                        Created = DateTimeOffset.UtcNow,
                        UserId = userId,
                        Description = $"New ticket title: {newTicket.Title}",
                    };

                    await _context.TicketHistories.AddAsync(history);
                }

                // Check ticket description
                if (oldTicket.Description != newTicket.Description)
                {
                    var history = new TicketHistory
                    {
                        TicketId = newTicket.Id,
                        Property = "Description",
                        OldValue = oldTicket.Description,
                        NewValue = newTicket.Description,
                        Created = DateTimeOffset.UtcNow,
                        UserId = userId,
                        Description = $"New ticket description: {newTicket.Description}",
                    };

                    await _context.TicketHistories.AddAsync(history);
                }

                // Check ticket priority
                if (oldTicket.TicketPriorityId != newTicket.TicketPriorityId)
                {
                    var history = new TicketHistory
                    {
                        TicketId = newTicket.Id,
                        Property = "TicketPriority",
                        OldValue = oldTicket.TicketPriority.Name,
                        NewValue = newTicket.TicketPriority.Name,
                        Created = DateTimeOffset.UtcNow,
                        UserId = userId,
                        Description = $"New ticket priority: {newTicket.TicketPriority.Name}",
                    };

                    await _context.TicketHistories.AddAsync(history);
                }

                // Check ticket status
                if (oldTicket.TicketStatusId != newTicket.TicketStatusId)
                {
                    var history = new TicketHistory
                    {
                        TicketId = newTicket.Id,
                        Property = "TicketStatus",
                        OldValue = oldTicket.TicketStatus.Name,
                        NewValue = newTicket.TicketStatus.Name,
                        Created = DateTimeOffset.UtcNow,
                        UserId = userId,
                        Description = $"New ticket status: {newTicket.TicketStatus.Name}",
                    };

                    await _context.TicketHistories.AddAsync(history);
                }

                // Check ticket type
                if (oldTicket.TicketTypeId != newTicket.TicketTypeId)
                {
                    var history = new TicketHistory
                    {
                        TicketId = newTicket.Id,
                        Property = "TicketTypeId",
                        OldValue = oldTicket.TicketType.Name,
                        NewValue = newTicket.TicketType.Name,
                        Created = DateTimeOffset.UtcNow,
                        UserId = userId,
                        Description = $"New ticket type: {newTicket.TicketType.Name}",
                    };

                    await _context.TicketHistories.AddAsync(history);
                }

                // Check ticket developer
                if (oldTicket.DeveloperUserId != newTicket.DeveloperUserId)
                {
                    var history = new TicketHistory
                    {
                        TicketId = newTicket.Id,
                        Property = "Developer",
                        OldValue = oldTicket.DeveloperUser?.FullName ?? "Not Assigned",
                        NewValue = newTicket.DeveloperUser?.FullName,
                        Created = DateTimeOffset.UtcNow,
                        UserId = userId,
                        Description = $"New ticket developer: {newTicket.DeveloperUser?.FullName ?? "Not Assigned"}",
                    };

                    await _context.TicketHistories.AddAsync(history);
                }

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public async Task AddHistoryAsync(int ticketId, string model, string userId)
        {
            try
            {
                var ticket = await _context.Tickets.FindAsync(ticketId);
                var description = model.ToLower().Replace("Ticket", "");
                description = $"New {description} added to ticket: {ticket.Title}";
                var history = new TicketHistory
                {
                    TicketId = ticketId,
                    Property = model,
                    OldValue = "",
                    NewValue = "",
                    Created = DateTimeOffset.UtcNow,
                    UserId = userId,
                    Description = description,
                };

                await _context.TicketHistories.AddAsync(history);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<TicketHistory>> GetCompanyTicketHistoriesAsync(int companyId)
        {
            try
            {
                var projects = (await _context.Companies
                                             .Include(c => c.Projects)
                                                .ThenInclude(p => p.Tickets)
                                                    .ThenInclude(t => t.History)
                                                        .ThenInclude(h => h.User)
                                             .FirstOrDefaultAsync(c => c.Id == companyId))?.Projects.ToList();

                var tickets = projects?.SelectMany(p => p.Tickets).ToList();
                var ticketHistories = tickets?.SelectMany(t => t.History).ToList() ?? [];

                return ticketHistories;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<TicketHistory>> GetProjectTicketsHistoriesAsync(int projectId, int companyId)
        {
            try
            {
                var project = await _context.Projects
                                            .Where(p => p.CompanyId == companyId)
                                            .Include(p => p.Tickets)
                                                .ThenInclude(t => t.History)
                                                    .ThenInclude(h => h.User)
                                            .FirstOrDefaultAsync(p => p.Id == projectId);

                var ticketHistory = project?.Tickets.SelectMany(t => t.History).ToList() ?? [];

                return ticketHistory;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}