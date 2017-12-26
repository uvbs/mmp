using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Activity.ActivityData
{
    /// <summary>
    /// List 的摘要说明
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLLJuActivity bllJuActivity = new BLLJIMP.BLLJuActivity();
        BLLJIMP.BLLActivity bllActivity = new BLLJIMP.BLLActivity("");
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string activityId = context.Request["activity_id"];
                int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
                int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
                string keyWord = context.Request["keyword"];
                if (string.IsNullOrEmpty(activityId))
                {
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                    resp.errmsg = "activity_id 为必填项,请检查";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }
                JuActivityInfo juActivity = bllJuActivity.GetJuActivity(int.Parse(activityId),false);
                if (juActivity == null)
                {
                    resp.errmsg = "不存在该条活动";
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }

                System.Text.StringBuilder sbWhere = new System.Text.StringBuilder(string.Format(" WebSiteOwner='{0}' AND isDelete=0 AND ActivityID='{1}' ", bllActivity.WebsiteOwner, juActivity.SignUpActivityID));

                if (!string.IsNullOrEmpty(keyWord))
                {
                    sbWhere.AppendFormat(" AND Name like '%{0}%' ", keyWord);
                }

                int totalCount = bllActivity.GetCount<ActivityDataInfo>(sbWhere.ToString());

                var dataList= bllActivity.GetLit<ActivityDataInfo>(pageSize, pageIndex, sbWhere.ToString());

                List<ActivityFieldMappingInfo> fieldMapList = bllActivity.GetActivityFieldMappingList(juActivity.SignUpActivityID);
                
                resp.isSuccess = true;

                List<dynamic> list = new List<dynamic>();
                List<dynamic> maplist = new List<dynamic>();
                var fieldlist = bllActivity.GetActivityFieldMappingList(juActivity.SignUpActivityID);
                foreach (var item in fieldlist)
                {
                    maplist.Add(new 
                    {
                        key=item.MappingName,
                        value=item.FieldName
                    });
                }

                for (int i = 0; i < dataList.Count; i++)
                {
                    list.Add(new
                    {
                        name = dataList[i].Name,
                        phone = dataList[i].Phone,
                        insert_time =bllActivity.GetTimeStamp(dataList[i].InsertDate),
                        k1 = dataList[i].K1,
                        k2 = dataList[i].K2,
                        k3 = dataList[i].K3,
                        k4 = dataList[i].K4,
                        k5 = dataList[i].K5,
                        k6 = dataList[i].K6,
                        k7 = dataList[i].K7,
                        k8 = dataList[i].K8,
                        k9 = dataList[i].K9,
                        k10 = dataList[i].K10,

                        k11 = dataList[i].K11,
                        k12 = dataList[i].K12,
                        k13 = dataList[i].K13,
                        k14 = dataList[i].K14,
                        k15 = dataList[i].K15,
                        k16 = dataList[i].K16,
                        k17 = dataList[i].K17,
                        k18 = dataList[i].K18,
                        k19 = dataList[i].K19,
                        k20 = dataList[i].K20,

                        k21 = dataList[i].K21,
                        k22 = dataList[i].K22,
                        k23 = dataList[i].K23,
                        k24 = dataList[i].K24,
                        k25 = dataList[i].K25,
                        k26 = dataList[i].K26,
                        k27 = dataList[i].K27,
                        k28 = dataList[i].K28,
                        k29 = dataList[i].K29,
                        k30 = dataList[i].K30,

                        k31 = dataList[i].K31,
                        k32 = dataList[i].K32,
                        k33 = dataList[i].K33,
                        k34 = dataList[i].K34,
                        k35 = dataList[i].K35,
                        k36 = dataList[i].K36,
                        k37 = dataList[i].K37,
                        k38 = dataList[i].K38,
                        k39 = dataList[i].K39,
                        k40 = dataList[i].K40,

                        k41 = dataList[i].K41,
                        k42 = dataList[i].K42,
                        k43 = dataList[i].K43,
                        k44 = dataList[i].K44,
                        k45 = dataList[i].K45,
                        k46 = dataList[i].K46,
                        k47 = dataList[i].K47,
                        k48 = dataList[i].K48,
                        k49 = dataList[i].K49,
                        k50 = dataList[i].K50,

                        k51 = dataList[i].K51,
                        k52 = dataList[i].K52,
                        k53 = dataList[i].K53,
                        k54 = dataList[i].K54,
                        k55 = dataList[i].K55,
                        k56 = dataList[i].K56,
                        k57 = dataList[i].K57,
                        k58 = dataList[i].K58,
                        k59 = dataList[i].K59,
                        k60 = dataList[i].K60,
                    });
                }


                var data = new {
                    totalcount = totalCount,
                    list = list,
                
                };

                resp.returnObj = new
                {
                    data=data,
                    maplist=maplist
                };
            }
            catch (Exception ex)
            {

                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                resp.errmsg = ex.Message;
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }



        //public class RequestModel 
        //{
        //    /// <summary>
        //    /// 活动id
        //    /// </summary>
        //    public string activity_id { get; set; }

        //    /// <summary>
        //    /// uid
        //    /// </summary>
        //    public int activity_uid { get; set; }

        //    /// <summary>
        //    /// 姓名
        //    /// </summary>
        //    public string activity_name { get; set; }
        //}
    }
}