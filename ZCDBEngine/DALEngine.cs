using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Configuration;
using System.IO;

namespace ZentCloud.ZCDALEngine
{
    abstract public class DALEngine
    {
        private static MetaTables _metaTables = null;

        //判断主键对应的记录是否存在
        static public bool Exists(object model)
        {
            Type t = model.GetType();
            MetaTable metaTable = DALEngine.GetMetas().Tables[t.Name];

            PropertyInfo[] properties = t.GetProperties();
            Dictionary<string, string> columns = new Dictionary<string, string>();
            foreach (string keyCol in metaTable.Keys[0])
            {
                foreach (PropertyInfo p in properties)
                {
                    if (keyCol == p.Name)
                    {
                        columns.Add(keyCol, p.GetValue(model, null).ToString());
                        break;
                    }
                }
            }
            return Exists(metaTable.Name, columns);


        }

        //判断主键对应的记录是否存在
        static public bool Exists(string tableName, string key)
        {
            MetaTable metaTable = DALEngine.GetMetas().Tables[tableName];
            string strPrimaryKey = metaTable.Keys[0][0];
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select count(1) from {0}", metaTable.Name);
            strSql.AppendFormat(" where {0}=@{0} ", strPrimaryKey);
            SqlParameter[] parameters = {
                    new SqlParameter(string.Format("@{0}", strPrimaryKey), metaTable.Columns[strPrimaryKey])			
                                        };
            parameters[0].Value = key;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }

        //判断组合字段and对应的记录是否存在
        static public bool Exists(string tableName, Dictionary<string, string> columns)
        {
            return Exists(tableName, columns, "and");
        }

        //判断组合字段对应的记录是否存在,可以选and， or操作
        static public bool Exists(string tableName, Dictionary<string, string> columns, string orand)
        {
            MetaTable metaTable = DALEngine.GetMetas().Tables[tableName];
            StringBuilder strSql = new StringBuilder();
            List<SqlParameter> parameters = new List<SqlParameter>();

            strSql.AppendFormat("select count(1) from {0}", metaTable.Name);
            int i = 0;
            foreach (KeyValuePair<string, string> kv in columns)
            {
                if (i == 0)
                {
                    strSql.AppendFormat(" where {0}=@{0} ", kv.Key);

                }
                else
                {
                    strSql.AppendFormat(" {0} {1}=@{1} ", orand, kv.Key);
                }
                parameters.Add(new SqlParameter(string.Format("@{0}", kv.Key), metaTable.Columns[kv.Key]));
                parameters[i++].Value = kv.Value;
            }
            return DbHelperSQL.Exists(strSql.ToString(), SqlParameterListToArray(parameters));
        }

        ///// <summary>
        ///// 查询表中是否存在对应记录，select count(1) from {tableName} where {strWhere}
        ///// </summary>
        //static public bool Exists(string tableName, string strWhere)
        //{

        //}

        /// <summary>
        /// 增加一条数据
        /// </summary>
        static public bool Add(string tableName, object model)
        {
            StringBuilder strSql;
            List<SqlParameter> parameters;
            GetAddSqlStrParams(tableName, model, out strSql, out parameters);

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), SqlParameterListToArray(parameters));
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        static public bool Add(string tableName, object model, SqlTransaction trans)
        {
            StringBuilder strSql;
            List<SqlParameter> parameters;

            int rows = 0;
            try
            {
                if (trans == null)
                {
                    return Add(tableName, model);
                }
                else
                {
                    GetAddSqlStrParams(tableName, model, out strSql, out parameters);
                    rows = DbHelperSQL.ExecuteSql(strSql.ToString(), trans, SqlParameterListToArray(parameters));
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 增加一条数据,并返回ID
        /// </summary>
        static public object AddReturnID(string tableName, object model, SqlTransaction tran)
        {
            StringBuilder strSql;
            List<SqlParameter> parameters;
            GetAddSqlStrParams(tableName, model, out strSql, out parameters, true);
            if (tran == null)
            {
                return DbHelperSQL.GetSingle(strSql.ToString(), SqlParameterListToArray(parameters));
            }
            else
            {
                return DbHelperSQL.GetSingle(strSql.ToString(), tran, SqlParameterListToArray(parameters));
            }
        }

        private static void GetAddSqlStrParams(string tableName, object model, out StringBuilder strSql, out List<SqlParameter> parameters, bool returnId = false)
        {
            Type t = model.GetType();

            MetaTable metaTable = DALEngine.GetMetas().Tables[tableName];

            PropertyInfo[] properties = t.GetProperties();

            strSql = new StringBuilder();
            parameters = new List<SqlParameter>();



            StringBuilder strColumns = new StringBuilder();
            StringBuilder strValues = new StringBuilder();


            int i = 0;
            foreach (PropertyInfo p in properties)
            {
                object val = p.GetValue(model, null);

                //如果该属性在表结构中不存在，或者model里属性值为null，则跳过。如扩展属性，AutoId
                if (!metaTable.Columns.ContainsKey(p.Name) || null == val || p.Name.ToLower().Equals("autoid"))
                {
                    continue;
                }
                if (i == 0)
                {
                    strColumns.AppendFormat("{0} ", p.Name);
                    strValues.AppendFormat("@{0} ", p.Name);
                }
                else
                {
                    strColumns.AppendFormat(", {0} ", p.Name);
                    strValues.AppendFormat(", @{0} ", p.Name);
                }

                parameters.Add(new SqlParameter(string.Format("@{0}", p.Name), metaTable.Columns[p.Name]));
                parameters[i].Value = val;
                i++;
            }
            strSql.AppendFormat("insert into {0} ", metaTable.Name);
            strSql.AppendFormat("({0}) values ({1}) ;", strColumns, strValues);
            if (returnId)
            {
                strSql.AppendFormat(" SELECT @@IDENTITY ; ");
            }
        }
        /// <summary>
        /// 更新ModelList
        /// </summary>
        //static public bool UpdateList(List<object> modelList)
        //{
        //    foreach (object model in modelList)
        //    {
        //        if (! Update(model))
        //        {
        //            return false;
        //        }
        //    }

        //    return true;
        //}

        /// <summary>
        /// 更新一条数据
        /// </summary>
        static public bool Update(string tableName, object model)
        {
            StringBuilder strSql;
            List<SqlParameter> parameters;
            GetUpdateStrParams(tableName, model, out strSql, out parameters);
            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), SqlParameterListToArray(parameters));
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        static public bool Update(string tableName, object model, SqlTransaction trans)
        {
            StringBuilder strSql;
            List<SqlParameter> parameters;
            GetUpdateStrParams(tableName, model, out strSql, out parameters);
            int rows = 0;
            try
            {
                if (trans == null)
                {
                    return Update(tableName, model);
                }
                else
                {
                    rows = DbHelperSQL.ExecuteSql(strSql.ToString(), trans, SqlParameterListToArray(parameters));
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        static public int Update(string tableName, string setPms, string strWhere)
        {
            return DbHelperSQL.Update(tableName, setPms, strWhere);
        }

        static public int Update(string tableName, string setPms, string strWhere, SqlTransaction trans)
        {
            int rows = 0;
            try
            {
                if (trans == null)
                {
                    rows = Update(tableName, setPms, strWhere);
                }
                else
                {
                    rows = DbHelperSQL.ExecuteSql(string.Format("UPDATE {0} set {1} WHERE {2}", tableName, setPms, strWhere), trans, null);
                }

                return rows;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        private static void GetUpdateStrParams(string tableName, object model, out StringBuilder strSql, out List<SqlParameter> parameters)
        {
            Type t = model.GetType();
            PropertyInfo[] properties = t.GetProperties();

            strSql = new StringBuilder();
            parameters = new List<SqlParameter>();

            MetaTable metaTable = DALEngine.GetMetas().Tables[tableName];
            strSql.Append(string.Format("update {0} set ", metaTable.Name));

            int paramIndex = 0; //SqlParameter 参数下标
            //修改表中
            foreach (var col in metaTable.Columns)
            {
                if (col.Key.ToLower().Equals("autoid")) continue;
                if (metaTable.Keys.Count > 0 && metaTable.Keys[0].Contains(col.Key)) continue;
                PropertyInfo p = properties.FirstOrDefault(pi => pi.Name == col.Key);
                if (p == null) continue;

                object val = p.GetValue(model, null);
                if (paramIndex == 0)
                {
                    strSql.AppendFormat("{0} = @{0}", col.Key);
                }
                else
                {
                    strSql.AppendFormat(", {0} = @{0}", col.Key);
                }
                parameters.Add(new SqlParameter(string.Format("@{0}", col.Key), metaTable.Columns[col.Key]));
                parameters[paramIndex++].Value = val;
            }

            if (metaTable.Keys.Count == 0 || metaTable.Keys[0].Count == 0)
            {
                throw new Exception("没有主键，不能执行model修改");
            }

            int keyIndex = 0; //SqlParameter 参数下标
            foreach (var key in metaTable.Keys[0])
            {
                if (keyIndex == 0)
                {
                    strSql.AppendFormat(" where {0} = @{0}", key);
                }
                else
                {
                    strSql.AppendFormat(" and {0} = @{0}", key);
                }
                keyIndex++;
                parameters.Add(new SqlParameter(string.Format("@{0}", key), metaTable.Columns[key]));

                PropertyInfo p = properties.FirstOrDefault(pi => pi.Name == key);
                parameters[paramIndex++].Value = p.GetValue(model, null);
            }
        }

        static public int Delete(string tableName, Dictionary<string, object> conditionColumns, bool andOperation, SqlTransaction trans)
        {
            StringBuilder strSql;
            List<SqlParameter> parameters;
            GetDeleteStrParams(tableName, conditionColumns, andOperation, out strSql, out parameters);

            int rows = 0;
            try
            {
                if (trans == null)
                {
                    rows = Delete(tableName, conditionColumns, andOperation);
                }
                else
                {
                    rows = DbHelperSQL.ExecuteSql(strSql.ToString(), trans, SqlParameterListToArray(parameters));
                }

                return rows;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        static public int Delete(string tableName, Dictionary<string, object> conditionColumns, bool andOperation)
        {
            StringBuilder strSql;
            List<SqlParameter> parameters;
            GetDeleteStrParams(tableName, conditionColumns, andOperation, out strSql, out parameters);

            return DbHelperSQL.ExecuteSql(strSql.ToString(), SqlParameterListToArray(parameters));
        }

        static public int Delete(string tableName, string strWhere)
        {
            return DbHelperSQL.ExecuteSql(string.Format("DELETE FROM {0} WHERE {1}", tableName, strWhere));
        }

        static public int Delete(string tableName, string strWhere, SqlTransaction trans)
        {
            int rows = 0;
            try
            {
                if (trans == null)
                {
                    rows = Delete(tableName, strWhere);
                }
                else
                {
                    rows = DbHelperSQL.ExecuteSql(string.Format("DELETE FROM {0} WHERE {1}", tableName, strWhere), trans, null);
                }
                return rows;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        private static void GetDeleteStrParams(string tableName, Dictionary<string, object> conditionColumns, bool andOperation, out StringBuilder strSql, out List<SqlParameter> parameters)
        {
            MetaTable metaTable = DALEngine.GetMetas().Tables[tableName];
            strSql = new StringBuilder();
            strSql.AppendFormat("delete from {0} ", metaTable.Name);

            parameters = new List<SqlParameter>();
            int paramIndex = 0;
            foreach (KeyValuePair<string, object> kv in conditionColumns)
            {
                if (paramIndex == 0)
                {
                    strSql.AppendFormat("where {0} = @{0}", kv.Key);
                }
                else
                {
                    strSql.AppendFormat(" {0} {1} = @{1}", andOperation == true ? "and" : "or", kv.Key);
                }
                parameters.Add(new SqlParameter(string.Format("@{0}", kv.Key), metaTable.Columns[kv.Key]));
                parameters[paramIndex++].Value = kv.Value;
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        static public int Delete(string tableName, string key, object val)
        {
            Dictionary<string, object> conditionColumns = new Dictionary<string, object>();
            conditionColumns.Add(key, val);
            return Delete(tableName, conditionColumns, true);
        }

        static public int Delete(string tableName, string key, object val, SqlTransaction trans)
        {
            Dictionary<string, object> conditionColumns = new Dictionary<string, object>();
            conditionColumns.Add(key, val);
            if (trans == null)
            {
                return Delete(tableName, conditionColumns, true);
            }
            else
            {
                return Delete(tableName, conditionColumns, true, trans);
            }
        }



        /// <summary>
        /// 批量删除数据
        /// </summary>
        //public bool DeleteList(MetaTable metaTable, List<string> keyList)
        //{
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.AppendFormat("delete from {0} ", metaTable.Name);
        //    strSql.AppendFormat(" where {0) in @{0}", metaTable.Keys[0][0]);
        //    SqlParameter[] parameters = {
        //             new SqlParameter("@MemberId", SqlDbType.NVarChar,50)			
        //                                };
        //    parameters[0].Value = key;

        //    int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        //    if (rows > 0)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}


        /// <summary>
        /// 
        /// </summary>
        static public DataSet Get(string tableName)
        {
            return Get(tableName, new Dictionary<string, object>());
        }


        /// <summary>
        /// 
        /// </summary>
        static public DataSet Get(string tableName, Dictionary<string, object> conditionColumns)
        {
            MetaTable metaTable = DALEngine.GetMetas().Tables[tableName];
            StringBuilder strSql = new StringBuilder();

            int i = 0;
            foreach (KeyValuePair<string, SqlDbType> kv in metaTable.Columns)
            {
                if (i == 0)
                {
                    strSql.AppendFormat("select {0}", kv.Key);
                }
                else
                {
                    strSql.AppendFormat(", {0}", kv.Key);
                }
                ++i;
            }
            strSql.AppendFormat(" from {0} ", metaTable.Name);

            List<SqlParameter> parameters = new List<SqlParameter>();
            int paramIndex = 0;
            foreach (KeyValuePair<string, object> kv in conditionColumns)
            {
                if (paramIndex == 0)
                {
                    strSql.AppendFormat("where {0} = @{0}", kv.Key);
                }
                else
                {
                    strSql.AppendFormat(" and {0} = @{0}", kv.Key);
                }
                parameters.Add(new SqlParameter(string.Format("@{0}", kv.Key), metaTable.Columns[kv.Key]));
                parameters[paramIndex++].Value = kv.Value;
            }
            return DbHelperSQL.Query(strSql.ToString(), SqlParameterListToArray(parameters));

        }


        static public DataSet Get(string tableName, string strWhere)
        {
            return Get(0, tableName, strWhere, string.Empty);
        }

        //        static public DataSet Get(int pageSize, int pageIndex, string tableName, string strWhere, string strOrder)
        //        {
        //            MetaTable metaTable = DALEngine.GetMetas().Tables[tableName];
        //            StringBuilder strSql = new StringBuilder();

        //            if (strWhere != null && strWhere.Trim() != string.Empty)
        //            {
        //                strSql.AppendFormat(@"select * from 
        //                                (select row_number() over(order by {0}) as COL_ROWNUMBER, * from {1} where {2} ) TABLE_ORDERDATA
        //                                where COL_ROWNUMBER between {3} and {4} order by {0}", strOrder, tableName, strWhere,
        //                                                                                       (pageIndex - 1) * pageSize + 1, pageIndex * pageSize);
        //            }
        //            else
        //            {
        //                strSql.AppendFormat(@"select * from 
        //                                (select row_number() over(order by {0}) as COL_ROWNUMBER, * from {1} ) TABLE_ORDERDATA
        //                                where COL_ROWNUMBER between {2} and {3} order by {0}", strOrder, tableName,
        //                                                                                       (pageIndex - 1) * pageSize + 1, pageIndex * pageSize);
        //            }

        //            DataSet ds = DbHelperSQL.Query(strSql.ToString());
        //            ds.Tables[0].TableName = tableName;
        //            return ds;
        //        }
        static public DataSet Get(int pageSize, int pageIndex, string tableName, string strWhere, string strOrder)
        {
            MetaTable metaTable = DALEngine.GetMetas().Tables[tableName];
            StringBuilder strSql = new StringBuilder();

            if (strWhere != null && strWhere.Trim() != string.Empty)
            {
                strSql.AppendFormat(@"select top {4} * from 
                                (select top {5} row_number() over(order by {0}) as COL_ROWNUMBER, * from {1} where {2} order by {0} ) TABLE_ORDERDATA
                                where COL_ROWNUMBER > {3}",
                                                                       strOrder,
                                                                       tableName,
                                                                       strWhere,
                                                                       (pageIndex - 1) * pageSize,
                                                                       pageSize,
                                                                       pageSize * pageIndex
                                                                       );
            }
            else
            {
                strSql.AppendFormat(@"select top {3} * from 
                                (select top {4} row_number() over(order by {0}) as COL_ROWNUMBER, * from {1} order by {0} ) TABLE_ORDERDATA
                                where COL_ROWNUMBER > {2}",
                                                                       strOrder,
                                                                       tableName,
                                                                       (pageIndex - 1) * pageSize,
                                                                       pageSize,
                                                                       pageSize * pageIndex

                                                                       );
            }

            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            ds.Tables[0].TableName = tableName;
            return ds;
        }

        static public DataSet Get(int pageSize, int pageIndex, string tableName, string strWhere)
        {
            MetaTable metaTable = DALEngine.GetMetas().Tables[tableName];
            StringBuilder strOrder = new StringBuilder();
            strWhere = strWhere.Replace("  "," ");
            int orderStringIndex = strWhere.ToLower().LastIndexOf("order by");
            if (orderStringIndex<=0)
            {
                foreach (string col in metaTable.Keys[0])
                {
                    strOrder.Append(string.Format(", {0}", col));
                }
                strOrder = strOrder.Remove(0, 1);
            }
            else
            {
                strOrder.Append(strWhere.Substring(orderStringIndex + 8));
                strWhere = strWhere.Substring(0, orderStringIndex);
            }

            return Get(pageSize, pageIndex, tableName, strWhere, strOrder.ToString());
        }

        static public DataSet GetCol(int pageSize, int pageIndex, string tableName, string colName, string strWhere, string strOrder)
        {
            MetaTable metaTable = DALEngine.GetMetas().Tables[tableName];
            StringBuilder strSql = new StringBuilder();
            if (strWhere != null && strWhere.Trim() != string.Empty)
            {
                strSql.AppendFormat(@"select top {4} * from 
                                (select top {5} row_number() over(order by {0}) as COL_ROWNUMBER, {6} from {1} where {2}  order by {0}) TABLE_ORDERDATA
                                where COL_ROWNUMBER > {3}",
                                                                       strOrder,
                                                                       tableName,
                                                                       strWhere,
                                                                       (pageIndex - 1) * pageSize,
                                                                       pageSize,
                                                                       pageSize * pageIndex,
                                                                       colName
                                                                       );
            }
            else
            {
                strSql.AppendFormat(@"select top {3} * from 
                                (select top {4} row_number() over(order by {0}) as COL_ROWNUMBER, {5} from {1} order by {0} ) TABLE_ORDERDATA
                                where COL_ROWNUMBER > {2}",
                                                                       strOrder,
                                                                       tableName,
                                                                       (pageIndex - 1) * pageSize,
                                                                       pageSize,
                                                                       pageSize * pageIndex,
                                                                       colName
                                                                       );
            }

            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            ds.Tables[0].TableName = tableName;
            return ds;
        }

        static public DataSet GetCol(int pageSize, int pageIndex, string tableName, string colName, string strWhere)
        {
            MetaTable metaTable = DALEngine.GetMetas().Tables[tableName];
            StringBuilder strOrder = new StringBuilder();

            strWhere = strWhere.Replace("  ", " ");
            int orderStringIndex = strWhere.ToLower().LastIndexOf("order by");
            if (orderStringIndex <= 0)
            {
                foreach (string col in metaTable.Keys[0])
                {
                    strOrder.Append(string.Format(", {0}", col));
                }
                strOrder = strOrder.Remove(0, 1);
            }
            else
            {
                strOrder.Append(strWhere.Substring(orderStringIndex + 8));
                strWhere = strWhere.Substring(0, orderStringIndex);
            }
            return GetCol(pageSize, pageIndex, tableName, colName, strWhere, strOrder.ToString());
        }

        static public DataSet Get(int top, string tableName, string strWhere, string strOrder)
        {
            MetaTable metaTable = DALEngine.GetMetas().Tables[tableName];
            StringBuilder strSql = new StringBuilder();
            if (top > 0)
            {
                strSql.AppendFormat("select top {0} ", top);
            }
            else
            {
                strSql.AppendFormat("select ");
            }

            int i = 0;
            foreach (KeyValuePair<string, SqlDbType> kv in metaTable.Columns)
            {
                if (i == 0)
                {
                    strSql.AppendFormat(" {0}", kv.Key);
                }
                else
                {
                    strSql.AppendFormat(", {0}", kv.Key);
                }
                ++i;
            }
            strSql.AppendFormat(" from {0} ", metaTable.Name);

            //strWhere值为空的时候where子句不加
            if (strWhere != null && strWhere != "")
                strSql.AppendFormat(" where {0}", strWhere);

            if (strOrder != null && strOrder.Trim().Length != 0)
            {
                strSql.AppendFormat(" order by {0}", strOrder);
            }

            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            ds.Tables[0].TableName = tableName;
            return ds;
        }

        /// <summary>
        /// 添加事务的查询处理
        /// </summary>
        /// <param name="top"></param>
        /// <param name="tableName"></param>
        /// <param name="strWhere"></param>
        /// <param name="strOrder"></param>
        /// <param name="trans"></param>
        /// <param name="isTabLockx">是否添加TABLOCKX进行锁表处理</param>
        /// <returns></returns>
        static public DataSet Get(int top, string tableName, string strWhere, string strOrder, SqlTransaction trans, bool isTabLockx = false)
        {
            MetaTable metaTable = DALEngine.GetMetas().Tables[tableName];
            StringBuilder strSql = new StringBuilder();
            if (top > 0)
            {
                strSql.AppendFormat("select top {0} ", top);
            }
            else
            {
                strSql.AppendFormat("select ");
            }

            int i = 0;
            foreach (KeyValuePair<string, SqlDbType> kv in metaTable.Columns)
            {
                if (i == 0)
                {
                    strSql.AppendFormat(" {0}", kv.Key);
                }
                else
                {
                    strSql.AppendFormat(", {0}", kv.Key);
                }
                ++i;
            }
            strSql.AppendFormat(" from {0} ", metaTable.Name);

            if (isTabLockx)
                strSql.Append(" with (TABLOCKX) ");

            //strWhere值为空的时候where子句不加
            if (strWhere != null && strWhere != "")
                strSql.AppendFormat(" where {0}", strWhere);

            if (strOrder != null && strOrder.Trim().Length != 0)
            {
                strSql.AppendFormat(" order by {0}", strOrder);
            }

            DataSet ds = new DataSet();
            if (trans == null)
            {
                ds = DbHelperSQL.Query(strSql.ToString());
            }
            else
            {
                ds = DbHelperSQL.Query(strSql.ToString(), trans);
            }
            ds.Tables[0].TableName = tableName;
            return ds;
        }




        /// <summary>
        /// 获取数量
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        static public int GetCount(string tableName, string strWhere)
        {
            int result = 0;
            if (string.IsNullOrWhiteSpace(strWhere))
                result = (int)DbHelperSQL.GetSingle(string.Format("SELECT COUNT(1) FROM {0}", tableName));
            else
                result = (int)DbHelperSQL.GetSingle(string.Format("SELECT COUNT(1) FROM {0} WHERE {1}", tableName, strWhere));

            return result;
        }

        /// <summary>
        /// 获取数量
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="disCol">去重字段</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        static public int GetCount(string tableName, string disCol, string strWhere)
        {
            int result = 0;
            if (string.IsNullOrWhiteSpace(strWhere))
                result = (int)DbHelperSQL.GetSingle(string.Format("SELECT COUNT(Distinct({1})) FROM {0}", tableName, disCol));
            else
                result = (int)DbHelperSQL.GetSingle(string.Format("SELECT COUNT(Distinct({2})) FROM {0} WHERE {1}", tableName, strWhere, disCol));

            return result;
        }

        public static object GetSingle(string strSql)
        {
            return DbHelperSQL.GetSingle(strSql);
        }

        public static object GetSingle(string SQLString, SqlTransaction trans)
        {
            if (trans == null)
            {
                return GetSingle(SQLString);
            }
            else
            {
                return DbHelperSQL.GetSingle(SQLString, trans);
            }
        }

        public static object GetSingle(string tableName, string colName, string strWhere)
        {
            MetaTable metaTable = DALEngine.GetMetas().Tables[tableName];
            StringBuilder strSql = new StringBuilder();

            strSql.AppendFormat("select top 1 {0} from {1}", colName, tableName);

            //strWhere值为空的时候where子句不加
            if (strWhere != "")
                strSql.AppendFormat(" where {0}", strWhere);

            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0].Rows[0][0];
            }
            else
            {
                return null;
            }


        }



        static public SqlParameter[] SqlParameterListToArray(List<SqlParameter> listParam)
        {
            SqlParameter[] arrayParam = new SqlParameter[listParam.Count];

            for (int i = 0; i < listParam.Count; i++)
            {
                arrayParam[i] = listParam[i];
            }
            return arrayParam;
        }


        public static DataSet Query(string SQLString)
        {
            return DbHelperSQL.Query(SQLString);
        }

        public static SqlDbType GetColType(string colType)
        {
            switch (colType)
            {
                case "image":
                    return SqlDbType.Image;
                case "text":
                    return SqlDbType.Text;
                case "uniqueidentifier":
                    return SqlDbType.UniqueIdentifier;
                case "date":
                    return SqlDbType.Date;
                case "time":
                    return SqlDbType.Time;
                case "datetime2":
                    return SqlDbType.DateTime2;
                case "datetimeoffset":
                    return SqlDbType.DateTimeOffset;
                case "tinyint":
                    return SqlDbType.TinyInt;
                case "smallint":
                    return SqlDbType.SmallInt;
                case "int":
                    return SqlDbType.Int;
                case "smalldatetime":
                    return SqlDbType.SmallDateTime;
                case "real":
                    return SqlDbType.Real;
                case "money":
                    return SqlDbType.Money;
                case "datetime":
                    return SqlDbType.DateTime;
                case "float":
                    return SqlDbType.Float;
                case "sql_variant":
                    return SqlDbType.Variant;
                case "ntext":
                    return SqlDbType.NText;
                case "bit":
                    return SqlDbType.Bit;
                case "decimal":
                    return SqlDbType.Decimal;
                //case "numeric":
                //return ;
                case "smallmoney":
                    return SqlDbType.SmallMoney;
                case "bigint":
                    return SqlDbType.BigInt;
                //case "hierarchyid":
                //return ;
                //case "geometry":
                //return ;
                //case "geography":
                //return ;
                case "varbinary":
                    return SqlDbType.VarBinary;
                case "varchar":
                    return SqlDbType.VarChar;
                case "binary":
                    return SqlDbType.Binary;
                case "char":
                    return SqlDbType.Char;
                case "timestamp":
                    return SqlDbType.Timestamp;
                case "nvarchar":
                    return SqlDbType.NVarChar;
                case "nchar":
                    return SqlDbType.NChar;
                case "xml":
                    return SqlDbType.Xml;
                default:
                    return SqlDbType.NVarChar;
            }
        }
        //static class table_col { }

        static public MetaTables GetMetas()
        {
            //经常报异常The given key was not present in the dictionary.待处理
            /* 2013.10.30 charles
             * 已确定原因：
             * web服务器本身多线程，module或者其他模块同时运行，
             * 如果第一次数据库初始化后，还没等第一次初始化完请求再次过来，
             * 这样就导致了_metaTables!=null，但里面没数据
             * 
             * 目前解决方案：在 Global.asax Application_Start 里立即执行一次 GetMetas()
             * 
             * 目前测试暂解
            */
            if (null == _metaTables)
            {
                _metaTables = new MetaTables();
                try
                {

                    #region 新方法
                    DataTable dsTable = DbHelperSQL.Query("SELECT name FROM sysobjects WHERE (xtype = 'U') order by name ").Tables[0];

                    DataTable dsFieldTable = DbHelperSQL.Query(@"Select T0.name as 'colname', T2.name as 'coltype' ,T1.name 'tablename' from syscolumns T0 
                    							inner join sysobjects T3 on T3.xtype = 'U'
                                                inner join sysobjects T1 on T0.id = T1.id and T1.name = T3.name
                                                inner join systypes T2 on T0.xtype = T2.xtype and T2.name <> 'sysname' 
                                                order by T1.name,T0.colid ").Tables[0];

                    DataTable dsKeyTable = DbHelperSQL.Query(@"select T0.indid as 'keyindex', T2.name as 'colname',T1.name 'tablename' from sysindexkeys T0 
                                                join sysobjects T1 on T0.id = T1.id and T1.xtype = 'U'
                                                join sys.syscolumns T2 on T0.colid = T2.colid and T1.id = T2.id
                                                order by T1.name,T0.indid").Tables[0];

                    foreach (DataRow dsRow in dsTable.Rows)
                    {
                        MetaTable metaTable = new MetaTable(dsRow[0].ToString());
                        string tableName = dsRow[0].ToString();
                        DataRow[] rowFields = dsFieldTable.Select(string.Format(" tablename='{0}' ", tableName));
                        DataRow[] rowKeys = dsKeyTable.Select(string.Format(" tablename='{0}' ", tableName));

                        foreach (DataRow nowField in rowFields)
                        {
                            metaTable.Columns.Add(nowField["colname"].ToString(), GetColType(nowField["coltype"].ToString()));
                        }

                        if (rowKeys.Length > 0)
                        {
                            int indid = int.Parse(rowKeys[0]["keyindex"].ToString());
                            List<string> key = new List<string>();
                            foreach (DataRow nowKey in rowKeys)
                            {
                                int keyindex = int.Parse(nowKey["keyindex"].ToString());
                                if (keyindex == indid)
                                {
                                    key.Add(nowKey["colname"].ToString());
                                }
                                else
                                {
                                    break;
                                }
                            }
                            metaTable.Keys.Add(key);
                        }
                        _metaTables.Tables.Add(tableName, metaTable);
                    }
                    #endregion
                    #region 原方法
                    //                    DataSet dsTable = DbHelperSQL.Query("SELECT name FROM sysobjects WHERE (xtype = 'U') order by name ");

                    //                    for (int i = 0; i < dsTable.Tables[0].Rows.Count; i++)
                    //                    {
                    //                        MetaTable metaTable = new MetaTable(dsTable.Tables[0].Rows[i][0].ToString());
                    //                        string tableName = dsTable.Tables[0].Rows[i][0].ToString();
                    //                        DataSet dsCol = DbHelperSQL.Query(string.Format(
                    //                            @"Select T0.name as 'colname', T2.name as 'coltype' from syscolumns T0 
                    //                            inner join sysobjects T1 on T0.id = T1.id and T1.name = '{0}' 
                    //                            inner join systypes T2 on T0.xtype = T2.xtype and T2.name <> 'sysname' 
                    //                            order by colid ", tableName));
                    //                        for (int j = 0; j < dsCol.Tables[0].Rows.Count; j++)
                    //                        {
                    //                            metaTable.Columns.Add(dsCol.Tables[0].Rows[j]["colname"].ToString(), GetColType(dsCol.Tables[0].Rows[j]["coltype"].ToString()));
                    //                        }

                    //                        DataSet dsKey = DbHelperSQL.Query(string.Format("{0} '{1}' {2}",
                    //                                                            @"select T0.indid as 'keyindex', T2.name as 'colname' from sysindexkeys T0 
                    //                                                        join sysobjects T1 on T0.id = T1.id 
                    //                                                        join sys.syscolumns T2 on T0.colid = T2.colid and T1.id = T2.id
                    //                                                        where T1.name = ", tableName, "order by T0.indid"));

                    //                        if (dsKey.Tables[0].Rows.Count > 0)
                    //                        {
                    //                            int indid = int.Parse(dsKey.Tables[0].Rows[0]["keyindex"].ToString());
                    //                            List<string> key = new List<string>();
                    //                            for (int k = 0; k < dsKey.Tables[0].Rows.Count; k++)
                    //                            {
                    //                                int keyindex = int.Parse(dsKey.Tables[0].Rows[k]["keyindex"].ToString());
                    //                                if (keyindex == indid)
                    //                                {
                    //                                    key.Add(dsKey.Tables[0].Rows[k]["colname"].ToString());
                    //                                }
                    //                                else
                    //                                {
                    //                                    break;
                    //                                }
                    //                            }
                    //                            metaTable.Keys.Add(key);
                    //                        }
                    //                        _metaTables.Tables.Add(tableName, metaTable);
                    //                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                }
            }
            return _metaTables;

        }

        public static void SaveMetas(string filePath)
        {
            string jsonMeta = Common.JSONHelper.ObjectToJson(_metaTables);

            StreamWriter sw = new StreamWriter(filePath);
            sw.Write(jsonMeta);
            sw.Close();
        }

        public static void LoadMetas(string filePath)
        {

            StreamReader sw = new StreamReader(filePath);
            string jsonMeta = sw.ReadToEnd();
            sw.Close();
            _metaTables = (MetaTables)Common.JSONHelper.JsonToObject(jsonMeta);


        }

        public static void LoadMetas()
        {
            Type t = typeof(DALEngine);
            StreamReader sw = new StreamReader(t.Assembly.Location.Replace(".dll", "") + ".json");
            string jsonMeta = sw.ReadToEnd();
            sw.Close();
            //object metaTables = Common.JSONHelper.JsonToObject(jsonMeta);
            _metaTables = Common.JSONHelper.JsonToModel<MetaTables>(jsonMeta);


        }

        public static int ExecuteSql(string SQLString)
        {
            return DbHelperSQL.ExecuteSql(SQLString);
        }

        public static int ExecuteSql(string SQLString, SqlTransaction trans)
        {
            int rows = 0;
            try
            {
                if (trans == null)
                {
                    rows = ExecuteSql(SQLString);
                }
                else
                {
                    rows = DbHelperSQL.ExecuteSql(SQLString, trans);
                }
                return rows;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        public static int ExecuteSqlTran(List<String> SQLStringList)
        {
            return DbHelperSQL.ExecuteSqlTran(SQLStringList);
        }
        //public static MetaTables Metas
        //{
        //    get { return GetMetas(); }

        //}

    }
}
