using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP;
using System.Reflection;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP.Enums;
using Newtonsoft.Json;

namespace ZentCloud.JubitIMP.Web.Serv.API.Mall
{
    /// <summary>
    /// 收货地址
    /// </summary>
    public class DeliverAddress : BaseHandlerNeedLogin
    {

        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLMall bllMall = new BLLMall();
        /// <summary>
        /// 站点BLL
        /// </summary>
        BLLWebSite bllWebsite = new BLLWebSite();

        /// <summary>
        /// 增加收货地址
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Add(HttpContext context)
        {

            string msg = "";
            if (bllMall.AddConsigneeAddress(currentUserInfo.UserID, context, out msg))
            {
                resp.errmsg = "ok";
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = msg;
            }

            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 删除收货地址
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Delete(HttpContext context)
        {
            string Id = context.Request["address_id"];
            if (bllMall.DeleteConsigneeAddress(currentUserInfo.UserID, Id))
            {
                resp.errmsg = "ok";
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "删除失败";
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
        }
        /// <summary>
        /// 查询我的收货地址
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string List(HttpContext context)
        {

            var sourceData = bllMall.GetConsigneeAddressList(currentUserInfo.UserID, bllMall.WebsiteOwner);
            var list = from p in sourceData
                       select new
                       {
                           address_id = p.AutoID,
                           consigneename = p.ConsigneeName,
                           phone = p.Phone,
                           address = p.Address,
                           province = p.Province,
                           province_code = p.ProvinceCode,
                           city = p.City,
                           city_code = p.CityCode,
                           dist = p.Dist,
                           dist_code = p.DistCode,
                           zip_code = p.ZipCode,
                           isdefault = (!string.IsNullOrEmpty(p.IsDefault) && p.IsDefault == "1") ? 1 : 0
                       };


            var data = new
            {
                totalcount = sourceData.Count,//总数
                list = list,//列表

            };
            return JsonConvert.SerializeObject(data);//ZentCloud.Common.JSONHelper.ObjectToJson(data);


        }
        /// <summary>
        /// 查询单个收货地址
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Get(HttpContext context)
        {
            string addressId = context.Request["address_id"];
            WXConsigneeAddress addressInfo = bllMall.GetConsigneeAddress(addressId);
            if (addressInfo==null)
            {
                resp.errcode = 1;
                resp.errmsg = "收货地址ID不存在";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }

            var data = new
            {
                address_id = addressInfo.AutoID,
                consigneename = addressInfo.ConsigneeName,
                phone = addressInfo.Phone,
                address = addressInfo.Address,
                province=addressInfo.Province,
                province_code=addressInfo.ProvinceCode,
                city=addressInfo.City,
                city_code=addressInfo.CityCode,
                dist=addressInfo.Dist,
                dist_code=addressInfo.DistCode,
                zip_code=addressInfo.ZipCode,
                isdefault = (!string.IsNullOrEmpty(addressInfo.IsDefault) && addressInfo.IsDefault == "1") ? 1 : 0
            };

            return ZentCloud.Common.JSONHelper.ObjectToJson(data);

        }
        /// <summary>
        /// 更新收货地址
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Update(HttpContext context)
        {
            string addressId = context.Request["address_id"];
            string consigneeName = context.Request["consigneename"];
            string address = context.Request["address"];
            string phone = context.Request["phone"];
            string isDefault = context.Request["isdefault"];
            string province = context.Request["province"];
            string provinceCode = context.Request["province_code"];

            string city = context.Request["city"];
            string cityCode = context.Request["city_code"];

            string dist = context.Request["dist"];
            string distCode = context.Request["dist_code"];
            string zipCode = context.Request["zip_code"];
            string msg = "";
            if (bllMall.EditConsigneeAddress(addressId, currentUserInfo.UserID, consigneeName, address, phone, isDefault, province, provinceCode, city, cityCode, dist, distCode, zipCode, out msg))
            {
                WebsiteInfo websiteModel = bllWebsite.GetWebsiteInfo(bllWebsite.WebsiteOwner);

                //if (websiteModel.IsSynchronizationData == 1 && websiteModel.IsSynchronizationData!=null)
                //{


                //    CurrentUserInfo.TrueName = consigneeName;
                //    CurrentUserInfo.Phone = phone;

                //    bllWebsite.Update(CurrentUserInfo);

                //}


                if (string.IsNullOrWhiteSpace(currentUserInfo.TrueName))
                {
                    currentUserInfo.TrueName = consigneeName;
                    bllMall.Update(currentUserInfo, string.Format(" TrueName = '{0}' ", consigneeName), string.Format(" AutoID = {0} ", currentUserInfo.AutoID));
                }

                if (string.IsNullOrWhiteSpace(currentUserInfo.Phone) && ZentCloud.Common.MyRegex.PhoneNumLogicJudge(phone))
                {
                    currentUserInfo.Phone = phone;
                    bllMall.Update(currentUserInfo, string.Format(" Phone = '{0}' ", phone), string.Format(" AutoID = {0} ", currentUserInfo.AutoID));
                }


                resp.errmsg = "ok";
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = msg;
            }

            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }
        /// <summary>
        /// 设置默认收货地址
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SetDefault(HttpContext context)
        {
            string addressId = context.Request["address_id"];//收货地址ID
            if (bllMall.SetDefaultConsigneeAddress(currentUserInfo.UserID, addressId))
            {
                resp.errmsg = "ok";
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "操作失败";
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);




        }



    }
}