using BugTrackerMVC.Models;
using BugTrackerMVC.Interfaces;

namespace BugTrackerMVC.Services
{
    public class BTCompanyInfoService : IBTCompanyInfoService
    {
        public Task<List<BTUser>> GetAllMembersAsync(int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Project>> GetAllProjectsAsync(int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Ticket>> GetAllTicketsAsync(int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<Company> GetCompanyInfoByIdAsync(int? companyId)
        {
            throw new NotImplementedException();
        }
    }
}