using Microsoft.AspNetCore.Mvc;

namespace BugTrackerMVC.Controllers
{
    public class UserRolesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}