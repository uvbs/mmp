using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Activity.ActivityData
{
    /// <summary>
    /// Update 的摘要说明
    /// </summary>
    public class Update : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLLJuActivity bllJuActivity = new BLLJIMP.BLLJuActivity();
        public void ProcessRequest(HttpContext context)
        {
            string data = context.Request["data"];
            RequestModel requestModel;
            try
            {
                requestModel = ZentCloud.Common.JSONHelper.JsonToModel<RequestModel>(context.Request["data"]);
            }
            catch (Exception)
            {
                resp.errcode = -1;
                resp.errmsg = "json格式错误,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.activity_id))
            {
                resp.errmsg = "activity_id 为必填项,请检查";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (requestModel.activity_uid <= 0)
            {
                resp.errmsg = "activity_uid 为必填项,请检查";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.name))
            {
                resp.errmsg = "name 为必填项,请检查";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.phone))
            {
                resp.errmsg = "phone 为必填项,请检查";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }

            JuActivityInfo juActivity = bllJuActivity.GetJuActivity(int.Parse(requestModel.activity_id), false);
            if (juActivity == null)
            {
                resp.errmsg = "不存在该条活动";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            ActivityDataInfo activityInfo = new ActivityDataInfo();
            activityInfo.ActivityID = juActivity.SignUpActivityID;
            activityInfo.UID = requestModel.activity_uid;
            activityInfo.Name = requestModel.name;
            activityInfo.Phone = requestModel.phone;
            activityInfo.K1 = requestModel.k1;
            activityInfo.K2 = requestModel.k2;
            activityInfo.K3 = requestModel.k3;
            activityInfo.K4 = requestModel.k4;
            activityInfo.K5 = requestModel.k5;
            activityInfo.K6 = requestModel.k6;
            activityInfo.K7 = requestModel.k7;
            activityInfo.K8 = requestModel.k8;
            activityInfo.K9 = requestModel.k9;
            activityInfo.K10 = requestModel.k10;

            activityInfo.K11 = requestModel.k11;
            activityInfo.K12 = requestModel.k12;
            activityInfo.K13 = requestModel.k13;
            activityInfo.K14 = requestModel.k14;
            activityInfo.K15 = requestModel.k15;
            activityInfo.K16 = requestModel.k16;
            activityInfo.K17 = requestModel.k17;
            activityInfo.K18 = requestModel.k18;
            activityInfo.K19 = requestModel.k19;
            activityInfo.K20 = requestModel.k20;

            activityInfo.K21 = requestModel.k21;
            activityInfo.K22 = requestModel.k22;
            activityInfo.K23 = requestModel.k23;
            activityInfo.K24 = requestModel.k24;
            activityInfo.K25 = requestModel.k25;
            activityInfo.K26 = requestModel.k26;
            activityInfo.K27 = requestModel.k27;
            activityInfo.K28 = requestModel.k28;
            activityInfo.K29 = requestModel.k29;
            activityInfo.K30 = requestModel.k30;

            activityInfo.K31 = requestModel.k31;
            activityInfo.K32 = requestModel.k32;
            activityInfo.K33 = requestModel.k33;
            activityInfo.K34 = requestModel.k34;
            activityInfo.K35 = requestModel.k35;
            activityInfo.K36 = requestModel.k36;
            activityInfo.K37 = requestModel.k37;
            activityInfo.K38 = requestModel.k38;
            activityInfo.K39 = requestModel.k39;
            activityInfo.K40 = requestModel.k40;

            activityInfo.K41 = requestModel.k41;
            activityInfo.K42 = requestModel.k42;
            activityInfo.K43 = requestModel.k43;
            activityInfo.K44 = requestModel.k44;
            activityInfo.K45 = requestModel.k45;
            activityInfo.K46 = requestModel.k46;
            activityInfo.K47 = requestModel.k47;
            activityInfo.K48 = requestModel.k48;
            activityInfo.K49 = requestModel.k49;
            activityInfo.K50 = requestModel.k50;

            activityInfo.K51 = requestModel.k51;
            activityInfo.K52 = requestModel.k52;
            activityInfo.K53 = requestModel.k53;
            activityInfo.K54 = requestModel.k54;
            activityInfo.K55 = requestModel.k55;
            activityInfo.K56 = requestModel.k56;
            activityInfo.K57 = requestModel.k57;
            activityInfo.K58 = requestModel.k58;
            activityInfo.K59 = requestModel.k59;
            activityInfo.K60 = requestModel.k60;

            if (bllJuActivity.Update(activityInfo))
            {
                resp.isSuccess = true;
                resp.errmsg = "ok";
            }
            else
            {
                resp.errmsg = "修改报名数据出错";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }

        public class RequestModel
        {
            /// <summary>
            /// 活动id
            /// </summary>
            public string activity_id { get; set; }

            /// <summary>
            /// uid
            /// </summary>
            public int activity_uid { get; set; }

            /// <summary>
            /// 活动名称
            /// </summary>
            public string name { get; set; }

            /// <summary>
            /// 地点
            /// </summary>
            public string phone { get; set; }

            public string k1 { get; set; }
            public string k2 { get; set; }
            public string k3 { get; set; }
            public string k4 { get; set; }
            public string k5 { get; set; }
            public string k6 { get; set; }
            public string k7 { get; set; }
            public string k8 { get; set; }
            public string k9 { get; set; }
            public string k10 { get; set; }
            public string k11 { get; set; }
            public string k12 { get; set; }
            public string k13 { get; set; }
            public string k14 { get; set; }
            public string k15 { get; set; }
            public string k16 { get; set; }
            public string k17 { get; set; }
            public string k18 { get; set; }
            public string k19 { get; set; }
            public string k20 { get; set; }
            public string k21 { get; set; }
            public string k22 { get; set; }
            public string k23 { get; set; }
            public string k24 { get; set; }
            public string k25 { get; set; }
            public string k26 { get; set; }
            public string k27 { get; set; }
            public string k28 { get; set; }
            public string k29 { get; set; }
            public string k30 { get; set; }
            public string k31 { get; set; }
            public string k32 { get; set; }
            public string k33 { get; set; }
            public string k34 { get; set; }
            public string k35 { get; set; }
            public string k36 { get; set; }
            public string k37 { get; set; }
            public string k38 { get; set; }
            public string k39 { get; set; }
            public string k40 { get; set; }
            public string k41 { get; set; }
            public string k42 { get; set; }
            public string k43 { get; set; }
            public string k44 { get; set; }
            public string k45 { get; set; }
            public string k46 { get; set; }
            public string k47 { get; set; }
            public string k48 { get; set; }
            public string k49 { get; set; }
            public string k50 { get; set; }
            public string k51 { get; set; }
            public string k52 { get; set; }
            public string k53 { get; set; }
            public string k54 { get; set; }
            public string k55 { get; set; }
            public string k56 { get; set; }
            public string k57 { get; set; }
            public string k58 { get; set; }
            public string k59 { get; set; }
            public string k60 { get; set; }
        }
    }
}