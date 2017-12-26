using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.Slide
{
    /// <summary>
    /// 根据类型更新
    /// </summary>
    public class UpdateByType : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
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
            var itemList = bllMall.GetList<BLLJIMP.Model.Slide>(string.Format("WebsiteOwner='{0}' And Type='{1}' order by Sort DESC", bllMall.WebsiteOwner, requestModel.slide_type));
            if (itemList.Count != requestModel.datalist.Where(p => p.slide_id > 0).Count())
            {
                //有删除的item 检查是否可以删除
                var delItemList = from req in itemList
                                  where !(from old in requestModel.datalist
                                          select old.slide_id).Contains(req.AutoID)
                                  select req;
                if (delItemList.Count() > 0)
                {
                    foreach (var item in delItemList)
                    {
                        if (bllMall.Delete(new ZentCloud.BLLJIMP.Model.Slide(), string.Format(" AutoID={0} And WebsiteOwner='{1}'", item.AutoID,bllMall.WebsiteOwner)) < 0)
                        {

                            resp.errcode = 1;
                            resp.errmsg = "删除旧数据失败";
                            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                            return;
                        }


                    }


                }




            }
            bllKeyValue.AddSlideProportion(requestModel.slide_type, requestModel.proportion);
            foreach (RequestModel item in requestModel.datalist)
            {
                ZentCloud.BLLJIMP.Model.Slide slide = new BLLJIMP.Model.Slide();
                if (item.slide_id == 0)
                {

                    slide.ImageUrl = item.img_url;
                    slide.Link = item.link;
                    slide.LinkText = item.link_text;
                    slide.Type = requestModel.slide_type;
                    slide.Sort = item.slide_sort;
                    slide.Width = item.width;
                    slide.Height = item.height;
                    slide.WebsiteOwner = bllMall.WebsiteOwner;
                    slide.Stext = item.s_text;
                    slide.Stype = item.s_type;
                    slide.Svalue = item.s_value;
                    slide.IsPc = !string.IsNullOrEmpty(requestModel.is_pc) ? (int.Parse(requestModel.is_pc)) : 0;
                    if (slide.Link.Length > 500)
                    {
                        resp.errcode = -1;
                        resp.errmsg = "最大长度为500";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                        return;
                    }
                    if (slide.LinkText.Length > 50)
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
                    if (!bllMall.Add(slide))
                    {
                        resp.errcode = 1;
                        resp.errmsg = "添加失败";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                        return;
                    }

                }
                else
                {
                    //更新
                    slide = bllMall.Get<ZentCloud.BLLJIMP.Model.Slide>(string.Format(" AutoID={0} And WebsiteOwner='{1}'", item.slide_id,bllMall.WebsiteOwner));
                    if (slide == null)
                    {
                        resp.errcode = 1;
                        resp.errmsg = "slide_id不存在,请检查";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                        return;
                    }

                    slide.ImageUrl = item.img_url;
                    slide.Link = item.link;
                    slide.LinkText = item.link_text;
                    slide.Width = item.width;
                    slide.Height = item.height;
                    slide.Sort = item.slide_sort;
                    slide.Stext = item.s_text;
                    slide.Stype = item.s_type;
                    slide.Svalue = item.s_value;
                    slide.IsPc = !string.IsNullOrEmpty(requestModel.is_pc) ? int.Parse(requestModel.is_pc) : 0;
                    if (!bllMall.Update(slide))
                    {
                        resp.errcode = 1;
                        resp.errmsg = "更新失败";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                        return;

                    }


                }



            }
            resp.isSuccess = true;
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));



        }





        /// <summary>
        /// 模型
        /// </summary>
        public class Model
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
            public string s_type { get; set; }
            public string s_value { get; set; }
            public string s_text { get; set; }
        }


    }
}