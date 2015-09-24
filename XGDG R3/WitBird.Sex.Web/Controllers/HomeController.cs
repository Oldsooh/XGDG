using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WitBird.Sex.Web.Models;
using System.Threading.Tasks;

namespace WitBird.Sex.Web.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            this.Internationalization();

            //HomeModel model = Cache.HomeData; //此model仅使用 IsForeignLanguages 一个参数

            //if (Request.Cookies["SelectedLanguage"] != null)
            //{
            //    HttpCookie lanCookie = Request.Cookies["SelectedLanguage"];
            //    //从Cookie里面读取
            //    string language = lanCookie["lan"];
            //    if (!string.IsNullOrEmpty(language))
            //    {
            //        if (language != "zh-cn")
            //        {
            //            model.IsForeignLanguages = true;
            //        }
            //    }
            //}
            //else
            //{
            //    model.IsFirstVisit = true;
            //}
            //model.IsFirstVisit = true;
            return View();
        }

        //直接打开首页读取缓存数据
        public ActionResult IndexDeferLoad()
        {
            this.Internationalization();
            HomeModel model = Cache.HomeData;

            if (model == null)
            {
                model = new HomeModel();
            }
            return PartialView(model);
        }

        public PartialViewResult FriendlyLink()
        {
            return PartialView();
        }

        public ActionResult AboutUs()
        {
            return View();
        }
    }
}
