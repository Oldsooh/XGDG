using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WitBird.Sex.BLL;
using WitBird.Sex.Model;
using WitBird.Sex.Web.Models;

namespace WitBird.Sex.Web.Controllers
{
    public class SearchController : BaseController
    {
        //
        // GET: /Search/

        public ActionResult Index(string keywords, string page)
        {
            this.Internationalization();
            SearchModel model = new SearchModel();

            AlbumService albumService = new AlbumService();
            NewsService newsService = new NewsService();

            int tempPage = 1;

            if (!string.IsNullOrEmpty(page))
            {
                Int32.TryParse(page, out tempPage);
            }

            //专辑列表
            int tempCount = 0;
            int tempCount1 = 0;
            int pageSize = 24;

            model.NewsResult = newsService.GetNewsList(string.Empty, keywords, 6, tempPage, out tempCount);

            if (tempCount > 0 && model.NewsResult != null && model.NewsResult.Count > 6)
            {
                pageSize = 8;
            }

            model.AlbumResult = albumService.GetAlbumList(null, keywords, pageSize, tempPage, out tempCount1);
            model.TotalResultsCount = tempCount + tempCount1;
            model.TotalNewsCount = tempCount;
            model.TotalAlbumCount = tempCount1;

            if (tempCount1 == 0 && tempCount < 10)
            {
                model.RecommendAlbum = albumService.GetRecommendRandom(12);
            }

            tempCount = tempCount1 > tempCount ? tempCount1 : tempCount;
            //分页
            if ((model.AlbumResult != null && model.AlbumResult.Count > 0) || (model.NewsResult != null && model.NewsResult.Count > 0))
            {
                model.PageIndex = tempPage;
                model.PageSize = pageSize;
                model.PageStep = 10;
                model.AllCount = tempCount;
                if (model.AllCount % model.PageSize == 0)
                {
                    model.PageCount = model.AllCount / model.PageSize;
                }
                else
                {
                    model.PageCount = model.AllCount / model.PageSize + 1;
                }
            }

            //关键字
            model.Keywords = keywords;

            if (model.AlbumResult == null)
            {
                model.AlbumResult = new List<Album>();
            }

            return View(model);
        }

    }
}
