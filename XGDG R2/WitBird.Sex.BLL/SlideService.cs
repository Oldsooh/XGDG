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
    public class SlideService
    {
        /// <summary>
        /// 获取幻灯片列表
        /// </summary>
        public List<Slide> GetSlides()
        {
            List<Slide> result = null;

            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                result = DBRepository.SelectMore<Slide>(null, "Id", false, conn);
            }
            catch (Exception e)
            {
                LogService.Log("获取幻灯片列表", e.ToString());
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 添加幻灯片
        /// </summary>
        public string AddSlide(Slide slide)
        {
            string result = "添加失败";

            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                if (slide != null)
                {
                    if (!string.IsNullOrEmpty(slide.Title) &&
                        !string.IsNullOrEmpty(slide.Url) &&
                        !string.IsNullOrEmpty(slide.Image))
                    {
                        slide.InsertTime = DateTime.Now;
                        List<string> filterColumns = new List<string>() { "Id" };
                        if (DBRepository.Insert<Slide>(slide, conn, filterColumns))
                        {
                            result = "success";
                        }
                    }
                }
                else
                {
                    result = "slide对象为空";
                    LogService.Log("SlideService.AddSlide", "slide对象为空");
                }
            }
            catch (Exception e)
            {
                result = "程序出现异常，详情见日志";
                LogService.Log("添加幻灯片", e.ToString());
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 真删除幻灯片
        /// </summary>
        public bool RemoveSlide(int id)
        {
            bool result = false;

            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                if (id > 0)
                {
                    result = DBRepository.Delete<Slide>("Id", id, conn);
                }
            }
            catch (Exception e)
            {
                LogService.Log("真删除幻灯片", e.ToString());
            }
            finally
            {
                conn.Close();
            }

            return result;
        }
    }
}
