using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WitBird.Sex.Web.Areas.Admin.Controllers
{
    public class IndexController : Controller
    {
        public ActionResult Index()
        {
            if (Session["UserId"] == null)
            {
                return Redirect("/admin/login");
            }
            return View();
        }
    }
}
