using System.Web.Mvc;
using WebApplication1.Attributes;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Authorize]
        public ActionResult Forbidden()
        {
            return View();
        }
        
        [Authorize]
        public ActionResult Protected()
        {
            ViewBag.Message = "Protected Page!";
            return View();
        }

        [RoleAuthorize(Roles = "readonly")]
        public ActionResult Readonly()
        {
            ViewBag.Message = "Readonly Page";
            return View();
        }

        [RoleAuthorize(Roles = "admin")]

        public ActionResult Admin()
        {
            ViewBag.Message = "Admin Page";
            return View();
        }
    }
}