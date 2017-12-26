using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using ZentCloud.Common;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Web;

namespace ZentCloud.ZCBLLEngine
{
    /// <summary>
    /// BLLBase
    /// </summary>
    [Serializable]
    abstract public class BLLBase
    {
        //static public bool Exists(ModelTable model)
        //{
        //    return ZCDALEngine.DALEngine.Exists(model);
        //}
        /// <summary>
        /// 获取真实表名
        /// </summary>
        /// <param name="modelName"></param>
        /// <returns></returns>
        protected virtual string GetRealTableName(string modelName)
        {
            return modelName;

        }
        /// <summary>
        ///检查指定数据是否存在
        /// </summary>
        /// <param name="model"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public bool Exists(ModelTable model, string col)
        {
            List<string> columns = new List<string>();
            columns.Add(col);
            return Exists(model, columns);
        }
        /// <summary>
        /// 检查指定数据是否存在
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cols"></param>
        /// <returns></returns>
        public bool Exists(ModelTable model, List<string> cols)
        {
            Type t = model.GetType();
            Dictionary<string, string> columns = new Dictionary<string, string>();
            PropertyInfo[] properties = t.GetProperties();
            //缓存用的语句
            string where = "";
            foreach (string col in cols)
            {
                foreach (var p in properties)
                {
                    if (p.Name == col)
                    {
                        string value = p.GetValue(model, null).ToString();
                        columns.Add(col, value);
                        where += col + ":" + value + "_";
                        break;
                    }
                }
            }
            //读取缓存
            string RealTableName = GetRealTableName(t.Name);
            string cacheKey = "";
            if (DataCacheCheck.CheckEnableDataCache(RealTableName))
            {
                cacheKey = t.Name + "_" + DataCacheCheck.GetWebsiteOwner(RealTableName) + "_Exists_" + DataCache.FormatKey(where);
                object cacheObject = DataCache.GetCache(cacheKey);
                if (cacheObject != null) return (bool)cacheObject;
            }

            bool result = ZCDALEngine.DALEngine.Exists(RealTableName, columns);

            //记录缓存
            if (DataCacheCheck.CheckEnableDataCache(RealTableName)) DataCache.SetCache(cacheKey, result);

            return result;
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Add(ModelTable model)
        {
            Type t = model.GetType();
            string RealTableName = GetRealTableName(t.Name);

            bool result = ZCDALEngine.DALEngine.Add(RealTableName, model);
            if (result && DataCacheCheck.CheckEnableDataCache(RealTableName))
            {
                DataCache.ClearCacheStartsWith(t.Name + "_" + DataCacheCheck.GetWebsiteOwner(RealTableName)); //清理该表缓存
            }
            return result;
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public bool Add(ModelTable model, BLLTransaction trans)
        {
            Type t = model.GetType();
            bool result = false;
            string RealTableName = GetRealTableName(t.Name);
            if (trans == null)
            {
                result = ZCDALEngine.DALEngine.Add(RealTableName, model);
            }
            else
            {
                result = ZCDALEngine.DALEngine.Add(RealTableName, model, trans.Transaction);
            }
            if (result && DataCacheCheck.CheckEnableDataCache(RealTableName))
            {
                DataCache.ClearCacheStartsWith(t.Name + "_" + DataCacheCheck.GetWebsiteOwner(RealTableName)); //清理该表缓存
            }
            return result;
        }

        public object AddReturnID(ModelTable model, BLLTransaction trans = null)
        {
            Type t = model.GetType();
            string RealTableName = GetRealTableName(t.Name);
            object result = null;
            if (trans == null)
            {
                result = ZCDALEngine.DALEngine.AddReturnID(RealTableName, model, null);
            }
            else
            {
                try
                {
                    result = ZCDALEngine.DALEngine.AddReturnID(RealTableName, model, trans == null ? null : trans.Transaction);
                }
                catch (Exception ex)
                {
                }
            }
            if (result!=null && DataCacheCheck.CheckEnableDataCache(RealTableName))
            {
                DataCache.ClearCacheStartsWith(t.Name + "_" + DataCacheCheck.GetWebsiteOwner(RealTableName)); //清理该表缓存
            }
            return result == null ? 0 : result;

        }
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="modelList"></param>
        /// <returns></returns>
        public bool AddList<T>(List<T> modelList) where T : ModelTable, new()
        {
            BLLTransaction tran = new BLLTransaction();
            foreach (T model in modelList)
            {

                if (!Add(model, tran))
                {
                    tran.Rollback();
                    return false;
                }
            }
            tran.Commit();
            return true;
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update(ModelTable model)
        {
            Type t = model.GetType();
            string RealTableName = GetRealTableName(t.Name);
            bool result = ZCDALEngine.DALEngine.Update(RealTableName, model);
            if (result && DataCacheCheck.CheckEnableDataCache(RealTableName))
            {
                DataCache.ClearCacheStartsWith(t.Name + "_" + DataCacheCheck.GetWebsiteOwner(RealTableName)); //清理该表缓存
            }
            if (result)
            {
                UpdateRedis(t, model);
            }
            return result;
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public bool Update(ModelTable model, BLLTransaction trans)
        {
            Type t = model.GetType();
            string RealTableName = GetRealTableName(t.Name);
            bool result = false;
            if (trans == null)
            {
                result = ZCDALEngine.DALEngine.Update(RealTableName, model);
            }
            else
            {
                result = ZCDALEngine.DALEngine.Update(RealTableName, model, trans.Transaction);
            }
            if (result && DataCacheCheck.CheckEnableDataCache(RealTableName))
            {
                DataCache.ClearCacheStartsWith(t.Name + "_" + DataCacheCheck.GetWebsiteOwner(RealTableName)); //清理该表缓存
            }
            if (result)
            {

                UpdateRedis(t, model);
            }
            return result;
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <param name="setPms"></param>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public int Update(ModelTable model, string setPms, string strWhere)
        {
            Type t = model.GetType();
            string RealTableName = GetRealTableName(t.Name);
            int result = ZCDALEngine.DALEngine.Update(RealTableName, setPms, strWhere);
            if (result > 0 && DataCacheCheck.CheckEnableDataCache(RealTableName))
            {
                DataCache.ClearCacheStartsWith(t.Name + "_" + DataCacheCheck.GetWebsiteOwner(RealTableName)); //清理该表缓存
            }
            if (result > 0)
            {

                UpdateRedis(t, model, strWhere);
            }
            return result;
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <param name="setPms"></param>
        /// <param name="strWhere"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public int Update(ModelTable model, string setPms, string strWhere, BLLTransaction trans)
        {
            Type t = model.GetType();
            string RealTableName = GetRealTableName(t.Name);
            int result = 0;
            if (trans == null)
            {
                result = ZCDALEngine.DALEngine.Update(RealTableName, setPms, strWhere);
            }
            else
            {
                result = ZCDALEngine.DALEngine.Update(RealTableName, setPms, strWhere, trans.Transaction);
            }
            if (result > 0 && DataCacheCheck.CheckEnableDataCache(RealTableName))
            {
                DataCache.ClearCacheStartsWith(t.Name + "_" + DataCacheCheck.GetWebsiteOwner(RealTableName)); //清理该表缓存
            }
            if (result > 0)
            {

                UpdateRedis(t, model, strWhere);
            }
            return result;
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="modelList"></param>
        /// <returns></returns>
        public bool UpdateList(List<ModelTable> modelList)
        {
            foreach (ModelTable model in modelList)
            {
                if (!Update(model))
                {
                    return false;
                }
            }

            return true;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Delete(ModelTable model)
        {
            Type t = model.GetType();
            string RealTableName = GetRealTableName(t.Name);
            ZCDALEngine.MetaTable metaTable = ZCDALEngine.DALEngine.GetMetas().Tables[RealTableName];
            Dictionary<string, object> conditionColumns = new Dictionary<string, object>();
            PropertyInfo[] properties = t.GetProperties();
            if (metaTable.Keys.Count == 0 || metaTable.Keys[0].Count == 0)
            {
                throw new Exception("该表没有主键不能通过Model删除");
            }

            foreach (string keyCol in metaTable.Keys[0])
            {
                foreach (PropertyInfo p in properties)
                {
                    if (p.Name == keyCol)
                    {
                        conditionColumns.Add(keyCol, p.GetValue(model, null));
                    }
                }
            }
            int result = ZCDALEngine.DALEngine.Delete(RealTableName, conditionColumns, true);
            if (result > 0 && DataCacheCheck.CheckEnableDataCache(RealTableName))
            {
                DataCache.ClearCacheStartsWith(t.Name + "_" + DataCacheCheck.GetWebsiteOwner(RealTableName)); //清理该表缓存
            }
            return result;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public int Delete(ModelTable model, BLLTransaction trans)
        {
            Type t = model.GetType();
            string RealTableName = GetRealTableName(t.Name);
            ZCDALEngine.MetaTable metaTable = ZCDALEngine.DALEngine.GetMetas().Tables[RealTableName];
            Dictionary<string, object> conditionColumns = new Dictionary<string, object>();

            PropertyInfo[] properties = t.GetProperties();

            if (metaTable.Keys.Count == 0 || metaTable.Keys[0].Count == 0)
            {
                throw new Exception("该表没有主键不能通过Model删除");
            }

            foreach (string keyCol in metaTable.Keys[0])
            {
                foreach (PropertyInfo p in properties)
                {
                    if (p.Name == keyCol)
                    {
                        conditionColumns.Add(keyCol, p.GetValue(model, null));
                    }
                }
            }

            int result = 0;
            if (trans == null)
            {
                result = ZCDALEngine.DALEngine.Delete(RealTableName, conditionColumns, true);
            }
            else
            {
                result = ZCDALEngine.DALEngine.Delete(RealTableName, conditionColumns, true, trans.Transaction);
            }
            if (result > 0 && DataCacheCheck.CheckEnableDataCache(RealTableName))
            {
                DataCache.ClearCacheStartsWith(t.Name + "_" + DataCacheCheck.GetWebsiteOwner(RealTableName)); //清理该表缓存
            }
            return result;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public int Delete(ModelTable model, string strWhere)
        {
            if (strWhere.Length == 0)
                return 0;
            Type t = model.GetType();
            string RealTableName = GetRealTableName(t.Name);
            int result = ZCDALEngine.DALEngine.Delete(RealTableName, strWhere);

            if (result > 0 && DataCacheCheck.CheckEnableDataCache(RealTableName))
            {
                DataCache.ClearCacheStartsWith(t.Name + "_" + DataCacheCheck.GetWebsiteOwner(RealTableName)); //清理该表缓存
            }
            return result;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <param name="strWhere"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public int Delete(ModelTable model, string strWhere, BLLTransaction trans)
        {
            if (strWhere.Length == 0)
                return 0;
            Type t = model.GetType();
            string RealTableName = GetRealTableName(t.Name);
            int result = 0;
            if (trans == null)
            {
                result = ZCDALEngine.DALEngine.Delete(RealTableName, strWhere);
            }
            else
            {
                result = ZCDALEngine.DALEngine.Delete(RealTableName, strWhere, trans.Transaction);
            }
            if (result > 0 && DataCacheCheck.CheckEnableDataCache(RealTableName))
            {
                DataCache.ClearCacheStartsWith(t.Name + "_" + DataCacheCheck.GetWebsiteOwner(RealTableName)); //清理该表缓存
            }
            return result;
        }
        /// <summary>
        /// 获取单条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public T Get<T>(string strWhere, BLLTransaction trans = null) where T : ModelTable, new()
        {
            T model = new T();
            Type t = model.GetType();
            var obj = GetRedis(t, strWhere);
            if (obj != null)
            {
                return (T)obj;
            }
            List<T> listModels = GetList<T>(1, strWhere, "", trans);

            if (listModels.Count > 0)
            {

                return listModels[0];
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 获取多条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public List<T> GetList<T>(string strWhere, BLLTransaction trans = null) where T : ModelTable, new()
        {
            return GetList<T>(0, strWhere, "", trans);
        }
        /// <summary>
        /// 获取多条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="top"></param>
        /// <param name="strWhere"></param>
        /// <param name="strOrder"></param>
        /// <returns></returns>
        public List<T> GetList<T>(int top, string strWhere, string strOrder) where T : ModelTable, new()
        {
            T model = new T();
            Type t = model.GetType();

            //缓存用的语句
            string where = top + "_" + strWhere + "_" + strOrder;
            //读取缓存
            string RealTableName = GetRealTableName(t.Name);
            string cacheKey = "";
            if (DataCacheCheck.CheckEnableDataCache(RealTableName))
            {
                cacheKey = t.Name + "_" + DataCacheCheck.GetWebsiteOwner(RealTableName) + "_List_" + DataCache.FormatKey(where);
                object cacheObject = DataCache.GetCache(cacheKey);
                if (cacheObject != null)
                {
                    return DataSetToModelList<T>((DataSet)cacheObject); //有缓存执行
                }
            }

            DataSet ds = ZCDALEngine.DALEngine.Get(top, RealTableName, strWhere, strOrder);
            //进行缓存
            if (DataCacheCheck.CheckEnableDataCache(RealTableName) && ds.Tables[0].Rows.Count >= 0) DataCache.SetCache(cacheKey, ds);

            return DataSetToModelList<T>(ds);

        }
        /// <summary>
        /// 获取多条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="top"></param>
        /// <param name="strWhere"></param>
        /// <param name="strOrder"></param>
        /// <param name="trans"></param>
        /// <param name="isTabLockx"></param>
        /// <returns></returns>
        public List<T> GetList<T>(int top, string strWhere, string strOrder, BLLTransaction trans, bool isTabLockx = false) where T : ModelTable, new()
        {
            T model = new T();
            Type t = model.GetType();
            string RealTableName = GetRealTableName(t.Name);
            //缓存用的语句
            string where = top + "_" + strWhere + "_" + strOrder;
            //读取缓存
            string cacheKey = "";
            if (DataCacheCheck.CheckEnableDataCache(RealTableName))
            {
                cacheKey = t.Name + "_" + DataCacheCheck.GetWebsiteOwner(RealTableName) + "_List_" + DataCache.FormatKey(where);
                object cacheObject = DataCache.GetCache(cacheKey);
                if (cacheObject != null)
                {
                    return DataSetToModelList<T>((DataSet)cacheObject); //有缓存执行
                }
            }

            DataSet ds = ZCDALEngine.DALEngine.Get(top, RealTableName, strWhere, strOrder, (trans == null?null: trans.Transaction), isTabLockx);
            if (DataCacheCheck.CheckEnableDataCache(RealTableName) && ds.Tables[0].Rows.Count >= 0) DataCache.SetCache(cacheKey, ds);

            return DataSetToModelList<T>(ds);
        }

        /// <summary>
        /// 获取所有记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> GetList<T>() where T : ModelTable, new()
        {
            if (!GetCheck()) return new List<T>();
            return GetList<T>(0, "", "");
        }

        /// <summary>
        /// 获取数量
        /// </summary>
        /// <typeparam name="T">数据泛型</typeparam>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public int GetCount<T>(string strWhere) where T : ModelTable, new()
        {
            T model = new T();
            Type t = model.GetType();

            string RealTableName = GetRealTableName(t.Name);
            if (!GetCheck()) return 0;

            //读取缓存
            string cacheKey = "";
            if (DataCacheCheck.CheckEnableDataCache(RealTableName))
            {
                cacheKey = t.Name + "_" + DataCacheCheck.GetWebsiteOwner(RealTableName) + "_Count_" + DataCache.FormatKey(strWhere);
                object cacheObject = DataCache.GetCache(cacheKey);
                if (cacheObject != null) return (int)cacheObject;
            }

            int result = ZCDALEngine.DALEngine.GetCount(RealTableName, strWhere);

            //记录缓存
            if (DataCacheCheck.CheckEnableDataCache(RealTableName) && result >= 0) DataCache.SetCache(cacheKey, result);

            return result;
        }
        /// <summary>
        /// 获取数量
        /// </summary>
        /// <typeparam name="T">数据泛型</typeparam>
        /// <param name="disCol">去重字段</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public int GetCount<T>(string disCol, string strWhere) where T : ModelTable, new()
        {
            T model = new T();
            Type t = model.GetType();
            string RealTableName = GetRealTableName(t.Name);
            if (!GetCheck()) return 0;

            //缓存用的语句
            string where = disCol + "_" + strWhere;
            //读取缓存
            string cacheKey = "";
            if (DataCacheCheck.CheckEnableDataCache(RealTableName))
            {
                cacheKey = t.Name + "_" + DataCacheCheck.GetWebsiteOwner(RealTableName) + "_Count_" + DataCache.FormatKey(where);
                object cacheObject = DataCache.GetCache(cacheKey);
                if (cacheObject != null) return (int)cacheObject;
            }
            int result = ZCDALEngine.DALEngine.GetCount(RealTableName, disCol, strWhere);

            //记录缓存
            if (DataCacheCheck.CheckEnableDataCache(RealTableName) && result >= 0) DataCache.SetCache(cacheKey, result);

            return result;
        }
        /// <summary>
        /// 获取指定列 多条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="strWhere"></param>
        /// <param name="colName"></param>
        /// <returns></returns>
        public List<T> GetColList<T>(int pageSize, int pageIndex, string strWhere, string colName) where T : ModelTable, new()
        {
            T model = new T();
            Type t = model.GetType();
            string RealTableName = GetRealTableName(t.Name);

            //缓存用的语句
            string where = pageSize + "_" + pageIndex + "_" + strWhere + "_" + colName;
            //读取缓存
            string cacheKey = "";
            if (DataCacheCheck.CheckEnableDataCache(RealTableName))
            {
                cacheKey = t.Name + "_" + DataCacheCheck.GetWebsiteOwner(RealTableName) + "_List_" + DataCache.FormatKey(where);
                object cacheObject = DataCache.GetCache(cacheKey);
                if (cacheObject != null)
                {
                    return DataSetToModelList<T>((DataSet)cacheObject); //有缓存执行
                }
            }

            DataSet ds = ZCDALEngine.DALEngine.GetCol(pageSize, pageIndex, RealTableName, colName, strWhere);
            if (DataCacheCheck.CheckEnableDataCache(RealTableName) && ds.Tables[0].Rows.Count >= 0) DataCache.SetCache(cacheKey, ds);
            return DataSetToModelList<T>(ds);
        }

        /// <summary>
        /// 检查
        /// </summary>
        /// <returns></returns>
        public static bool GetCheck()
        {

            return true;

            //try
            //{
            //    bool result = false;
            //    List<string> list = new List<string>() { "02E#8DA781F505!B!7#AF029E90AA875", "!D1BC9ACBF87C7#B078BD57!D5!2174F",
            //    "FB152!BFE2EC5B57CE2#BF9!A40480AA", "0CFF477DD177!B8A#C2D42!71FE1!F57", "DBF1EA4D1E##A2CB4B!A1B8#4EC9A7DE",
            //    "509DC9CBAB51!#A!2F!5C!1D1!D#FCC1", "025D44!E!F8BE880BAFF7129AA291C9A", "#CA!B7B85F9C7#AAEDA7EBBC2#D1E9F2",
            //    "2E52015DF0709EEAAFC#94F5424CE2#1","2E52015DF0709EEAAFC#94F5424CE2#1","E2751FCE1009910E597B99F10DC1D85D","22CC#C19A90B#0E#1AC5721A0F!F1E2F"};
            //    var tmp = HttpContext.Current.Request.ServerVariables.Get("Local_Addr").ToString();
            //    var key = DEncrypt.ZCEncrypt(HttpContext.Current.Request.ServerVariables.Get("Local_Addr").ToString()).ToUpper();

            //    result = list.Contains(key);

            //    //result = true;
            //    return result;
            //}
            //catch (Exception ex)
            //{
            //    return true;
            //}
        }
        /// <summary>
        /// 获取指定列多条数据分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="strWhere"></param>
        /// <param name="strOrder"></param>
        /// <param name="colName"></param>
        /// <returns></returns>
        public List<T> GetColList<T>(int pageSize, int pageIndex, string strWhere, string strOrder, string colName) where T : ModelTable, new()
        {
            T model = new T();
            Type t = model.GetType();

            string RealTableName = GetRealTableName(t.Name);
            //缓存用的语句
            string where = pageSize + "_" + pageIndex + "_" + strWhere + "_" + strOrder + "_" + colName;
            //读取缓存
            string cacheKey = "";
            if (DataCacheCheck.CheckEnableDataCache(RealTableName))
            {
                cacheKey = t.Name + "_" + DataCacheCheck.GetWebsiteOwner(RealTableName) + "_List_" + DataCache.FormatKey(where);
                object cacheObject = DataCache.GetCache(cacheKey);
                if (cacheObject != null)
                {
                    return DataSetToModelList<T>((DataSet)cacheObject); //有缓存执行
                }
            }

            DataSet ds = ZCDALEngine.DALEngine.GetCol(pageSize, pageIndex, RealTableName, colName, strWhere, strOrder);
            if (DataCacheCheck.CheckEnableDataCache(RealTableName) && ds.Tables[0].Rows.Count >= 0) DataCache.SetCache(cacheKey, ds);
            return DataSetToModelList<T>(ds);
        }
        /// <summary>
        /// 获取数据 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public List<T> GetLit<T>(int pageSize, int pageIndex, string strWhere) where T : ModelTable, new()
        {
            T model = new T();
            Type t = model.GetType();
            string RealTableName = GetRealTableName(t.Name);
            //缓存用的语句
            string where = pageSize + "_" + pageIndex + "_" + strWhere;
            //读取缓存
            string cacheKey = "";
            if (!GetCheck()) return new List<T>();
            if (DataCacheCheck.CheckEnableDataCache(RealTableName))
            {
                cacheKey = t.Name + "_" + DataCacheCheck.GetWebsiteOwner(RealTableName) + "_List_" + DataCache.FormatKey(where);
                object cacheObject = DataCache.GetCache(cacheKey);
                if (cacheObject != null)
                {
                    return DataSetToModelList<T>((DataSet)cacheObject); //有缓存执行
                }
            }

            DataSet ds = ZCDALEngine.DALEngine.Get(pageSize, pageIndex, RealTableName, strWhere);
            if (DataCacheCheck.CheckEnableDataCache(RealTableName) && ds.Tables[0].Rows.Count >= 0) DataCache.SetCache(cacheKey, ds);
            return DataSetToModelList<T>(ds);
        }
        /// <summary>
        /// 获取数据 分页 排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="strWhere"></param>
        /// <param name="strOrder"></param>
        /// <returns></returns>
        public List<T> GetLit<T>(int pageSize, int pageIndex, string strWhere, string strOrder) where T : ModelTable, new()
        {
            T model = new T();
            Type t = model.GetType();
            string RealTableName = GetRealTableName(t.Name);
            //缓存用的语句
            string where = pageSize + "_" + pageIndex + "_" + strWhere + "_" + strOrder;
            if (!GetCheck()) return new List<T>();
            //读取缓存
            string cacheKey = "";
            if (DataCacheCheck.CheckEnableDataCache(RealTableName))
            {
                cacheKey = t.Name + "_" + DataCacheCheck.GetWebsiteOwner(RealTableName) + "_List_" + DataCache.FormatKey(where);
                object cacheObject = DataCache.GetCache(cacheKey);
                if (cacheObject != null)
                {
                    return DataSetToModelList<T>((DataSet)cacheObject); //有缓存执行
                }
            }

            DataSet ds = ZCDALEngine.DALEngine.Get(pageSize, pageIndex, RealTableName, strWhere, strOrder);
            if (DataCacheCheck.CheckEnableDataCache(RealTableName) && ds.Tables[0].Rows.Count >= 0) DataCache.SetCache(cacheKey, ds);
            return DataSetToModelList<T>(ds);
        }
        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="colName"></param>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public static object GetSingle(string tableName, string colName, string strWhere)
        {
            //缓存用的语句
            string where = colName + "_" + strWhere;

            string RealTableName = tableName.Contains("ZCJ_") ? tableName : "ZCJ_" + tableName;

            //读取缓存
            string cacheKey = "";
            if (DataCacheCheck.CheckEnableDataCache(RealTableName))
            {
                cacheKey = tableName + "_" + DataCacheCheck.GetWebsiteOwner(RealTableName) + "_" + DataCache.FormatKey(where);
                object cacheObject = DataCache.GetCache(cacheKey);
                if (cacheObject != null) return cacheObject;
            }
            object result = ZCDALEngine.DALEngine.GetSingle(RealTableName, colName, strWhere);

            //记录缓存
            if (DataCacheCheck.CheckEnableDataCache(RealTableName)) DataCache.SetCache(cacheKey, result);

            return result;
        }
        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static object GetSingle(string strSql, string tableName = null)
        {
            //读取缓存
            string cacheKey = "";
            string RealTableName = !string.IsNullOrWhiteSpace(tableName) && tableName.Contains("ZCJ_") ? tableName : "ZCJ_" + tableName;
            if (DataCacheCheck.CheckEnableDataCache(RealTableName) && !string.IsNullOrWhiteSpace(tableName))
            {
                cacheKey = tableName + "_" + DataCacheCheck.GetWebsiteOwner(RealTableName) + "_" + DataCache.FormatKey(strSql);
                object cacheObject = DataCache.GetCache(cacheKey);
                if (cacheObject != null) return cacheObject;
            }

            object result = ZCDALEngine.DALEngine.GetSingle(strSql);
            //记录缓存
            if (DataCacheCheck.CheckEnableDataCache(RealTableName) && !string.IsNullOrWhiteSpace(tableName)) DataCache.SetCache(cacheKey, result);

            return result;
        }
        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="trans"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static object GetSingle(string strSql, BLLTransaction trans, string tableName = null)
        {
            //读取缓存
            string cacheKey = "";
            string RealTableName = !string.IsNullOrWhiteSpace(tableName) && tableName.Contains("ZCJ_") ? tableName : "ZCJ_" + tableName;
            if (DataCacheCheck.CheckEnableDataCache(RealTableName) && !string.IsNullOrWhiteSpace(tableName))
            {
                cacheKey = tableName + "_" + DataCacheCheck.GetWebsiteOwner(RealTableName) + "_" + DataCache.FormatKey(strSql);
                object cacheObject = DataCache.GetCache(cacheKey);
                if (cacheObject != null) return cacheObject;
            }
            object result = ZCDALEngine.DALEngine.GetSingle(strSql, (trans == null ? null : trans.Transaction));
            //记录缓存
            if (DataCacheCheck.CheckEnableDataCache(RealTableName) && !string.IsNullOrWhiteSpace(tableName)) DataCache.SetCache(cacheKey, result);

            return result;
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public static List<T> Query<T>(string strSql) where T : ModelTable, new()
        {
            T model = new T();
            Type t = model.GetType();

            //读取缓存
            string cacheKey = "";
            string RealTableName = "ZCJ_" + t.Name;
            if (!GetCheck()) return new List<T>();
            if (DataCacheCheck.CheckEnableDataCache(RealTableName))
            {
                cacheKey = t.Name + "_" + DataCacheCheck.GetWebsiteOwner(RealTableName) + "_List_" + DataCache.FormatKey(strSql);
                object cacheObject = DataCache.GetCache(cacheKey);
                if (cacheObject != null)
                {
                    return DataSetToModelList<T>((DataSet)cacheObject); //有缓存执行
                }
            }

            DataSet ds = ZCDALEngine.DALEngine.Query(strSql);
            if (DataCacheCheck.CheckEnableDataCache(RealTableName) && ds.Tables[0].Rows.Count >= 0) DataCache.SetCache(cacheKey, ds);
            return DataSetToModelList<T>(ds);
        }
        /// <summary>
        /// DataSet转成List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static List<T> DataSetToModelList<T>(DataSet ds) where T : ModelTable, new()
        {
            T m = new T();
            Type type = m.GetType();
            //Assembly assembly = Assembly.GetExecutingAssembly();
            List<T> modelList = new List<T>();
            PropertyInfo[] properties = type.GetProperties();

            for (int i = 0; i < ds.Tables[0].Rows.Count; ++i)
            {
                T model = new T();
                foreach (DataColumn dc in ds.Tables[0].Columns)
                {
                    PropertyInfo pi = properties.FirstOrDefault(p => p.Name.ToLower() == dc.ColumnName.ToLower());
                    if (pi != null && ds.Tables[0].Rows[i][pi.Name] != DBNull.Value)
                    {
                        try
                        {
                            pi.SetValue(model, ds.Tables[0].Rows[i][pi.Name], null);
                        }
                        catch (Exception ex)
                        {

                            throw ex;
                        }
                    }
                }
                modelList.Add(model);
            }
            return modelList;
        }
       /// <summary>
       /// 查询
       /// </summary>
       /// <param name="strSql"></param>
       /// <param name="tableName"></param>
       /// <returns></returns>
        public static DataSet Query(string strSql, string tableName = null)
        {
            //读取缓存
            string cacheKey = "";
            string RealTableName = !string.IsNullOrWhiteSpace(tableName) && tableName.Contains("ZCJ_") ? tableName : "ZCJ_" + tableName;
            if (DataCacheCheck.CheckEnableDataCache(RealTableName) && !string.IsNullOrWhiteSpace(tableName))
            {
                cacheKey = tableName + "_" + DataCacheCheck.GetWebsiteOwner(RealTableName) + DataCache.FormatKey(strSql);
                object cacheObject = DataCache.GetCache(cacheKey);
                if (cacheObject != null) return (DataSet)cacheObject;
            }

            DataSet ds = ZCDALEngine.DALEngine.Query(strSql);

            //记录缓存
            if (DataCacheCheck.CheckEnableDataCache(RealTableName) && !string.IsNullOrWhiteSpace(tableName) && ds.Tables[0].Rows.Count >= 0) DataCache.SetCache(cacheKey, ds);

            return ds;
        }
        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static int ExecuteSql(string strSql, string tableName = null)
        {

            int result = ZCDALEngine.DALEngine.ExecuteSql(strSql);
            string RealTableName = !string.IsNullOrWhiteSpace(tableName) && tableName.Contains("ZCJ_") ? tableName : "ZCJ_" + tableName;
            //是否启用缓存
            if (result > 0 && DataCacheCheck.CheckEnableDataCache(RealTableName) && !string.IsNullOrWhiteSpace(tableName))
            {
                DataCache.ClearCacheStartsWith(tableName + "_" + DataCacheCheck.GetWebsiteOwner(RealTableName)); //清空所有缓存
            }
            else if (result > 0 && DataCacheCheck.CheckEnableDataCache(RealTableName) && string.IsNullOrWhiteSpace(tableName))
            {
                DataCache.ClearCacheAll(); //清空所有缓存
            }
            if (!GetCheck()) return 0;
            return result;
        }
        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="trans"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static int ExecuteSql(string strSql, BLLTransaction trans, string tableName = null)
        {
            string RealTableName = !string.IsNullOrWhiteSpace(tableName) && tableName.Contains("ZCJ_") ? tableName : "ZCJ_" + tableName;
            int result = ZCDALEngine.DALEngine.ExecuteSql(strSql,(trans == null?null: trans.Transaction));
            //是否启用缓存
            if (result > 0 && DataCacheCheck.CheckEnableDataCache(RealTableName) && !string.IsNullOrWhiteSpace(tableName))
            {
                DataCache.ClearCacheStartsWith(tableName + "_" + DataCacheCheck.GetWebsiteOwner(RealTableName)); //清空所有缓存
            }
            else if (result > 0 && DataCacheCheck.CheckEnableDataCache(RealTableName) && string.IsNullOrWhiteSpace(tableName))
            {
                DataCache.ClearCacheAll(); //清空所有缓存
            }
            return result;
        }
        /// <summary>
        /// 执行sql 事务
        /// </summary>
        /// <param name="SQLStringList"></param>
        /// <returns></returns>
        public static int ExecuteSqlTran(List<String> SQLStringList)
        {
            int result = ZCDALEngine.DALEngine.ExecuteSqlTran(SQLStringList);

            //是否启用缓存
            if (result > 0) DataCache.ClearCacheAll(); //清空所有缓存
            return result;
        }

        /// <summary>
        /// 更新Redis
        /// </summary>
        /// <param name="t"></param>
        /// <param name="model"></param>
        /// <param name="where"></param>
        private void UpdateRedis(Type t, ModelTable model, string where = "")
        {
            try
            {

                switch (t.Name)
                {
                    #region ZCJ_WebsiteInfo 表存到Redis

                    case "WebsiteInfo"://

                        ToLog("开始更新 WebsiteInfo");

                        try
                        {
                            if (!string.IsNullOrEmpty(where))
                            {
                                ToLog("更新1：" + where);
                                ZentCloud.ZCBLLEngine.Model.WebsiteInfo websiteInfo = new Model.WebsiteInfo();
                                Type typeWebsite = websiteInfo.GetType();
                                DataSet ds = ZCDALEngine.DALEngine.Get(1, "ZCJ_" + t.Name, where, "");
                                websiteInfo = DataSetToModelList<ZentCloud.ZCBLLEngine.Model.WebsiteInfo>(ds)[0];
                                string websiteOwner = typeWebsite.GetProperty("WebsiteOwner").GetValue(websiteInfo, null).ToString();
                                RedisHelper.RedisHelper.HashSetSerialize(RedisHelper.Enums.RedisKeyEnum.WebsiteInfo, websiteOwner, websiteInfo);
                                ToLog("更新1完成");
                            }
                            else
                            {
                                ToLog("更新2");
                                string websiteOwner = t.GetProperty("WebsiteOwner").GetValue(model, null).ToString();
                                RedisHelper.RedisHelper.HashSetSerialize(RedisHelper.Enums.RedisKeyEnum.WebsiteInfo, websiteOwner, model);
                                ToLog("更新2完成");
                            }
                        }
                        catch (Exception ex)
                        {
                            ToLog("更新 WebsiteInfo 异常：" + ex.Message);
                        }
                        
                        break;
                    #endregion

                    case "UserInfo"://

                        try
                        {
                            string websiteOwner=string.Empty, userId=string.Empty;
                            if (!string.IsNullOrEmpty(where))
                            {
                                Model.UserInfo userInfo = new Model.UserInfo();
                                DataSet ds = ZCDALEngine.DALEngine.Get(1, "ZCJ_" + t.Name, where, "");
                                userInfo = DataSetToModelList<Model.UserInfo>(ds)[0];
                                websiteOwner = userInfo.WebsiteOwner;
                                userId = userInfo.UserID;
                            }
                            else
                            {
                                
                                websiteOwner = t.GetProperty("WebsiteOwner").GetValue(model, null).ToString();
                                userId = t.GetProperty("UserID").GetValue(model, null).ToString();

                            }

                            if (!string.IsNullOrWhiteSpace(websiteOwner) && !string.IsNullOrWhiteSpace(userId))
                            {
                                var key = string.Format("{0}:User:{1}", websiteOwner, userId);
                                RedisHelper.RedisHelper.KeyDelete(key);
                                RedisHelper.RedisHelper.KeyBatchDelete(key);

                            }


                        }
                        catch (Exception ex)
                        {
                            ToLog("更新 UserInfo 异常：" + ex.Message);
                        }

                        break;
                   
                    case "Slide"://

                        try
                        {
                            string websiteOwner = string.Empty, slideType = string.Empty;
                            if (!string.IsNullOrEmpty(where))
                            {
                                Model.Slide slideInfo = new Model.Slide();
                                DataSet ds = ZCDALEngine.DALEngine.Get(1, "ZCJ_" + t.Name, where, "");
                                slideInfo = DataSetToModelList<Model.Slide>(ds)[0];
                                websiteOwner = slideInfo.WebsiteOwner;
                                slideType = slideInfo.Type;
                            }
                            else
                            {
                                websiteOwner = t.GetProperty("WebsiteOwner").GetValue(model, null).ToString();
                                slideType = t.GetProperty("Type").GetValue(model, null).ToString();
                            }

                            if (!string.IsNullOrWhiteSpace(websiteOwner))
                            {
                                var key = string.Format("{0}:{1}:{2}", websiteOwner, Common.SessionKey.SliderByType, slideType);
                                RedisHelper.RedisHelper.KeyDelete(key);

                                //批量删除 所有幻灯片
                                RedisHelper.RedisHelper.KeyBatchDelete(string.Format("{0}:{1}:*", websiteOwner, Common.SessionKey.SliderByType));


                            }
                            
                        }
                        catch (Exception ex)
                        {
                            ToLog("更新 slideInfo 异常：" + ex.Message);
                        }

                        break;

                   
                    default:
                        break;
                }

            }
            catch (Exception)
            {


            }




        }


        /// <summary>
        /// Redis读取
        /// </summary>
        /// <param name="t"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        private object GetRedis(Type t,string where = "")
        {
            try
            {

                switch (t.Name)
                {
                   

                    case "WebsiteInfo"://

                        ToLog(" 读取 WebsiteInfo ");

                        try
                        {
                            if (!string.IsNullOrEmpty(where))
                            {
                                where = where.ToLower();
                                if (where.Contains("websiteowner="))
                                {
                                    where = where.ToLower();
                                    where = where.Replace("websiteowner=", null);
                                    where = where.Replace("'", null);
                                    return RedisHelper.RedisHelper.HashGet<object>(RedisHelper.Enums.RedisKeyEnum.WebsiteInfo, where);

                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            ToLog(" 读取 WebsiteInfo 异常：" + ex.Message);
                        }

                        break;
                  
                    default:
                        break;
                }

            }
            catch (Exception)
            {


            }
            return null;



        }


        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="message"></param>
        public void ToLog(string message)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter("D:\\log\\BLLBase.txt", true, Encoding.UTF8))
                {
                    sw.WriteLine(string.Format("{0}  {1}", DateTime.Now.ToString(), message));
                }

            }
            catch { }
        }

    }
}
