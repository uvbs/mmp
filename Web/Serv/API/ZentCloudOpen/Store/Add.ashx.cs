using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.ZentCloudOpen.Store
{
    /// <summary>
    /// 添加门店
    /// </summary>
    public class Add : BaseHanderOpen
    {

        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            
            try
            {
                
                StoreModel requestModel= bllUser.ConvertRequestToModel<StoreModel>(new StoreModel());
                if (string.IsNullOrEmpty(requestModel.user_id))
                {
                    resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                    resp.msg = "user_id 参数必传";
                    bllUser.ContextResponse(context, resp);
                    return;
                }
                if (string.IsNullOrEmpty(requestModel.store_name))
                {
                    resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                    resp.msg = "store_name 参数必传";
                     bllUser.ContextResponse(context, resp);
                     return;
                }
                if (string.IsNullOrEmpty(requestModel.store_tel))
                {
                    resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                    resp.msg = "store_tel 参数必传";
                    bllUser.ContextResponse(context, resp);
                    return;
                }
                if (string.IsNullOrEmpty(requestModel.province))
                {
                    resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                    resp.msg = "province 参数必传";
                    bllUser.ContextResponse(context, resp);
                    return;
                }
                if (string.IsNullOrEmpty(requestModel.city))
                {
                    resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                    resp.msg = "city 参数必传";
                    bllUser.ContextResponse(context, resp);
                    return;
                }
                if (string.IsNullOrEmpty(requestModel.district))
                {
                    resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                    resp.msg = "district 参数必传";
                    bllUser.ContextResponse(context, resp);
                    return;
                }
                if (string.IsNullOrEmpty(requestModel.address))
                {
                    resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                    resp.msg = "address 参数必传";
                    bllUser.ContextResponse(context, resp);
                    return;
                }
                if (string.IsNullOrEmpty(requestModel.longitude))
                {
                    resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                    resp.msg = "longitude  参数必传";
                    bllUser.ContextResponse(context, resp);
                    return;
                }
                if (string.IsNullOrEmpty(requestModel.latitude))
                {
                    resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                    resp.msg = "latitude  参数必传";
                    bllUser.ContextResponse(context, resp);
                    return;
                }
                if (string.IsNullOrEmpty(requestModel.password))
                {
                    requestModel.password=ZentCloud.Common.Rand.Str_char(12);
                    requestModel.password_confirm = requestModel.password;
                }
                string permissionGroupId = ZentCloud.Common.ConfigHelper.GetConfigString("SupplierPermissionGroupId"); 
                string msg="";
                bool addUserResult = bllUser.AddSupplier(requestModel.user_id, requestModel.password, requestModel.password_confirm, requestModel.store_name, "", requestModel.store_tel, "", "", permissionGroupId, requestModel.store_image, requestModel.store_image, "", "", "", "", out msg, requestModel.address, requestModel.province, "", requestModel.city, "", requestModel.district, "", "", "");
                
                if (addUserResult)
                {

                    #region 添加门店
                    UserInfo supplierInfo = bllUser.GetUserInfo(requestModel.user_id, bllUser.WebsiteOwner);
                    //添加门店表
                    JuActivityInfo pInfo = new JuActivityInfo();
                    pInfo.JuActivityID = int.Parse(bllUser.GetGUID(BLLJIMP.TransacType.ActivityAdd));
                    pInfo.ActivityName = requestModel.store_name;
                    pInfo.ActivityAddress = requestModel.address;
                    //pInfo.ServerTimeMsg = requestModel.server_time;
                    //pInfo.ServicesMsg = requestModel.server_msg;
                    pInfo.ThumbnailsPath = requestModel.store_image;
                    pInfo.ArticleType = "Outlets";
                    // pInfo.CategoryId = requestModel.cate_id;
                    pInfo.Province = requestModel.province;
                    //pInfo.ProvinceCode = requestModel.province_code;
                    pInfo.City = requestModel.city;
                    //pInfo.CityCode = requestModel.city_code;
                    pInfo.District = requestModel.district;
                    //pInfo.DistrictCode = requestModel.district_code;
                    //pInfo.K1 = requestModel.k1;
                    pInfo.K4 = requestModel.store_tel;
                    pInfo.K5 = supplierInfo.AutoID.ToString();
                    //pInfo.Tags = requestModel.tags;
                    //pInfo.Sort = requestModel.sort;
                    pInfo.UserLongitude = requestModel.longitude;
                    pInfo.UserLatitude = requestModel.latitude;
                    pInfo.IsHide = 0;
                    pInfo.CreateDate = DateTime.Now;
                    pInfo.LastUpdateDate = DateTime.Now;
                    pInfo.WebsiteOwner = bllUser.WebsiteOwner;
                    pInfo.UserID = bllUser.WebsiteOwner;
                    pInfo.ServerTimeMsg = requestModel.server_time;
                    if (bllUser.Add(pInfo))
                    {
                        resp.status = true;
                        resp.msg = "ok";
                        resp.result = new {store_id=supplierInfo.AutoID };
                    }
                    else
                    {
                        resp.msg = "添加门店失败";
                    }


                    //添加门店表 
                    #endregion


                }
                else
                {
                    resp.msg = "添加账户失败";
                    resp.msg = msg;
                }


            }
            catch (Exception ex)
            {

                resp.msg = ex.Message;
                resp.result = ex.ToString();
               
            }
            bllUser.ContextResponse(context, resp);
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
            /// <summary>
            /// 密码
            /// </summary>
            public string password { get; set; }
            /// <summary>
            /// 密码确认
            /// </summary>
            public string password_confirm { get; set; }
            /// <summary>
            /// 门店名称
            /// </summary>
            public string store_name { get; set; }
            /// <summary>
            /// 门店图片
            /// </summary>
            public string store_image { get; set; }
            /// <summary>
            /// 门店联系电话
            /// </summary>
            public string store_tel { get; set; }
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