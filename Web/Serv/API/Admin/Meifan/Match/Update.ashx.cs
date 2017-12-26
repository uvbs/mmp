using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Meifan.Match
{
    /// <summary>
    /// 更新
    /// </summary>
    public class Update : BaseHandlerNeedLoginAdminNoAction
    {

        BLLJIMP.BLLMeifan bll = new BLLJIMP.BLLMeifan();
        public void ProcessRequest(HttpContext context)
        {
            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            try
            {
                string json = context.Request["data"];
                ActivityModel requestModel = ZentCloud.Common.JSONHelper.JsonToModel<ActivityModel>(json);//jSON 反序

                JuActivityInfo model = bll.GetActivity(requestModel.activity_id);
                //model.ActivityId = bll.GetGUID(BLLJIMP.TransacType.CommAdd);
                model.ThumbnailsPath = requestModel.activity_img;
                model.ActivityName = requestModel.activity_name;
                //model.ActivityType = "match";
                model.ActivityDescription = requestModel.description;
                // model.InsertDate = DateTime.Now;
                model.IsFee = requestModel.is_need_pay;
                model.MainPoints = requestModel.main_points;
                model.Summary = requestModel.summary;
                //model.Websiteowner = bll.WebsiteOwner;

                if (!bll.Update(model, tran))
                {
                    tran.Rollback();
                    apiResp.msg = "添加失败";
                    bll.ContextResponse(context, apiResp);
                    return;

                }

                if (requestModel.is_need_pay == 1)
                {
                    #region 删除旧选项
                    var oldItems = bll.ActivityItemList(requestModel.activity_id);
                    foreach (var item in oldItems)
                    {
                        if (requestModel.items.Count(p => p.item_id == item.AutoId.ToString()) == 0)
                        {

                            if (bll.Delete(item) == 0)
                            {
                                tran.Rollback();
                                apiResp.msg = "操作失败";
                                bll.ContextResponse(context, apiResp);
                                return;
                            }

                        }

                    }
                    #endregion
                    foreach (var item in requestModel.items)
                    {

                        #region 添加选项
                        if (string.IsNullOrEmpty(item.item_id))//添加
                        {
                            if (string.IsNullOrEmpty(item.from_date))
                            {
                                tran.Rollback();
                                apiResp.msg = "请输入开始时间";
                                bll.ContextResponse(context, apiResp);
                                return;
                            }
                            if (string.IsNullOrEmpty(item.from_date))
                            {
                                tran.Rollback();
                                apiResp.msg = "请输入结束时间";
                                bll.ContextResponse(context, apiResp);
                                return;
                            }
                            if (string.IsNullOrEmpty(item.amount))
                            {
                                tran.Rollback();
                                apiResp.msg = "请输入金额";
                                bll.ContextResponse(context, apiResp);
                                return;
                            }
                            if (requestModel.items.Count(p => p.from_date == item.from_date && p.to_date == item.to_date && p.group_type == item.group_type && p.is_member == item.is_member) > 1)
                            {
                                tran.Rollback();
                                apiResp.msg = "时间,组别,会员类型不能重复";
                                bll.ContextResponse(context, apiResp);
                                return;
                            }
                            MeifanActivityItem itemModel = new MeifanActivityItem();
                            itemModel.ActivityId = model.JuActivityID.ToString();
                            itemModel.Amount = decimal.Parse(item.amount);
                            itemModel.FromDate = Convert.ToDateTime(item.from_date).ToString("yyyy/MM/dd HH:mm");
                            itemModel.ToDate = Convert.ToDateTime(item.to_date).ToString("yyyy/MM/dd HH:mm");
                            itemModel.GroupType = item.group_type;
                            itemModel.IsMember = item.is_member;

                            if (!bll.Add(itemModel))
                            {
                                tran.Rollback();
                                apiResp.msg = "操作失败";
                                bll.ContextResponse(context, apiResp);
                                return;
                            }
                        }
                        #endregion

                        #region 编辑选项
                        else//编辑
                        {

                            MeifanActivityItem itemModel = bll.Get<MeifanActivityItem>(string.Format(" AutoId={0}", item.item_id));
                            itemModel.ActivityId = model.JuActivityID.ToString();
                            itemModel.Amount = decimal.Parse(item.amount);
                            itemModel.FromDate = Convert.ToDateTime(item.from_date).ToString("yyyy/MM/dd HH:mm");
                            itemModel.ToDate = Convert.ToDateTime(item.to_date).ToString("yyyy/MM/dd HH:mm");
                            itemModel.GroupType = item.group_type;
                            itemModel.IsMember = item.is_member;
                            if (!bll.Update(itemModel))
                            {
                                tran.Rollback();
                                apiResp.msg = "操作失败";
                                bll.ContextResponse(context, apiResp);
                                return;
                            }

                        }
                        #endregion

                    }
                
                }

             


                tran.Commit();
                apiResp.status = true;

            }
            catch (Exception ex)
            {
                tran.Rollback();
                apiResp.msg = "操作失败";
                bll.ContextResponse(context, apiResp);
                return;

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
            /// 详细
            /// </summary>
            public string description { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int is_need_pay { get; set; }
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
            /// <summary>
            /// 
            /// </summary>
            public string from_date { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string to_date { get; set; }
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