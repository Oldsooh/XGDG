using WitBird.Sex.WebV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WitBird.Sex.WebV2.Controllers
{
    public class ErrorController : BaseController
    {
        //
        // GET: /Error/

        public ActionResult Index()
        {
            this.Internationalization();
            WitBird.Sex.BLL.NewsService newsService = new BLL.NewsService();
            WitBird.Sex.BLL.AlbumService albumService = new BLL.AlbumService();
            SearchModel model = new SearchModel();
            model.NewsResult = newsService.GetHotNews();
            model.AlbumResult = albumService.GetRecommendRandom(12);
            Response.StatusCode = 404;
            return View(model);
        }
    }
}
