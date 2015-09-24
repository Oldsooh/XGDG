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
    public class FriendlyLinkController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["UserId"] == null)
            {
                return Redirect("/admin/login");
            }

            FriendlyLinkModel model = new FriendlyLinkModel();

            FriendlyLinkService friendlyLinkService = new FriendlyLinkService();
            model.FriendlyLinks = friendlyLinkService.GetFriendlyLinks();

            return View(model);
        }

        [HttpPost]
        public ActionResult AddFriendlyLink(FriendlyLink friendlyLink)
        {
            if (Session["UserId"] == null)
            {
                return Redirect("/admin/login");
            }

            if (friendlyLink != null)
            {
                if (!string.IsNullOrEmpty(friendlyLink.Name) &&
                    !string.IsNullOrEmpty(friendlyLink.Link) &&
                    friendlyLink.Sort > 0)
                {
                    FriendlyLinkService friendlyLinkService = new FriendlyLinkService();
                    friendlyLinkService.AddFriendlyLink(friendlyLink);
                    WitBird.Sex.Web.Cache.UpdateFriendlyLinks();//更新缓存
                }
            }

            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditFriendlyLink(FriendlyLink friendlyLink)
        {
            if (Session["UserId"] == null)
            {
                return Redirect("/admin/login");
            }

            if (friendlyLink != null)
            {
                if (friendlyLink.Id > 0 &&
                    !string.IsNullOrEmpty(friendlyLink.Name) &&
                    !string.IsNullOrEmpty(friendlyLink.Link) &&
                    friendlyLink.Sort > 0)
                {
                    FriendlyLinkService friendlyLinkService = new FriendlyLinkService();
                    friendlyLinkService.EditFriendlyLink(friendlyLink);
                    WitBird.Sex.Web.Cache.UpdateFriendlyLinks();//更新缓存
                }
            }

            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }

        [HttpGet]
        public ActionResult DeleteFriendlyLink(int id)
        {
            if (Session["UserId"] == null)
            {
                return Redirect("/admin/login");
            }

            FriendlyLinkService friendlyLinkService = new FriendlyLinkService();

            friendlyLinkService.RemoveFriendlyLink(id);
            WitBird.Sex.Web.Cache.UpdateFriendlyLinks();//更新缓存

            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }
    }
}
