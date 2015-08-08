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
    public class VideoController : Controller
    {
        [HttpGet]
        public ActionResult Index(string keywords, string page)
        {
            if (Session["UserId"] == null)
            {
                return Redirect("/admin/login");
            }

            VideoModel model = new VideoModel();

            VideoService videoService = new VideoService();

            int tempPage = 1;

            if (!string.IsNullOrEmpty(page))
            {
                Int32.TryParse(page, out tempPage);
            }

            //视频列表
            int tempCount = 0;
            model.Videos = videoService.GetVideos(keywords, 15, tempPage, out tempCount);
            //分页
            if (model.Videos != null && model.Videos.Count > 0)
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
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(string t, string k, string d, string imageurl, string u, string um)
        {
            string result = "发布失败";

            if (Session["UserId"] == null)
            {
                return Content("长时间未操作，请重新登录。");
            }

            if (!string.IsNullOrEmpty(t) && !string.IsNullOrEmpty(k) && !string.IsNullOrEmpty(d) && !string.IsNullOrEmpty(imageurl) && !string.IsNullOrEmpty(u) && !string.IsNullOrEmpty(um))
            {
                Video video = new Video();
                video.Title = t;
                video.Keywords = k;
                video.Description = d;
                video.Thumbnail = imageurl;
                video.Url = u;
                video.UrlM = um;
                video.InsertTime = DateTime.Now;
                video.ViewTime = 0;

                VideoService videoService = new VideoService();
                if (videoService.AddVideo(video))
                {
                    result = "success";
                }
            }
            else
            {
                result = "必填项不能为空";
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

            VideoModel model = new VideoModel();

            VideoService videoService = new VideoService();
            model.Video = videoService.GetVideoById(id);

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, string t, string k, string d, string imageurl, string u, string um)
        {
            string result = "更新失败";

            if (Session["UserId"] == null)
            {
                return Content("长时间未操作，请重新登录。");
            }

            if (!string.IsNullOrEmpty(t) && !string.IsNullOrEmpty(k) && !string.IsNullOrEmpty(d) && !string.IsNullOrEmpty(imageurl) && !string.IsNullOrEmpty(u) && !string.IsNullOrEmpty(um))
            {
                VideoService videoService = new VideoService();
                Video video = videoService.GetVideoById(id);
                if (video != null)
                {
                    video.Title = t;
                    video.Keywords = k;
                    video.Description = d;
                    video.Thumbnail = imageurl;
                    video.Url = u;
                    video.UrlM = um;

                    if (videoService.EditVideo(video))
                    {
                        result = "success";
                    }
                }
            }
            else
            {
                result = "必填项不能为空";
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

            VideoService videoService = new VideoService();

            videoService.RemoveVideo(id);

            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }
    }
}
