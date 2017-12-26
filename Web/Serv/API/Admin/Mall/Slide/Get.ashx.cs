using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.Slide
{
    /// <summary>
    /// 获取单个信息
    /// </summary>
    public class Get : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL();

        public  void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string slideId = context.Request["slide_id"];
            if (string.IsNullOrEmpty(slideId))
            {
                resp.errmsg = "slide_id 为必填项,请检查";
                resp.errcode = 1;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            ZentCloud.BLLJIMP.Model.Slide slide = bll.Get<ZentCloud.BLLJIMP.Model.Slide>(string.Format(" AutoID={0} AND WebsiteOwner='{1}' ", slideId,bll.WebsiteOwner));
            if (slide == null)
            {
                resp.errmsg = "没有找到广告,请检查";
                resp.errcode = 1;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            RequestModel requestModel = new RequestModel();
            requestModel.img_url = slide.ImageUrl;
            requestModel.link = slide.Link;
            requestModel.link_text = slide.LinkText;
            requestModel.slide_sort = slide.Sort;
            requestModel.slide_type = slide.Type;
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(requestModel));
            return;
        }


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
            ///  播放顺序 从小到大
            /// </summary>
            public int slide_sort { get; set; }

            /// <summary>
            /// 类型
            /// </summary>
            public string slide_type { get; set; }
            /// <summary>
            /// 链接文字
            /// </summary>
            public string link_text { get; set; }
        }
    }
}