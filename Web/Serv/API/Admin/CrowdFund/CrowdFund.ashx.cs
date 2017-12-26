using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.CrowdFund
{
    /// <summary>
    ///众筹
    /// </summary>
    public class CrowdFund : BaseHandlerNeedLoginAdmin
    {

        /// <summary>
        /// BLL
        /// </summary>
        BLLJIMP.BLL bll = new BLLJIMP.BLL();

        /// <summary>
        /// 获取众筹活动列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string List(HttpContext context)
        {

            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            string keyWord = context.Request["keyword"];
            string type = context.Request["crowdfund_type"];
            System.Text.StringBuilder sbWhere = new System.Text.StringBuilder(string.Format(" WebSiteOwner='{0}'", bll.WebsiteOwner));
            if (!string.IsNullOrEmpty(keyWord))
            {
                sbWhere.AppendFormat(" And Title like '%{0}%'", keyWord);
            }
            if (!string.IsNullOrEmpty(type))
            {
                sbWhere.AppendFormat(" And Type ={0}", type);
            }
            int totalCount = bll.GetCount<CrowdFundInfo>(sbWhere.ToString());
            var sourceData = bll.GetLit<CrowdFundInfo>(pageSize, pageIndex, sbWhere.ToString(), " AutoID DESC");
            var list = from p in sourceData
                       select new
                       {
                           crowdfund_id = p.CrowdFundID,
                           crowdfund_type = p.Type,
                           crowdfund_title = p.Title,
                           crowdfund_img_url = p.CoverImage,
                           crowdfund_pay_amount = p.TotalPayAmount,
                           crowdfund_pay_count = p.PayPersionCount,
                           crowdfund_percent = p.PayPercent,
                           crowdfund_amount = p.FinancAmount,
                           crowdfund_stoptime=bll.GetTimeStamp(p.StopTime),
                           crowdfund_status=p.Status

                       };

            var data = new
            {
                totalcount = totalCount,
                list = list,//列表

            };
            return ZentCloud.Common.JSONHelper.ObjectToJson(data);

        }

        /// <summary>
        /// 获取众筹活动详细信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Get(HttpContext context)
        {
            string crowdfundId = context.Request["crowdfund_id"];
            if (string.IsNullOrEmpty(crowdfundId))
            {
                resp.errcode = 1;
                resp.errmsg = "crowdfund_id 参数不能为空";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            var crowdFundInfo = bll.Get<CrowdFundInfo>(string.Format("CrowdFundID={0}", crowdfundId));
            var sourceItemList = bll.GetList<CrowdFundItem>(string.Format("CrowdFundID={0}", crowdfundId));
            var itemList = from p in sourceItemList
                           select new
                           {
                               item_id = p.ItemId,
                               item_amount = p.Amount,
                               item_desc = p.Description,
                               item_productname = p.ProductName,
                               item_order_count=bll.GetCount<CrowdFundRecord>(string.Format(" ItemId={0}",p.ItemId))
                           };

            var data = new
            {
                crowdfund_id = crowdFundInfo.CrowdFundID,
                crowdfund_type = crowdFundInfo.Type,
                crowdfund_title = crowdFundInfo.Title,
                crowdfund_img_url = crowdFundInfo.CoverImage,
                crowdfund_amount =crowdFundInfo.FinancAmount,
                crowdfund_pay_amount = crowdFundInfo.TotalPayAmount,
                crowdfund_pay_count = crowdFundInfo.PayPersionCount,
                crowdfund_percent = crowdFundInfo.PayPercent,
                crowdfund_intro = crowdFundInfo.Introduction,
                crowdfund_originator=crowdFundInfo.Originator,
                crowdfund_stoptime=bll.GetTimeStamp(crowdFundInfo.StopTime),
                item_list = itemList

            };
            return ZentCloud.Common.JSONHelper.ObjectToJson(data);

        }


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Add(HttpContext context)
        {
            RequestModel requestModel;
            try
            {
                requestModel = ZentCloud.Common.JSONHelper.JsonToModel<RequestModel>(context.Request["data"]);
            }
            catch (Exception)
            {

                resp.errcode = -1;
                resp.errmsg = "json格式错误,请检查";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }

            //检查必填项

            if (string.IsNullOrEmpty(requestModel.crowdfund_title))
            {
                resp.errcode = 1;
                resp.errmsg = "crowdfund_title 参数必填";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (requestModel.crowdfund_stoptime <= 0)
            {
                resp.errcode = 1;
                resp.errmsg = "crowdfund_stoptime 参数必填";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrEmpty(requestModel.crowdfund_img_url))
            {
                resp.errcode = 1;
                resp.errmsg = "crowdfund_img_url 参数必填";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (requestModel.crowdfund_amount <= 0)
            {
                resp.errcode = 1;
                resp.errmsg = "crowdfund_amount 参数须大于0";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (requestModel.crowdfund_stoptime <= 0)
            {
                resp.errcode = 1;
                resp.errmsg = "crowdfund_stoptime 参数必填";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrEmpty(requestModel.crowdfund_intro))
            {
                resp.errcode = 1;
                resp.errmsg = "crowdfund_intro 参数必填";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrEmpty(requestModel.crowdfund_originator))
            {
                resp.errcode = 1;
                resp.errmsg = "crowdfund_originator 参数必填";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (requestModel.item_list == null || requestModel.item_list.Count == 0)
            {
                resp.errcode = -1;
                resp.errmsg = "item_list 元素个数需大于0";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            try
            {
                CrowdFundInfo crowdFundInfo = new CrowdFundInfo();
                crowdFundInfo.CrowdFundID = int.Parse(bll.GetGUID(BLLJIMP.TransacType.CommAdd));
                crowdFundInfo.Type = requestModel.crowdfund_type;
                crowdFundInfo.Title = requestModel.crowdfund_title;
                crowdFundInfo.CoverImage = requestModel.crowdfund_img_url;
                crowdFundInfo.FinancAmount = requestModel.crowdfund_amount;
                crowdFundInfo.Introduction = requestModel.crowdfund_intro;
                crowdFundInfo.WebSiteOwner = bll.WebsiteOwner;
                crowdFundInfo.StopTime = bll.GetTime(requestModel.crowdfund_stoptime);
                crowdFundInfo.Originator = requestModel.crowdfund_originator;
                crowdFundInfo.Status = 1;
                if (!bll.Add(crowdFundInfo, tran))
                {
                    tran.Rollback();
                    resp.errcode = 1;
                    resp.errmsg = "操作失败";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

                }
                foreach (var item in requestModel.item_list)
                {
                    CrowdFundItem model = new CrowdFundItem();
                    model.Amount = item.item_amount;
                    model.CrowdFundID = crowdFundInfo.CrowdFundID;
                    model.Description = item.item_desc;
                    model.ItemId = int.Parse(bll.GetGUID(BLLJIMP.TransacType.CommAdd));
                    model.WebsiteOwner = bll.WebsiteOwner;
                    model.ProductName = item.item_productname;
                    if (!bll.Add(model, tran))
                    {
                        tran.Rollback();
                        resp.errcode = 1;
                        resp.errmsg = "操作失败";
                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                    }


                }
                tran.Commit();
                resp.errmsg = "ok";


            }
            catch (Exception ex)
            {
                tran.Rollback();
                resp.errcode = -1;
                resp.errmsg = ex.Message;

            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Update(HttpContext context)
        {
            RequestModel requestModel;
            try
            {
                requestModel = ZentCloud.Common.JSONHelper.JsonToModel<RequestModel>(context.Request["data"]);
            }
            catch (Exception)
            {

                resp.errcode = -1;
                resp.errmsg = "json格式错误,请检查";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }

            //检查必填项
            if (string.IsNullOrEmpty(requestModel.crowdfund_title))
            {
                resp.errcode = 1;
                resp.errmsg = "crowdfund_title 参数必填";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrEmpty(requestModel.crowdfund_img_url))
            {
                resp.errcode = 1;
                resp.errmsg = "crowdfund_img_url 参数必填";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (requestModel.crowdfund_amount <= 0)
            {
                resp.errcode = 1;
                resp.errmsg = "crowdfund_amount 参数须大于0";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrEmpty(requestModel.crowdfund_intro))
            {
                resp.errcode = 1;
                resp.errmsg = "crowdfund_intro 参数必填";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (requestModel.crowdfund_stoptime<=0)
            {
                resp.errcode = 1;
                resp.errmsg = "crowdfund_stoptime 参数必填";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (requestModel.item_list == null || requestModel.item_list.Count == 0)
            {
                resp.errcode = -1;
                resp.errmsg = "item_list 元素个数需大于0";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }

            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            try
            {
                CrowdFundInfo crowdFundInfo = bll.Get<CrowdFundInfo>(string.Format(" CrowdFundID={0} And WebSiteOwner='{1}'", requestModel.crowdfund_id, bll.WebsiteOwner));
                if (crowdFundInfo == null)
                {
                    resp.errcode = 1;
                    resp.errmsg = "众筹活动不存在";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }

                var itemList = bll.GetList<CrowdFundItem>(string.Format(" CrowdFundID={0}",crowdFundInfo.CrowdFundID));
                var delItemIdList = new List<int>();
                if (itemList.Count!=requestModel.item_list.Where(p=>p.item_id>0).Count())
                {
                    //有删除的item 检查是否可以删除
                    var delItemList = from req in itemList
                                     where !(from old in requestModel.item_list
                                             select old.item_id).Contains(req.ItemId)
                                     select req;
                    if (delItemList.Count()>0)
                    {

                        foreach (var item in delItemList)
                        {
                            
                            if (bll.GetCount<CrowdFundRecord>(string.Format(" ItemId={0}",item.ItemId))>0)
                            {
                                resp.errcode = 1;
                                resp.errmsg =string.Format("{0}已经有下单记录，不能删除。",item.ProductName);
                                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

                            }
                            delItemIdList.Add(item.ItemId);
                        }
                        
                    }




                }




                crowdFundInfo.Type = requestModel.crowdfund_type;
                crowdFundInfo.Title = requestModel.crowdfund_title;
                crowdFundInfo.CoverImage = requestModel.crowdfund_img_url;
                crowdFundInfo.FinancAmount = requestModel.crowdfund_amount;
                crowdFundInfo.Introduction = requestModel.crowdfund_intro;
                crowdFundInfo.Originator = requestModel.crowdfund_originator;
                crowdFundInfo.StopTime = bll.GetTime(requestModel.crowdfund_stoptime);
                if (!bll.Update(crowdFundInfo, tran))
                {
                    tran.Rollback();
                    resp.errcode = 1;
                    resp.errmsg = "操作失败";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

                }
                foreach (var item in requestModel.item_list)
                {
                    CrowdFundItem model;
                    if (item.item_id==0)
                    {
                        model = new CrowdFundItem();
                        model.Amount = item.item_amount;
                        model.CrowdFundID = crowdFundInfo.CrowdFundID;
                        model.Description = item.item_desc;
                        model.ItemId = int.Parse(bll.GetGUID(BLLJIMP.TransacType.CommAdd));
                        model.WebsiteOwner = bll.WebsiteOwner;
                        model.ProductName = item.item_productname;
                        if (!bll.Add(model, tran))
                        {
                            tran.Rollback();
                            resp.errcode = 1;
                            resp.errmsg = "操作失败";
                            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                        }
                    }
                    else
                    {
                        model = bll.Get<CrowdFundItem>(string.Format(" ItemId={0} And WebSiteOwner='{1}'", item.item_id, bll.WebsiteOwner));
                        if (model == null)
                        {
                            tran.Rollback();
                            resp.errcode = 1;
                            resp.errmsg = " item 不存在";
                            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                        }
                        model.Amount = item.item_amount;
                        model.Description = item.item_desc;
                        model.ProductName = item.item_productname;
                        if (!bll.Update(model, tran))
                        {
                            tran.Rollback();
                            resp.errcode = 1;
                            resp.errmsg = "操作失败";
                            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                        }
                    }




                }
                tran.Commit();
                if (delItemIdList.Count > 0)
                {
                    foreach (var itemId in delItemIdList)
                    {
                        if (bll.Delete(new CrowdFundItem(), string.Format(" ItemId={0}", itemId)) <= 0)
                        {
                            resp.errcode = 1;
                            resp.errmsg = "删除旧选项失败";
                            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                        }
                    }
                }
                resp.errmsg = "ok";


            }
            catch (Exception ex)
            {
                tran.Rollback();
                resp.errcode = -1;
                resp.errmsg = ex.Message;

            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 删除公司信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Delete(HttpContext context)
        {
            string crowdfundIds = context.Request["crowdfund_ids"];
            if (string.IsNullOrEmpty(crowdfundIds))
            {
                resp.errcode = -1;
                resp.errmsg = "crowdfund_ids 参数不能为空";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }

            var orderCount = bll.GetCount<CrowdFundRecord>(string.Format(" CrowdFundID in ({0})", crowdfundIds));
            if (orderCount > 0)
            {
                resp.errmsg = "已存在订单,不能删除";
                resp.errcode = 1;
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            string sql = string.Format("Delete from ZCJ_CrowdFundInfo where WebSiteOwner='{0}' And  CrowdFundID in({1});",bll.WebsiteOwner,crowdfundIds);
            sql += string.Format("Delete from ZCJ_CrowdFundItem where WebSiteOwner='{0}' And  CrowdFundID in({1});",bll.WebsiteOwner,crowdfundIds);

            var excResult = ZentCloud.ZCBLLEngine.BLLBase.ExecuteSql(sql);
            if (excResult>0)
            {
                resp.errmsg = "ok";
                
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "删除失败";
            }

            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }
        /// <summary>
        /// 请求模型
        /// </summary>
        public class RequestModel
        {
            /// <summary>
            /// 众筹编号
            /// </summary>
            public int crowdfund_id { get; set; }
            /// <summary>
            /// 产品类型
            /// 0 产品众筹
            /// 1 股权众筹
            /// 2 公益众筹
            /// </summary>
            public int crowdfund_type { get; set; }
            /// <summary>
            /// 众筹名称
            /// </summary>
            public string crowdfund_title { get; set; }
            /// <summary>
            /// 众筹图片
            /// </summary>
            public string crowdfund_img_url { get; set; }
            /// <summary>
            /// 总共需要筹集金额
            /// </summary>
            public decimal crowdfund_amount { get; set; }
            /// <summary>
            /// 详细介绍
            /// </summary>
            public string crowdfund_intro { get; set; }
            /// <summary>
            /// 众筹选项列表
            /// </summary>
            public List<ItemModel> item_list { get; set; }

            /// <summary>
            /// 发起人
            /// </summary>
            public string crowdfund_originator { get; set; }
            /// <summary>
            /// 结束时间
            /// </summary>
            public long crowdfund_stoptime { get; set; }

            /// <summary>
            /// 众筹状态
            /// </summary>
            public int corwdfund_status { get; set; }

        }

        /// <summary>
        /// 众筹选项模型
        /// </summary>
        public class ItemModel
        {
            /// <summary>
            /// 选项Id
            /// </summary>
            public int item_id { get; set; }
            /// <summary>
            /// 选项金额
            /// </summary>
            public decimal item_amount { get; set; }
            /// <summary>
            /// 选项说明
            /// </summary>
            public string item_desc { get; set; }
            /// <summary>
            /// 商品名称
            /// </summary>
            public string item_productname { get; set; }
        }


    }
}