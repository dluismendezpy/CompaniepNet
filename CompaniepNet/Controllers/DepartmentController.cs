using Microsoft.AspNetCore.Mvc;

namespace CompaniepNet.Controllers
{
    public class DepartmentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
