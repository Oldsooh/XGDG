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
    public class AlbumController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 类别及分页
        /// </summary>
        /// <param name="category">类别Id</param>
        /// <param name="page">类别分页页码</param>
        public ActionResult Category(string category, string page)
        {
            this.Internationalization();

            CategoryModel model = new CategoryModel();

            CategoryService categoryService = new CategoryService();
            AlbumService albumService = new AlbumService();

            model.Category = categoryService.GetCategoryById(category);
            model.PageSize = 30;//每页显示30张
            int pageIndex = 1;
            if (Int32.TryParse(page, out pageIndex))
            {
                model.PageIndex = pageIndex;
            }
            else
            {
                model.PageIndex = 1;
            }
            int count = 0;
            //分类列表
            model.Albums = albumService.GetAlbumByCategoryId(category, model.PageSize, model.PageIndex, out count);
            //分页
            if (model.Albums != null && model.Albums.Count > 0)
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

            if (model.Category == null)
            {
                model.Category = new Category();
            }

            return View(model);
        }

        /// <summary>
        /// 图片展示
        /// </summary>
        /// <param name="id">专辑Id</param>
        public ActionResult Show(string id, string page)
        {
            this.Internationalization();

            ImageModel model = new ImageModel();

            try
            {
                AlbumService albumService = new AlbumService();

                AlbumDetail albumDetail = albumService.GetAlbumDetail(Int32.Parse(id));
                
                if (albumDetail != null)
                {
                    int pageIndex = 1;
                    if (Int32.TryParse(page, out pageIndex))
                    {
                        model.Page = pageIndex;
                    }
                    else
                    {
                        model.Page = 1;
                    }
                    model.Album = albumDetail.Album;
                    model.Category = albumDetail.Category;
                    model.Images = albumDetail.Images;
                    model.Image = model.Images[model.Page - 1];
                    model.LastAlbum = albumDetail.LastAlbum;
                    model.NextAlbum = albumDetail.NextAlbum;
                    model.RelatedAlbum = albumDetail.RelatedAlbum;
                    model.RecommendAlbum = albumDetail.RecommendAlbum;
                    model.RelatedNews = new NewsService().GetRelatedNewsByAlbumId(model.Album.Id);
                }
            }
            catch (Exception e)
            {
                model = new ImageModel();
                model.Category = new Category();
                model.Album = new Album();
                model.Album.Title = e.Message;
                model.Album.Description = e.ToString();
            }

            model.Album = model.Album ?? new Album();
            model.Category = model.Category ?? new Category();
            //model.LastAlbum = model.LastAlbum ?? new Album();
            //model.NextAlbum = model.NextAlbum ?? new Album();
            model.RelatedAlbum = model.RelatedAlbum ?? new List<Album>();
            model.RecommendAlbum = model.RecommendAlbum ?? new List<Album>();

            return View(model);
        }
    }
}
