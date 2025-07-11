﻿using BugTrackerMVC.Models;

namespace BugTrackerMVC.Interfaces
{
    public interface IBTCompanyInfoService
    {
        Task<Company> GetCompanyInfoByIdAsync(int? companyId);
        Task<List<BTUser>> GetAllMembersAsync(int companyId);
        Task<List<Project>> GetAllProjectsAsync(int companyId);
        Task<List<Ticket>> GetAllTicketsAsync(int companyId);
    }
}