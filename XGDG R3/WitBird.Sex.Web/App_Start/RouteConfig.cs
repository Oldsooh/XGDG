using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WitBird.Sex.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //展示图片(第一张)
            routes.MapRoute(
                name: "Show1",
                url: "{category}/{id}.html",
                defaults: new { controller = "Album", action = "Show" },
                constraints: new { category = @"damimi|dapigu|nenmeimei|show", id = @"[\d]{0,8}" },
                namespaces: new string[] { "WitBird.Sex.Web.Controllers" }
            );

            //展示图片（从第二张起）
            routes.MapRoute(
                name: "Show2",
                url: "{category}/{id}-{page}.html",
                defaults: new { controller = "Album", action = "Show", page = "1" },
                constraints: new { category = @"damimi|dapigu|nenmeimei|show", id = @"[\d]{0,8}", page = @"[\d]{0,5}" },
                namespaces: new string[] { "WitBird.Sex.Web.Controllers" }
            );

            //图片分类
            routes.MapRoute(
                name: "Category",
                url: "{category}/{page}",
                defaults: new { controller = "Album", action = "Category", page = "1" },
                constraints: new { category = @"damimi|dapigu|nenmeimei", page = @"[\d]{0,5}" },
                namespaces: new string[] { "WitBird.Sex.Web.Controllers" }
            );

            //新闻查看
            routes.MapRoute(
                name: "newsshow",
                url: "{category}/{id}.html",
                defaults: new { controller = "News", action = "Show" },
                constraints: new { category = @"news|yulebagua|qingganliangxing|meironghufu|jiankangyangshen|jianfeijianshen|shishangchaoliu", id = @"[\d]{0,8}" },
                namespaces: new string[] { "WitBird.Sex.Web.Controllers" }
            );
            //新闻分类
            routes.MapRoute(
                name: "newspage",
                url: "{category}/{page}",
                defaults: new { controller = "News", action = "Index", page = "1" },
                constraints: new { category = @"news|yulebagua|qingganliangxing|meironghufu|jiankangyangshen|jianfeijianshen|shishangchaoliu", page = @"[\d]{0,5}" },
                namespaces: new string[] { "WitBird.Sex.Web.Controllers" }
            );

            //搜索
            routes.MapRoute(
                name: "Search",
                url: "search/{keywords}/{page}",
                defaults: new { controller = "Search", action = "Index", page = "1" },
                constraints: new { page = @"[\d]{0,5}" },
                namespaces: new string[] { "WitBird.Sex.Web.Controllers" }
            );


            //默认
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                constraints: null,
                namespaces: new string[] { "WitBird.Sex.Web.Controllers" }
            );
        }
    }
}