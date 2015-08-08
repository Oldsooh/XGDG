using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WitBird.Sex.BLL;
using WitBird.Sex.Web.Models;

namespace WitBird.Sex.Web.Controllers
{
    public class NewsController : BaseController
    {
        /// <summary>
        /// 新闻首页
        /// </summary>
        public ActionResult Index(string page)
        {
            this.Internationalization();

            NewsModel model = new NewsModel();

            if (string.IsNullOrEmpty(page))
            {
                page = "1";
            }

            NewsService newsService = new NewsService();
            model.Page = Int32.Parse(page);

            model.PageSize = 10;//每页显示10张
            model.PageIndex = Int32.Parse(page);
            int count = 0;
            //视频列表
            model.NewsList = newsService.GetNewsList(null, null, model.PageSize, model.PageIndex, out count);
            //分页
            if (model.NewsList != null && model.NewsList.Count > 0)
            {
                model.PageStep = 10; //页码10个
                model.AllCount = count;
                if (model.AllCount % model.PageSize == 0)
                {
                    model.PageCount = model.AllCount / model.PageSize;
                }
                else
                {
                    model.PageCount = model.AllCount / model.PageSize + 1;
                }
            }

            CategoryService categoryService = new CategoryService();
            model.Category = categoryService.GetCategoryById("news");

            VideoService videoService = new VideoService();
            //猜你喜欢
            //model.Like = videoService.GetLike(16);

            return View(model);
        }

        public ActionResult Show(string id)
        {
            this.Internationalization();

            NewsModel model = new NewsModel();

            if (string.IsNullOrEmpty(id))
            {
                id = "1";
            }

            NewsService newsService = new NewsService();
            model.News = newsService.GetNewsById(Int32.Parse(id));

            if (model.News == null)
            {
                model.News = new Model.News();
                model.News.Title = "该新闻不存在或已被删除";
            }
            else
            {
                AlbumService albumService = new AlbumService();
                int outCount = 0;
                model.RelatedAlbum = albumService.GetAlbumList(string.Empty, model.News.Keywords, 5, 1, out outCount);
                model.RelatedNews = newsService.GetRelatedNews(model.News.Id);
                model.LastNews = newsService.GetPrvNewsById(model.News.Id);
                model.NextNews = newsService.GetNextNewsById(model.News.Id);
            }

            return View(model);
        }

        public PartialViewResult NewsRight()
        {
            NewsRightModel model = new NewsRightModel();
            NewsService newsService = new NewsService();

            model.RecommendNews = newsService.GetRecommendNews();
            model.TopNews = newsService.GetHotNews();

            return PartialView(model);
        }

        public PartialViewResult HotKeywords()
        {
            return PartialView();
        }
    }
}
