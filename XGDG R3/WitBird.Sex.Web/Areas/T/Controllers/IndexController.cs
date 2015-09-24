using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WitBird.Sex.BLL;
using WitBird.Sex.Web.Areas.T.Models;

namespace WitBird.Sex.Web.Areas.T.Controllers
{
    public class IndexController : Controller
    {
        //
        // GET: /T/Index/

        AlbumService albumService = new AlbumService();
        CategoryService categoryService = new CategoryService();

        public ActionResult Index()
        {
            ImagesModel model = new ImagesModel();
            int totalCount = 0;
            model.AlbumList = albumService.GetAlbumList("", "", 5, 10, out totalCount, 0);

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            ImagesModel model = new ImagesModel();

            model.Album = albumService.GetAlbumById(id);
            model.Categories = categoryService.GetCategories("album");

            return View(model);
        }

        public ActionResult Edit()
        {
            return Redirect("/t/index/");
        }

        public ActionResult Delete(string id)
        {
            try
            {
                albumService.RemoveAlbum(id);
            }
            catch
            {

            }

            return Redirect("/t/index/");
        }
    }
}
