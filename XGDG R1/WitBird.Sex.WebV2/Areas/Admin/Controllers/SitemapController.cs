using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using WitBird.Sex.BLL;
using WitBird.Sex.Common;
using WitBird.Sex.Model;

namespace WitBird.Sex.WebV2.Areas.Admin.Controllers
{
    public class SitemapController : Controller
    {
        //
        // GET: /Admin/Sitemap/

        public ActionResult Index()
        {
            ViewData["sitemap"] = CreateSitemap();

            return View();
        }

        XmlDocument xmlDoc = new XmlDocument();

        public string CreateSitemap()
        {
            string result = "生成成功";

            try
            {
                xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", ""));

                XmlNode urlset = xmlDoc.CreateElement("urlset");

                WebConfig websetting = WitBird.Sex.WebV2.Cache.WebConfig;
                if (websetting != null)
                {
                    websetting.DomainName.Trim();

                    CreateUrl(urlset, "http://" + websetting.DomainName + "/", DateTime.Now, "hourly", "1.0");
                    CreateUrl(urlset, "http://" + websetting.DomainName + "/news/", DateTime.Now, "daily", "0.9");
                    CreateUrl(urlset, "http://" + websetting.DomainName + "/pure/", DateTime.Now, "daily", "0.9");
                    CreateUrl(urlset, "http://" + websetting.DomainName + "/sexy/", DateTime.Now, "daily", "0.9");
                    CreateUrl(urlset, "http://" + websetting.DomainName + "/star/", DateTime.Now, "weekly", "0.9");
                    CreateUrl(urlset, "http://" + websetting.DomainName + "/costume/", DateTime.Now, "weekly", "0.9");
                    CreateUrl(urlset, "http://" + websetting.DomainName + "/stylishman/", DateTime.Now, "weekly", "0.9");

                    //生成新闻
                    List<News> newsList = NewsService.GetSiteMap();
                    if (newsList != null && newsList.Count > 0)
                    {
                        foreach (var item in newsList)
                        {
                            string url = "http://" + websetting.DomainName + "/news/" + item.Id.ToString() + ".html";
                            CreateUrl(urlset, url, item.InsertTime, "monthly", "0.8");
                        }
                    }

                    //生成小说
                    //List<Novel> novelList = NovelService.GetSiteMap();
                    //if (novelList != null && novelList.Count > 0)
                    //{
                    //    foreach (var item in novelList)
                    //    {
                    //        string url = "http://" + websetting.DomainName + "/novel/" + item.Id.ToString() + ".html";
                    //        CreateUrl(urlset, url, item.InsertTime, "monthly", "0.7");
                    //        if (item.ArticleCount > 0)
                    //        {
                    //            for (int i = 1; i <= item.ArticleCount; i++)
                    //            {
                    //                string url2 = "http://" + websetting.DomainName + "/novel/" + item.Id.ToString() + "-" + i.ToString() + ".html";
                    //                CreateUrl(urlset, url2, item.InsertTime, "monthly", "0.7");
                    //            }
                    //        }
                    //    }
                    //}

                    //List<Video> videlList = VideoService.GetSiteMap();
                    //if (videlList != null && videlList.Count > 0)
                    //{
                    //    foreach (var item in videlList)
                    //    {
                    //        string url = "http://" + websetting.DomainName + "/play/" + item.Id.ToString() + ".html";
                    //        CreateUrl(urlset, url, item.InsertTime, "monthly", "0.7");
                    //    }
                    //}

                    //生成专辑
                    List<Album> albumList = AlbumService.GetSiteMap();
                    if (albumList != null && albumList.Count > 0)
                    {
                        foreach (var item in albumList)
                        {
                            string url = "http://" + websetting.DomainName + "/show/" + item.Id.ToString() + ".html";
                            CreateUrl(urlset, url, item.InsertTime, "monthly", "0.8");
                        }
                    }

                }
                xmlDoc.AppendChild(urlset);
                xmlDoc.Save(Server.MapPath("/sitemap.xml"));
            }
            catch (Exception e)
            {
                result = e.ToString();
                LogService.Log("生成站点地图失败", e.ToString());
            }

            return result;
        }

        /// <summary>
        /// 生成地图url节点
        /// </summary>
        /// <param name="urlset">Xml文件的根节点</param>
        /// <param name="loc">收录链接 格式：http://www.huhailong.cn</param>
        /// <param name="lastmod">更新时间</param>
        /// <param name="changefreq">更新频率 always、hourly、daily、weekly、monthly、yearly、never</param>
        /// <param name="priority">优先级 从0.0到1.0，0.0优先级最低、1.0最高。</param>
        public void CreateUrl(XmlNode urlset, string loc, DateTime lastmod, string changefreq, string priority)
        {
            try
            {
                //创建url结点...............................................................................Begin
                XmlNode urlXML = xmlDoc.CreateElement("url");

                XmlNode locXML = xmlDoc.CreateElement("loc");
                locXML.InnerText = loc;
                urlXML.AppendChild(locXML);

                XmlNode lastmodXML = xmlDoc.CreateElement("lastmod");
                lastmodXML.InnerText = lastmod.ToString("yyyy-MM-dd"); ;
                urlXML.AppendChild(lastmodXML);

                XmlNode changefreqXML = xmlDoc.CreateElement("changefreq");
                changefreqXML.InnerText = changefreq;
                urlXML.AppendChild(changefreqXML);

                XmlNode priorityXML = xmlDoc.CreateElement("priority");
                priorityXML.InnerText = priority;
                urlXML.AppendChild(priorityXML);

                //XmlNode data = xmlDoc.CreateElement("data");
                //XmlNode display = xmlDoc.CreateElement("display");
                //XmlNode html5_url = xmlDoc.CreateElement("html5_url");
                //html5_url.InnerText = loc;
                //display.AppendChild(html5_url);
                //data.AppendChild(display);
                //urlXML.AppendChild(data);

                urlset.AppendChild(urlXML);
                //创建url结点...............................................................................End
            }
            catch (Exception e)
            {
                LogService.Log("生成站点地图失败", e.ToString());
            }
        }

    }
}
