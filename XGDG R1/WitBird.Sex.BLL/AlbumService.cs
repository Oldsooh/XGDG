using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WitBird.Sex.Common;
using WitBird.Sex.DAL;
using WitBird.Sex.Model;
using System.Data.SqlClient;

namespace WitBird.Sex.BLL
{
    public class AlbumService
    {
        /// <summary>
        /// 分布条件查询新闻列表
        /// </summary>
        /// <param name="categoryId">分类列表</param>
        /// <param name="keywords">关键字</param>
        /// <param name="pageCount">每页显示条数</param>
        /// <param name="pageIndex">当前页页码</param>
        /// <param name="count">满足条件的新闻总数</param>
        /// <returns>新闻列表</returns>
        public List<Album> GetAlbumList(string categoryId, string keywords, int pageCount, int pageIndex, out int count)
        {
            List<Album> result = null;
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
                    string[] keys = keywords.Split(' ');
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
                    where += " and IsDelete = 0  and IsShow = 1";
                }
                else
                {
                    where = "IsDelete = 0 and IsShow = 1";
                }
                result = DBRepository.SelectMore<Album>(where, "Id", false, pageCount, pageIndex, out tempCount, conn);
                count = tempCount;
            }
            catch (Exception e)
            {
                LogService.Log("专辑列表", e.ToString());
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 根据Id查询专辑
        /// </summary>
        /// <param name="id">专辑Id</param>
        /// <returns>专辑对象</returns>
        public Album GetAlbumById(int id)
        {
            Album album = new Album();
            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                album = DBRepository.SelectOne<Album>("Id=" + id.ToString() + " and IsDelete=0 and IsShow = 1 ", conn);
                if (album != null)
                {
                    DBRepository.UpdateOneColumn<Album>("ViewTime", album.ViewTime + 1, "Id=" + album.Id.ToString(), conn);
                }
            }
            catch (Exception e)
            {
                LogService.Log("查询专辑", e.ToString());
            }
            finally
            {
                conn.Close();
            }
            return album;
        }

        /// <summary>
        /// 根据分类Id查询专辑列表
        /// </summary>
        /// <param name="categoryId">分类Id</param>
        /// <param name="pageCount">每页显示条数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="count">该分类专辑总数</param>
        /// <returns>专辑列表</returns>
        public List<Album> GetAlbumByCategoryId(string categoryId, int pageCount, int pageIndex, out int count)
        {
            List<Album> albums = new List<Album>();
            count = 0;
            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                int tempCount = 0;
                albums = DBRepository.SelectMore<Album>("CategoryId='" + categoryId + "' and IsDelete=0", "Id", false, pageCount, pageIndex, out tempCount, conn);
                count = tempCount;
            }
            catch (Exception e)
            {
                LogService.Log("根据分类Id查询专辑列表", e.ToString());
            }
            finally
            {
                conn.Close();
            }
            return albums;
        }

        /// <summary>
        /// 根据分类Id查询推荐专辑
        /// </summary>
        /// <param name="categoryId">分类Id</param>
        /// <param name="count">获取数量</param>
        /// <returns>专辑列表</returns>
        public List<Album> GetRecommendAlbumByCategoryId(string categoryId, int count)
        {
            List<Album> albums = new List<Album>();

            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();

                if (categoryId.Equals("All", StringComparison.CurrentCultureIgnoreCase))
                {
                    albums = DBRepository.SelectMore<Album>("IsTop=1 and IsDelete=0 and IsShow = 1", "Id", false, conn, count);
                }
                else if (categoryId.Equals("TopWomen", StringComparison.CurrentCultureIgnoreCase))
                {
                    string categorySql = "(CategoryId = 'Costume' OR CategoryId = 'pure' OR CategoryId = 'sexy' OR CategoryId = 'star') AND ";
                    albums = DBRepository.SelectMore<Album>(categorySql + "IsDelete=0 and IsShow=1 and (convert(date,UpdatedTime) between convert(date,dateadd(dd,-7,getdate())) and convert(date,getdate())) ", "ViewTime", false, conn, count);
                }
                else if (categoryId.Equals("TopMen", StringComparison.CurrentCultureIgnoreCase))
                {
                    string categorySql = "(CategoryId = 'StylishMan') AND ";
                    albums = DBRepository.SelectMore<Album>(categorySql + "IsDelete=0 and IsShow=1 and (convert(date,UpdatedTime) between convert(date,dateadd(dd,-7,getdate())) and convert(date,getdate())) ", "ViewTime", false, conn, count);
                }
                else
                {
                    albums = DBRepository.SelectMore<Album>("CategoryId='" + categoryId + "' and IsTop=1 and IsDelete=0 and IsShow = 1", "Id", false, conn, count);
                }
            }
            catch (Exception e)
            {
                LogService.Log("根据分类Id查询专辑列表", e.ToString());
            }
            finally
            {
                conn.Close();
            }
            return albums;
        }

        /// <summary>
        /// 首页最新上传
        /// </summary>
        /// <returns>专辑列表</returns>
        public List<Album> GetAlbumByNew()
        {
            List<Album> albums = new List<Album>();
            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                albums = DBRepository.SelectMore<Album>("IsDelete=0 and IsShow=1", "Id", false, conn, 10);
            }
            catch (Exception e)
            {
                LogService.Log("查询首页最新上传专辑列表", e.ToString());
            }
            finally
            {
                conn.Close();
            }
            return albums;
        }

        /// <summary>
        /// 查询首页随机专辑列表
        /// </summary>
        /// <returns>专辑列表</returns>
        public List<Album> GetAlbumByRandom(int count)
        {
            List<Album> albums = new List<Album>();
            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                string sql = "select top  " + count.ToString() +
                             " Album.Id,Album.CategoryId,Album.Title,Album.Keywords,Album.Description,Album.IsDelete,Album.IsShow,Album.IsTop,Album.InsertTime,Album.ViewTime, " +
                             " Album.ImagesCount,Image.UrlOriginal,Image.UrlThumbnailWidth102x102,Image.UrlThumbnailWidth235x350,Image.UrlThumbnailWidth490x350,Image.Url " +
                             " from Album join [Image] on Album.Id = [Image].AlbumId where IsDelete=0 and IsShow=1 order by newid()";
                albums = DBRepository.SelectMoreBase<Album>(sql, conn);
            }
            catch (Exception e)
            {
                LogService.Log("查询首页随机专辑列表", e.ToString());
            }
            finally
            {
                conn.Close();
            }
            return albums;
        }

        /// <summary>
        /// 查询首页幻灯专辑列表
        /// </summary>
        /// <returns>专辑列表</returns>
        public List<Slide> GetAlbumBySlide()
        {
            List<Slide> albums = new List<Slide>();
            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                albums = DBRepository.SelectMore<Slide>(null, "Id", false, conn, 5);
            }
            catch (Exception e)
            {
                LogService.Log("查询首页幻灯专辑列表", e.ToString());
            }
            finally
            {
                conn.Close();
            }
            return albums;
        }





        /// <summary>
        /// 根据关键字查询
        /// </summary>
        /// <returns>专辑列表</returns>
        public List<Album> GetAlbumByKey(string key)
        {
            List<Album> albums = new List<Album>();
            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                //albums = albumDao.SelectAlbumByKey(key, conn);
            }
            catch (Exception e)
            {
                LogService.Log("根据关键字查询", e.ToString());
            }
            finally
            {
                conn.Close();
            }
            return albums;
        }

        /// <summary>
        /// 添加专辑及其图片
        /// </summary>
        /// <param name="album">专辑</param>
        /// <param name="images">所属图片</param>
        /// <returns>是否添加成功</returns>
        public bool AddAlbum(Album album, List<Image> images)
        {
            bool result = false;
            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                if (album != null && images != null && images.Count > 0)
                {
                    album.ImagesCount = images.Count;
                    album.UrlOriginal = images[0].UrlOriginal;
                    album.UrlThumbnailWidth102x102 = images[0].UrlThumbnailWidth102x102;
                    album.UrlThumbnailWidth235x350 = images[0].UrlThumbnailWidth235x350;
                    album.UrlThumbnailWidth490x350 = images[0].UrlThumbnailWidth490x350;
                    album.UrlThumbnailHeight200 = images[0].UrlThumbnailHeight200;

                    List<string> filterColumns = new List<string>() { "Id" };
                    int albumId = Int32.Parse(DBRepository.Insert<Album>(album, filterColumns, conn));

                    foreach (var item in images)
                    {
                        item.AlbumId = albumId;
                        DBRepository.Insert<Image>(item, conn, filterColumns);
                    }

                    if (albumId > 0)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception e)
            {
                LogService.Log("添加专辑及其图片", e.ToString());
            }
            finally
            {
                conn.Close();
            }
            return result;
        }

        /// <summary>
        /// 删除专辑
        /// </summary>
        public bool RemoveAlbum(string id)
        {
            bool result = false;

            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                result = DBRepository.Delete<Album>("IsDelete", true, "Id", id, conn);
            }
            catch (Exception e)
            {
                LogService.Log("软删除专辑", e.ToString());
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 获取该专辑的所有图片
        /// </summary>
        public List<Image> GetImagesByAlbumId(int albumId)
        {
            List<Image> images = new List<Image>();

            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                images = DBRepository.SelectMore<Image>("AlbumId='" + albumId.ToString() + "'", "Id", true, conn);
            }
            catch (Exception e)
            {
                LogService.Log("获取该专辑的所有图片", e.ToString());
            }
            finally
            {
                conn.Close();
            }
            return images;
        }

        //获取上一专辑
        public Album GetLastAlbumById(string id, string categoryId)
        {
            Album album = null;
            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                album = DBRepository.SelectOne<Album>("CategoryId = '" + categoryId +
                    "' and IsDelete=0 and IsShow=1 and Id < " + id + " order by Id desc", conn);
            }
            catch (Exception e)
            {
                LogService.Log("获取上一专辑", e.ToString());
            }
            finally
            {
                conn.Close();
            }
            return album;
        }

        /// <summary>
        /// 根据图片Id查询下一专辑
        /// </summary>
        /// <param name="id">专辑Id</param>
        /// <returns>专辑对象</returns>
        public Album GetNextAlbumById(string id, string categoryId)
        {
            Album album = null;
            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                album = DBRepository.SelectOne<Album>("CategoryId = '" + categoryId +
                    "' and IsDelete=0 and IsShow=1 and Id > " + id + " order by Id asc", conn);
            }
            catch (Exception e)
            {
                LogService.Log("根据图片Id查询下一专辑", e.ToString());
            }
            finally
            {
                conn.Close();
            }
            return album;
        }

        /// <summary>
        /// 随机推荐专辑列表
        /// </summary>
        /// <returns>专辑列表</returns>
        public List<Album> GetRecommendRandom(int count)
        {
            List<Album> albums = new List<Album>();
            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                string sql = "select top " + count.ToString() + " * from Album where IsTop=1 and IsDelete=0 and IsShow = 1 order by newid()";
                albums = DBRepository.SelectMoreBase<Album>(sql, conn);
            }
            catch (Exception e)
            {
                LogService.Log("随机推荐专辑列表", e.ToString());
            }
            finally
            {
                conn.Close();
            }
            return albums;
        }

        /// <summary>
        /// 查询猜你喜欢
        /// </summary>
        /// <returns>专辑列表</returns>
        public List<Image> GetLike(int count)
        {
            List<Image> images = new List<Image>();
            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                string sql = "select top " + count.ToString() + " * from Image order by newid()";
                images = DBRepository.SelectMoreBase<Image>(sql, conn);
            }
            catch (Exception e)
            {
                LogService.Log("查询猜你喜欢", e.ToString());
            }
            finally
            {
                conn.Close();
            }
            return images;
        }

        /// <summary>
        /// 推荐或取消
        /// </summary>
        public bool Recommend(int id, bool isRecommend)
        {
            bool result = false;

            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                result = DBRepository.Delete<Album>("IsTop", isRecommend, "Id", id, conn);
            }
            catch (Exception e)
            {
                LogService.Log("软删除专辑", e.ToString());
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
        public static List<Album> GetSiteMap()
        {
            List<Album> result = new List<Album>();
            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                string sql = "SELECT [Id],[CategoryId]='',[Title]='',[Keywords]='',[Description]='',[IsDelete],[IsShow],[IsTop],[InsertTime],"
                    + "[ViewTime],[ImagesCount],[UrlOriginal]='',[UrlThumbnailWidth102x102]='',[UrlThumbnailWidth235x350]='',[UrlThumbnailWidth490x350]='',"
                    + "[Url]='' FROM [Album] where IsDelete = 0 order by Id desc";
                result = DBRepository.SelectMoreBase<Album>(sql, conn);
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

        public AlbumDetail GetAlbumDetail(int id)
        {
            AlbumDetail detail = new AlbumDetail();
            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                Album album  = GetAlbumById(id);

                if (album != null)
                {
                    detail.Album = album;
                    DBRepository.UpdateOneColumn<Album>("ViewTime", detail.Album.ViewTime + 1, "Id=" + detail.Album.Id.ToString(), conn);
                    DBRepository.UpdateOneColumn<Album>("UpdatedTime", DateTime.Now, "Id=" + detail.Album.Id.ToString(), conn);

                    StringBuilder sqlBuilder = new StringBuilder();
                    // Category
                    sqlBuilder.Append("select top 1 * from Category where Id = '");
                    sqlBuilder.Append(detail.Album.CategoryId);
                    sqlBuilder.Append("';");
                    // Images
                    sqlBuilder.Append("select * from Image where AlbumId = ");
                    sqlBuilder.Append(album.Id);
                    sqlBuilder.Append(";");
                    //Last Album
                    sqlBuilder.Append("select top 1 * from Album where CategoryId = '");
                    sqlBuilder.Append(detail.Album.CategoryId);
                    sqlBuilder.Append("' and IsDelete=0 and IsShow=1 and Id < ");
                    sqlBuilder.Append(album.Id);
                    sqlBuilder.Append(" order by Id desc;");
                    // Next Album
                    sqlBuilder.Append("select top 1 * from Album where CategoryId = '");
                    sqlBuilder.Append(detail.Album.CategoryId);
                    sqlBuilder.Append("' and IsDelete=0 and IsShow=1 and Id > ");
                    sqlBuilder.Append(album.Id);
                    sqlBuilder.Append(" order by Id asc;");
                    // Related album
                    if (!string.IsNullOrEmpty(album.Keywords.Trim()))
                    {
                        sqlBuilder.Append("select top 4 * from Album where (");
                        char[] FilterChar = {' ', ',', '，' };
                        string [] keywords = album.Keywords.Split(' ');
                        for (int i = 0; i < keywords.Length; i++ )
                        {
                            string item = keywords[i];

                            if (!string.IsNullOrEmpty(item))
                            {
                                sqlBuilder.Append(" Title like '%");
                                sqlBuilder.Append(item);
                                sqlBuilder.Append("%' or Keywords like '%");
                                sqlBuilder.Append(item);
                                sqlBuilder.Append("%'");

                                if (i != keywords.Length - 1)
                                {
                                    sqlBuilder.Append(" or ");
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        sqlBuilder.Append(" ) and IsDelete = 0 and IsShow = 1;");
                    }
                    // Random recommend album
                    sqlBuilder.Append("select top 8 * from Album where IsTop=1 and IsDelete=0 and IsShow = 1 order by newid();");

                    SqlCommand cmd = new SqlCommand(sqlBuilder.ToString(), conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    // Category
                    while (reader.Read())
                    {
                        detail.Category = new Category
                        {
                            Id = reader[0] != DBNull.Value ? reader.GetString(0) : string.Empty,
                            EntityType = reader[1] != DBNull.Value ? reader.GetString(1) : string.Empty,
                            Title = reader[2] != DBNull.Value ? reader.GetString(2) : string.Empty,
                            Keywords = reader[3] != DBNull.Value ? reader.GetString(3) : string.Empty,
                            Description = reader[4] != DBNull.Value ? reader.GetString(4) : string.Empty,
                            Name = reader[5] != DBNull.Value ? reader.GetString(5) : string.Empty,
                            OrderNumber = reader[6] != DBNull.Value ? reader.GetInt32(6) : 0,
                            IsShow = reader[7] != DBNull.Value ? reader.GetBoolean(7) : false,
                            UpdatedTime = reader[8] != DBNull.Value ? reader.GetDateTime(8) : DateTime.Now
                        };
                    }

                    // Images
                    reader.NextResult();
                    detail.Images = new List<Image>();
                    while (reader.Read())
                    {
                        Image image = new Image
                        {
                            Id = reader[0] != DBNull.Value ? reader.GetInt32(0) : 0,
                            AlbumId = reader[1] != DBNull.Value ? reader.GetInt32(1) : 0,
                            UrlOriginal = reader[2] != DBNull.Value ? reader.GetString(2) : string.Empty,
                            UrlThumbnailWidth102x102 = reader[3] != DBNull.Value ? reader.GetString(3) : string.Empty,
                            UrlThumbnailWidth235x350 = reader[4] != DBNull.Value ? reader.GetString(4) : string.Empty,
                            UrlThumbnailWidth490x350 = reader[5] != DBNull.Value ? reader.GetString(5) : string.Empty,
                            UrlThumbnailHeight200 = reader[6] != DBNull.Value ? reader.GetString(6) : string.Empty
                        };

                        detail.Images.Add(image);
                    }

                    List<Album> albums = null;
                    reader.NextResult();
                    // Last
                    GetHomeAlbums(out albums, reader);
                    detail.LastAlbum = albums != null && albums.Count > 0 ? albums[0] : null;
                    
                    // Next
                    GetHomeAlbums(out albums, reader);
                    detail.NextAlbum = albums != null && albums.Count > 0 ? albums[0] : null;
                    
                    // Related
                    GetHomeAlbums(out albums, reader);
                    detail.RelatedAlbum = albums;

                    // Recommended
                    GetHomeAlbums(out albums, reader);
                    detail.RecommendAlbum = albums;

                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                }
            }
            catch (Exception e)
            {
                LogService.Log("查询专辑", e.ToString());
            }
            finally
            {
                conn.Close();
            }

            return detail;
        }

        public HomeData GetHomeData()
        {
            HomeData homeData = new HomeData();
            var conn = DBRepository.GetSqlConnection();

            try
            {
                StringBuilder sqlBuilder = new StringBuilder();
                //Slides
                sqlBuilder.Append("select top 10 * from Slide order by id desc; ");
                //New upload
                sqlBuilder.Append("select top 4 * from Album where IsDelete=0 and IsShow=1 order by id desc; ");
                //Recomended
                sqlBuilder.Append("select top 6 * from Album where IsTop=1 and IsShow = 1 and IsDelete = 0 order by id desc; ");
                // Top women per week
                sqlBuilder.Append(@"
                    select top 6 * from Album 
                    where 
                    (CategoryId = 'Costume' OR CategoryId = 'pure' OR CategoryId = 'sexy' OR CategoryId = 'star') 
                    AND IsDelete=0 
                    and IsShow=1 
                    and (convert(date,UpdatedTime) between convert(date,dateadd(dd,-7,getdate())) and convert(date,getdate())) 
                    order by ViewTime desc;");
                // Top men per week
                sqlBuilder.Append(@"
                    select top 6 * from Album 
                    where 
                    CategoryId = 'StylishMan'
                    AND IsDelete=0 
                    and IsShow=1 
                    and (convert(date,UpdatedTime) between convert(date,dateadd(dd,-7,getdate())) and convert(date,getdate())) 
                    order by ViewTime desc;");
                //RecommendPure
                sqlBuilder.Append("select top 12 * from Album where CategoryId = 'pure' and IsDelete=0 and IsTop = 1 and IsShow = 1 order by id desc;");
                //RecommendSexy
                sqlBuilder.Append("select top 12 * from Album where CategoryId = 'sexy' and IsDelete=0 and IsTop = 1 and IsShow = 1 order by id desc;");
                //Recommend Star
                sqlBuilder.Append("select top 12 * from Album where CategoryId = 'star' and IsDelete=0 and IsTop = 1 and IsShow = 1 order by id desc;");
                //Recommend Costume
                sqlBuilder.Append("select top 12 * from Album where CategoryId = 'Costume' and IsDelete=0 and IsTop = 1 and IsShow = 1 order by id desc;");
                //Recommend stylish man
                sqlBuilder.Append("select top 12 * from Album where CategoryId = 'StylishMan' and IsDelete=0 and IsTop = 1 and IsShow = 1 order by id desc;");

                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlBuilder.ToString(), conn);
                SqlDataReader reader = cmd.ExecuteReader();

                List<Album> albums = null;
                GetHomeSlides(ref homeData, reader);

                GetHomeAlbums(out albums, reader);
                homeData.NewUploads = albums;

                GetHomeAlbums(out albums, reader);
                homeData.RecommendPictures = albums;

                GetHomeAlbums(out albums, reader);
                homeData.TopWomen = albums;

                GetHomeAlbums(out albums, reader);
                homeData.TopMen = albums;

                GetHomeAlbums(out albums, reader);
                homeData.RecommendPure = albums;

                GetHomeAlbums(out albums, reader);
                homeData.RecommendSexy = albums;

                GetHomeAlbums(out albums, reader);
                homeData.RecommendStar = albums;

                GetHomeAlbums(out albums, reader);
                homeData.RecommendCostume = albums;

                GetHomeAlbums(out albums, reader);
                homeData.RecommendStylishMan = albums;

                if (!reader.IsClosed)
                {
                    reader.Close();
                }
            }
            catch (Exception e)
            {
                LogService.Log("获取首页数据抛异常啦", e.ToString());
            }
            finally
            {
                if (conn.State != System.Data.ConnectionState.Closed)
                {
                    conn.Close();
                }
            }

            return homeData;
        }

        private static void GetHomeSlides(ref HomeData homeData, SqlDataReader reader)
        {
            homeData.Slides = new List<Slide>();
            while (reader.Read())
            {
                Slide slide = new Slide
                {
                    Id = reader[0] != DBNull.Value ? reader.GetInt32(0) : 0,
                    Title = reader[0] != DBNull.Value ? reader.GetString(1) : string.Empty,
                    Url = reader[0] != DBNull.Value ? reader.GetString(2) : string.Empty,
                    Image = reader[0] != DBNull.Value ? reader.GetString(3) : string.Empty,
                    InsertTime = reader[0] != DBNull.Value ? reader.GetDateTime(4) : DateTime.Now
                };

                homeData.Slides.Add(slide);
            }

            reader.NextResult();
        }

        private static void GetHomeAlbums(out List<Album> albums, SqlDataReader reader)
        {
            albums = new List<Album>();
            while (reader.Read())
            {
                Album album = new Album
                {
                    Id = reader[0] != DBNull.Value ? reader.GetInt32(0) : 0,
                    CategoryId = reader[1] != DBNull.Value ? reader.GetString(1) : string.Empty,
                    Title = reader[2] != DBNull.Value ? reader.GetString(2) : string.Empty,
                    Keywords = reader[3] != DBNull.Value ? reader.GetString(3) : string.Empty,
                    Description = reader[4] != DBNull.Value ? reader.GetString(4) : string.Empty,
                    IsDelete = reader[5] != DBNull.Value ? reader.GetBoolean(5) : false,
                    IsShow = reader[6] != DBNull.Value ? reader.GetBoolean(6) : true,
                    IsTop = reader[7] != DBNull.Value ? reader.GetBoolean(7) : true,
                    InsertTime = reader[8] != DBNull.Value ? reader.GetDateTime(8) : DateTime.Now,
                    ViewTime = reader[9] != DBNull.Value ? reader.GetInt32(9) : 0,
                    ImagesCount = reader[10] != DBNull.Value ? reader.GetInt32(10) : 0,
                    UrlOriginal = reader[11] != DBNull.Value ? reader.GetString(11) : string.Empty,
                    UrlThumbnailWidth102x102 = reader[12] != DBNull.Value ? reader.GetString(12) : string.Empty,
                    UrlThumbnailWidth235x350 = reader[13] != DBNull.Value ? reader.GetString(13) : string.Empty,
                    UrlThumbnailWidth490x350 = reader[14] != DBNull.Value ? reader.GetString(14) : string.Empty,
                    UrlThumbnailHeight200 = reader[15] != DBNull.Value ? reader.GetString(15) : string.Empty,
                    UpdatedTime = reader[16] != DBNull.Value ? reader.GetDateTime(16) : DateTime.Now
                };

                albums.Add(album);
            }

            reader.NextResult(); ;
        }
    }
}
