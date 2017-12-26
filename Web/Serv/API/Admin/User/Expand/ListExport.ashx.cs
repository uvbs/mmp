using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP.Model.API.File;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.User.Expand
{
    /// <summary>
    /// ListExport 的摘要说明
    /// </summary>
    public class ListExport : BaseHandlerNeedLoginAdminNoAction
    {
        BLLUserExpand bllUserEx = new BLLUserExpand();

        public void ProcessRequest(HttpContext context)
        {
            string type = context.Request["type"];
            string member = context.Request["member"];
            string websiteOwner = bllUser.WebsiteOwner;
            string userIds = "";
            UserExpandType nType = new UserExpandType();
            if (!Enum.TryParse(type, out nType))
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "类型格式不能识别";
                bllUserEx.ContextResponse(context, apiResp);
                return;
            }
            string tname = BLLUserExpand.dicTypes[type];
            if (!string.IsNullOrWhiteSpace(member)) userIds = bllUser.GetSpreadUserIds(member, websiteOwner);
            List<UserExpand> list = bllUserEx.GetExpandList(int.MaxValue, 1, nType, userIds);


            DataTable dt = new DataTable();
            dt.Columns.Add("会员编号", typeof(int));
            dt.Columns.Add("会员手机", typeof(string));
            dt.Columns.Add("会员姓名", typeof(string));
            if(BLLUserExpand.dicColumns.ContainsKey(type)){
                foreach (var item in BLLUserExpand.dicColumns[type])
                {
                    dt.Columns.Add(item.name, typeof(string));
                }
            }
            if (list.Count > 0)
            {
                string uIds = ZentCloud.Common.MyStringHelper.ListToStr(list.Select(p => p.UserId).ToList(), "'", ",");
                List<UserInfo> uList = bllUser.GetColMultListByKey<UserInfo>(int.MaxValue, 1, "UserID", uIds, "AutoID,WXNickname,TrueName,Phone,UserID", websiteOwner: websiteOwner);
                foreach (var item in list)
                {
                    UserInfo u = uList.FirstOrDefault(p => p.UserID == item.UserId);
                    DataRow dr = dt.NewRow();
                    dr["会员编号"] = u == null ? 0 : u.AutoID;
                    dr["会员手机"] = u == null ? "" : u.Phone;
                    dr["会员姓名"] = u == null ? "" : bllUser.GetUserDispalyName(u);

                    if (BLLUserExpand.dicColumns.ContainsKey(type))
                    {
                        JObject jOb = JObject.FromObject(item);
                        foreach (var col in BLLUserExpand.dicColumns[type])
                        {
                            if (!string.IsNullOrWhiteSpace(col.mfield))
                                dr[col.name] = jOb[col.mfield];
                        }
                    }
                    dt.Rows.Add(dr);
                }
                dt.TableName = tname;
                dt.AcceptChanges();
            }
            MemoryStream ms = Web.DataLoadTool.NPOIHelper.Export(dt, tname);
            ExportCache exCache = new ExportCache()
            {
                FileName = string.Format("{0}.xls", tname),
                Stream = ms
            };
            string cache = Guid.NewGuid().ToString("N").ToUpper();
            RedisHelper.RedisHelper.StringSetSerialize(cache, exCache, TimeSpan.FromMinutes(5));

            apiResp.status = true;
            apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsSuccess;
            apiResp.msg = "生成完成";
            apiResp.result = new
            {
                cache = cache
            };
            bllUser.ContextResponse(context, apiResp);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}