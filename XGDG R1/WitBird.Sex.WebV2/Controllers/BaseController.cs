﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace WitBird.Sex.WebV2.Controllers
{
    public class BaseController : Controller
    {
        public void Internationalization()
        {
            //if (Request.Cookies["SelectedLanguage"] != null)
            //{
            //    HttpCookie lanCookie = Request.Cookies["SelectedLanguage"];
            //    //从Cookie里面读取
            //    string language = lanCookie["lan"];
            //    //当前线程的语言采用哪种语言（比如zh，en等）
            //    Thread.CurrentThread.CurrentUICulture = new
            //        System.Globalization.CultureInfo(language);
            //    //决定各种数据类型是如何组织，如数字与日期
            //    Thread.CurrentThread.CurrentCulture =
            //        System.Globalization.CultureInfo.CreateSpecificCulture(language);
            //}
            //else
            //{
            //    HttpCookie lanCookie = new HttpCookie("SelectedLanguage");
            //    //默认为中文
            //    lanCookie["lan"] = "zh-cn";
            //    Response.Cookies.Add(lanCookie);
            //}
        }
        public ActionResult SetLanguage(string language)
        {
            //if (string.IsNullOrEmpty(language))
            //{
            //    language = "zh-cn";
            //}
            //HttpCookie lanCookie = Request.Cookies["SelectedLanguage"];
            //lanCookie["lan"] = language;
            //Response.Cookies.Add(lanCookie);
            ////刷新当前页面
            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}
