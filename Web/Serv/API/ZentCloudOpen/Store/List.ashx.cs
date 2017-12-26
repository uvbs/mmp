using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.ZentCloudOpen.Store
{
    /// <summary>
    /// List 的摘要说明
    /// </summary>
    public class List : BaseHanderOpen
    {

        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            try
            {


                int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
                int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
                string keyWord = context.Request["key_word"];
                string supplierUserId = "";

                int totalCount;
                var sourceData = bllUser.GetSupplierList(bllUser.WebsiteOwner, pageIndex, pageSize, keyWord, supplierUserId, out totalCount);
                List<StoreModel> list = new List<StoreModel>();
                foreach (var item in sourceData)
                {
                    JuActivityInfo storeInfo = bllUser.Get<JuActivityInfo>(string.Format("WebsiteOwner='{0}' And ArticleType='Outlets' And K5='{1}'", item.WebsiteOwner, item.AutoID));
                    if (storeInfo==null)
                    {
                        storeInfo = new JuActivityInfo();
                    }
                    StoreModel model = new StoreModel();
                    model.store_id = item.AutoID.ToString();
                    model.user_id = item.UserID;
                    model.store_name = item.Company;
                    model.store_tel = storeInfo.K4;
                    model.store_image = storeInfo.ThumbnailsPath;
                    model.server_time = storeInfo.ServerTimeMsg;
                    model.province = storeInfo.Province;
                    model.city = storeInfo.City;
                    model.district = storeInfo.District;
                    model.address = storeInfo.ActivityAddress;
                    model.longitude = storeInfo.UserLongitude;
                    model.latitude = storeInfo.UserLatitude;
                    
                    list.Add(model);
                    
                }
                resp.status = true;
                resp.msg = "ok";
                resp.result = new
                {
                    totalcount = totalCount,
                    list = list

                };
                bllUser.ContextResponse(context, resp);
            }
            catch (Exception ex)
            {
                resp.result = ex.ToString();
                bllUser.ContextResponse(context, resp);
            }
        }

        /// <summary>
        /// 门店模型
        /// </summary>
        public class StoreModel
        {
            /// <summary>
            /// 门店Id
            /// </summary>
            public string store_id { get; set; }
            /// <summary>
            /// 用户账号
            /// </summary>
            public string user_id { get; set; }
            ///// <summary>
            ///// 密码
            ///// </summary>
            //public string password { get; set; }
            ///// <summary>
            ///// 密码确认
            ///// </summary>
            //public string password_confirm { get; set; }
            /// <summary>
            /// 门店名称
            /// </summary>
            public string store_name { get; set; }
            /// <summary>
            /// 门店电话
            /// </summary>
            public string store_tel { get; set; }
            /// <summary>
            /// 门店图片
            /// </summary>
            public string store_image { get; set; }
            /// <summary>
            /// 服务时间
            /// </summary>
            public string server_time { get; set; }
            /// <summary>
            /// 省
            /// </summary>
            public string province { get; set; }
            /// <summary>
            /// 市
            /// </summary>
            public string city { get; set; }
            /// <summary>
            /// 区域
            /// </summary>
            public string district { get; set; }
            /// <summary>
            /// 详细地址
            /// </summary>
            public string address { get; set; }
            /// <summary>
            /// 经度
            /// </summary>
            public string longitude { get; set; }
            /// <summary>
            /// 纬度
            /// </summary>
            public string latitude { get; set; }



        }
    }
}