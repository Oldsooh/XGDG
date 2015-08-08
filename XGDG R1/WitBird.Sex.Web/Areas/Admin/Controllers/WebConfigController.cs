using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WitBird.Sex.BLL;
using WitBird.Sex.Model;

namespace WitBird.Sex.Web.Areas.Admin.Controllers
{
    public class WebConfigController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["UserId"] == null)
            {
                return Redirect("/admin/login");
            }

            WebConfig model = null;

            WebConfigService service = new WebConfigService();
            model = service.GetWebConfig();

            if (model == null)
            {
                model = new WebConfig();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(WebConfig newWebConfig)
        {
            if (Session["UserId"] == null)
            {
                return Redirect("/admin/login");
            }

            WebConfigService service = new WebConfigService();
            WebConfig webConfig = service.GetWebConfig();

            webConfig.WebName = string.IsNullOrEmpty(newWebConfig.WebName) ? string.Empty : newWebConfig.WebName;
            webConfig.Title = string.IsNullOrEmpty(newWebConfig.Title) ? string.Empty : newWebConfig.Title;
            webConfig.Keywords = string.IsNullOrEmpty(newWebConfig.Keywords) ? string.Empty : newWebConfig.Keywords;
            webConfig.Description = string.IsNullOrEmpty(newWebConfig.Description) ? string.Empty : newWebConfig.Description;
            webConfig.DomainName = string.IsNullOrEmpty(newWebConfig.DomainName) ? string.Empty : newWebConfig.DomainName;
            webConfig.DomainMobile = string.IsNullOrEmpty(newWebConfig.DomainMobile) ? string.Empty : newWebConfig.DomainMobile;
            webConfig.DomainImage = string.IsNullOrEmpty(newWebConfig.DomainImage) ? string.Empty : newWebConfig.DomainImage;
            webConfig.Notice = string.IsNullOrEmpty(newWebConfig.Notice) ? string.Empty : newWebConfig.Notice;
            webConfig.ICP = string.IsNullOrEmpty(newWebConfig.ICP) ? string.Empty : newWebConfig.ICP;
            webConfig.Tel = string.IsNullOrEmpty(newWebConfig.Tel) ? string.Empty : newWebConfig.Tel;
            webConfig.Phone = string.IsNullOrEmpty(newWebConfig.Phone) ? string.Empty : newWebConfig.Phone;
            webConfig.QQ = string.IsNullOrEmpty(newWebConfig.QQ) ? string.Empty : newWebConfig.QQ;
            webConfig.Email = string.IsNullOrEmpty(newWebConfig.Email) ? string.Empty : newWebConfig.Email;
            webConfig.Weibo = string.IsNullOrEmpty(newWebConfig.Weibo) ? string.Empty : newWebConfig.Weibo;
            webConfig.Address = string.IsNullOrEmpty(newWebConfig.Address) ? string.Empty : newWebConfig.Address;
            webConfig.StatisticalCode = string.IsNullOrEmpty(newWebConfig.StatisticalCode) ? string.Empty : newWebConfig.StatisticalCode;
            webConfig.Tags = string.IsNullOrEmpty(newWebConfig.Tags) ? string.Empty : newWebConfig.Tags;

            if (!string.IsNullOrEmpty(webConfig.WebName))
            {
                service.EditWebConfig(webConfig);
                WitBird.Sex.Web.Cache.UpdateWebConfig();//更新缓存
            }

            return Redirect("/admin/webconfig");
        }
    }
}
