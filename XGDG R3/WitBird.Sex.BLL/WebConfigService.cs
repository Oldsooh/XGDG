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
    public class WebConfigService
    {
        /// <summary>
        /// 查询网站配置信息
        /// </summary>
        public WebConfig GetWebConfig()
        {
            WebConfig result = null;

            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                result = DBRepository.SelectOne<WebConfig>("Id", 1, conn);
            }
            catch (Exception e)
            {
                LogService.Log("查询网站配置信息", e.ToString());
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 更新网站配置信息
        /// </summary>
        /// <param name="webConfig">网站配置信息对象</param>
        public bool EditWebConfig(WebConfig webConfig)
        {
            bool result = false;

            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                result = DBRepository.Update<WebConfig>(webConfig, "Id", conn);
            }
            catch (Exception e)
            {
                LogService.Log("更新网站配置信息", e.ToString());
            }
            finally
            {
                conn.Close();
            }

            return result;
        }
    }
}
