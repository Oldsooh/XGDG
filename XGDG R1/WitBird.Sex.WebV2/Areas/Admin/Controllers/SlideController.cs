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
    public class SlideController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["UserId"] == null)
            {
                return Redirect("/admin/login");
            }

            SlideModel model = new SlideModel();

            SlideService slideService = new SlideService();
            model.Slides = slideService.GetSlides();

            return View(model);
        }

        [HttpPost]
        public ActionResult Add(Slide slide)
        {
            if (Session["UserId"] == null)
            {
                return Redirect("/admin/login");
            }

            if (slide != null)
            {
                if (!string.IsNullOrEmpty(slide.Title) &&
                    !string.IsNullOrEmpty(slide.Url) &&
                    !string.IsNullOrEmpty(slide.Image))
                {
                    SlideService slideService = new SlideService();
                    slideService.AddSlide(slide);
                }
            }

            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (Session["UserId"] == null)
            {
                return Redirect("/admin/login");
            }

            SlideService slideService = new SlideService();

            slideService.RemoveSlide(id);

            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }
    }
}
