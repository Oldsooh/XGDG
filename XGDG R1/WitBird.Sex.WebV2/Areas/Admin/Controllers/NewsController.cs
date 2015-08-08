using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WitBird.Sex.BLL;
using WitBird.Sex.Model;
using WitBird.Sex.WebV2.Areas.Admin.Models;

namespace WitBird.Sex.WebV2.Areas.Admin.Controllers
{
    public class NewsController : Controller
    {
        public ActionResult Index(string categoryId, string keywords, string page)
        {
            if (Session["UserId"] == null)
            {
                return Redirect("/admin/login");
            }

            NewsService newsService = new NewsService();

            int tempPage = 1;

            if (!string.IsNullOrEmpty(page))
            {
                Int32.TryParse(page, out tempPage);
            }

            NewsModel model = new NewsModel();

            //新闻类别列表
            model.NewsCategories = newsService.GetNewsCategories();

            //当前选中新闻类别
            model.NewsCategory = new NewsCategory();
            if (!string.IsNullOrEmpty(categoryId))
            {
                if (model.NewsCategories != null && model.NewsCategories.Count > 0)
                {
                    foreach (var item in model.NewsCategories)
                    {
                        if (item.Id == categoryId)
                        {
                            model.NewsCategory = item;
                            break;
                        }
                    }
                }
            }

            //新闻列表
            int tempCount = 0;
            model.NewsList = newsService.GetNewsList(categoryId, keywords, 15, tempPage, out tempCount);
            //分页
            if (model.NewsList != null && model.NewsList.Count > 0)
            {
                model.PageIndex = tempPage;
                model.PageSize = 15;
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

            return View(model);
        }

        [HttpGet]
        public ActionResult Add()
        {
            if (Session["UserId"] == null)
            {
                return Redirect("/admin/login");
            }

            NewsModel model = new NewsModel();

            NewsService newsService = new NewsService();
            model.NewsCategories = newsService.GetNewsCategories();

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(News news)
        {
            string result = "发布失败";

            if (Session["UserId"] == null)
            {
                return Content("长时间未操作，请重新登录。");
            }

            if (news == null)
            {
                result = "参数为空";
            }
            else
            {
                if (!string.IsNullOrEmpty(news.Title) &&
                    !string.IsNullOrEmpty(news.CategoryId) &&
                    !string.IsNullOrEmpty(news.ContentStyle))
                {
                    news.ContentText = news.ContentStyle;
                    news.InsertTime = DateTime.Now;
                    news.UpdateTime = news.InsertTime;
                    news.ViewCount = 0;
                    news.IsActive = true;
                    NewsService newsService = new NewsService();
                    result = newsService.AddNews(news);
                }
                else
                {
                    result = "必填项 [标题、分类、内容] 不能为空";
                }
            }

            return Content(result);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Session["UserId"] == null)
            {
                return Redirect("/admin/login");
            }

            NewsModel model = new NewsModel();

            NewsService newsService = new NewsService();

            model.NewsCategories = newsService.GetNewsCategories();
            model.News = newsService.GetNewsById(id);
            if (model.News != null && !model.News.IsActive)
            {
                model.News = null;
            }
            if (model.News == null)
            {
                model.News = new News();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(News news)
        {
            string result = "更新失败";

            if (Session["UserId"] == null)
            {
                return Content("长时间未操作，请重新登录。");
            }

            if (news == null)
            {
                result = "参数为空";
            }
            else
            {
                if (!string.IsNullOrEmpty(news.Title) &&
                    !string.IsNullOrEmpty(news.CategoryId) &&
                    !string.IsNullOrEmpty(news.ContentStyle))
                {
                    news.ContentText = news.ContentStyle;
                    news.UpdateTime = DateTime.Now;
                    news.IsActive = true;

                    if (news.Keywords == null)
                    {
                        news.Keywords = string.Empty;
                    }
                    if (news.Description == null)
                    {
                        news.Description = string.Empty;
                    }
                    if (news.Author == null)
                    {
                        news.Author = string.Empty;
                    }
                    if (news.ComeFrom == null)
                    {
                        news.ComeFrom = string.Empty;
                    }
                    if (news.Cover == null)
                    {
                        news.Cover = string.Empty;
                    }
                    if (news.PictureGroup == null)
                    {
                        news.PictureGroup = string.Empty;
                    }

                    NewsService newsService = new NewsService();
                    result = newsService.EditNews(news);
                }
                else
                {
                    result = "必填项 [标题、分类、内容] 不能为空";
                }
            }

            return Content(result);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (Session["UserId"] == null)
            {
                return Redirect("/admin/login");
            }

            NewsService newsService = new NewsService();

            newsService.RemoveNews(id);

            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }

        [HttpGet]
        public ActionResult Category()
        {
            if (Session["UserId"] == null)
            {
                return Redirect("/admin/login");
            }

            NewsModel model = new NewsModel();

            NewsService newsService = new NewsService();
            model.NewsCategories = newsService.GetNewsCategories();

            return View(model);
        }

        [HttpPost]
        public ActionResult AddCategory(NewsCategory category)
        {
            if (Session["UserId"] == null)
            {
                return Redirect("/admin/login");
            }

            if (category != null)
            {
                if (!string.IsNullOrEmpty(category.Id) &&
                    !string.IsNullOrEmpty(category.Name) &&
                    category.Sort > 0)
                {
                    category.IsActive = true;
                    NewsService newsService = new NewsService();
                    newsService.AddNewsCategory(category);
                    Cache.UpdateNewsCategories();//更新缓存
                }
            }

            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditCategory(NewsCategory category)
        {
            if (Session["UserId"] == null)
            {
                return Redirect("/admin/login");
            }

            if (category != null)
            {
                if (!string.IsNullOrEmpty(category.Id) &&
                    !string.IsNullOrEmpty(category.Name) &&
                    category.Sort > 0)
                {
                    category.IsActive = true;
                    NewsService newsService = new NewsService();
                    newsService.EditNewsCategory(category);
                    Cache.UpdateNewsCategories();//更新缓存
                }
            }

            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }

        [HttpGet]
        public ActionResult DeleteCategory(string id)
        {
            if (Session["UserId"] == null)
            {
                return Redirect("/admin/login");
            }

            NewsService newsService = new NewsService();

            newsService.RemoveNewsCategory(id);

            Cache.UpdateNewsCategories();//更新缓存

            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }
    }
}
