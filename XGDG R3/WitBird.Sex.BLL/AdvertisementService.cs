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
    public class AdvertisementService
    {
        /// <summary>
        /// 获取广告列表
        /// </summary>
        public List<Advertisement> GetAdvertisements()
        {
            List<Advertisement> result = null;

            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                result = DBRepository.SelectMore<Advertisement>(null, "Id", true, conn);
            }
            catch (Exception e)
            {
                LogService.Log("获取广告列表", e.ToString());
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 获取指定广告
        /// </summary>
        public Advertisement GetAdvertisementById(int id)
        {
            Advertisement result = null;

            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                result = DBRepository.SelectOne<Advertisement>("Id", id, conn);
            }
            catch (Exception e)
            {
                LogService.Log("获取指定广告", e.ToString());
            }
            finally
            {
                conn.Close();
            }

            return result;
        }


        /// <summary>
        /// 更新广告列表
        /// </summary>
        public string EditAdvertisement(Advertisement advertisement)
        {
            string result = "更新失败";

            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                if (advertisement != null)
                {
                    if (advertisement.Id > 0 && !string.IsNullOrEmpty(advertisement.Name))
                    {
                        if (DBRepository.Update<Advertisement>(advertisement, "Id", conn))
                        {
                            result = "success";
                        }
                    }
                }
                else
                {
                    result = "friendlyLink对象为空";
                    LogService.Log("AdvertisementService.EditAdvertisement", "advertisement对象为空");
                }
            }
            catch (Exception e)
            {
                result = "程序出现异常，详情见日志";
                LogService.Log("更新广告列表", e.ToString());
            }
            finally
            {
                conn.Close();
            }

            return result;
        }
    }
}
