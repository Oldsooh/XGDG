using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WitBird.Sex.DAL
{
    public class DBRepository
    {
        /// <summary>
        /// Sql数据库连接字符串
        /// </summary>
        static string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["SqlConn"].ConnectionString;

        /// <summary>
        /// 获取Sql数据库连接对象
        /// </summary>
        public static SqlConnection GetSqlConnection()
        {
            return new SqlConnection(connStr);
        }

        /// <summary>
        /// 过虑SQL语句，防注入
        /// </summary>
        /// <param name="sql">SQL语句</param>
        public static string FilterSQL(string sql)
        {
            if (!string.IsNullOrEmpty(sql))
            {
                if (sql.Contains("'"))
                {
                    sql = sql.Replace("'", "''");
                }
            }
            return sql;
        }



        #region 私有方法

        /// <summary>
        /// 查询实体列名集，可过虑指定列. 返回结果如：“Id,Title,Name,Desctription,InsertTime”
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="filterColumns">指定过虑列名集合</param>
        /// <returns>实体列名集, 以“,”隔开</returns>
        private static string GetColumns<T>(List<string> filterColumns)
        {
            string columns = string.Empty;

            T t = default(T);
            PropertyInfo[] propertypes = null;

            t = Activator.CreateInstance<T>();
            propertypes = t.GetType().GetProperties();

            if (propertypes != null && propertypes.Count() > 0)
            {
                foreach (PropertyInfo pi in propertypes)
                {
                    if (filterColumns != null && filterColumns.Count > 0)
                    {
                        foreach (string filterColumn in filterColumns)
                        {
                            if (filterColumn != pi.Name)//如果指定过虑列名，则过虑掉它
                            {
                                columns += pi.Name + ",";
                            }
                        }
                    }
                    else
                    {
                        columns += pi.Name + ",";//如果没有指定过虑列名，那么直接添加全部列名
                    }
                }
            }

            if (!string.IsNullOrEmpty(columns))
            {
                columns = columns.Substring(0, columns.Length - 1);
            }

            return columns;
        }

        /// <summary>
        /// 是否包含指定列名
        /// </summary>
        /// <param name="target">目标列名</param>
        /// <param name="filterColumns">指定过虑列名集合</param>
        /// <returns>是否包含</returns>
        private static bool IsContainColumn(string target, List<string> filterColumns)
        {
            bool result = false;

            if (filterColumns != null && filterColumns.Count > 0)
            {
                foreach (string filterColumn in filterColumns)
                {
                    if (filterColumn == target)
                    {
                        result = true;
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 查询单个Int类型值
        /// </summary>
        public static int SelectInt(string sql, string columnName, SqlConnection conn)
        {
            int result = 0;

            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows && reader.FieldCount > 0)
            {
                while (reader.Read())
                {
                    result = Int32.Parse(reader[columnName].ToString());
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
            }

            return result;
        }

        /// <summary>
        /// 查询多条实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="conn">连接对象</param>
        /// <returns>实体列表</returns>
        public static List<T> SelectMoreBase<T>(string sql, SqlConnection conn)
        {
            List<T> list = new List<T>();
            T t = default(T);
            PropertyInfo[] propertypes = null;

            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows && reader.FieldCount > 0)
            {
                while (reader.Read())
                {
                    t = Activator.CreateInstance<T>();
                    propertypes = t.GetType().GetProperties();

                    foreach (PropertyInfo pi in propertypes)
                    {
                        object piValue = null;
                        try
                        {
                            piValue = reader[pi.Name];
                            if (!string.IsNullOrEmpty(piValue.ToString()))
                            {
                                pi.SetValue(t, piValue, null);
                            }
                        }
                        catch (IndexOutOfRangeException e)
                        {
                            //查询结果中没的指定的列名不用赋值
                        }
                    }

                    list.Add(t);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
            }

            return list.Count == 0 ? null : list;
        }

        #endregion



        #region 查询操作

        /// <summary>
        /// 查询一个实体(columnName为条件列名，value为列表值，如："Id, 123")
        /// </summary>
        public static T SelectOne<T>(string columnName, object value, SqlConnection conn)
        {
            T t = default(T);
            PropertyInfo[] propertypes = null;

            if (!string.IsNullOrEmpty(columnName) && value != null)
            {
                T tempT = Activator.CreateInstance<T>();
                string table = tempT.GetType().Name;
                string sql = "select top 1 * from " + table + " where " + columnName + "='" + value.ToString() + "'";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows && reader.FieldCount > 0)
                {
                    t = Activator.CreateInstance<T>();
                    propertypes = t.GetType().GetProperties();

                    while (reader.Read())
                    {
                        foreach (PropertyInfo pi in propertypes)
                        {
                            object piValue = null;
                            try
                            {
                                piValue = reader[pi.Name];
                                if (!string.IsNullOrEmpty(piValue.ToString()))
                                {
                                    pi.SetValue(t, piValue, null);
                                }
                            }
                            catch (IndexOutOfRangeException e)
                            {
                                //查询结果中没的指定的列名不用赋值
                            }
                        }
                    }
                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                }
            }

            return t;
        }

        /// <summary>
        /// 查询一个实体(where为打条，如：“ age > 10 and Sex ='男' ”)
        /// </summary>
        public static T SelectOne<T>(string where, SqlConnection conn)
        {
            T t = default(T);
            PropertyInfo[] propertypes = null;

            if (!string.IsNullOrEmpty(where))
            {
                T tempT = Activator.CreateInstance<T>();
                string table = tempT.GetType().Name;
                string sql = "select top 1 * from " + table + " where " + where;
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows && reader.FieldCount > 0)
                {
                    t = Activator.CreateInstance<T>();
                    propertypes = t.GetType().GetProperties();

                    while (reader.Read())
                    {
                        foreach (PropertyInfo pi in propertypes)
                        {
                            object piValue = null;
                            try
                            {
                                piValue = reader[pi.Name];
                                if (!string.IsNullOrEmpty(piValue.ToString()))
                                {
                                    pi.SetValue(t, piValue, null);
                                }
                            }
                            catch (IndexOutOfRangeException e)
                            {
                                //查询结果中没的指定的列名不用赋值
                            }
                        }
                    }
                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                }
            }

            return t;
        }

        /// <summary>
        /// SQL查询
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="conn">连接对象</param>
        /// <returns>实体列表</returns>
        public static List<T> Select<T>(string sql, SqlConnection conn)
        {
            return SelectMoreBase<T>(sql, conn);
        }

        /// <summary>
        /// 查询多条实体
        /// (where为查询条件，如："age > 25". 为空则无需任务条件)
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="where">查询条件</param>
        /// <param name="conn">连接对象</param>
        /// <returns>实体列表</returns>
        public static List<T> SelectMore<T>(string where, SqlConnection conn)
        {
            T t = Activator.CreateInstance<T>();
            string table = t.GetType().Name;

            string sql = "select top 1000 * from " + table;
            if (!string.IsNullOrEmpty(where))
            {
                sql = sql + " where " + where;
            }

            return SelectMoreBase<T>(sql, conn);
        }

        /// <summary>
        /// 查询多条实体
        /// (
        /// where为查询条件，如："age > 25". 为空则无需任务条件. 
        /// orderby为排序列名，如："PostTime". 为空则按数据库为默认排序. 
        /// isASC为是否升序，true为升序，false为降序.
        /// )
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序列名</param>
        /// <param name="isASC">是否升序</param>
        /// <param name="top">前多少条</param>
        /// <param name="conn">连接对象</param>
        /// <returns>实体列表</returns>
        public static List<T> SelectMore<T>(string where, string orderby, bool isASC, SqlConnection conn, int top = 0)
        {
            T t = Activator.CreateInstance<T>();
            string table = t.GetType().Name;

            string sql = string.Empty;
            if (top == 0)
            {
                sql = "select top 1000 * from " + table;
            }
            else
            {
                sql = "select top " + top.ToString() + " * from " + table;
            }

            if (!string.IsNullOrEmpty(where))
            {
                sql = sql + " where " + where;
            }

            if (!string.IsNullOrEmpty(orderby))
            {
                if (isASC)
                {
                    sql = sql + " order by " + orderby + " asc";
                }
                else
                {
                    sql = sql + " order by " + orderby + " desc";
                }
            }

            return SelectMoreBase<T>(sql, conn);
        }

        /// <summary>
        /// 查询多条实体，分第一、第二排序
        /// (
        /// where为查询条件，如："age > 25". 为空则无需任务条件. 
        /// orderbyFirst为排序列名，如："PostTime". 非空. 
        /// isASCFirst为是否升序，true为升序，false为降序.
        /// orderbySecond为排序列名，如："PostTime". 非空. 
        /// isASCSecond为是否升序，true为升序，false为降序.
        /// )
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="where">查询条件</param>
        /// <param name="orderbyFirst">排序列名一</param>
        /// <param name="isASCFirst">是否升序一</param>
        /// <param name="orderbySecond">排序列名二</param>
        /// <param name="isASCSecond">是否升序二</param>
        /// <param name="conn">连接对象</param>
        /// <returns>实体列表</returns>
        public static List<T> SelectMore<T>(string where, string orderbyFirst, bool isASCFirst, string orderbySecond, bool isASCSecond, SqlConnection conn, int top = 0)
        {
            T t = Activator.CreateInstance<T>();
            string table = t.GetType().Name;

            string sql = string.Empty;

            if (top == 0)
            {
                sql = "select top 1000 * from " + table;
            }
            else
            {
                sql = "select top " + top.ToString() + " * from " + table;
            }

            if (!string.IsNullOrEmpty(where))
            {
                sql = sql + " where " + where;
            }

            if (!string.IsNullOrEmpty(orderbyFirst) && !string.IsNullOrEmpty(orderbySecond))
            {
                string sort1 = isASCFirst ? " asc, " : " desc, ";
                string sort2 = isASCSecond ? " asc " : " desc ";

                sql = sql + " order by " + orderbyFirst + sort1 + orderbySecond + sort2;
            }

            return SelectMoreBase<T>(sql, conn);
        }

        /// <summary>
        /// 查询多条实体
        /// (
        /// where为查询条件，如："age > 25". 为空则无需任务条件. 
        /// orderby为排序列名，如："PostTime". 不能为空. 
        /// isASC为是否升序，true为升序，false为降序. 
        /// pageCount为每页查询条数. 
        /// pageIndex当前页码.
        /// )
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序列名(不能为空)</param>
        /// <param name="isASC">是否升序</param>
        /// <param name="pageCount">每页查询条数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="conn">连接对象</param>
        /// <returns>实体列表</returns>
        public static List<T> SelectMore<T>(string where, string orderby, bool isASC, int pageCount, int pageIndex, out int totalCount, SqlConnection conn)
        {
            List<T> result = null;
            totalCount = 0;
            if (pageCount > 1000)
            {
                pageCount = 1000;
            }

            string sql = string.Empty;

            if (!string.IsNullOrEmpty(orderby))
            {
                where = string.IsNullOrEmpty(where) ? string.Empty : " where " + where;

                string sort = isASC ? "asc" : "desc";

                T t = Activator.CreateInstance<T>();
                string table = t.GetType().Name;

                string totalSql = "select count(1) as TotalCount from " + table + where;
                totalCount = SelectInt(totalSql, "TotalCount", conn);

                string tempTable = "(select *,ROW_NUMBER() over(order by " + orderby + " " + sort + ") as RowNumber from " + table + where + ") as temp";

                sql = "select * from " + tempTable
                    + " where temp.RowNumber>(" + pageIndex.ToString() + "-1)*" + pageCount.ToString()
                    + " and temp.RowNumber<=" + pageIndex.ToString() + "*" + pageCount.ToString();
            }

            result = SelectMoreBase<T>(sql, conn);

            return result;
        }

        #endregion



        #region 添加操作

        /// <summary>
        /// 添加实体(可指定过虑列名，为空则不过虑)
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="t">实体对象</param>
        /// <param name="conn">连接对象</param>
        /// <param name="filterColumns">指定过虑列名集合</param>
        /// <returns>添加成功与否</returns>
        public static bool Insert<T>(T t, SqlConnection conn, List<string> filterColumns = null)
        {
            bool result = false;

            Type type = t.GetType();
            PropertyInfo[] props = type.GetProperties();

            StringBuilder sb = new StringBuilder();

            sb.Append("insert into " + type.Name + " (");

            //拼接 Para1,Para2,Para3
            if (props != null && props.Count() > 0)
            {
                foreach (PropertyInfo pi in props)
                {
                    object value = pi.GetValue(t, null);
                    if (value != null && //去掉null值，保留string.Empty
                        value.ToString() != "0001/1/1 0:00:00")//去掉默认时间
                    {
                        if (filterColumns != null && filterColumns.Count > 0)//如果指定过虑列名，则过虑掉它
                        {
                            if (!IsContainColumn(pi.Name, filterColumns))
                            {
                                sb.Append("[" + pi.Name + "],");
                            }
                        }
                        else
                        {
                            sb.Append("[" + pi.Name + "],");//如果没有指定过虑列名，那么直接添加全部列名
                        }
                    }
                }
                if (sb.ToString().Contains(","))
                {
                    sb.Remove(sb.Length - 1, 1);//去掉最后一个逗号
                }
            }

            sb.Append(") values (");

            //赋值 Value1,Value2,Value3
            if (props != null && props.Count() > 0)
            {
                foreach (PropertyInfo pi in props)
                {
                    object value = pi.GetValue(t, null);
                    if (value != null && //去掉null值，保留string.Empty
                        value.ToString() != "0001/1/1 0:00:00")//去掉默认时间
                    {
                        if (filterColumns != null && filterColumns.Count > 0)//如果指定过虑列名，则过虑掉它
                        {
                            if (!IsContainColumn(pi.Name, filterColumns))
                            {
                                sb.Append("'" + FilterSQL(value.ToString()) + "',");
                            }
                        }
                        else
                        {
                            sb.Append("'" + FilterSQL(value.ToString()) + "',");//如果没有指定过虑列名，那么直接添加全部列名
                        }
                    }
                }
                if (sb.ToString().Contains(","))
                {
                    sb.Remove(sb.Length - 1, 1);//去掉最后一个逗号
                }
            }

            sb.Append(")");

            string sql = sb.ToString();

            SqlCommand cmd = new SqlCommand(sql, conn);
            if (cmd.ExecuteNonQuery() > 0)
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// 添加实体并返回string类型自增值，Int32/Int64需自行转换. (至少指定过虑 自增主键 列名)
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="t">实体对象</param>
        /// <param name="filterColumns">指定过虑列名集合</param>
        /// <param name="conn">连接对象</param>
        /// <returns>返回string类型自增值</returns>
        public static string Insert<T>(T t, List<string> filterColumns, SqlConnection conn)
        {
            string result = null;

            Type type = t.GetType();
            PropertyInfo[] props = type.GetProperties();

            StringBuilder sb = new StringBuilder();

            sb.Append("insert into " + type.Name + " (");

            //拼接 Para1,Para2,Para3
            if (props != null && props.Count() > 0)
            {
                foreach (PropertyInfo pi in props)
                {
                    object value = pi.GetValue(t, null);
                    if (value != null && //去掉null值，保留string.Empty
                        value.ToString() != "0001/1/1 0:00:00")//去掉默认时间
                    {
                        if (filterColumns != null && filterColumns.Count > 0)//如果指定过虑列名，则过虑掉它
                        {
                            if (!IsContainColumn(pi.Name, filterColumns))
                            {
                                sb.Append("[" + pi.Name + "],");
                            }
                        }
                        else
                        {
                            sb.Append("[" + pi.Name + "],");//如果没有指定过虑列名，那么直接添加全部列名
                        }
                    }
                }
                if (sb.ToString().Contains(","))
                {
                    sb.Remove(sb.Length - 1, 1);//去掉最后一个逗号
                }
            }

            sb.Append(") values (");

            //赋值 Value1,Value2,Value3
            if (props != null && props.Count() > 0)
            {
                foreach (PropertyInfo pi in props)
                {
                    object value = pi.GetValue(t, null);
                    if (value != null && //去掉null值，保留string.Empty
                        value.ToString() != "0001/1/1 0:00:00")//去掉默认时间
                    {
                        if (filterColumns != null && filterColumns.Count > 0)//如果指定过虑列名，则过虑掉它
                        {
                            if (!IsContainColumn(pi.Name, filterColumns))
                            {
                                sb.Append("'" + FilterSQL(value.ToString()) + "',");
                            }
                        }
                        else
                        {
                            sb.Append("'" + FilterSQL(value.ToString()) + "',");//如果没有指定过虑列名，那么直接添加全部列名
                        }
                    }
                }
                if (sb.ToString().Contains(","))
                {
                    sb.Remove(sb.Length - 1, 1);//去掉最后一个逗号
                }
            }

            sb.Append(")");

            sb.Append(" select @@IDENTITY");

            string sql = sb.ToString();

            SqlCommand cmd = new SqlCommand(sql, conn);
            object obj = cmd.ExecuteScalar();
            if (obj != null)
            {
                result = obj.ToString();
            }

            return result;
        }

        #endregion



        #region 更新操作

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="t">实体对象</param>
        /// <param name="primaryKey">主键</param>
        /// <param name="conn">连接对象</param>
        /// <returns>更新成功与否</returns>
        public static bool Update<T>(T t, string primaryKey, SqlConnection conn)
        {
            bool result = false;

            Type type = t.GetType();
            PropertyInfo[] props = type.GetProperties();
            string primaryKeyValue = string.Empty;

            StringBuilder sb = new StringBuilder();

            sb.Append("update " + type.Name + " set ");

            //拼接 Para1=Value1,Para2=Value2,Para3=Value3
            if (props != null && props.Count() > 0)
            {
                foreach (PropertyInfo pi in props)
                {
                    object value = pi.GetValue(t, null);
                    if (value != null && //去掉null值，保留string.Empty
                        value.ToString() != "0001/1/1 0:00:00" && //去掉默认时间
                        !string.IsNullOrEmpty(primaryKey) &&
                        pi.Name != primaryKey
                        )
                    {
                        sb.Append("[" + pi.Name + "]='" + FilterSQL(value.ToString()) + "',");
                    }
                    else if (!string.IsNullOrEmpty(primaryKey) && pi.Name == primaryKey)
                    {
                        primaryKeyValue = FilterSQL(value.ToString());
                    }
                }
                if (sb.ToString().Contains(","))
                {
                    sb.Remove(sb.Length - 1, 1);//去掉最后一个逗号
                }
            }

            sb.Append(" where " + primaryKey + "='" + primaryKeyValue + "'");

            string sql = sb.ToString();

            SqlCommand cmd = new SqlCommand(sql, conn);
            if (cmd.ExecuteNonQuery() > 0)
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// 更新一列数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="column">被更新列名</param>
        /// <param name="value">被更新值</param>
        /// <param name="where">更新条件</param>
        /// <param name="conn">连接对象</param>
        /// <returns>更新成功与否</returns>
        public static bool UpdateOneColumn<T>(string column, object value, string where, SqlConnection conn)
        {
            bool result = false;

            if (value != null)
            {
                string sql = "update ";

                T t = Activator.CreateInstance<T>();
                string table = t.GetType().Name;

                sql = sql + table + " set " + column + "='" + FilterSQL(value.ToString()) + "'";

                if (!string.IsNullOrEmpty(where))
                {
                    sql = sql + " where " + where;
                }

                SqlCommand cmd = new SqlCommand(sql, conn);
                if (cmd.ExecuteNonQuery() > 0)
                {
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// 更新一列数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="column">被更新列名</param>
        /// <param name="value">被更新值</param>
        /// <param name="primaryKey">主键</param>
        /// <param name="primaryKeyValue">主键值</param>
        /// <param name="conn">连接对象</param>
        /// <returns>更新成功与否</returns>
        public static bool UpdateOneColumn<T>(string column, object value, string primaryKey, object primaryKeyValue, SqlConnection conn)
        {
            bool result = false;

            if (value != null && !string.IsNullOrEmpty(primaryKey) && primaryKeyValue != null)
            {
                string sql = "update ";

                T t = Activator.CreateInstance<T>();
                string table = t.GetType().Name;

                sql = sql + table + " set " + column + "='" + FilterSQL(value.ToString()) + "'";

                sql = sql + " where " + primaryKey + "='" + FilterSQL(primaryKeyValue.ToString()) + "'";

                SqlCommand cmd = new SqlCommand(sql, conn);
                if (cmd.ExecuteNonQuery() > 0)
                {
                    result = true;
                }
            }

            return result;
        }

        #endregion



        #region 删除操作

        /// <summary>
        /// 根据条件 真删除
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="where">删除条件</param>
        /// <param name="conn">连接对象</param>
        /// <returns>是否删除成功</returns>
        public static bool Delete<T>(string where, SqlConnection conn)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(where))
            {
                string sql = "delete from ";

                T t = Activator.CreateInstance<T>();
                string table = t.GetType().Name;

                sql = sql + table + " where " + where;

                SqlCommand cmd = new SqlCommand(sql, conn);
                if (cmd.ExecuteNonQuery() > 0)
                {
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// 真删除 一条记录
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="primaryKey">主键</param>
        /// <param name="value">主键值</param>
        /// <param name="conn">连接对象</param>
        /// <returns>是否删除成功</returns>
        public static bool Delete<T>(string primaryKey, object value, SqlConnection conn)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(primaryKey) && value != null)
            {
                result = Delete<T>(primaryKey + "='" + FilterSQL(value.ToString()) + "'", conn);
            }

            return result;
        }

        /// <summary>
        /// 软删除 一条记录
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="column">被更新列名</param>
        /// <param name="value">被更新值</param>
        /// <param name="primaryKey">主键值</param>
        /// <param name="primaryKeyValue">主键值</param>
        /// <param name="conn">连接对象</param>
        /// <returns>是否删除成功</returns>
        public static bool Delete<T>(string column, object value, string primaryKey, object primaryKeyValue, SqlConnection conn)
        {
            return UpdateOneColumn<T>(column, value, primaryKey + "='" + primaryKeyValue.ToString() + "'", conn);
        }

        #endregion
    }
}
