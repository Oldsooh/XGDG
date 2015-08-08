using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WitBird.Sex.Web.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["UserId"] != null)
            {
                return Redirect("/admin/");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(string username, string password)
        {
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                if (username.Length < 17 && password.Length < 17)
                {
                    try
                    {
                        if (username.ToLower().Equals("admin"))
                        {
                            string adminUsername = System.Configuration.ConfigurationManager.ConnectionStrings["AdminUsername"].ConnectionString;
                            string adminPassword = System.Configuration.ConfigurationManager.ConnectionStrings["AdminPassword"].ConnectionString;
                            if (username == adminUsername && password == adminPassword)
                            {
                                Session["UserId"] = "admin";
                                return Redirect("/admin/");
                            }
                        }
                    }
                    catch
                    {
                        //To Do
                    }
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session.Clear();
            return Redirect("/admin/login");
        }
    }
}
