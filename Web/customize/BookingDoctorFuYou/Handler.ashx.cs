using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.customize.BookingDoctorFuYou
{
    /// <summary>
    /// Handler 的摘要说明
    /// </summary>
    public class Handler : ZentCloud.JubitIMP.Web.Serv.BaseHandler
    {
        /// <summary>
        /// Api响应模型
        /// </summary>
        protected BaseResponse apiResp = new BaseResponse();
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        ///用户
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        /// <summary>
        /// 微信BLL
        /// </summary>
        BLLJIMP.BLLWeixin bllWeixin = new BLLJIMP.BLLWeixin();
        /// <summary>
        /// 添加预约
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddOrder(HttpContext context)
        {
            try
            {


                string id = context.Request["id"];//多个医生id
                string[] ids = new string[] { };//多个医生id
                if (!string.IsNullOrEmpty(id))
                {
                    id = id.TrimStart(',').TrimEnd(',');
                    ids = id.Split(',');
                }
                if (ids.Length >= 1)//检查是否可以预约
                {
                    foreach (var item in ids)
                    {
                        WXMallProductInfo productInfoCheck = bllMall.GetProduct(item);
                        if (productInfoCheck != null)
                        {
                            if (productInfoCheck.Stock <= 0)
                            {
                                apiResp.msg = string.Format("专家{0}的预约已满", productInfoCheck.PName);
                                return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
                            }
                        }
                    }

                }

                WXMallProductInfo productInfo = new WXMallProductInfo();
                WXMallOrderInfo orderInfo = bllMall.ConvertRequestToModel<WXMallOrderInfo>(new WXMallOrderInfo());
                if (string.IsNullOrEmpty(orderInfo.Consignee))
                {
                    apiResp.msg = "请填写姓名";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
                }
                if (string.IsNullOrEmpty(orderInfo.Ex1))
                {
                    apiResp.msg = "请填写年龄";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
                }
                if (string.IsNullOrEmpty(orderInfo.Ex2))
                {
                    apiResp.msg = "请选择性别";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
                }
                if (string.IsNullOrEmpty(orderInfo.Phone))
                {
                    apiResp.msg = "请填写手机号";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
                }
                if (!Common.MyRegex.PhoneNumLogicJudge(orderInfo.Phone))
                {
                    apiResp.msg = "请输入正确手机号";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
                }

                StringBuilder sbWhere = new StringBuilder();
                sbWhere.AppendFormat("  WebsiteOwner='{0}' And TableName ='ZCJ_WXMallOrderInfo' Order by Sort DESC", bllMall.WebsiteOwner);
                var fieldList = bllMall.GetList<TableFieldMapping>(sbWhere.ToString());
                if (fieldList != null && fieldList.Count > 0)
                {
                    Type type = orderInfo.GetType();
                    fieldList = fieldList.Where(p => p.FieldIsNull == 0).ToList();
                    foreach (var field in fieldList)
                    {
                        System.Reflection.PropertyInfo propertyInfo = type.GetProperty(field.Field); //获取指定名称的属性
                        var value = propertyInfo.GetValue(orderInfo, null); //获取属性值
                        if (value == null || string.IsNullOrEmpty(value.ToString()))
                        {
                            switch (field.FieldType)
                            {
                                case "text":
                                    apiResp.msg = "请填写 " + field.MappingName;
                                    break;
                                case "combox"://下拉框
                                    apiResp.msg = "请选择 " + field.MappingName;
                                    break;
                                case "checkbox"://下拉框
                                    apiResp.msg = "请选择 " + field.MappingName;
                                    break;
                                default:
                                    break;
                            }

                            return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
                        }




                    }
                }



                orderInfo.OrderID = bllMall.GetGUID(BLLJIMP.TransacType.AddMallOrder);
                orderInfo.WebsiteOwner = bllMall.WebsiteOwner;
                orderInfo.InsertDate = DateTime.Now;
                orderInfo.OrderUserID = "defualt";
                orderInfo.Status = "未确认";
                if (bllMall.IsLogin)
                {
                    orderInfo.OrderUserID = bllUser.GetCurrUserID();
                }
                if (!string.IsNullOrEmpty(orderInfo.Ex6))//科系
                {
                    //推荐
                    int categoryId;
                    if (int.TryParse(orderInfo.Ex6, out categoryId))
                    {
                        WXMallCategory category = bllMall.Get<WXMallCategory>(string.Format(" AutoId={0}", categoryId));
                        if (category != null)
                        {
                            orderInfo.Ex6 = category.CategoryName;
                        }

                    }
                }
                else
                {
                    //正常预约
                    if (ids.Length == 1)
                    {
                        productInfo = bllMall.GetProduct(ids[0]);
                        if (productInfo != null)
                        {

                            if (!string.IsNullOrEmpty(productInfo.CategoryId))
                            {
                                WXMallCategory category = bllMall.Get<WXMallCategory>(string.Format(" AutoId={0}", productInfo.CategoryId));
                                if (category != null)
                                {
                                    orderInfo.Ex6 = category.CategoryName;
                                }
                            }

                        }
                    }


                }
                if (!string.IsNullOrEmpty(orderInfo.Ex5))//医生 名字或多个Id
                {
                    orderInfo.Ex5 = orderInfo.Ex5.TrimStart(',').TrimEnd(',');
                    string names = "";
                    foreach (var item in orderInfo.Ex5.Split(','))
                    {
                        int pId;
                        if (int.TryParse(item, out pId))
                        {
                            productInfo = bllMall.GetProduct(pId.ToString());
                            if (productInfo != null)
                            {
                                names += productInfo.PName + ",";
                                if (productInfo.Stock <= 0)
                                {
                                    apiResp.msg = string.Format("专家{0}的预约已满", productInfo.PName);
                                    return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
                                }
                            }

                        }
                    }
                    if (orderInfo.Ex5.Split(',').Length>= 1&&(!string.IsNullOrEmpty(names)))
                    {
                        orderInfo.Ex5 = names.TrimEnd(',');
                    }


                }

                ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
                if (!bllMall.Add(orderInfo, tran))
                {
                    apiResp.msg = "操作失败";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);

                }
                if (ids.Length > 0)
                {
                    if (bllMall.Update(productInfo, string.Format("Stock-=1,SaleCount+=1"), string.Format("PID in({0})", id)) < ids.Length)
                    {
                        tran.Rollback();
                        apiResp.msg = "操作失败";
                        return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
                    }
                }

                tran.Commit();
                apiResp.status = true;
                bllWeixin.SendTemplateMessageToKefu("有新的预约", string.Format("姓名:{0}\\n手机:{1}",orderInfo.Consignee,orderInfo.Phone));

            }
            catch (Exception ex)
            {
                apiResp.msg = ex.Message;

            }

            return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
        }

        /// <summary>
        /// 获取医生列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetDoctorList(HttpContext context)
        {
            string categoryId = context.Request["categoryId"];
            string currCategoryid = context.Request["currcategoryid"];
            string tags=context.Request["tags"];
            List<WXMallProductInfo> productList = new List<WXMallProductInfo>();
            if (!string.IsNullOrEmpty(categoryId))
            {
                string categoryIds = bllMall.GetCateAndChildIds(Convert.ToInt32(categoryId));
                currCategoryid = bllMall.GetCateAndChildIds(Convert.ToInt32(currCategoryid));
                categoryIds += ","+currCategoryid;
                productList = bllMall.GetList<WXMallProductInfo>(string.Format(" WebsiteOwner='{0}' And CategoryId in({1}) And ArticleCategoryType='BookingDoctorFuYou'  And Stock>0  And IsDelete=0", bllMall.WebsiteOwner, categoryIds));
                foreach (var item in productList)
                {
                    item.PDescription = "";
                }
            }
            //标签
            if (!string.IsNullOrEmpty(tags))
            {
                foreach (var tag in tags.Split(','))
                {
                    var data = bllMall.GetList<WXMallProductInfo>(string.Format(" WebsiteOwner='{0}'  And Stock>0 And ArticleCategoryType='BookingDoctorFuYou' And IsDelete=0 And Tags like '%{1}%'", bllMall.WebsiteOwner, tag));
                    foreach (var item in data)
                    {
                        item.PDescription = "";
                        productList.Add(item);
                    }
                   

                }


            }

            productList = productList.DistinctBy(p => p.PID).ToList();
            apiResp.status = true;
            apiResp.result = productList;
            return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);

        }
        /// <summary>
        /// 医生列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DoctorList(HttpContext context)
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            string keyWord = context.Request["keyWord"];
            System.Text.StringBuilder sbWhere = new System.Text.StringBuilder();
            sbWhere.AppendFormat(" WebsiteOwner='{0}' And ArticleCategoryType='BookingDoctorFuYou' And IsDelete=0", bllMall.WebsiteOwner);
            if (!string.IsNullOrEmpty(keyWord))
            {
                sbWhere.AppendFormat(" And (PName like '%{0}%' Or ExArticleTitle_2 like '%{0}%' Or ExArticleTitle_3 like '%{0}%' Or ExArticleTitle_4 like '%{0}%' Or Tags like '%{0}%')", keyWord);
            }
            List<WXMallProductInfo> productList = bllMall.GetLit<WXMallProductInfo>(pageSize, pageIndex, sbWhere.ToString(), "Sort DESC,InsertDate DESC");
            foreach (var item in productList)
            {
                item.PDescription = "";
            }

            apiResp.status = true;
            apiResp.result = productList;
            return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);

        }

        /// <summary>
        /// 文章列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string ArticleList(HttpContext context)
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            // string keyWord = context.Request["keyWord"];
            System.Text.StringBuilder sbWhere = new System.Text.StringBuilder();
            sbWhere.AppendFormat(" WebsiteOwner='{0}' And ArticleType='article' And IsHide=0 And IsDelete=0", bllMall.WebsiteOwner);
            List<JuActivityInfo> dataList = bllMall.GetLit<JuActivityInfo>(pageSize, pageIndex, sbWhere.ToString(), "Sort DESC,JuActivityID DESC");
            foreach (var item in dataList)
            {
                item.ActivityDescription = "";
                if (string.IsNullOrEmpty(item.Summary))
                {
                    item.Summary = "";
                }
            }
            apiResp.status = true;
            apiResp.result = dataList;
            return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);

        }

    }
}