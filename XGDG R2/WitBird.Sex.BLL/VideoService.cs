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
    public class VideoService
    {
        /// <summary>
        /// 根据Id查询视频
        /// </summary>
        /// <param name="id">视频Id</param>
        /// <returns>视频对象</returns>
        public Video GetVideoById(int id)
        {
            Video video = new Video();
            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                video = DBRepository.SelectOne<Video>("Id=" + id.ToString() + " and IsDelete=0", conn);
                if (video != null)
                {
                    DBRepository.UpdateOneColumn<Video>("ViewTime", video.ViewTime + 1, "Id=" + video.Id.ToString(), conn);
                }
            }
            catch (Exception e)
            {
                LogService.Log("查询视频", e.ToString());
            }
            finally
            {
                conn.Close();
            }
            return video;
        }

        /// <summary>
        /// 查询视频列表
        /// </summary>
        /// <param name="categoryId">视频Id</param>
        /// <param name="pageCount">每页显示条数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="count">视频总数</param>
        /// <returns>视频列表</returns>
        public List<Video> GetVideos(string keywords, int pageCount, int pageIndex, out int count)
        {
            List<Video> videos = new List<Video>();
            count = 0;
            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                int tempCount = 0;
                string where = string.Empty;
                if (!string.IsNullOrEmpty(keywords))
                {
                    where += "Title like '%" + DBRepository.FilterSQL(keywords) + "%' or Keywords like '%" + DBRepository.FilterSQL(keywords) + "%' and IsDelete=0";
                }
                else
                {
                    where = "IsDelete=0";
                }
                videos = DBRepository.SelectMore<Video>(where, "Id", false, pageCount, pageIndex, out tempCount, conn);
                count = tempCount;
            }
            catch (Exception e)
            {
                LogService.Log("查询视频列表", e.ToString());
            }
            finally
            {
                conn.Close();
            }
            return videos;
        }

        //最新视频
        public List<Video> GetNewVideos(int count)
        {
            List<Video> videos = new List<Video>();

            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                videos = DBRepository.SelectMore<Video>("IsDelete=0", "Id", false, conn, count);
            }
            catch (Exception e)
            {
                LogService.Log("查询视频列表", e.ToString());
            }
            finally
            {
                conn.Close();
            }
            return videos;
        }

        /// <summary>
        /// 添加视频
        /// </summary>
        public bool AddVideo(Video video)
        {
            bool result = false;
            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                if (video != null)
                {
                    List<string> filterColumns = new List<string>() { "Id" };
                    video.IsDelete = false;
                    result = DBRepository.Insert<Video>(video, conn, filterColumns);
                }
            }
            catch (Exception e)
            {
                LogService.Log("添加视频", e.ToString());
            }
            finally
            {
                conn.Close();
            }
            return result;
        }

        /// <summary>
        /// 更新视频
        /// </summary>
        public bool EditVideo(Video video)
        {
            bool result = false;
            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                if (video != null)
                {
                    List<string> filterColumns = new List<string>() { "Id" };
                    video.IsDelete = false;
                    result = DBRepository.Update<Video>(video, "Id", conn);
                }
            }
            catch (Exception e)
            {
                LogService.Log("添加视频", e.ToString());
            }
            finally
            {
                conn.Close();
            }
            return result;
        }

        /// <summary>
        /// 软件删除视频
        /// </summary>
        public bool RemoveVideo(int id)
        {
            bool result = false;

            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                if (id > 0)
                {
                    result = DBRepository.Delete<Video>("IsDelete", true, "Id", id, conn);
                }
            }
            catch (Exception e)
            {
                LogService.Log("软件删除视频", e.ToString());
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 随机推荐视频列表
        /// </summary>
        /// <returns>视频列表</returns>
        public List<Video> GetRecommendRandom(int count)
        {
            List<Video> videos = new List<Video>();
            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                string sql = "select top " + count.ToString() + " * from Video where IsDelete=0  order by newid()";
                videos = DBRepository.SelectMoreBase<Video>(sql, conn);
            }
            catch (Exception e)
            {
                LogService.Log("随机推荐视频列表", e.ToString());
            }
            finally
            {
                conn.Close();
            }
            return videos;
        }

        /// <summary>
        /// 查询猜你喜欢
        /// </summary>
        /// <returns>专辑列表</returns>
        public List<Album> GetLike(int count)
        {
            List<Album> albums = new List<Album>();
            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                string sql = "select top " + count.ToString() + " * from Album where IsTop=1 and IsDelete=0 order by newid()";
                albums = DBRepository.SelectMoreBase<Album>(sql, conn);
            }
            catch (Exception e)
            {
                LogService.Log("查询猜你喜欢", e.ToString());
            }
            finally
            {
                conn.Close();
            }
            return albums;
        }


        /// <summary>
        /// 网站地图
        /// </summary>
        public static List<Video> GetSiteMap()
        {
            List<Video> result = new List<Video>();
            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                string sql = "SELECT [Id],[Title]='',[Keywords]='',[Description]='',[Thumbnail]='',[Url]='',[UrlM]='',[InsertTime],[ViewTime],[IsDelete]"
                    + "FROM [Video] where IsDelete = 0 order by Id desc";
                result = DBRepository.SelectMoreBase<Video>(sql, conn);
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
