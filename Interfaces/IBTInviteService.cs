﻿using BugTrackerMVC.Models;

namespace BugTrackerMVC.Interfaces
{
    public interface IBTInviteService
    {
        Task<bool> AcceptInviteAsync(Guid? token, string userId, int companyId);
        Task AddNewInviteAsync(Invite invite);
        Task<bool> AnyInviteAsync(Guid token, string email, int companyId);
        Task<Invite> GetInviteAsync(int inviteId, int companyId);
        Task<Invite> GetInviteAsync(Guid token, string email, int companyId);
        Task<bool> ValidateInviteCodeAsync(Guid? token);
    }
}