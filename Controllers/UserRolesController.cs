using BugTrackerMVC.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BugTrackerMVC.Controllers
{
    public class UserRolesController : Controller
    {
        private readonly IBTRolesService _rolesService;
        private readonly IBTCompanyInfoService _companyInfoService;

        public UserRolesController(IBTRolesService rolesService, IBTCompanyInfoService companyInfoService)
        {
            _rolesService = rolesService;
            _companyInfoService = companyInfoService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}