using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WitBird.Sex.Common;
using WitBird.Sex.DAL;
using WitBird.Sex.Model;

namespace WitBird.Sex.BLL
{
    public class NewsService
    {
        /// <summary>
        /// 发布新闻(成功返回success)
        /// </summary>
        public string AddNews(News news)
        {
            string result = "发布失败";

            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                if (news != null)
                {
                    if (!string.IsNullOrEmpty(news.Title) &&
                        !string.IsNullOrEmpty(news.CategoryId) &&
                        !string.IsNullOrEmpty(news.ContentStyle))
                    {
                        List<string> filterColumns = new List<string>() { "Id" };
                        if (DBRepository.Insert<News>(news, conn, filterColumns))
                        {
                            result = "success";
                        }
                    }
                    else
                    {
                        result = "标题，分类，内容必须有值";
                        LogService.Log("NewsService.AddNews", "news非空属性没赋值");
                    }
                }
                else
                {
                    result = "新闻对象不能为空";
                    LogService.Log("NewsService.AddNews", "news对象为空");
                }
            }
            catch (Exception e)
            {
                result = "程序出现异常，详情见日志";
                LogService.Log("发布新闻", e.ToString());
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 更新新闻(成功返回success)
        /// </summary>
        public string EditNews(News news)
        {
            string result = "更新失败";

            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                if (news != null)
                {
                    if (!string.IsNullOrEmpty(news.Title) &&
                        !string.IsNullOrEmpty(news.CategoryId) &&
                        !string.IsNullOrEmpty(news.ContentStyle))
                    {
                        if (DBRepository.Update<News>(news, "Id", conn))
                        {
                            result = "success";
                        }
                    }
                    else
                    {
                        result = "标题，分类，内容必须有值";
                        LogService.Log("NewsService.EditNews", "news非空属性没赋值");
                    }
                }
                else
                {
                    result = "新闻对象不能为空";
                    LogService.Log("NewsService.EditNews", "news对象为空");
                }
            }
            catch (Exception e)
            {
                result = "程序出现异常，详情见日志";
                LogService.Log("更新失败", e.ToString());
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 分布条件查询新闻列表
        /// </summary>
        /// <param name="categoryId">分类列表</param>
        /// <param name="keywords">关键字</param>
        /// <param name="pageCount">每页显示条数</param>
        /// <param name="pageIndex">当前页页码</param>
        /// <param name="count">满足条件的新闻总数</param>
        /// <returns>新闻列表</returns>
        public List<News> GetNewsList(string categoryId, string keywords, int pageCount, int pageIndex, out int count)
        {
            List<News> result = null;
            count = 0;

            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                int tempCount = 0;
                string where = string.Empty;
                if (!string.IsNullOrEmpty(categoryId))
                {
                    where = "CategoryId='" + categoryId + "'";
                }
                if (!string.IsNullOrEmpty(keywords))
                {
                    if (!string.IsNullOrEmpty(where))
                    {
                        where += " and ";
                    }

                    where += " ( ";
                    char[] FilterChar = { ' ', ',', '，' };
                    string[] keys = keywords.Split(FilterChar);
                    for (int i = 0; i < keys.Length; i++)
                    {
                        if (keys[i] == string.Empty)
                        {
                            continue;
                        }

                        if (i != keys.Length - 1)
                        {
                            where += "Keywords like '%" + DBRepository.FilterSQL(keys[i]) + "%' or ";
                            where += "Title like '%" + DBRepository.FilterSQL(keys[i]) + "%' or ";
                        }
                        else
                        {
                            where += "Keywords like '%" + DBRepository.FilterSQL(keys[i]) + "%' or ";
                            where += "Title like '%" + DBRepository.FilterSQL(keys[i]) + "%' ";
                        }
                    }

                    where += ") ";
                }
                if (!string.IsNullOrEmpty(where))
                {
                    where += " and IsActive = 1";
                }
                else
                {
                    where = "IsActive = 1";
                }
                result = DBRepository.SelectMore<News>(where, "Id", false, pageCount, pageIndex, out tempCount, conn);
                count = tempCount;
                if (result != null && result.Count > 0)
                {
                    List<NewsCategory> newsCategories = DBRepository.SelectMore<NewsCategory>("IsActive=1", "Sort", true, conn);
                    foreach (var news in result)
                    {
                        news.Category = new NewsCategory();
                        if (newsCategories != null && newsCategories.Count > 0)
                        {
                            foreach (var category in newsCategories)
                            {
                                if (news.CategoryId == category.Id)
                                {
                                    news.Category.Id = category.Id;
                                    news.Category.Name = category.Name;
                                    news.Category.Sort = category.Sort;
                                    news.Category.IsActive = category.IsActive;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogService.Log("新闻列表", e.ToString());
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 根据ID查询新闻
        /// </summary>
        public News GetNewsById(int id)
        {
            News result = null;

            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                result = DBRepository.SelectOne<News>("Id", id, conn);
                if (result != null)
                {
                    DBRepository.UpdateOneColumn<News>("ViewCount", result.ViewCount + 1, "Id=" + result.Id.ToString(), conn);
                    DBRepository.UpdateOneColumn<News>("UpdateTime", DateTime.Now, "Id=" + result.Id.ToString(), conn);
                    result.Category = DBRepository.SelectOne<NewsCategory>("Id", result.CategoryId, conn);
                    if (result.Category == null)
                    {
                        result.Category = new NewsCategory();
                        result.Category.Id = result.CategoryId;
                    }
                }
            }
            catch (Exception e)
            {
                LogService.Log("根据Id查询新闻", e.ToString());
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 获取上一条新闻
        /// </summary>
        public News GetPrvNewsById(int id)
        {
            News result = null;

            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                result = DBRepository.SelectOne<News>("Id < " + id.ToString() + " and IsActive = 1 order by Id desc", conn);
            }
            catch (Exception e)
            {
                LogService.Log("获取上一条新闻", e.ToString());
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 获取下一条新闻
        /// </summary>
        /// <returns></returns>
        public News GetNextNewsById(int id)
        {
            News result = null;

            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                result = DBRepository.SelectOne<News>("Id > " + id.ToString() + " and IsActive = 1 order by Id asc", conn);
            }
            catch (Exception e)
            {
                LogService.Log("获取下一条新闻", e.ToString());
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 根据Id查询新闻分类
        /// </summary>
        public NewsCategory GetNewsCategoryById(string id)
        {
            NewsCategory result = null;

            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                result = DBRepository.SelectOne<NewsCategory>("Id", id, conn);
            }
            catch (Exception e)
            {
                LogService.Log("根据Id查询新闻分类", e.ToString());
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 获取新闻分类列表
        /// </summary>
        public List<NewsCategory> GetNewsCategories()
        {
            List<NewsCategory> result = null;

            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                result = DBRepository.SelectMore<NewsCategory>("IsActive=1", "Sort", true, conn);
            }
            catch (Exception e)
            {
                LogService.Log("新闻分类列表", e.ToString());
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 删除新闻
        /// </summary>
        public bool RemoveNews(int id)
        {
            bool result = false;

            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                result = DBRepository.Delete<News>("IsActive", false, "Id", id, conn);
            }
            catch (Exception e)
            {
                LogService.Log("软删除新闻", e.ToString());
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 添加分类(成功返回success)
        /// </summary>
        public string AddNewsCategory(NewsCategory category)
        {
            string result = "添加失败";

            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                if (category != null)
                {
                    if (!string.IsNullOrEmpty(category.Id) && !string.IsNullOrEmpty(category.Name) && category.Sort > 0)
                    {
                        NewsCategory temp = DBRepository.SelectOne<NewsCategory>("Id", category.Id, conn);
                        conn.Close();
                        conn.Open();
                        if (temp == null)
                        {
                            category.IsActive = true;
                            if (DBRepository.Insert<NewsCategory>(category, conn))
                            {
                                result = "success";
                            }
                        }
                    }
                }
                else
                {
                    result = "category对象为空";
                    LogService.Log("NewsService.AddNewsCategory", "category对象为空");
                }
            }
            catch (Exception e)
            {
                result = "程序出现异常，详情见日志";
                LogService.Log("添加分类", e.ToString());
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 更新分类
        /// </summary>
        public string EditNewsCategory(NewsCategory category)
        {
            string result = "更新失败";

            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                if (category != null)
                {
                    if (!string.IsNullOrEmpty(category.Id) && !string.IsNullOrEmpty(category.Name) && category.Sort > 0)
                    {
                        if (DBRepository.Update<NewsCategory>(category, "Id", conn))
                        {
                            result = "success";
                        }
                    }
                }
                else
                {
                    result = "category对象为空";
                    LogService.Log("NewsService.AddNewsCategory", "category对象为空");
                }
            }
            catch (Exception e)
            {
                result = "程序出现异常，详情见日志";
                LogService.Log("更新分类", e.ToString());
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 删除分类
        /// </summary>
        public bool RemoveNewsCategory(string id)
        {
            bool result = false;

            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                result = DBRepository.Delete<NewsCategory>("IsActive", false, "Id", id, conn);
            }
            catch (Exception e)
            {
                LogService.Log("软删除新闻分类", e.ToString());
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 热门列表
        /// </summary>
        public List<News> GetTopNews()
        {
            List<News> result = null;

            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                result = DBRepository.SelectMore<News>("IsTop = 1 and IsActive = 1", "Id", false, conn, 8);
            }
            catch (Exception e)
            {
                LogService.Log("热门列表", e.ToString());
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 推荐列表
        /// </summary>
        public List<News> GetRecommendNews()
        {
            List<News> result = null;

            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                result = DBRepository.SelectMore<News>("IsRecommend = 1 and IsActive = 1", "Id", false, conn, 5);
            }
            catch (Exception e)
            {
                LogService.Log("推荐列表", e.ToString());
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 最新列表
        /// </summary>
        public List<News> GetNewNews()
        {
            List<News> result = null;

            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                result = DBRepository.SelectMore<News>("IsActive = 1", "Id", false, conn, 10);
                if (result != null && result.Count > 0)
                {
                    List<NewsCategory> newsCategories = DBRepository.SelectMore<NewsCategory>("IsActive=1", "Sort", true, conn);
                    foreach (var news in result)
                    {
                        news.Category = new NewsCategory();
                        if (newsCategories != null && newsCategories.Count > 0)
                        {
                            foreach (var category in newsCategories)
                            {
                                if (news.CategoryId == category.Id)
                                {
                                    news.Category.Id = category.Id;
                                    news.Category.Name = category.Name;
                                    news.Category.Sort = category.Sort;
                                    news.Category.IsActive = category.IsActive;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogService.Log("最新列表", e.ToString());
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 幻灯列表
        /// </summary>
        public List<News> GetSlideNews()
        {
            List<News> result = null;

            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                result = DBRepository.SelectMore<News>("IsActive = 1 and PictureGroup !=''", "Id", false, conn, 5);
            }
            catch (Exception e)
            {
                LogService.Log("幻灯列表", e.ToString());
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 热门列表
        /// </summary>
        /// <returns></returns>
        public List<News> GetHotNews()
        {
            List<News> result = null;

            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                string dt = DateTime.Now.Date.AddDays(-7).ToString();
                result = DBRepository.SelectMore<News>("IsActive = 1 and UpdateTime >'" + dt + "'", "ViewCount", false, conn, 5);
            }
            catch (Exception e)
            {
                LogService.Log("热门列表", e.ToString());
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 随机查询4条
        /// </summary>
        /// <returns></returns>
        public List<News> GetRandomNews()
        {
            List<News> result = null;

            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                string sql = "select top 5  * from News where IsActive=1 order by newid()";
                result = DBRepository.Select<News>(sql, conn);
            }
            catch (Exception e)
            {
                LogService.Log("随机查询", e.ToString());
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 相关列表
        /// </summary>
        /// <returns></returns>
        public List<News> GetRelatedNews(int id)
        {
            List<News> result = null;

            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                News news = DBRepository.SelectOne<News>("Id", id, conn);
                if (news != null && news.IsActive && !string.IsNullOrEmpty(news.Keywords))
                {
                    if (!string.IsNullOrEmpty(news.Keywords))
                    {
                        string where = " IsActive = 1 and ";
                        where += " ( ";
                        char[] FilterChar = { ' ', ',', '，' };
                        string[] keys = news.Keywords.Split(FilterChar);
                        for (int i = 0; i < keys.Length; i++)
                        {
                            if (keys[i] == string.Empty)
                            {
                                continue;
                            }

                            if (i != keys.Length - 1)
                            {
                                where += "Keywords like '%" + DBRepository.FilterSQL(keys[i]) + "%' or ";
                                where += "Title like '%" + DBRepository.FilterSQL(keys[i]) + "%' or ";
                            }
                            else
                            {
                                where += "Keywords like '%" + DBRepository.FilterSQL(keys[i]) + "%' or ";
                                where += "Title like '%" + DBRepository.FilterSQL(keys[i]) + "%' ";
                            }
                        }

                        where += ") ";

                        result = DBRepository.SelectMore<News>(where, "Id", false, conn, 5);
                    }
                }
            }
            catch (Exception e)
            {
                LogService.Log("热门列表", e.ToString());
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 相关列表
        /// </summary>
        /// <returns></returns>
        public List<News> GetRelatedNewsByAlbumId(int id)
        {
            List<News> result = null;

            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                Album album = DBRepository.SelectOne<Album>("Id", id, conn);
                if (album != null && !album.IsDelete && !string.IsNullOrEmpty(album.Keywords))
                {
                    if (!string.IsNullOrEmpty(album.Keywords))
                    {
                        string where = " IsActive = 1 and ";
                        where += " ( ";
                        char[] FilterChar = { ' ', ',', '，' };
                        string[] keys = album.Keywords.Split(FilterChar);
                        for (int i = 0; i < keys.Length; i++)
                        {
                            if (keys[i] == string.Empty)
                            {
                                continue;
                            }

                            if (i != keys.Length - 1)
                            {
                                where += "Keywords like '%" + DBRepository.FilterSQL(keys[i]) + "%' or ";
                                where += "Title like '%" + DBRepository.FilterSQL(keys[i]) + "%' or ";
                            }
                            else
                            {
                                where += "Keywords like '%" + DBRepository.FilterSQL(keys[i]) + "%' or ";
                                where += "Title like '%" + DBRepository.FilterSQL(keys[i]) + "%' ";
                            }
                        }

                        where += ") ";

                        result = DBRepository.SelectMore<News>(where, "Id", false, conn, 5);
                    }
                }
            }
            catch (Exception e)
            {
                LogService.Log("热门列表", e.ToString());
            }
            finally
            {
                conn.Close();
            }

            return result;
        }


        /// <summary>
        /// 查询简短新闻列表
        /// </summary>
        /// <returns></returns>
        public List<News> GetNewsSimple()
        {
            List<News> result = null;

            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                string sql = "select Id,Title,Keywords from News where IsActive=1 order by Id desc";
                result = DBRepository.Select<News>(sql, conn);
            }
            catch (Exception e)
            {
                LogService.Log("查询简短新闻列表", e.ToString());
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 随机推荐新闻列表
        /// </summary>
        public static List<News> GetRecommendRandom(int count)
        {
            List<News> result = new List<News>();
            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                string sql = "select top " + count.ToString() + " * from News where IsActive=1 and IsRecommend=1 order by newid()";
                result = DBRepository.SelectMoreBase<News>(sql, conn);
                if (result != null && result.Count > 0)
                {
                    List<NewsCategory> newsCategories = DBRepository.SelectMore<NewsCategory>("IsActive=1", "Sort", true, conn);
                    foreach (var news in result)
                    {
                        news.Category = new NewsCategory();
                        if (newsCategories != null && newsCategories.Count > 0)
                        {
                            foreach (var category in newsCategories)
                            {
                                if (news.CategoryId == category.Id)
                                {
                                    news.Category.Id = category.Id;
                                    news.Category.Name = category.Name;
                                    news.Category.Sort = category.Sort;
                                    news.Category.IsActive = category.IsActive;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogService.Log("随机推荐新闻列表", e.ToString());
            }
            finally
            {
                conn.Close();
            }
            return result;
        }

        /// <summary>
        /// 网站地图
        /// </summary>
        public static List<News> GetSiteMap()
        {
            List<News> result = new List<News>();
            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                string sql = "SELECT [Id],[CategoryId],[Title] = '',[Keywords]= '',[Description]= '',[Cover]= '',[PictureGroup]= '',[ContentStyle]= '',[ContentText]= ''"
                    + ",[PostType]= '',[Author]= '',[ComeFrom]= '',[InsertTime] ,[UpdateTime],[ViewCount],[IsRecommend],[IsTop],[IsSlide],[IsActive] FROM [News] "
                    + "where IsActive = 1 order by Id desc";
                result = DBRepository.SelectMoreBase<News>(sql, conn);
            }
            catch (Exception e)
            {
                LogService.Log("网站地图", e.ToString());
            }
            finally
            {
                conn.Close();
            }
            return result;
        }
    }
}
