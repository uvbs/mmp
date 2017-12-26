using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Reflection;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP;
using System.Text;
namespace ZentCloud.JubitIMP.Web.Serv.API.Mall
{
    /// <summary>
    /// 商品收藏
    /// </summary>
    public class Collect : BaseHandlerNeedLogin
    {
        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLMall bllMall = new BLLMall();
        /// <summary>
        /// 通用关系BLL
        /// </summary>
        BLLCommRelation bllCommRela = new BLLCommRelation();

        /// <summary>
        /// 增加商品收藏
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Add(HttpContext context)
        {
            var collectType = context.Request["type"];
            var productInfo = bllMall.GetProduct(context.Request["id"]);//商品ID
            if (productInfo == null)
            {
                resp.errcode = 1;
                resp.errmsg = "商品不存在";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            //检查是否已经收藏
            //CommRelationType.

            BLLJIMP.Enums.CommRelationType relaType = BLLJIMP.Enums.CommRelationType.ProductCollect;

            if (!string.IsNullOrEmpty(collectType))
            {
                Enum.TryParse(collectType, out relaType);
            }
            if (bllCommRela.ExistRelation(relaType, currentUserInfo.UserID, productInfo.PID))
            {
                resp.errcode = 1;
                resp.errmsg = "已经收藏过了";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            //检查是否已经收藏

            if (bllCommRela.AddCommRelation(relaType, currentUserInfo.UserID, productInfo.PID))
            {
                resp.errcode = 0;
                resp.errmsg = "收藏成功";

            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "收藏失败,请重试";
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary> 
        /// 删除商品收藏
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Delete(HttpContext context)
        {
            string collectType=context.Request["type"];
            string id=context.Request["id"];
            BLLJIMP.Enums.CommRelationType relaType = BLLJIMP.Enums.CommRelationType.ProductCollect;
            if (!string.IsNullOrEmpty(collectType))
            {
                Enum.TryParse(collectType, out relaType);
            }
            if (bllCommRela.DelCommRelation(relaType, currentUserInfo.UserID, id))
            {
                resp.errmsg = "删除成功";
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "删除失败";
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 查询商品收藏
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string List(HttpContext context)
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            var type = context.Request["type"];

            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" MainId='{0}'  ", currentUserInfo.UserID);

            if (!string.IsNullOrEmpty(type))
            {
                sbWhere.AppendFormat(" AND RelationType='{0}'",type);
            }
            else
            {
                sbWhere.AppendFormat(" AND RelationType='ProductCollect' ");
            }

            var relationData = bllCommRela.GetLit<CommRelationInfo>(pageSize, pageIndex, sbWhere.ToString(), "AutoId Desc");
            int totalCount = bllCommRela.GetCount<CommRelationInfo>(sbWhere.ToString());
            List<object> list = new List<object>();
            foreach (var item in relationData)
            {
                var productInfo = bllMall.GetProduct(item.RelationId);
                if (productInfo!=null&&productInfo.IsDelete==0)
                {
                    var collectProductInfo = new
                    {
                        product_id = int.Parse(productInfo.PID),
                        collect_time = Math.Round(bllCommRela.GetTimeStamp(item.RelationTime), 0),
                        title=productInfo.PName,
                        category_id=productInfo.CategoryId,
                        quote_price=productInfo.PreviousPrice,
                        price=productInfo.Price,
                        img_url=bllMall.GetImgUrl(productInfo.RecommendImg),
                        is_onsale=productInfo.IsOnSale,
                        product_type=productInfo.ArticleCategoryType,
                        province = productInfo.Province,
                        city = productInfo.City,
                        ex1 = productInfo.Ex1,
                        ex19 = productInfo.Ex19,
                        tags=productInfo.Tags,
                        category_name=productInfo.CategoryName
                    };
                    list.Add(collectProductInfo);

                }

            }
            var data = new
            {
                totalcount =totalCount,
                list = list

            };
            return ZentCloud.Common.JSONHelper.ObjectToJson(data);

        }


        /// <summary>
        /// 获取商品的收藏数量
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public int GetProductCollectCount(string pid,string relationType)
        {
            return bllCommRela.GetCount<CommRelationInfo>(string.Format("  RelationId='{0}' AND RelationType='{1}' ",  pid, relationType));
        }



    }
}