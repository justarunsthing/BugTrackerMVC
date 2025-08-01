﻿using BugTrackerMVC.Data;
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
            var invite = await _context.Invites.FirstOrDefaultAsync(i => i.CompanyToken == token);

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
                                                   .AnyAsync(i => i.CompanyToken == token && i.InviteeEmail == email);

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

        public async Task<Invite> GetInviteAsync(Guid token, string email, int companyId)
        {
            try
            {
                var invite = await _context.Invites.Where(i => i.CompanyId == companyId)
                                                   .Include(i => i.Company)
                                                   .Include(i => i.Project)
                                                   .Include(i => i.Invitor)
                                                   .FirstOrDefaultAsync(i => i.CompanyToken == token && i.InviteeEmail == email);

                return invite;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> ValidateInviteCodeAsync(Guid? token)
        {
            if (token == null)
            {
                return false;
            }

            var result = false;
            var invite = await _context.Invites.FirstOrDefaultAsync(i => i.CompanyToken == token);

            if (invite != null)
            {
                // Deterime invite date
                var inviteDate = invite.InviteDate.DateTime;

                // Custom validation of invite based on the date it was issued
                // In this case we are allowing an invite to be valid for 7 days
                bool isValid = (DateTime.Now - inviteDate).TotalDays <= 7;

                if (isValid)
                {
                    result = invite.IsValid;
                }
            }

            return result;
        }
    }
}