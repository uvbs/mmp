using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.Slide
{
    /// <summary>
    /// 批量添加
    /// </summary>
    public class AddBatch : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        BLLJIMP.BLLKeyValueData bllKeyValue = new BLLJIMP.BLLKeyValueData();
        public void ProcessRequest(HttpContext context)
        {
            string data = context.Request["data"];
            Model requestModel;
            try
            {
                requestModel = ZentCloud.Common.JSONHelper.JsonToModel<Model>(context.Request["data"]);
            }
            catch (Exception)
            {
                resp.errcode = -1;
                resp.errmsg = "json格式错误,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (requestModel.list == null || requestModel.list.Count <= 0)
            {
                resp.errcode = 1;
                resp.errmsg = "数据不能为空";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }

            foreach (ListModel item in requestModel.list)
            {
                bllKeyValue.AddSlideProportion(item.slide_type, item.proportion);
                foreach (RequestModel model in item.datalist)
                {
                    if (string.IsNullOrEmpty(model.img_url))
                    {
                        resp.errcode = -1;
                        resp.errmsg = "img_url 为必填项,请检查";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                        return;
                    }
                    //if (string.IsNullOrEmpty(model.link))
                    //{
                    //    resp.errcode = -1;
                    //    resp.errmsg = "link 为必填项,请检查";
                    //    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    //    return;
                    //}
                    if (string.IsNullOrEmpty(item.slide_type))
                    {
                        resp.errcode = -1;
                        resp.errmsg = "slide_type 为必填项,请检查";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                        return;
                    }


                    ZentCloud.BLLJIMP.Model.Slide slide = new BLLJIMP.Model.Slide();
                    slide.ImageUrl = model.img_url;
                    slide.Link = model.link;
                    slide.LinkText = model.link_text;
                    slide.Type = item.slide_type;
                    slide.Sort = model.slide_sort;
                    slide.Width = model.width;
                    slide.Height = model.height;
                    slide.WebsiteOwner = bll.WebsiteOwner;
                    slide.IsPc = !string.IsNullOrEmpty(item.is_pc) ? int.Parse(item.is_pc) : 0;
                    if (slide.Link.Length>500)
                    {
                        resp.errcode = -1;
                        resp.errmsg = "最大长度为500";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                        return;
                    }
                    if (slide.LinkText.Length>50)
                    {
                        resp.errcode = -1;
                        resp.errmsg = "最大长度为50";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                        return;
                    }
                    if (slide.Type.Length > 50)
                    {
                        resp.errcode = -1;
                        resp.errmsg = "最大长度为50";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                        return;
                    }
                    if (!bll.Add(slide))
                    {
                        resp.errmsg = "添加出错";
                        resp.errcode = 1;
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                        return;
                    }
                }

                



            }
            resp.isSuccess = true;
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));



        }


        /// <summary>
        /// 列表模型
        /// </summary>
        public class Model
        {
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
            /// 是否是pc端
            /// </summary>
            public string is_pc { get; set; }
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
            ///  播放顺序 从小到大
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