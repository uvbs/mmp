using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ZentCloud.BLLPermission.Model;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.BLLPermission
{
    [Serializable]
    public class BLL : ZentCloud.ZCBLLEngine.BLLBase
    {
        public BLL()
        {
            _userid = "";
        }
        public BLL(string userID)
        {
            _userid = userID;
        }
        private string _userid;

        public string UserID
        {
            get { return _userid; }
        }

        protected override string GetRealTableName(string modelName)
        {
            string tableName = modelName.EndsWith("Ex", true, null) ? modelName.Substring(0, modelName.Length - 2) : modelName;

            return "ZCJ_" + tableName;
        }

        public string GetGUID(Common.TransacType transacType)
        {
            string strSql = string.Format(
                                @"insert into ZCJ_GUID (UserID, TransacDescription, TransacDate) 
                                    values ('{0}', {1}, GETDATE()) select @@IDENTITY",
                                                           this.UserID, (int)transacType);
            return GetSingle(strSql).ToString();

        }

        /// <summary>
        /// 当前站点所有者
        /// </summary>
        public string WebsiteOwner
        {
            get
            {
                if (HttpContext.Current.Session["WebsiteOwner"] != null)
                    return (string)HttpContext.Current.Session["WebsiteOwner"];
                return null;
            }
        }


        /// <summary>
        /// 获取当前登录用户的信息
        /// </summary>
        /// <returns></returns>
        public UserInfo GetCurrentUserInfo()
        {
            try
            {
                return Get<UserInfo>(string.Format("UserID = '{0}'", HttpContext.Current.Session["userID"]));
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 响应内容返回
        /// </summary>
        /// <param name="context"></param>
        /// <param name="result"></param>
        public void ContextResponse(HttpContext context, dynamic result)
        {
            string respStr = JsonConvert.SerializeObject(result);

            context.Response.ContentType = "application/json";
            context.Response.Expires = 0;
            context.Response.Clear();
            context.Response.ClearContent();

            if (!string.IsNullOrEmpty(context.Request["callback"]))
            {
                //返回 jsonp数据
                context.Response.Write(string.Format("{0}({1})", context.Request["callback"], respStr));
            }
            else
                context.Response.Write(respStr);
        }
        public T GetByKey<T>(string keyName, string value) where T : ModelTable, new()
        {
            return Get<T>(string.Format("{0}='{1}'", keyName, value));
        }
        public T GetColByKey<T>(string keyName, string value, string colName) where T : ModelTable, new()
        {
            List<T> list = GetColList<T>(1, 1, string.Format("{0}='{1}'", keyName, value), colName);
            if (list.Count == 0) return null;
            return list[0];
        }
        public List<T> GetListByKey<T>(string keyName, string value) where T : ModelTable, new()
        {
            return GetList<T>(string.Format("{0}='{1}'", keyName, value));
        }
        public List<T> GetListByKey<T>(int rows, int page, string keyName, string value) where T : ModelTable, new()
        {
            return GetLit<T>(rows, page, string.Format("{0}='{1}'", keyName, value));
        }
        public List<T> GetMultListByKey<T>(string keyName, string value) where T : ModelTable, new()
        {
            return GetList<T>(string.Format("{0} In ({1})", keyName, value));
        }
        public List<T> GetMultListByKey<T>(int rows, int page, string keyName, string value) where T : ModelTable, new()
        {
            return GetLit<T>(rows, page, string.Format("{0} In ({1})", keyName, value));
        }
        public int GetCountByKey<T>(string keyName, string value) where T : ModelTable, new()
        {
            return GetCount<T>(string.Format("{0}='{1}'", keyName, value));
        }

        public int DeleteByKey<T>(string keyName, string value, BLLTransaction tran = null) where T : ModelTable, new()
        {
            return Delete(new T(), string.Format("{0}='{1}'", keyName, value), tran);
        }

        public int DeleteMultByKey<T>(string keyName, string value, BLLTransaction tran = null) where T : ModelTable, new()
        {
            return Delete(new T(), string.Format("{0} In ({1})", keyName, value), tran);
        }

        public int UpdateByKey<T>(string keyName, string value, string toKeyName, string toValue, BLLTransaction tran = null) where T : ModelTable, new()
        {
            return Update(new T(), string.Format("{0}='{1}'", toKeyName, toValue), string.Format("{0}='{1}'", keyName, value), tran);
        }
    }
}
