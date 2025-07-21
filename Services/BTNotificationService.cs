using BugTrackerMVC.Data;
using BugTrackerMVC.Models;
using BugTrackerMVC.Interfaces;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace BugTrackerMVC.Services
{
    public class BTNotificationService : IBTNotificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly IBTRolesService _rolesService;

        public BTNotificationService(ApplicationDbContext context, IEmailSender emailSender, IBTRolesService rolesService)
        {
            _context = context;
            _emailSender = emailSender;
            _rolesService = rolesService;
        }

        public Task AddNotificationAsync(Notification notification)
        {
            throw new NotImplementedException();
        }

        public Task<List<Notification>> GetReceivedNotificationsAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Notification>> GetSentNotificationsAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task SendEmailNotificationAsync(Notification notification, string emailSubject)
        {
            throw new NotImplementedException();
        }

        public Task SendEmailNotificationsByRoleAsync(Notification notification, int companyId, string role)
        {
            throw new NotImplementedException();
        }

        public Task SendMembersEmailNotificationsAsync(Notification notification, List<BTUser> members)
        {
            throw new NotImplementedException();
        }
    }
}