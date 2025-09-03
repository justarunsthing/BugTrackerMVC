using System.Diagnostics;
using BugTrackerMVC.Models;
using Microsoft.AspNetCore.Mvc;
using BugTrackerMVC.Extensions;
using BugTrackerMVC.Interfaces;

namespace BugTrackerMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBTCompanyInfoService _companyInfoService;

        public HomeController(ILogger<HomeController> logger, IBTCompanyInfoService companyInfoService)
        {
            _logger = logger;
            _companyInfoService = companyInfoService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Dashboard()
        {
            int companyId = User.Identity.GetCompanyId().Value;

            return View();
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
    }
}