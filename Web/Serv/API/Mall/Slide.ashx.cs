using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Mall
{
    /// <summary>
    /// 广告
    /// </summary>
    public class Slide : BaseHandler
    {
        /// <summary>
        ///商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLKeyValueData bllKeyValue = new BLLJIMP.BLLKeyValueData();

        BLLJIMP.BLLSlide bllSlide = new BLLJIMP.BLLSlide();

        /// <summary>
        /// 获取滑动图片列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string List(HttpContext context)
        {

            List<object> listResult = new List<object>();
            var sourceData1 = bllMall.GetList<BLLJIMP.Model.Slide>(string.Format("WebsiteOwner='{0}' And Type='slide1' order by Sort DESC", bllMall.WebsiteOwner));
            var list1 = from p in sourceData1
                       select new
                       {
                           img_url =bllMall.GetImgUrl(p.ImageUrl),
                           link = p.Link


                       };

            listResult.Add(new { 
            name="slide1",
            datalist=list1
            });

            var sourceData2 = bllMall.GetList<BLLJIMP.Model.Slide>(string.Format("WebsiteOwner='{0}' And Type='slide2' order by Sort DESC", bllMall.WebsiteOwner));
            var list2 = from p in sourceData2
                        select new
                        {
                            img_url = bllMall.GetImgUrl(p.ImageUrl),
                            link = p.Link


                        };
            listResult.Add(new
            {
                name = "slide2",
                datalist = list2
            });

            var sourceData3 = bllMall.GetList<BLLJIMP.Model.Slide>(string.Format("WebsiteOwner='{0}' And Type='slide3' order by Sort DESC", bllMall.WebsiteOwner));
            var list3 = from p in sourceData3
                        select new
                        {
                            img_url = bllMall.GetImgUrl(p.ImageUrl),
                            link = p.Link


                        };

            listResult.Add(new
            {
                name = "slide3",
                datalist = list3
            });

            var sourceData4 = bllMall.GetList<BLLJIMP.Model.Slide>(string.Format("WebsiteOwner='{0}' And Type='slide4' order by Sort DESC", bllMall.WebsiteOwner));
            var list4 = from p in sourceData4
                        select new
                        {
                            img_url = bllMall.GetImgUrl(p.ImageUrl),
                            link = p.Link


                        };

            listResult.Add(new
            {
                name = "slide4",
                datalist = list4
            });

            var data = new
            {
                totalcount = listResult.Count,
                list = listResult,//列表

            };

            return ZentCloud.Common.JSONHelper.ObjectToJson(data);

        }

        /// <summary>
        /// 根据类型获取
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string ListByType(HttpContext context)
        {
            string slideType = context.Request["slide_type"];

            var sourceData = bllSlide.ListByType(slideType, bllSlide.WebsiteOwner);

            var list = from p in sourceData
                       select new
                       {
                           img_url = bllMall.GetImgUrl(p.ImageUrl),
                           link = p.Link,
                           slide_type = p.Type,
                           link_text = p.LinkText
                       };


            var data = new
            {
                totalcount = sourceData.Count,
                proportion=bllKeyValue.GetSlideProportion(slideType),
                list = list,//列表

            };

            return ZentCloud.Common.JSONHelper.ObjectToJson(data);
        }

        /// <summary>
        /// 获取滑动图片列表所有
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string ListAll(HttpContext context)
        {

            List<object> listResult = new List<object>();
            var sourceData = bllMall.GetList<BLLJIMP.Model.Slide>(string.Format("WebsiteOwner='{0}'  order by Sort DESC", bllMall.WebsiteOwner));

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
                list.proportion = bllKeyValue.GetSlideProportion(type);
                list.datalist = new List<RequestModel>();
                list.list = new List<RequestModel>();
                foreach (var model in sourceData.Where(p => p.Type == type))
                {

                    RequestModel modelNew = new RequestModel();
                    modelNew.img_url = model.ImageUrl;
                    modelNew.link = model.Link;
                    modelNew.link_text = model.LinkText;
                    list.datalist.Add(modelNew);
                    list.list.Add(modelNew);

                }

                result.list.Add(list);



            }




            return ZentCloud.Common.JSONHelper.ObjectToJson(result);

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
            /// <summary>
            /// 列表数据模型
            /// </summary>
            public List<RequestModel> list { get; set; }

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
        }

    }
}