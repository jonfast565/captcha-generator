using System.Web.Mvc;

namespace CaptchaGenerator.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "CaptchaGenerator";
            return View();
        }
    }
}
