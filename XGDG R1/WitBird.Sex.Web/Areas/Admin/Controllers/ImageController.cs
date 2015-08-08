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
    public class ImageController : Controller
    {
        CategoryService categoryService = new CategoryService();
        AlbumService albumService = new AlbumService();
        ImageService imageService = new ImageService();
        VideoService videoService = new VideoService();

        public ActionResult Index(string categoryId, string keywords, string page)
        {
            if (Session["UserId"] == null)
            {
                return Redirect("/admin/login");
            }

            AlbumService newsService = new AlbumService();

            int tempPage = 1;

            if (!string.IsNullOrEmpty(page))
            {
                Int32.TryParse(page, out tempPage);
            }

            ImagesModel model = new ImagesModel();

            //专辑类别列表
            model.Categories = categoryService.GetCategories("album");

            //当前选中专辑类别
            model.Category = new Category();
            if (!string.IsNullOrEmpty(categoryId))
            {
                if (model.Categories != null && model.Categories.Count > 0)
                {
                    foreach (var item in model.Categories)
                    {
                        if (item.Id == categoryId)
                        {
                            model.Category = item;
                        }
                    }
                }
            }

            //专辑列表
            int tempCount = 0;
            model.AlbumList = newsService.GetAlbumList(categoryId, keywords, 15, tempPage, out tempCount);
            //分页
            if (model.AlbumList != null && model.AlbumList.Count > 0)
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
        public ActionResult Upload()
        {
            if (Session["UserId"] == null)
            {
                return Redirect("/admin/");
            }
            UploadModel model = new UploadModel();
            model.CategoryList = categoryService.GetCategories("album");
            model.isSuccessful = "";
            model.SelectedCategory = "xinggan";
            return View(model);
        }

        [HttpPost]
        public ActionResult Upload(string c, string t, string k, string d, string isShow, string isTop, string urls)
        {
            if (Session["UserId"] == null)
            {
                return Redirect("/admin/");
            }
            UploadModel model = new UploadModel();
            model.CategoryList = categoryService.GetCategories("album");
            model.isSuccessful = "添加失败";
            model.SelectedCategory = c;

            if (!string.IsNullOrEmpty(c) && !string.IsNullOrEmpty(t) && !string.IsNullOrEmpty(k) && !string.IsNullOrEmpty(d) && !string.IsNullOrEmpty(urls))
            {
                Album album = new Album();
                album.CategoryId = c;
                album.Title = t;
                album.Keywords = k;
                album.Description = d;
                album.IsDelete = false;
                album.IsShow = false;
                album.IsTop = false;
                album.InsertTime = DateTime.Now;
                album.ViewTime = 0;
                if (!string.IsNullOrEmpty(isShow) && isShow == "true")
                {
                    album.IsShow = true;
                }
                if (!string.IsNullOrEmpty(isTop) && isTop == "true")
                {
                    album.IsTop = true;
                }

                urls = urls.Substring(0, urls.Length);
                string[] us = urls.Split('|');

                List<Image> images = new List<Image>();
                foreach (var original in us)
                {
                    if (original.Contains(".jpg") || original.Contains(".png") || original.Contains(".gif"))
                    {
                        string imageName = original.Substring(0, original.Length - 4);
                        string filetrype = original.Substring(original.Length - 4, 4);

                        Model.Image image = new Model.Image();
                        image.UrlOriginal = original;
                        image.UrlThumbnailWidth102x102 = imageName + "-102x102" + filetrype;
                        image.UrlThumbnailWidth235x350 = imageName + "-235x350" + filetrype;
                        image.UrlThumbnailWidth490x350 = imageName + "-490x350" + filetrype;
                        image.UrlThumbnailHeight200 = imageName + "-big" + filetrype;

                        images.Add(image);
                    }
                }

                if (albumService.AddAlbum(album, images))
                {
                    model.isSuccessful = "添加成功";
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            if (Session["UserId"] == null)
            {
                return Redirect("/admin/login");
            }

            albumService.RemoveAlbum(id);

            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }

        //推荐
        [HttpGet]
        public ActionResult Recommend(int id)
        {
            if (Session["UserId"] == null)
            {
                return Redirect("/admin/login");
            }

            albumService.Recommend(id, true);

            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }

        //取消推荐
        [HttpGet]
        public ActionResult Cancel(int id)
        {
            if (Session["UserId"] == null)
            {
                return Redirect("/admin/login");
            }

            albumService.Recommend(id, false);

            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }
    }
}
