using BugTrackerMVC.Data;
using BugTrackerMVC.Models;
using BugTrackerMVC.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BugTrackerMVC.Services
{
    public class BTInviteService : IBTInviteService
    {
        private readonly ApplicationDbContext _context;

        public BTInviteService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AcceptInviteAsync(Guid? token, string userId, int companyId)
        {
            var invite = await _context.Invites.FirstOrDefaultAsync(i => i.CompanyToke == token);

            if (invite == null)
            {
                return false;
            }

            try
            {
                invite.IsValid = false; // Invite accepted, no longer available to use
                invite.InviteeId = userId;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task AddNewInviteAsync(Invite invite)
        {
            try
            {
                await _context.Invites.AddAsync(invite);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> AnyInviteAsync(Guid token, string email, int companyId)
        {
            try
            {
                var result = await _context.Invites.Where(i => i.CompanyId == companyId)
                                                   .AnyAsync(i => i.CompanyToke == token && i.InviteeEmail == email);

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Invite> GetInviteAsync(int inviteId, int companyId)
        {
            try
            {
                var invite = await _context.Invites.Where(i => i.CompanyId == companyId)
                                                   .Include(i => i.Company)
                                                   .Include(i => i.Project)
                                                   .Include(i => i.Invitor)
                                                   .FirstOrDefaultAsync(i => i.Id == inviteId);

                return invite;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<Invite> GetInviteAsync(Guid token, string email, int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ValidateInviteCodeAsync(Guid? token)
        {
            throw new NotImplementedException();
        }
    }
}