using System.Web.Mvc;
using Microsoft.SharePoint;

namespace FLS.Sharepoint.MVC.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var lists = SPContext.Current.Web.Lists;
            ViewData.Model = lists;
            return View();
        }
    }
}
