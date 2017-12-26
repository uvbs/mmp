using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.Slide
{
    /// <summary>
    /// 全部列表
    /// </summary>
    public class ListAll :BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall=new BLLJIMP.BLLMall();
        BLLJIMP.BLLKeyValueData bllKeyValue = new BLLJIMP.BLLKeyValueData();
        public void ProcessRequest(HttpContext context)
        {
            string keyWord=context.Request["keyword"];
            string isPc=context.Request["is_pc"];
            System.Text.StringBuilder sbWhere = new System.Text.StringBuilder();
            sbWhere.AppendFormat("WebsiteOwner='{0}'",bllUser.WebsiteOwner);
            if (!string.IsNullOrEmpty(isPc))
            {
                sbWhere.AppendFormat(" And IsPc={0}", isPc);
            }
            else
            {
                sbWhere.AppendFormat(" And ( IsPc=0 or IsPc = '' or IsPc is null )");
            }
            sbWhere.AppendFormat(" Order by Sort DESC");
            List<object> listResult = new List<object>();
            var sourceData = bllMall.GetList<BLLJIMP.Model.Slide>(sbWhere.ToString());
            if (!string.IsNullOrEmpty(keyWord))
            {
                sourceData = sourceData.Where(p => p.Type.Contains(keyWord)).ToList();
            }
            List<string> typeList = new List<string>();
            foreach (var item in sourceData)
            {
                if (!typeList.Contains(item.Type))
                {
                    typeList.Add(item.Type);
                }
            }

            Model result = new Model();
            result.totalcount = sourceData.Count;
            result.list = new List<ListModel>();

            foreach (var type in typeList)
            {
                ListModel list = new ListModel();
                list.slide_type = type;
                list.proportion = bllKeyValue.GetSlideProportion(list.slide_type);
                list.datalist = new List<RequestModel>();
                foreach (var model in sourceData.Where(p => p.Type == type))
                {

                    RequestModel modelNew = new RequestModel();
                    modelNew.img_url = model.ImageUrl;
                    modelNew.link = model.Link;
                    modelNew.link_text = model.LinkText;
                    modelNew.slide_id = model.AutoID;
                    list.datalist.Add(modelNew);


                }

                result.list.Add(list);


            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(result));

        }



        /// <summary>
        /// 列表模型
        /// </summary>
        public class Model
        {
            /// <summary>
            /// 总数量
            /// </summary>
            public int totalcount { get; set; }
            /// <summary>
            /// 列表数据模型
            /// </summary>
            public List<ListModel> list { get; set; }

        }


        /// <summary>
        /// 列表模型
        /// </summary>
        public class ListModel
        {
            /// <summary>
            /// 类型
            /// </summary>
            public string slide_type { get; set; }
            /// <summary>
            /// 比例
            /// </summary>
            public string proportion { get; set; }
            /// <summary>
            /// 列表数据模型
            /// </summary>
            public List<RequestModel> datalist { get; set; }

        }
        /// <summary>
        /// 单个广告模型
        /// </summary>
        public class RequestModel
        {
            /// <summary>
            /// 图片路径
            /// </summary>
            public string img_url { get; set; }

            /// <summary>
            /// 跳转链接
            /// </summary>
            public string link { get; set; }



            /// <summary>
            /// 链接文字
            /// </summary>
            public string link_text { get; set; }
            /// <summary>
            /// 广告ID
            /// </summary>
            public int slide_id { get; set; }
        }



    }
}