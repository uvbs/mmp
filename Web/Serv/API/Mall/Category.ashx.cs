using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Mall
{
    /// <summary>
    /// 商品分类
    /// </summary>
    public class Category : BaseHandler
    {
        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        /// 获取分类信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string List(HttpContext context)
        {

            string parentId = context.Request["parentid"];//上级分类id
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            int totalCount = 0;
            var sourceData = bllMall.GetCategoryList(pageIndex, pageSize, parentId, out totalCount);

            var list = from p in sourceData
                       select new
                       {
                           category_id = p.AutoID,
                           category_name = p.CategoryName,
                           pre_id = p.PreID,
                           description = p.Description,
                           img_url = p.CategoryImg,
                           sort = p.Sort,
                           min_price = p.MinPrice,
                           max_price = p.MaxPrice
                           //category_subcount = bllMall.GetCount<ZentCloud.BLLJIMP.Model.WXMallCategory>(string.Format(" PreID={0}", p.AutoID))// 直接子分类数量
                       };

            //构造另外一个二级列表，并获取前6个的价格范围
            List<dynamic> levelList = new List<dynamic>();

            foreach (var item in sourceData.Where(sData => sData.PreID == 0))
            {
                levelList.Add(new {
                    category_id = item.AutoID,
                    category_name = item.CategoryName,
                    pre_id = item.PreID,
                    description = item.Description,
                    img_url = item.CategoryImg,
                    sort = item.Sort,
                    min_price = item.MinPrice,
                    max_price = item.MaxPrice,
                    sub_list = sourceData.Where(sbData => sbData.PreID == item.AutoID).Select(outSbData => new {
                        category_id = outSbData.AutoID,
                        category_name = outSbData.CategoryName,
                        pre_id = outSbData.PreID,
                        description = outSbData.Description,
                        img_url = outSbData.CategoryImg,
                        sort = outSbData.Sort,
                        min_price = outSbData.MinPrice,
                        max_price = outSbData.MaxPrice,
                    })
                });
            }

            var data = new
            {
                totalcount = totalCount,
                list = list,//列表
                level_list= levelList
            };
            return ZentCloud.Common.JSONHelper.ObjectToJson(data);

        }

        /// <summary>
        /// 获取分类信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string ListAll(HttpContext context)
        {
            var data = bllMall.GetCategoryList();
            var rootData = data.Where(p => p.PreID == 0);
            List<CategoryModel> list = new List<CategoryModel>();
            foreach (var item in rootData)
            {
                var model = new CategoryModel();
                model.category_id = item.AutoID;
                model.category_name = item.CategoryName;
                model.category_img_url = item.CategoryImg;
                list.Add(GetTree(model, data, model.category_id));
            }

            return ZentCloud.Common.JSONHelper.ObjectToJson(new { 
            status=true,
            msg="ok",
            code=0,
            result=list
            
            });

        }


        /// <summary>
        /// 返回模型
        /// </summary>
        public class CategoryModel
        {
            /// <summary>
            /// 导航id
            /// </summary>
            public int category_id { get; set; }
            /// <summary>
            /// 导航名称
            /// </summary>
            public string category_name { get; set; }
            /// <summary>
            /// 导航图片地址
            /// </summary>
            public string category_img_url { get; set; }
            /// <summary>
            /// 子节点
            /// </summary>
            public List<CategoryModel> children { get; set; }


        }

        /// <summary>
        /// 递归获取子节点
        /// </summary>
        /// <param name="model"></param>
        /// <param name="list"></param>
        /// <param name="parentID"></param>
        /// <returns></returns>
        private CategoryModel GetTree(CategoryModel model, IList<BLLJIMP.Model.WXMallCategory> list, int? parentID = 0)
        {
            var query = list.Where(m => m.PreID == parentID);
            if (query.Any())
            {
                if (model.children == null)
                {
                    model.children = new List<CategoryModel>();
                }
                foreach (var item in query)
                {
                    CategoryModel child = new CategoryModel()
                    {
                        category_id = item.AutoID,
                        category_name = item.CategoryName,
                        category_img_url = bllMall.GetImgUrl(item.CategoryImg)
                    };
                    model.children.Add(child);
                    this.GetTree(child, list, item.AutoID);
                }
            }
            return model;
        }




    }
}