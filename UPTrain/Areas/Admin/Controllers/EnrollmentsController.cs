using Microsoft.AspNetCore.Mvc;

namespace UPTrain.Areas.Admin.Controllers
{
    public class EnrollmentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
