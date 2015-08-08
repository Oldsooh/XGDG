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
    public class CategoryService
    {
        /// <summary>
        /// 根据Id查询分类
        /// </summary>
        /// <param name="id">分类Id</param>
        /// <returns>分类对象</returns>
        public Category GetCategoryById(string id)
        {
            Category album = new Category();
            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                album = DBRepository.SelectOne<Category>("Id='" + id + "'", conn);
            }
            catch (Exception e)
            {
                LogService.Log("查询分类", e.ToString());
            }
            finally
            {
                conn.Close();
            }
            return album;
        }

        /// <summary>
        /// 查询所有分类
        /// </summary>
        /// <returns>所有分类</returns>
        public List<Category> GetCategories(string entitytype)
        {
            List<Category> albums = new List<Category>();
            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                string where = string.Empty;
                if (!string.IsNullOrEmpty(entitytype))
                {
                    where = "EntityType='" + entitytype + "'"; ;
                }
                albums = DBRepository.SelectMore<Category>(where, "OrderNumber", true, conn);
            }
            catch (Exception e)
            {
                LogService.Log("所有分类", e.ToString());
            }
            finally
            {
                conn.Close();
            }
            return albums;
        }

        /// <summary>
        /// 更新分类
        /// </summary>
        public string EditCategory(Category category)
        {
            string result = "更新失败";

            var conn = DBRepository.GetSqlConnection();
            try
            {
                conn.Open();
                if (category != null)
                {
                    if (!string.IsNullOrEmpty(category.Title) &&
                        !string.IsNullOrEmpty(category.Name) &&
                        category.OrderNumber > 0)
                    {
                        if (DBRepository.Update<Category>(category, "Id", conn))
                        {
                            result = "success";
                        }
                    }
                }
                else
                {
                    result = "category对象为空";
                    LogService.Log("CategoryService.EditCategory", "category对象为空");
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
    }
}
