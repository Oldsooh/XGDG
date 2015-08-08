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
    public class NovelService
    {

        /// <summary>
        /// 获取小说分类列表
        /// </summary>
        public List<NovelCategory> GetNovelCategories()
        {
            List<NovelCategory> result = null;

            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                result = DBRepository.SelectMore<NovelCategory>("IsActive=1", "Sort", true, conn);
            }
            catch (Exception e)
            {
                LogService.Log("小说分类列表", e.ToString());
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
        public string AddNovelCategory(NovelCategory category)
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
                        NovelCategory temp = DBRepository.SelectOne<NovelCategory>("Id", category.Id, conn);
                        conn.Close();
                        conn.Open();
                        if (temp == null)
                        {
                            category.IsActive = true;
                            if (DBRepository.Insert<NovelCategory>(category, conn))
                            {
                                result = "success";
                            }
                        }
                    }
                }
                else
                {
                    result = "category对象为空";
                    LogService.Log("NovelService.AddNovelCategory", "category对象为空");
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
        public string EditNovelCategory(NovelCategory category)
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
                        if (DBRepository.Update<NovelCategory>(category, "Id", conn))
                        {
                            result = "success";
                        }
                    }
                }
                else
                {
                    result = "category对象为空";
                    LogService.Log("NovelService.EditNovelCategory", "category对象为空");
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
        public bool RemoveNovelCategory(string id)
        {
            bool result = false;

            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                result = DBRepository.Delete<NovelCategory>("IsActive", false, "Id", id, conn);
            }
            catch (Exception e)
            {
                LogService.Log("软删除小说分类", e.ToString());
            }
            finally
            {
                conn.Close();
            }

            return result;
        }




        public List<Novel> GetNovelList(string categoryId, string keywords, int pageCount, int pageIndex, out int count)
        {
            List<Novel> result = null;
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
                    where += "Title like '%" + DBRepository.FilterSQL(keywords) + "%' or Keywords like '%" + DBRepository.FilterSQL(keywords) + "%'";
                }
                if (!string.IsNullOrEmpty(where))
                {
                    where += " and IsDelete = 0";
                }
                else
                {
                    where = "IsDelete = 0";
                }
                result = DBRepository.SelectMore<Novel>(where, "Id", false, pageCount, pageIndex, out tempCount, conn);
                count = tempCount;
                if (result != null && result.Count > 0)
                {
                    List<NovelCategory> novelCategories = DBRepository.SelectMore<NovelCategory>("IsActive=1", "Sort", true, conn);
                    foreach (var novel in result)
                    {
                        novel.Category = new NovelCategory();
                        if (novelCategories != null && novelCategories.Count > 0)
                        {
                            foreach (var category in novelCategories)
                            {
                                if (novel.CategoryId == category.Id)
                                {
                                    novel.Category.Id = category.Id;
                                    novel.Category.Name = category.Name;
                                    novel.Category.Sort = category.Sort;
                                    novel.Category.IsActive = category.IsActive;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogService.Log("小说列表", e.ToString());
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 获取最新小说
        /// </summary>
        public static List<Novel> GetNewNovels(int count)
        {
            List<Novel> result = new List<Novel>();
            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                result = DBRepository.SelectMore<Novel>("IsDelete=0", "Id", false, conn, count);
                if (result != null && result.Count > 0)
                {
                    List<NovelCategory> newsCategories = DBRepository.SelectMore<NovelCategory>("IsActive=1", "Sort", true, conn);
                    foreach (var novel in result)
                    {
                        novel.Category = new NovelCategory();
                        if (newsCategories != null && newsCategories.Count > 0)
                        {
                            foreach (var category in newsCategories)
                            {
                                if (novel.CategoryId == category.Id)
                                {
                                    novel.Category.Id = category.Id;
                                    novel.Category.Name = category.Name;
                                    novel.Category.Sort = category.Sort;
                                    novel.Category.IsActive = category.IsActive;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogService.Log("获取最新小说", e.ToString());
            }
            finally
            {
                conn.Close();
            }
            return result;
        }

        /// <summary>
        /// 随机推荐小说列表
        /// </summary>
        public static List<Novel> GetRecommendRandom(int count)
        {
            List<Novel> result = new List<Novel>();
            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                string sql = "select top " + count.ToString() + " * from Novel where IsDelete=0 order by newid()";
                result = DBRepository.SelectMoreBase<Novel>(sql, conn);
                if (result != null && result.Count > 0)
                {
                    List<NovelCategory> newsCategories = DBRepository.SelectMore<NovelCategory>("IsActive=1", "Sort", true, conn);
                    foreach (var novel in result)
                    {
                        novel.Category = new NovelCategory();
                        if (newsCategories != null && newsCategories.Count > 0)
                        {
                            foreach (var category in newsCategories)
                            {
                                if (novel.CategoryId == category.Id)
                                {
                                    novel.Category.Id = category.Id;
                                    novel.Category.Name = category.Name;
                                    novel.Category.Sort = category.Sort;
                                    novel.Category.IsActive = category.IsActive;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogService.Log("随机推荐小说列表", e.ToString());
            }
            finally
            {
                conn.Close();
            }
            return result;
        }

        /// <summary>
        /// 获取小说
        /// </summary>
        public Novel GetNovelById(int id)
        {
            Novel result = new Novel();
            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                result = DBRepository.SelectOne<Novel>("Id", id, conn);
                if (result != null)
                {
                    DBRepository.UpdateOneColumn<Novel>("ViewTime", result.ViewTime + 1, "Id=" + result.Id.ToString(), conn);
                    result.Category = DBRepository.SelectOne<NovelCategory>("Id", result.CategoryId, conn);
                    if (result.Category == null)
                    {
                        result.Category = new NovelCategory();
                        result.Category.Id = result.CategoryId;
                    }
                }

            }
            catch (Exception e)
            {
                LogService.Log("获取小说", e.ToString());
            }
            finally
            {
                conn.Close();
            }
            return result;
        }

        /// <summary>
        /// 发布小说
        /// </summary>
        public string AddNovel(Novel novel)
        {
            string result = "添加失败";

            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                if (novel != null)
                {
                    if (!string.IsNullOrEmpty(novel.Title) &&
                    !string.IsNullOrEmpty(novel.CategoryId) &&
                    !string.IsNullOrEmpty(novel.Name))
                    {
                        List<string> filterColumns = new List<string>() { "Id" };
                        if (DBRepository.Insert<Novel>(novel, conn, filterColumns))
                        {
                            result = "success";
                        }
                    }
                }
                else
                {
                    result = "category对象为空";
                    LogService.Log("NovelService.AddNovel", "novel对象为空");
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
        /// 更新小说
        /// </summary>
        public string EditNovel(Novel novel)
        {
            string result = "更新失败";

            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                if (novel != null)
                {
                    if (!string.IsNullOrEmpty(novel.Title) &&
                    !string.IsNullOrEmpty(novel.CategoryId) &&
                    !string.IsNullOrEmpty(novel.Name))
                    {
                        if (DBRepository.Update<Novel>(novel, "Id", conn))
                        {
                            result = "success";
                        }
                    }
                }
                else
                {
                    result = "category对象为空";
                    LogService.Log("NovelService.EditNovel", "novel对象为空");
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
        /// 软删除小说
        /// </summary>
        public bool RemoveNovel(int id)
        {
            bool result = false;

            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                result = DBRepository.Delete<Novel>("IsDelete", true, "Id", id, conn);
            }
            catch (Exception e)
            {
                LogService.Log("软删除小说", e.ToString());
            }
            finally
            {
                conn.Close();
            }

            return result;
        }


        /// <summary>
        /// 获取小说文章列表
        /// </summary>
        public List<NovelArticle> GetNovelArticles(int novelId)
        {
            List<NovelArticle> result = null;

            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                result = DBRepository.SelectMore<NovelArticle>("NovelId=" + novelId.ToString(), "Id", true, conn);
            }
            catch (Exception e)
            {
                LogService.Log("小说文章列表", e.ToString());
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 获取小说文章
        /// </summary>
        public NovelArticle GetNovelArticleById(int id)
        {
            NovelArticle result = new NovelArticle();
            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                result = DBRepository.SelectOne<NovelArticle>("Id", id, conn);

            }
            catch (Exception e)
            {
                LogService.Log("获取小说文章", e.ToString());
            }
            finally
            {
                conn.Close();
            }
            return result;
        }

        /// <summary>
        /// 添加小说文章
        /// </summary>
        public string AddNovelArticle(NovelArticle novelArticle)
        {
            string result = "添加失败";

            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                if (novelArticle != null)
                {
                    if (novelArticle.NovelId > 0 &&
                    !string.IsNullOrEmpty(novelArticle.ContentStyle) &&
                    !string.IsNullOrEmpty(novelArticle.Title))
                    {
                        novelArticle.InsertTime = DateTime.Now;
                        List<string> filterColumns = new List<string>() { "Id" };
                        if (DBRepository.Insert<NovelArticle>(novelArticle, conn, filterColumns))
                        {
                            result = "success";
                            //更新内容章节数
                            List<NovelArticle> articles = DBRepository.SelectMore<NovelArticle>("NovelId=" + novelArticle.NovelId, conn);
                            if (articles != null && articles.Count > 0)
                            {
                                DBRepository.UpdateOneColumn<Novel>("ArticleCount", articles.Count, "Id=" + novelArticle.NovelId, conn);
                            }
                        }
                    }
                }
                else
                {
                    result = "novelArticle对象为空";
                    LogService.Log("NovelService.AddNovelArticle", "novelArticle对象为空");
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
        /// 更新小说文章
        /// </summary>
        public string EditNovelArticle(NovelArticle novelArticle)
        {
            string result = "更新失败";

            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                if (novelArticle != null)
                {
                    if (novelArticle.NovelId > 0 &&
                    !string.IsNullOrEmpty(novelArticle.ContentStyle) &&
                    !string.IsNullOrEmpty(novelArticle.Title))
                    {
                        if (DBRepository.Update<NovelArticle>(novelArticle, "Id", conn))
                        {
                            result = "success";
                        }
                    }
                }
                else
                {
                    result = "category对象为空";
                    LogService.Log("NovelService.EditNovelArticle", "novelArticle对象为空");
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
        /// 网站地图
        /// </summary>
        public static List<Novel> GetSiteMap()
        {
            List<Novel> result = new List<Novel>();
            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                string sql = "SELECT [Id],[CategoryId]='',[Title]='',[Keywords]='',[Description]='',[Name]='',[Author]='',[ArticleCount],"
                    + "[InsertTime],[ViewTime],[IsDelete] FROM [Novel] where IsDelete = 0 order by Id desc";
                result = DBRepository.SelectMoreBase<Novel>(sql, conn);
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
