using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.ZCBLLEngine;
using ZentCloud.ZCDALEngine;
namespace ZentCloud.BLLJIMP
{
    public class BLLRedis : BLLBase
    {
        /// <summary>
        /// 重写表名
        /// </summary>
        /// <param name="modelName"></param>
        /// <returns></returns>
        protected override string GetRealTableName(string modelName)
        {
            string tableName = modelName.EndsWith("Ex", true, null) ? modelName.Substring(0, modelName.Length - 2) : modelName;

            return "ZCJ_" + tableName;
        }

        /// <summary>
        /// 获取指定站点信息 Redis
        /// </summary>
        /// <param name="websiteOwner">站点所有者</param>
        /// <returns></returns>
        public WebsiteInfo GetWebsiteInfo(string websiteOwner)
        {
            
            try
            {
                if (RedisHelper.RedisHelper.HashExists(RedisHelper.Enums.RedisKeyEnum.WebsiteInfo, websiteOwner))
                {
                    return RedisHelper.RedisHelper.HashGet<BLLJIMP.Model.WebsiteInfo>(RedisHelper.Enums.RedisKeyEnum.WebsiteInfo, websiteOwner);
                }
                else
                {
                    var websiteInfo = Get<WebsiteInfo>(string.Format("WebsiteOwner='{0}'", websiteOwner));
                    RedisHelper.RedisHelper.HashSetSerialize(RedisHelper.Enums.RedisKeyEnum.WebsiteInfo, websiteOwner, websiteInfo);
                    return websiteInfo;
                }
            }
            catch (Exception)
            {

                return Get<WebsiteInfo>(string.Format("WebsiteOwner='{0}'", websiteOwner));

            }
        }
        /// <summary>
        /// 获取指定站点域名信息 Redis
        /// </summary>
        /// <param name="doMain">域名</param>
        /// <returns></returns>
        public WebsiteDomainInfo GetWebsiteDomainInfo(string doMain)
        {
            try
            {
                if (RedisHelper.RedisHelper.HashExists(RedisHelper.Enums.RedisKeyEnum.WebsiteDomainInfo, doMain))
                {
                    return RedisHelper.RedisHelper.HashGet<BLLJIMP.Model.WebsiteDomainInfo>(RedisHelper.Enums.RedisKeyEnum.WebsiteDomainInfo, doMain);
                }
                else
                {
                    var websiteDomainInfo = Get<WebsiteDomainInfo>(string.Format("WebsiteDomain='{0}'", doMain));
                    RedisHelper.RedisHelper.HashSetSerialize(RedisHelper.Enums.RedisKeyEnum.WebsiteDomainInfo, doMain, websiteDomainInfo);
                    return websiteDomainInfo;
                }
            }
            catch (Exception)
            {

                return Get<WebsiteDomainInfo>(string.Format("WebsiteDomain='{0}'", doMain));

            }
        }


        /// <summary>
        /// 获取过滤页面列表 (权限排除,微信授权)
        /// </summary>
        /// <returns></returns>
        public List<BLLPermission.Model.ModuleFilterInfo> GetModuleFilterInfoList()
        {

            List<BLLPermission.Model.ModuleFilterInfo> pathList = new List<BLLPermission.Model.ModuleFilterInfo>();
            try
            {

                pathList = RedisHelper.RedisHelper.StringGet<List<BLLPermission.Model.ModuleFilterInfo>>(RedisHelper.Enums.RedisKeyEnum.WXModuleFilterInfo);
                if (pathList == null)
                {
                    pathList = new List<BLLPermission.Model.ModuleFilterInfo>();
                }


            }
            catch (Exception ex)
            {
            }
            if (pathList.Count == 0)
            {
                pathList = GetList<BLLPermission.Model.ModuleFilterInfo>("");
                try
                {
                    RedisHelper.RedisHelper.StringSetSerialize(RedisHelper.Enums.RedisKeyEnum.WXModuleFilterInfo, pathList);
                }
                catch (Exception ex)
                {

                }
            }

            return pathList;

        }


        //--------------------------数据清除------------------------

        /// <summary>
        /// 清除站点商品列表
        /// 后台编辑
        /// </summary>
        /// <param name="websiteOwner"></param>
        public static void ClearProductList(string websiteOwner)
        {
            var key = websiteOwner + ":" + Common.SessionKey.ShopListKeys;

            //直接模糊查询删除keys
            RedisHelper.RedisHelper.KeyBatchDelete(string.Format("{0}:PL:*", websiteOwner));

            //var dataList = RedisHelper.RedisHelper.SetMembers(key);
            //foreach (var item in dataList)
            //{
            //    RedisHelper.RedisHelper.KeyDelete(item);
            //}
            //最后删除主key
            RedisHelper.RedisHelper.KeyDelete(key);
        }
        
        /// <summary>
        /// 清除指定商品
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="productId"></param>
        /// <param name="isClearList">清除列表，有时候不需要每次更新都清除列表的可以禁止</param>
        /// <param name="isClearSkus"></param>
        public static void ClearProduct(string websiteOwner,string productId,bool isClearList = true,bool isClearSkus = true)
        {
            var key = websiteOwner + ":PD:" + productId;
            RedisHelper.RedisHelper.KeyDelete(key);
            RedisHelper.RedisHelper.KeyBatchDelete(key);

            if (isClearList)
            {
                //商品跟列表关联，同时清除商品列表
                ClearProductList(websiteOwner);
            }

            if (isClearSkus)
            {
                //同时清除商品sku
                ClearProductSkus(websiteOwner, int.Parse(productId));
            }
            
        }
        /// <summary>
        /// 清除指定商品
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="productIds">逗号分隔的商品id列表</param>
        /// <param name="isClearList">清除列表，有时候不需要每次更新都清除列表的可以禁止</param>
        /// <param name="isClearSkus"></param>
        public static void ClearProductByIds(string websiteOwner, string productIds, bool isClearList = true, bool isClearSkus = true)
        {
            foreach (var item in productIds.Split(','))
            {
                ClearProduct(websiteOwner, item, isClearList, isClearSkus);
            }
        }
        public static void ClearProductByIds(string websiteOwner, List<string> productIds, bool isClearList = true, bool isClearSkus = true)
        {
            foreach (var item in productIds)
            {
                ClearProduct(websiteOwner, item, isClearList, isClearSkus);
            }
        }

        /// <summary>
        /// 清除指定幻灯片组
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="type"></param>
        public static void ClearSlider(string websiteOwner,string type)
        {
            //清除组为空的key
            var key = string.Format("{0}:{1}:", websiteOwner, Common.SessionKey.SliderByType);
            RedisHelper.RedisHelper.KeyDelete(key);
            RedisHelper.RedisHelper.KeyBatchDelete(key);
            //清除指定组的key
            if (!string.IsNullOrWhiteSpace(type))
            {
                key += type;
                RedisHelper.RedisHelper.KeyDelete(key);
                RedisHelper.RedisHelper.KeyBatchDelete(key);
            }

        }

        /// <summary>
        /// 清除指定用户信息
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="userId"></param>
        public static void ClearUser (string websiteOwner,string userId)
        {
            var key = string.Format("{0}:User:{1}", websiteOwner, userId);
            RedisHelper.RedisHelper.KeyDelete(key);
            RedisHelper.RedisHelper.KeyBatchDelete(key);
        }

        /// <summary>
        /// 清除指定产品的sku列表
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="productId"></param>
        public static void ClearProductSkus(string websiteOwner, int productId, bool isClearSingle = true)
        {
            var key = string.Format("{0}:{1}:{2}", websiteOwner, Common.SessionKey.ProductSkus, productId);

            if (true)
            {
                //相应的每个sku信息缓存也清除，否则这些sku就变成死数据了
                var cacheDataStr = RedisHelper.RedisHelper.StringGet(key);
                if (!string.IsNullOrWhiteSpace(cacheDataStr))
                {
                    var data = JsonConvert.DeserializeObject<List<ProductSku>>(cacheDataStr);

                    foreach (var item in data)
                    {
                        ClearProductSkuSingle(websiteOwner, item.SkuId, false);
                    }

                }
            }

            RedisHelper.RedisHelper.KeyDelete(key);
            RedisHelper.RedisHelper.KeyBatchDelete(key);
        }

        /// <summary>
        /// 清除指定sku
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="skuId"></param>
        public static void ClearProductSkuSingle(string websiteOwner, int skuId, bool isClearList = true)
        {
            var key = string.Format("{0}:{1}:{2}", websiteOwner, Common.SessionKey.ProductSkuSingle, skuId);

            if (isClearList)
            {
                //需要清除相应的商品sku列表
                var cacheDataStr = RedisHelper.RedisHelper.StringGet(key);
                if (!string.IsNullOrWhiteSpace(cacheDataStr))
                {
                    var data = JsonConvert.DeserializeObject<ProductSku>(cacheDataStr);
                    ClearProductSkus(websiteOwner, data.ProductId, false);
                }
            }

            RedisHelper.RedisHelper.KeyDelete(key);
            RedisHelper.RedisHelper.KeyBatchDelete(key);
        }

        /// <summary>
        /// 清除购物车
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="userId"></param>
        public static void ClearShoppingCart(string websiteOwner,string userId)
        {
            var key = string.Format("{0}:{1}:{2}", websiteOwner, Common.SessionKey.ShoppingCart, userId);
            RedisHelper.RedisHelper.KeyDelete(key);
            RedisHelper.RedisHelper.KeyBatchDelete(key);
        }

        /// <summary>
        /// 清除评价列表
        /// </summary>
        /// <param name="websiteOwner"></param>
        public static void ClearReviewList(string websiteOwner)
        {
            var key = websiteOwner + ":" + Common.SessionKey.ReviewListKeys;
            var dataList = RedisHelper.RedisHelper.SetMembers(key);
            foreach (var item in dataList)
            {
                RedisHelper.RedisHelper.KeyDelete(item);
                RedisHelper.RedisHelper.KeyBatchDelete(item);
            }
            //最后删除主key
            RedisHelper.RedisHelper.KeyDelete(key);
            RedisHelper.RedisHelper.KeyBatchDelete(key);
        }

    }
}
