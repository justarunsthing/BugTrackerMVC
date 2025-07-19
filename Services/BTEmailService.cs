using Microsoft.AspNetCore.Identity.UI.Services;

namespace BugTrackerMVC.Services
{
    public class BTEmailService : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            throw new NotImplementedException();
        }
    }
}