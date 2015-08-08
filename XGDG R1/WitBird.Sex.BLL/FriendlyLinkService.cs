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
    public class FriendlyLinkService
    {
        /// <summary>
        /// 获取友情链接
        /// </summary>
        public List<FriendlyLink> GetFriendlyLinks()
        {
            List<FriendlyLink> result = null;

            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                result = DBRepository.SelectMore<FriendlyLink>(null, "Sort", true, conn);
            }
            catch (Exception e)
            {
                LogService.Log("获取友情链接", e.ToString());
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 添加友情链接
        /// </summary>
        public string AddFriendlyLink(FriendlyLink friendlyLink)
        {
            string result = "添加失败";

            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                if (friendlyLink != null)
                {
                    if (!string.IsNullOrEmpty(friendlyLink.Name) &&
                        !string.IsNullOrEmpty(friendlyLink.Link) &&
                        friendlyLink.Sort > 0)
                    {
                        FriendlyLink temp = DBRepository.SelectOne<FriendlyLink>("Id", friendlyLink.Id, conn);
                        conn.Close();
                        conn.Open();
                        if (temp == null)
                        {
                            List<string> filterColumns = new List<string>() { "Id" };
                            if (DBRepository.Insert<FriendlyLink>(friendlyLink, conn, filterColumns))
                            {
                                result = "success";
                            }
                        }
                    }
                }
                else
                {
                    result = "friendlyLink对象为空";
                    LogService.Log("FriendlyLinkService.AddFriendlyLink", "friendlyLink对象为空");
                }
            }
            catch (Exception e)
            {
                result = "程序出现异常，详情见日志";
                LogService.Log("添加友情链接", e.ToString());
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 更新友情链接
        /// </summary>
        public string EditFriendlyLink(FriendlyLink friendlyLink)
        {
            string result = "更新失败";

            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                if (friendlyLink != null)
                {
                    if (friendlyLink.Id > 0 &&
                        !string.IsNullOrEmpty(friendlyLink.Name) &&
                        !string.IsNullOrEmpty(friendlyLink.Link) &&
                        friendlyLink.Sort > 0)
                    {
                        if (DBRepository.Update<FriendlyLink>(friendlyLink, "Id", conn))
                        {
                            result = "success";
                        }
                    }
                }
                else
                {
                    result = "friendlyLink对象为空";
                    LogService.Log("FriendlyLinkService.EditFriendlyLink", "friendlyLink对象为空");
                }
            }
            catch (Exception e)
            {
                result = "程序出现异常，详情见日志";
                LogService.Log("更新友情链接", e.ToString());
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 真删除友情链接
        /// </summary>
        public bool RemoveFriendlyLink(int id)
        {
            bool result = false;

            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                if (id > 0)
                {
                    result = DBRepository.Delete<FriendlyLink>("Id", id, conn);
                }
            }
            catch (Exception e)
            {
                LogService.Log("真删除友情链接", e.ToString());
            }
            finally
            {
                conn.Close();
            }

            return result;
        }
    }
}
