using System.Diagnostics;
using BugTrackerMVC.Models;
using Microsoft.AspNetCore.Mvc;
using BugTrackerMVC.Extensions;
using BugTrackerMVC.Interfaces;
using BugTrackerMVC.ViewModels;

namespace BugTrackerMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBTCompanyInfoService _companyInfoService;
        private readonly IBTProjectService _projectService;

        public HomeController(ILogger<HomeController> logger, IBTCompanyInfoService companyInfoService, IBTProjectService projectService)
        {
            _logger = logger;
            _companyInfoService = companyInfoService;
            _projectService = projectService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Dashboard()
        {
            int companyId = User.Identity.GetCompanyId().Value;
            var model = new DashboardViewModel
            {
                Company = await _companyInfoService.GetCompanyInfoByIdAsync(companyId),
                Projects = (await _companyInfoService.GetAllProjectsAsync(companyId)).Where(p => p.IsArchived == false).ToList()
            };
            model.Tickets = model.Projects.SelectMany(p => p.Tickets).Where(t => t.IsArchived == false).ToList();
            model.Members = model.Company.Members.ToList();

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<JsonResult> GglProjectTickets()
        {
            int companyId = User.Identity.GetCompanyId().Value;

            List<Project> projects = await _projectService.GetAllProjectsByCompanyAsync(companyId);

            List<object> chartData = new();
            chartData.Add(new object[] { "ProjectName", "TicketCount" });

            foreach (Project prj in projects)
            {
                chartData.Add(new object[] { prj.Name, prj.Tickets.Count() });
            }

            return Json(chartData);
        }
    }
}