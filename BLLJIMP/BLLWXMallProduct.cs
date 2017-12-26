using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP
{
    public class BLLWXMallProduct : BLL
    {

        /// <summary>
        /// 执行价格区间存储过程结果对象，只有该bll用
        /// </summary>
        public class PriceRangeResult : ZCBLLEngine.ModelTable
        {
            public decimal? Max { get; set; }
            public decimal? Min { get; set; }
        }

        /// <summary>
        /// 获取指定商品价格区间
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public PriceRangeResult GetProductPriceRange(string productId,string websiteOwner)
        {
            PriceRangeResult result = new PriceRangeResult();

            var query = Query<PriceRangeResult>(string.Format(" EXEC Proc_GetProductPriceRange '{0}','{1}' ", productId, websiteOwner));

            if (query != null && query.Count > 0)
            {
                result = query[0];
            }

            return result;
        }

        /// <summary>
        /// 获取指定分类价格区间
        /// </summary>
        /// <param name="cateId"></param>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public PriceRangeResult GetMallCatePriceRange(int cateId, string websiteOwner)
        {
            PriceRangeResult result = new PriceRangeResult();

            var query = Query<PriceRangeResult>(string.Format(" EXEC Proc_GetMallCatePriceRange {0},'{1}' ", cateId, websiteOwner));

            if (query != null && query.Count > 0)
            {
                result = query[0];
            }

            return result;
        }


        /// <summary>
        /// 更新商品价格区间（商品及商品相关的分类）
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public bool UpdateProductPriceRange(string productId, string websiteOwner)
        {
            if (websiteOwner!="jikuwifi")
            {
                return false;
            }
            bool result = true;

            //取出商品所属分类以及所有上级分类
            var product = GetProductInfo(productId);

            if (product == null) return false;

            //查询商品的价格区间并更新赋值
            var productPriceRange = GetProductPriceRange(productId, websiteOwner);

            Update(product, string.Format(" MaxPrice={0},MinPrice={1} ",
                    productPriceRange.Max == null ? 0 : productPriceRange.Max.Value,
                    productPriceRange.Min == null ? 0 : productPriceRange.Min.Value
                ), string.Format(" PID = '{0}' ", productId));

            int cateId = 0;

            if (Int32.TryParse(product.CategoryId, out cateId))
            {
                UpdateCateProductPriceRange(cateId, websiteOwner);
            }

            return result;
        }

        /// <summary>
        /// 更新分类的商品价格区间
        /// </summary>
        /// <param name="cateId"></param>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public bool UpdateCateProductPriceRange(int cateId,string websiteOwner)
        {
            if (websiteOwner!="jikuwifi")
            {
                return false;

            }
            bool result = true;

            var cateList = GetCateAndAllUpLevelCate(cateId);

            if (cateList != null)
            {
                foreach (var item in cateList)
                {
                    //查询分类的价格区间并更新赋值
                    var catePriceRange = GetMallCatePriceRange(item.AutoID, websiteOwner);

                    Update(item, string.Format(" MaxPrice={0},MinPrice={1} ",
                        catePriceRange.Max == null ? 0 : catePriceRange.Max.Value,
                        catePriceRange.Min == null ? 0 : catePriceRange.Min.Value
                    ), string.Format(" AutoID = {0} ", item.AutoID));

                }
            }

            return result;
        }

        /// <summary>
        /// 获取指定分类和所有上级分类
        /// </summary>
        /// <param name="cateId"></param>
        /// <returns></returns>
        public List<Model.WXMallCategory> GetCateAndAllUpLevelCate(int cateId)
        {
            List<Model.WXMallCategory> result = new List<Model.WXMallCategory>();

            var cate = Get<Model.WXMallCategory>(string.Format(" AutoID = {0} ",cateId));

            if (cate != null)
            {
                result.Add(cate);
                if (cate.PreID != 0)
                {
                    var list = GetCateAndAllUpLevelCate(cate.PreID);
                    if (list.Count > 0)
                    {
                        result.AddRange(GetCateAndAllUpLevelCate(cate.PreID));
                    }
                }
            }

            return result;
        }

        public Model.WXMallProductInfo GetProductInfo(string productId)
        {
            return Get<Model.WXMallProductInfo>(string.Format(" PID = '{0}' ", productId));
        }

    }
}
