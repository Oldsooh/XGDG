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
    public class AdvertisementController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["UserId"] == null)
            {
                return Redirect("/admin/login");
            }

            AdvertisementModel model = new AdvertisementModel();

            AdvertisementService advertisementService = new AdvertisementService();
            model.Advertisements = advertisementService.GetAdvertisements();

            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Session["UserId"] == null)
            {
                return Redirect("/admin/login");
            }

            AdvertisementModel model = new AdvertisementModel();

            AdvertisementService advertisementService = new AdvertisementService();
            model.Advertisement = advertisementService.GetAdvertisementById(id);

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Advertisement ad)
        {
            if (Session["UserId"] == null)
            {
                return Redirect("/admin/login");
            }

            AdvertisementService advertisementService = new AdvertisementService();
            if (ad != null && ad.Id > 0 && !string.IsNullOrEmpty(ad.Name))
            {
                if (string.IsNullOrEmpty(ad.AdLink))
                {
                    ad.AdLink = string.Empty;
                }
                advertisementService.EditAdvertisement(ad);
                WitBird.Sex.WebV2.Cache.UpdateAdvertisements();//更新缓存
            }


            return Redirect("/admin/advertisement/");
        }

    }
}
