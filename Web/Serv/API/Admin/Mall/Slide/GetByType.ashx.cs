using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.Slide
{
    /// <summary>
    /// 根据类型获取
    /// </summary>
    public class GetByType : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLL bllMall = new BLLJIMP.BLL();
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLKeyValueData bllKeyValue = new BLLJIMP.BLLKeyValueData();
        public void ProcessRequest(HttpContext context)
        {

            string slideType = context.Request["slide_type"];
            if (string.IsNullOrEmpty(slideType))
            {
                resp.errmsg = "slide_type 为必填项,请检查";
                resp.errcode = 1;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            var sourceData = bllMall.GetList<BLLJIMP.Model.Slide>(string.Format("WebsiteOwner='{0}' And Type='{1}' order by Sort DESC", bllMall.WebsiteOwner, slideType));
            ListModel list = new ListModel();
            list.slide_type = slideType;
            list.is_pc = 0;
            list.proportion = bllKeyValue.GetSlideProportion(slideType);
            list.datalist = new List<RequestModel>();
            foreach (var item in sourceData)
            {

                RequestModel model = new RequestModel();
                model.slide_id = item.AutoID;
                model.slide_sort = item.Sort;
                model.img_url = item.ImageUrl;
                model.link = item.Link;
                model.link_text = item.LinkText;
                model.width = item.Width;
                model.height = item.Height;
                list.is_pc = item.IsPc;
                list.datalist.Add(model);


            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(list));
            return;
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
            /// 是否pc端
            /// </summary>
            public int is_pc { get; set; }
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
            /// 广告ID
            /// </summary>
            public int slide_id { get; set; }
            /// <summary>
            /// 图片路径
            /// </summary>
            public string img_url { get; set; }

            /// <summary>
            /// 跳转链接
            /// </summary>
            public string link { get; set; }
            /// <summary>
            /// 排序
            /// </summary>
            public int slide_sort { get; set; }

            /// <summary>
            /// 链接文字
            /// </summary>
            public string link_text { get; set; }
            /// <summary>
            ///  图片宽
            /// </summary>
            public int width { get; set; }
            /// <summary>
            ///  图片高
            /// </summary>
            public int height { get; set; }
        }


    }
}