using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WitBird.Sex.BLL;
using WitBird.Sex.Model;
using WitBird.Sex.WebV2.Models;
using System.Threading.Tasks;

namespace WitBird.Sex.WebV2
{
    public static class Cache
    {
        private static WebConfig webConfig;
        private static List<FriendlyLink> friendlyLinks;
        private static List<Advertisement> advertisements;
        private static List<NewsCategory>  newsCategories;
        private static List<NovelCategory> novelCategories;
        private static List<Category> categories;
        private static HomeModel homeData;

        /// <summary>
        ///配置信息
        /// </summary>
        public static WebConfig WebConfig
        {
            get
            {
                if (webConfig == null)
                {
                    UpdateWebConfig();
                }
                if (webConfig == null)
                {
                    webConfig = new WebConfig();
                    webConfig.Title = "数据库连接失败";
                    webConfig.WebName = "数据库连接失败";
                    webConfig.DomainMobile = "/m/";
                }
                return webConfig;
            }
        }
        public static void UpdateWebConfig()
        {
            WebConfigService webConfigService = new WebConfigService();
            webConfig = webConfigService.GetWebConfig();
        }

        /// <summary>
        /// 友情链接
        /// </summary>
        public static List<FriendlyLink> FriendlyLinks
        {
            get
            {
                if (friendlyLinks == null)
                {
                    UpdateFriendlyLinks();
                }
                return friendlyLinks;
            }
        }
        public static void UpdateFriendlyLinks()
        {
            FriendlyLinkService friendlyLinkService = new FriendlyLinkService();
            friendlyLinks = friendlyLinkService.GetFriendlyLinks();
        }

        /// <summary>
        /// 广告列表
        /// </summary>
        public static List<Advertisement> Advertisements
        {
            get
            {
                if (advertisements == null)
                {
                    UpdateAdvertisements();
                }
                return advertisements;
            }
        }
        public static void UpdateAdvertisements()
        {
            AdvertisementService advertisementService = new AdvertisementService();
            advertisements = advertisementService.GetAdvertisements();
        }
        public static Advertisement GetAdvertisement(int id)
        {
            Advertisement ad = new Advertisement();

            if (advertisements == null)
            {
                UpdateAdvertisements();
            }

            if (advertisements != null && advertisements.Count > 0)
            {
                foreach (var item in advertisements)
                {
                    if (item.Id == id)
                    {
                        ad = item;
                        break;
                    }
                }
            }

            return ad;
        }

        public static List<NewsCategory> NewsCategories
        {
            get
            {
                if (newsCategories == null)
                {
                    UpdateNewsCategories();
                }
                return newsCategories;
            }
        }
        public static void UpdateNewsCategories()
        {
            NewsService newsService = new NewsService();
            newsCategories = newsService.GetNewsCategories();
        }

        public static List<Category> Categories
        {
            get
            {
                if (categories == null)
                {
                    UpdateCategories();
                }
                return categories;
            }
        }
        public static void UpdateCategories()
        {
            CategoryService service = new CategoryService();
            categories = service.GetCategories(string.Empty);
        }

        public static List<NovelCategory> NovelCategories
        {
            get
            {
                if (novelCategories == null)
                {
                    UpdateNovelCategories();
                }
                return novelCategories;
            }
        }
        public static void UpdateNovelCategories()
        {
            NovelService novelCategoriesService = new NovelService();
            novelCategories = novelCategoriesService.GetNovelCategories();
        }

        public static HomeModel HomeData
        {
            get
            {
                if (homeData == null)
                {
                    UpdateHomeData();
                }
                else
                {
                    Task.Run(delegate
                    {
                        UpdateHomeData();//首页异步读取最新数据
                    });
                }
                return homeData;
            }
        }

        static object HomeDataObj = new object();
        /// <summary>
        /// 更新首页数据缓存
        /// </summary>
        public static void UpdateHomeData()
        {
            lock (HomeDataObj)
            {
                AlbumService albumService = new AlbumService();
                HomeData data = albumService.GetHomeData();

                homeData = new HomeModel();
                if (data != null)
                {
                    homeData.Slides = data.Slides;
                    homeData.NewUploads = data.NewUploads;
                    homeData.RecommendPictures = data.RecommendPictures;
                    homeData.TopWomen = data.TopWomen;
                    homeData.TopMen = data.TopMen;
                    homeData.RecommendPure = data.RecommendPure;
                    homeData.RecommendSexy = data.RecommendSexy;
                    homeData.RecommendStar = data.RecommendStar;
                    homeData.RecommendCostume = data.RecommendCostume;
                    homeData.RecommendStylishMan = data.RecommendStylishMan;
                }

                //homeData.Slides = albumService.GetAlbumBySlide();
                //homeData.NewUploads = albumService.GetAlbumByNew();
                //homeData.RecommendPictures = albumService.GetRecommendAlbumByCategoryId("All", 4);
                //homeData.TopWomen = albumService.GetRecommendAlbumByCategoryId("TopWomen", 7);
                //homeData.TopMen = albumService.GetRecommendAlbumByCategoryId("TopMen", 7);
                //homeData.RecommendPure = albumService.GetRecommendAlbumByCategoryId("Pure", 4);
                //homeData.RecommendSexy = albumService.GetRecommendAlbumByCategoryId("Sexy", 4);
                //homeData.RecommendStar = albumService.GetRecommendAlbumByCategoryId("Star", 4);
                //homeData.RecommendCostume = albumService.GetRecommendAlbumByCategoryId("Costume", 4);
                //homeData.RecommendStylishMan = albumService.GetRecommendAlbumByCategoryId("StylishMan", 4);
            }
        }
    }
}