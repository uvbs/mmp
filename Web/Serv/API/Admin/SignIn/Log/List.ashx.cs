using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.SignIn.Log
{
    /// <summary>
    ///签到日志列表
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLLSignIn bllSignIn = new BLLJIMP.BLLSignIn();
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            string keyWord = context.Request["keyword"];
            string addressId = context.Request["address_id"];
            string startTime = context.Request["start"];
            string stopTime = context.Request["stop"];
            string userId = context.Request["user_id"];
            string status = context.Request["status"];
            string isHide = context.Request["is_show"];
            string isGroup = context.Request["is_group"];
            string groupTime=context.Request["group_time"];
            int totalCount = 0;
            List<SignInLog> list = bllSignIn.GetSignLogList(1, int.MaxValue, keyWord, addressId, out totalCount, startTime, stopTime, userId, status);
            SignInAddress signModel = bllSignIn.GetSignInAddress(bllSignIn.WebsiteOwner, addressId);
            List<SignInLog> sLogList=new List<SignInLog>();
            if (!string.IsNullOrEmpty(groupTime))
            {
                List<TimeList> timeList = JsonConvert.DeserializeObject<List<TimeList>>(signModel.SignInTime);
                foreach (TimeList temp in timeList)
                {
                    list = list.Where(p => p.CreateDate >= temp.start && p.CreateDate <= temp.stop).OrderBy(p=>p.AutoID).ToList();
                }
            }
            apiResp.status = true;
            JArray returnList = new JArray();
            if (!string.IsNullOrEmpty(signModel.SignInTime) && !string.IsNullOrEmpty(isGroup))
            {
                var userList = list.Select(p => p.UserID).Distinct().ToList();

                List<TimeList> timeList = JsonConvert.DeserializeObject<List<TimeList>>(signModel.SignInTime);
                for (var i = 0; i < userList.Count; i++)
                {
                    string user = userList[i];
                    UserInfo userModel = bllUser.GetUserInfo(user);

                    JToken jt = new JObject();
                    jt["wxnick_name"] = userModel.WXNickname;
                    jt["name"] = userModel.TrueName;
                    jt["phone"] = userModel.Phone;
                    jt["head_img_url"] = userModel.WXHeadimgurl;
                    for (int j = 0; j < timeList.Count; j++)
                    {
                        SignInLog log = list.Where(p => p.UserID == user && p.CreateDate >= timeList[j].start && p.CreateDate <= timeList[j].stop).OrderBy(p => p.AutoID).FirstOrDefault();
                        if (log != null)
                        {
                            jt["time" + j] = log.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
                            jt["distance" + j] = log.Distance;
                        }
                        else
                        {
                            jt["time" + j] = "";
                            jt["distance" + j] = "";
                        }
                    }

                    returnList.Add(jt);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(isHide))
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
                        JToken jt = new JObject();
                        jt["wxnick_name"] = userModel.WXNickname;
                        jt["name"] = userModel.TrueName;
                        jt["phone"] = userModel.Phone;
                        jt["head_img_url"] = userModel.WXHeadimgurl;
                        jt["createtime"] = log.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
                        jt["remark"] = log.Remark;
                        jt["distance"] = log.Distance;
                        jt["status"] = log.Status;
                        returnList.Add(jt);
                    }
                }
                else
                {
                    for (var i = 0; i < list.Count; i++)
                    {
                        UserInfo userModel = bllUser.GetUserInfo(list[i].UserID);
                        JToken jt = new JObject();
                        jt["wxnick_name"] = userModel.WXNickname;
                        jt["name"] = userModel.TrueName;
                        jt["phone"] = userModel.Phone;
                        jt["head_img_url"] = userModel.WXHeadimgurl;
                        jt["createtime"] = list[i].CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
                        jt["remark"] = list[i].Remark;
                        jt["distance"] = list[i].Distance;
                        jt["status"] = list[i].Status;
                        returnList.Add(jt);
                    }
                }

            }
            bllSignIn.ContextResponse(context, new
            {
                total = totalCount,
                rows = returnList
            });
        }


        public class TimeList
        {
            public string name { get; set; }
            public DateTime start { get; set; }
            public DateTime stop { get; set; }
        }
    }
}