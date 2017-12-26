using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Meifan.Activity
{



    /// <summary>
    /// 添加
    /// </summary>
    public class Add : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLLMeifan bll = new BLLJIMP.BLLMeifan();
        public void ProcessRequest(HttpContext context)
        {
            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            try
            {
                string json = context.Request["data"];
                ActivityModel requestModel = ZentCloud.Common.JSONHelper.JsonToModel<ActivityModel>(json);//jSON 反序

                JuActivityInfo model = new JuActivityInfo();
                model.JuActivityID =int.Parse(bll.GetGUID(BLLJIMP.TransacType.CommAdd));
                model.ThumbnailsPath = requestModel.activity_img;
                model.ActivityName = requestModel.activity_name;
                model.ArticleType = "activity";
                model.ActivityDescription = requestModel.description;
                model.CreateDate = DateTime.Now;
                model.IsDelete = 0;
                model.IsFee = requestModel.is_need_pay;
                model.IsPublish = 0;
                model.MainPoints = requestModel.main_points;
                model.Summary = requestModel.summary;
                DateTime beginDate;
                if (!DateTime.TryParse(requestModel.begin_date, out beginDate))
                {
                    apiResp.msg = "请输入活动开始时间";
                    bll.ContextResponse(context, apiResp);
                    return;
                }
                DateTime endDate;
                if (!DateTime.TryParse(requestModel.end_date, out endDate))
                {
                    apiResp.msg = "请输入活动结束时间";
                    bll.ContextResponse(context, apiResp);
                    return;
                }

                model.BeginDate = beginDate.ToString("yyyy/MM/dd");
                model.EndDate = endDate.ToString("yyyy/MM/dd");
                model.ActivityAddress = requestModel.address;
                model.WebsiteOwner = bll.WebsiteOwner;
                model.UserID = currentUserInfo.UserID;
                if (!bll.Add(model, tran))
                {
                    tran.Rollback();
                    apiResp.msg = "添加失败";
                    bll.ContextResponse(context, apiResp);
                    return;

                }

                if (requestModel.is_need_pay==1)
                {
                    List<MeifanActivityItem> activityItems = new List<MeifanActivityItem>();
                    foreach (var item in requestModel.items)
                    {

                        if (string.IsNullOrEmpty(item.amount))
                        {
                            tran.Rollback();
                            apiResp.msg = "请输入金额";
                            bll.ContextResponse(context, apiResp);
                            return;
                        }
                        if (requestModel.items.Count(p=>p.group_type == item.group_type && p.is_member == item.is_member) > 1)
                        {
                            tran.Rollback();
                            apiResp.msg = "时间,组别,会员类型不能重复";
                            bll.ContextResponse(context, apiResp);
                            return;
                        }
                        MeifanActivityItem itemModel = new MeifanActivityItem();
                        itemModel.ActivityId = model.JuActivityID.ToString();
                        itemModel.Amount = decimal.Parse(item.amount);
                        //itemModel.FromDate = Convert.ToDateTime(item.from_date).ToString("yyyy/MM/dd HH:mm");
                        //itemModel.ToDate = Convert.ToDateTime(item.to_date).ToString("yyyy/MM/dd HH:mm");
                        itemModel.GroupType = item.group_type;
                        itemModel.IsMember = item.is_member;
                        activityItems.Add(itemModel);

                    }
                    if (!bll.AddList<MeifanActivityItem>(activityItems))
                    {
                        tran.Rollback();
                        apiResp.msg = "添加失败";
                        bll.ContextResponse(context, apiResp);
                        return;
                    }
                }


                tran.Commit();
                apiResp.status = true;

            }
            catch (Exception ex)
            {
                tran.Rollback();
                apiResp.msg = ex.Message;
                bll.ContextResponse(context, apiResp);

            }
            bll.ContextResponse(context, apiResp);



        }

        /// <summary>
        /// 模型
        /// </summary>
        public class ActivityModel
        {
            /// <summary>
            /// 
            /// </summary>
            public string activity_id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string activity_name { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string activity_img { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string summary { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string address { get; set; }
            /// <summary>
            /// 要点
            /// </summary>
            public string main_points { get; set; }
            /// <summary>
            /// 是否需要付款
            /// </summary>
            public int is_need_pay { get; set; }
            /// <summary>
            /// 详细
            /// </summary>
            public string description { get; set; }
            /// <summary>
            /// 开始时间
            /// </summary>
            public string begin_date { get; set; }
            /// <summary>
            /// 结束时间
            /// </summary>
            public string end_date { get; set; }
            /// <summary>
            /// 选项
            /// </summary>
            public List<ActivityItemModel> items { get; set; }


        }
        /// <summary>
        /// 
        /// </summary>
        public class ActivityItemModel
        {
            /// <summary>
            /// 
            /// </summary>
            public string item_id { get; set; }
            ///// <summary>
            ///// 
            ///// </summary>
            //public string from_date { get; set; }
            ///// <summary>
            ///// 
            ///// </summary>
            //public string to_date { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string group_type { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string is_member { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string amount { get; set; }



        }


    }

}