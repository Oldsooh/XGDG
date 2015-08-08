using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WitBird.Sex.BLL;
using WitBird.Sex.Model;
using WitBird.Sex.Web.Areas.Admin.Models;

namespace WitBird.Sex.Web.Areas.Admin.Controllers
{
    public class NovelController : Controller
    {
        //
        // GET: /Admin/Novel/

        public ActionResult Index(string categoryId, string keywords, string page)
        {
            if (Session["UserId"] == null)
            {
                return Redirect("/admin/login");
            }

            NovelService novelService = new NovelService();

            int tempPage = 1;

            if (!string.IsNullOrEmpty(page))
            {
                Int32.TryParse(page, out tempPage);
            }

            NovelModel model = new NovelModel();

            //当前选中小说类别
            model.NovelCategories = novelService.GetNovelCategories();
            if (!string.IsNullOrEmpty(categoryId))
            {
                if (model.NovelCategories != null && model.NovelCategories.Count > 0)
                {
                    foreach (var item in model.NovelCategories)
                    {
                        if (item.Id == categoryId)
                        {
                            model.NovelCategory = item;
                            break;
                        }
                    }
                }
            }

            //小说列表
            int tempCount = 0;
            model.PageIndex = 1;
            model.Novels = novelService.GetNovelList(categoryId, keywords, 15, tempPage, out tempCount);
            //分页
            if (model.Novels != null && model.Novels.Count > 0)
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

            if (model.NovelCategory == null)
            {
                model.NovelCategory = new NovelCategory();
            }

            return View(model);
        }

        public ActionResult Category()
        {
            if (Session["UserId"] == null)
            {
                return Redirect("/admin/login");
            }

            NovelModel model = new NovelModel();

            NovelService novelService = new NovelService();
            model.NovelCategories = novelService.GetNovelCategories();

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddCategory(NovelCategory category)
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
                    NovelService novelService = new NovelService();
                    novelService.AddNovelCategory(category);
                }
            }

            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditCategory(NovelCategory category)
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
                    NovelService novelService = new NovelService();
                    novelService.EditNovelCategory(category);
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

            NovelService novelService = new NovelService();

            novelService.RemoveNovelCategory(id);

            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }

        [HttpGet]
        public ActionResult Add()
        {
            if (Session["UserId"] == null)
            {
                return Redirect("/admin/login");
            }

            NovelModel model = new NovelModel();

            NovelService novelService = new NovelService();
            model.NovelCategories = novelService.GetNovelCategories();

            return View(model);
        }

        [HttpPost]
        public ActionResult Add(Novel novel)
        {
            string result = "发布失败";

            if (Session["UserId"] == null)
            {
                return Content("长时间未操作，请重新登录。");
            }

            if (novel == null)
            {
                result = "参数为空";
            }
            else
            {
                if (!string.IsNullOrEmpty(novel.Title) &&
                    !string.IsNullOrEmpty(novel.CategoryId) &&
                    !string.IsNullOrEmpty(novel.Name))
                {
                    novel.ArticleCount = 0;
                    novel.InsertTime = DateTime.Now;
                    novel.ViewTime = 0;
                    novel.IsDelete = false;
                    NovelService novelService = new NovelService();
                    result = novelService.AddNovel(novel);
                }
                else
                {
                    result = "必填项 [标题、分类、名称、作者] 不能为空";
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

            NovelModel model = new NovelModel();

            NovelService novelService = new NovelService();
            model.NovelCategories = novelService.GetNovelCategories();
            model.Novel = novelService.GetNovelById(id);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Novel novel)
        {
            string result = "更新失败";

            if (Session["UserId"] == null)
            {
                return Content("长时间未操作，请重新登录。");
            }

            if (novel == null)
            {
                result = "参数为空";
            }
            else
            {
                if (!string.IsNullOrEmpty(novel.Title) &&
                    !string.IsNullOrEmpty(novel.CategoryId) &&
                    !string.IsNullOrEmpty(novel.Name))
                {
                    NovelService novelService = new NovelService();

                    Novel oldNovel = novelService.GetNovelById(novel.Id);
                    if (oldNovel != null)
                    {
                        oldNovel.CategoryId = string.IsNullOrEmpty(novel.CategoryId) ? string.Empty : novel.CategoryId;
                        oldNovel.Title = string.IsNullOrEmpty(novel.Title) ? string.Empty : novel.Title;
                        oldNovel.Keywords = string.IsNullOrEmpty(novel.Keywords) ? string.Empty : novel.Keywords;
                        oldNovel.Description = string.IsNullOrEmpty(novel.Description) ? string.Empty : novel.Description;
                        oldNovel.Name = string.IsNullOrEmpty(novel.Name) ? string.Empty : novel.Name;
                        oldNovel.Author = string.IsNullOrEmpty(novel.Author) ? string.Empty : novel.Author;
                        result = novelService.EditNovel(novel);
                    }
                }
                else
                {
                    result = "必填项 [标题、分类、名称、作者] 不能为空";
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

            NovelService novelService = new NovelService();

            novelService.RemoveNovel(id);

            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }

        [HttpGet]
        public ActionResult Article(int id)
        {
            if (Session["UserId"] == null)
            {
                return Redirect("/admin/login");
            }

            NovelModel model = new NovelModel();

            NovelService novelService = new NovelService();
            model.Novel = novelService.GetNovelById(id);
            model.NovelArticles = novelService.GetNovelArticles(id);

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddArticle(NovelArticle article)
        {
            string result = "发布失败";

            if (Session["UserId"] == null)
            {
                return Content("长时间未操作，请重新登录。");
            }

            if (article == null)
            {
                result = "参数为空";
            }
            else
            {
                if (!string.IsNullOrEmpty(article.Title) &&
                    !string.IsNullOrEmpty(article.ContentStyle) &&
                    article.NovelId > 0)
                {
                    NovelService novelService = new NovelService();
                    result = novelService.AddNovelArticle(article);
                }
                else
                {
                    result = "必填项 [标题、内容] 不能为空";
                }
            }

            return Content(result);
        }
    }
}
