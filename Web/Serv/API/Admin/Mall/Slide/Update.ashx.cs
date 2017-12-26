using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.Slide
{
    /// <summary>
    /// 更新 无用
    /// </summary>
    public class Update : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string data = context.Request["data"];
            RequestModel requestModel;
            try
            {
                requestModel = ZentCloud.Common.JSONHelper.JsonToModel<RequestModel>(context.Request["data"]);
            }
            catch (Exception)
            {

                resp.errcode = -1;
                resp.errmsg = "json格式错误,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (requestModel.slide_id<=0)
            {
                resp.errcode = -1;
                resp.errmsg = "slide_id 为必填项,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.img_url))
            {
                resp.errcode = -1;
                resp.errmsg = "img_url 为必填项,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.link))
            {
                resp.errcode = -1;
                resp.errmsg = "link 为必填项,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.link_text))
            {
                resp.errcode = -1;
                resp.errmsg = "link_text 为必填项,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.slide_type))
            {
                resp.errcode = -1;
                resp.errmsg = "slide_type 为必填项,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            ZentCloud.BLLJIMP.Model.Slide slide = bll.Get<ZentCloud.BLLJIMP.Model.Slide>(string.Format(" WebsiteOwner='{0}' AND AutoID={1} ",bll.WebsiteOwner,requestModel.slide_id));
            if (slide == null)
            {
                resp.errmsg = "没有找到记录";
                resp.errcode = 1;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            slide.ImageUrl = requestModel.img_url;
            slide.Link = requestModel.link;
            slide.LinkText = requestModel.link_text;
            slide.Type = requestModel.slide_type;
            slide.Sort = requestModel.slide_sort;
            slide.WebsiteOwner = bll.WebsiteOwner;
            if (bll.Update(slide))
            {
                resp.errcode = 0;
                resp.errmsg = "ok";
                resp.isSuccess = true;
            }
            else
            {
                resp.errmsg = "修改广告出错";
                resp.errcode = 1;
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }

        public class RequestModel
        {
            /// <summary>
            /// 广告id
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