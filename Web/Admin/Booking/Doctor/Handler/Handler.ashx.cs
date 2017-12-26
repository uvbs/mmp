using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.JubitIMP.Web.Serv;

namespace ZentCloud.JubitIMP.Web.Admin.Booking.Doctor.Handler
{
    /// <summary>
    /// 医生预约处理文件
    /// </summary>
    public class Handler : BaseHandlerNeedLoginAdmin
    {
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();

        #region 医生模块
        /// <summary>
        /// 医生列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DoctorList(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string keyWord = context.Request["keyWord"];
            string categoryId = context.Request["CategoryId"];
            string isOnSale = context.Request["IsOnSale"];
            string type = context.Request["type"];
            StringBuilder sbWhere = new StringBuilder(string.Format("WebsiteOwner='{0}'  And IsDelete=0  ", bllMall.WebsiteOwner));
            if (!string.IsNullOrEmpty(type))
            {
                sbWhere.AppendFormat("And ArticleCategoryType='{0}'", type);
            }
            else
            {
                sbWhere.AppendFormat("And ArticleCategoryType='BookingDoctor'");
            }
            if (!string.IsNullOrEmpty(keyWord))
            {
                sbWhere.AppendFormat(" And (PName like '%{0}%' Or ExArticleTitle_2 like '%{0}%' Or ExArticleTitle_4 like '%{0}%')", keyWord);
            }
            if (!string.IsNullOrEmpty(categoryId) && (!categoryId.Equals("0")))
            {
                string categoryIds = bllMall.GetCateAndChildIds(Convert.ToInt32(categoryId));
                sbWhere.AppendFormat(" And CategoryId in ({0})", categoryIds);

            }




            int totalCount = bllMall.GetCount<WXMallProductInfo>(sbWhere.ToString());
            List<WXMallProductInfo> dataList = new List<WXMallProductInfo>();
            dataList = bllMall.GetLit<WXMallProductInfo>(pageSize, pageIndex, sbWhere.ToString(), "Sort DESC, InsertDate DESC");
            for (int i = 0; i < dataList.Count; i++)
            {
                dataList[i].PDescription = null;

            }
            return Common.JSONHelper.ObjectToJson(
    new
    {
        total = totalCount,
        rows = dataList
    });



        }

        /// <summary>
        /// 添加编辑医生
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DoctorAddEdit(HttpContext context)
        {
            WXMallProductInfo model = bllMall.ConvertRequestToModel<WXMallProductInfo>(new WXMallProductInfo());
            if (model.PID == "0")//添加
            {
                model.PID = bllMall.GetGUID(BLLJIMP.TransacType.AddWXMallProductID);
                model.WebsiteOwner = bllMall.WebsiteOwner;
                model.UserID = currentUserInfo.UserID;
                model.InsertDate = DateTime.Now;
                if (model.ArticleCategoryType == "BookingDoctorFuYou")
                {
                    model.ArticleCategoryType = "BookingDoctorFuYou";
                }
                else
                {
                    model.ArticleCategoryType = "BookingDoctor";
                }

                if (bllMall.Add(model))
                {
                    apiResp.status = true;
                    apiResp.msg = "添加成功";
                }
                else
                {
                    apiResp.msg = "添加失败";
                }
            }
            else//编辑
            {
                WXMallProductInfo productInfo = bllMall.GetProduct(model.PID);
                productInfo.PName = model.PName;
                productInfo.RecommendImg = model.RecommendImg;
                productInfo.Stock = model.Stock;
                productInfo.ExArticleTitle_1 = model.ExArticleTitle_1;
                productInfo.ExArticleTitle_2 = model.ExArticleTitle_2;
                productInfo.ExArticleTitle_3 = model.ExArticleTitle_3;
                productInfo.ExArticleTitle_4 = model.ExArticleTitle_4;
                productInfo.Sort = model.Sort;
                productInfo.CategoryId = model.CategoryId;
                productInfo.SaleCount = model.SaleCount;
                productInfo.PDescription = model.PDescription;
                productInfo.ShowImage1 = model.ShowImage1;
                productInfo.Summary = model.Summary;
                productInfo.IsOnSale = model.IsOnSale;
                productInfo.Tags = model.Tags;
                if (bllMall.Update(productInfo))
                {
                    apiResp.status = true;
                    apiResp.msg = "修改成功";
                }
                else
                {
                    apiResp.msg = "修改失败";
                }

            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);

        }

        /// <summary>
        /// 删除医生
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DoctorDelete(HttpContext context)
        {
            string ids = context.Request["ids"];
            int count = bllMall.Update(new WXMallProductInfo(), "IsDelete=1", string.Format(" WebsiteOwner='{0}' And PID in({1}) ", bllMall.WebsiteOwner, ids));
            apiResp.status = true;
            apiResp.result = count;
            return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);

        }
        /// <summary>
        /// 修改医生排序号
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateDoctorSortIndex(HttpContext context)
        {
            string id = context.Request["id"];
            string sort = context.Request["sort"];
            int count = bllMall.Update(new WXMallProductInfo(), string.Format(" Sort={0}", sort), string.Format(" WebsiteOwner='{0}' And PID ='{1}'", bllMall.WebsiteOwner, id));
            if (count > 0)
            {
                apiResp.status = true;

            }
            apiResp.result = count;
            return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);

        }
        /// <summary>
        /// 修改医生可预约数量
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateDoctorStock(HttpContext context)
        {
            string id = context.Request["id"];
            string stock = context.Request["stock"];
            int count = bllMall.Update(new WXMallProductInfo(), string.Format(" Stock={0}", stock), string.Format(" WebsiteOwner='{0}' And PID ='{1}' ", bllMall.WebsiteOwner, id));
            if (count > 0)
            {
                apiResp.status = true;

            }
            apiResp.result = count;
            return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);

        }


        #endregion


        #region 订单管理
        /// <summary>
        /// 订单列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string OrderList(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string keyWord = context.Request["KeyWord"];
            string fromDate = context.Request["FromDate"];
            string toDate = context.Request["ToDate"];
            string type = context.Request["type"];
            string orderType = context.Request["OrderType"];
            string status = context.Request["status"];
            StringBuilder sbWhere = new StringBuilder(string.Format(" WebsiteOwner='{0}' ", bllMall.WebsiteOwner));

            if (string.IsNullOrEmpty(orderType))
            {

                sbWhere.AppendFormat(" And OrderType in(5,6) ");
            }
            else
            {
                sbWhere.AppendFormat(" And OrderType in(7,8) ");
            }
            if (!string.IsNullOrEmpty(type))
            {
                sbWhere.AppendFormat("And OrderType={0}", type);
            }
            if (!string.IsNullOrEmpty(status))
            {
                sbWhere.AppendFormat("And Status='{0}'", status);
            }
            if (!string.IsNullOrEmpty(keyWord))
            {
                sbWhere.AppendFormat("And ( Consignee like'%{0}%' Or Ex5 like'%{0}%' Or Ex6  like'%{0}%')", keyWord);
            }
            if ((!string.IsNullOrEmpty(fromDate)))//大于开始时间
            {
                sbWhere.AppendFormat("And InsertDate>='{0}'", Convert.ToDateTime(fromDate));
            }
            if ((!string.IsNullOrEmpty(toDate)))//小于结束时间
            {
                sbWhere.AppendFormat("And InsertDate<'{0}'", Convert.ToDateTime(toDate).AddDays(1));
            }
            int totalCount = bllMall.GetCount<WXMallOrderInfo>(sbWhere.ToString());
            List<WXMallOrderInfo> dataList = new List<WXMallOrderInfo>();
            dataList = bllMall.GetLit<WXMallOrderInfo>(pageSize, pageIndex, sbWhere.ToString(), "InsertDate DESC");
            return Common.JSONHelper.ObjectToJson(
                new
                {
                    total = totalCount,
                    rows = dataList
                });

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateOrderStatus(HttpContext context)
        {
            string ids = context.Request["ids"];
            ids = "'" + ids.Replace(",", "','") + "'";
            string status = context.Request["status"];
            if (bllMall.Update(new WXMallOrderInfo(), string.Format(" Status='{0}'", status), string.Format(" OrderId in({0})", ids)) > 0)
            {
                apiResp.status = true;
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);

        }

        #endregion

        #region 预约字段管理
        /// <summary>
        /// 字段列表 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string FieldList(HttpContext context)
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat("  WebsiteOwner='{0}' And TableName ='ZCJ_WXMallOrderInfo' Order by Sort DESC", bllMall.WebsiteOwner);
            var list = bllMall.GetList<TableFieldMapping>(sbWhere.ToString());
            return ZentCloud.Common.JSONHelper.ObjectToJson(
               new
               {
                   total = list.Count,
                   rows = list

               });


        }
        /// <summary>
        /// 添加预约字段
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string FieldAdd(HttpContext context)
        {
            string field = context.Request["Field"];
            string fieldShowName = context.Request["FieldShowName"];
            string isNull = context.Request["IsNull"];
            string sort = context.Request["Sort"];
            string fieldType = context.Request["FieldType"];
            string options = context.Request["Options"];
            string isMultiline = context.Request["IsMultiline"];

            TableFieldMapping model = new TableFieldMapping();
            model.TableName = "ZCJ_WXMallOrderInfo";
            model.WebSiteOwner = bllMall.WebsiteOwner;
            model.Field = field;
            model.MappingName = fieldShowName;
            model.FieldType = fieldType;
            model.Options = options;
            model.IsMultiline = int.Parse(isMultiline);

            if (!string.IsNullOrEmpty(isNull))
            {
                model.FieldIsNull = int.Parse(isNull);

            }
            if (!string.IsNullOrEmpty(sort))
            {
                model.Sort = int.Parse(sort);

            }
            if (bllMall.Add(model))
            {
                apiResp.status = true;
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);

        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string FieldUpdate(HttpContext context)
        {
            string field = context.Request["Field"];
            string fieldShowName = context.Request["FieldShowName"];
            string autoId = context.Request["AutoId"];
            string isNull = context.Request["IsNull"];
            string sort = context.Request["Sort"];
            string fieldType = context.Request["FieldType"];
            string options = context.Request["Options"];
            string isMultiline = context.Request["IsMultiline"];
            TableFieldMapping model = bllMall.Get<TableFieldMapping>(string.Format("AutoId={0}", autoId));
            model.Field = field;
            model.MappingName = fieldShowName;
            model.FieldType = fieldType;
            model.Options = options;
            model.IsMultiline = int.Parse(isMultiline);
            if (!string.IsNullOrEmpty(isNull))
            {
                model.FieldIsNull = int.Parse(isNull);

            }
            if (!string.IsNullOrEmpty(sort))
            {
                model.Sort = int.Parse(sort);

            }
            if (bllMall.Update(model))
            {
                apiResp.status = true;
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);

        }




        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string FieldDelete(HttpContext context)
        {
            string autoIds = context.Request["autoIds"];
            if (bllMall.Delete(new TableFieldMapping(), string.Format(" AutoId in({0})", autoIds)) > 0)
            {
                apiResp.status = true;
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);

        }



        #endregion

    }
}