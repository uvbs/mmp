using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using ZCJson;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.SignIn.Log
{
    /// <summary>
    /// 签到记录导出
    /// </summary>
    public class Export : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLLSignIn bllSignIn = new BLLJIMP.BLLSignIn();
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            string addressId=context.Request["address"];
            string isGroup=context.Request["is_group"];
            string isShow = context.Request["is_show"];
            string startTime = context.Request["start"];
            string stopTime = context.Request["stop"];
            string userId = context.Request["user_id"];
            int totalCount = 0;
            List<SignInLog> list = bllSignIn.GetSignLogList(1, int.MaxValue, "", addressId, out totalCount, startTime, stopTime, userId, "");
            SignInAddress signModel = bllSignIn.GetSignInAddress(bllSignIn.WebsiteOwner, addressId);
            apiResp.status = true;
            DataTable dt = new DataTable();
            dt.Columns.Add("微信昵称");
            dt.Columns.Add("姓名");
            dt.Columns.Add("手机");
            if (!string.IsNullOrEmpty(signModel.SignInTime)&&!string.IsNullOrEmpty(isGroup))
            {
                var userList = list.Select(p => p.UserID).Distinct().ToList();
                List<TimeList> timeList = JsonConvert.DeserializeObject<List<TimeList>>(signModel.SignInTime);
                for (int i = 0; i < timeList.Count; i++)
                {
                    dt.Columns.Add(timeList[i].name);
                    dt.Columns.Add(timeList[i].name+"签到距离");
                }
                for (var i = 0; i < userList.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    string user = userList[i];
                    UserInfo userModel = bllUser.GetUserInfo(user);
                    dr["微信昵称"] = userModel.WXNickname;
                    dr["姓名"] = userModel.TrueName;
                    dr["手机"] = userModel.Phone;
                    for (int j = 0; j < timeList.Count; j++)
                    {
                        SignInLog log = list.Where(p => p.UserID == user && p.CreateDate >= timeList[j].start && p.CreateDate <= timeList[j].stop).OrderBy(p => p.AutoID).FirstOrDefault();
                        if (log != null)
                        {
                            dr[timeList[j].name] = log.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
                            dr[timeList[j].name+"签到距离"] = log.Distance;
                        }
                        else
                        {
                            dr[timeList[j].name] = "";
                            dr[timeList[j].name + "签到距离"] = "";
                        }
                    }
                    dt.Rows.Add(dr);
                }
            }
            else
            {
                dt.Columns.Add("签到时间");
                dt.Columns.Add("说明");
                dt.Columns.Add("距离");
                if (!string.IsNullOrEmpty(isShow))
                {
                    var userList = list.Select(p => p.UserID).Distinct().ToList();
                    SignInLog log = new SignInLog();
                    for (int i = 0; i < userList.Count; i++)
                    {
                        string user = userList[i];
                        log = list.Where(p => p.UserID == user && p.Status == 1).OrderBy(p => p.AutoID).FirstOrDefault();
                        if (log == null) log = list.Where(p => p.UserID == user && p.Status == 0).OrderBy(p => p.AutoID).FirstOrDefault();
                        if (log == null) continue;
                        UserInfo userModel = bllUser.GetUserInfo(user);
                        DataRow dr = dt.NewRow();
                        dr["微信昵称"] = userModel.WXNickname;
                        dr["姓名"] = userModel.TrueName;
                        dr["手机"] = userModel.Phone;
                        dr["签到时间"] = list[i].CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
                        dr["说明"] = list[i].Remark;
                        dr["距离"] = list[i].Distance;
                        dt.Rows.Add(dr);
                    }
                }
                else
                {
                    for (var i = 0; i < list.Count; i++)
                    {
                        UserInfo userModel = bllUser.GetUserInfo(list[i].UserID);
                        DataRow dr = dt.NewRow();
                        dr["微信昵称"] = userModel.WXNickname;
                        dr["姓名"] = userModel.TrueName;
                        dr["手机"] = userModel.Phone;
                        dr["签到时间"] = list[i].CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
                        dr["说明"] = list[i].Remark;
                        dr["距离"] = list[i].Distance;
                        dt.Rows.Add(dr);
                    }
                }
            }
            DataLoadTool.ExportDataTable(dt, string.Format("{0}_{1}_data.xls", signModel.Address, DateTime.Now.ToString()));
            return;
        }
        public class TimeList
        {
            public string name { get; set; }
            public DateTime start { get; set; }
            public DateTime stop { get; set; }
        }
    }
}