using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.User.Supplier
{
    /// <summary>
    /// 供应商列表
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        BLLJIMP.BLLUserExpand bllUserExpand = new BLLJIMP.BLLUserExpand();
        public void ProcessRequest(HttpContext context)
        {
            try
            {

           
            int pageIndex = !string.IsNullOrEmpty(context.Request["page"]) ? int.Parse(context.Request["page"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["rows"]) ? int.Parse(context.Request["rows"]) : 10;
            string keyWord = context.Request["keyWord"];
            string supplierUserId = "";
            if (currentUserInfo.UserType==7)
            {
                supplierUserId = currentUserInfo.UserID;
            }
            int totalCount;
            var sourceData = bllUser.GetSupplierList(bllUser.WebsiteOwner, pageIndex, pageSize, keyWord,supplierUserId, out totalCount);

            List<RespUserInfo> returnList=new List<RespUserInfo>();
            foreach (var p in sourceData)
	        {
                RespUserInfo model = new RespUserInfo();
                UserExpand userExpand = bllUserExpand.GetUserExpand(BLLJIMP.Enums.UserExpandType.BankInfo,p.UserID,p.WebsiteOwner);
                model.id=p.AutoID;
                model.user_id=p.UserID;
                model.company_name=p.Company;
                model.true_name=p.TrueName;
                model.phone=p.Phone;
                model.email=p.Email;
                model.desc=p.Description;
                model.permission_group_id=p.PermissionGroupID;
                model.permission_group_name = p.PermissionGroupID.HasValue ? bllUser.Get<ZentCloud.BLLPermission.Model.PermissionGroupInfo>(string.Format("GroupID={0}", p.PermissionGroupID)).GroupName : "";
                model.head_img_url=p.WXHeadimgurl;
                model.image=p.Images;
                model.ex1=p.Ex1;
                model.ex2=p.Ex2;//供应商代码
                model.ex3=p.Ex3;
                model.ex4=p.Ex4;//提醒
                model.address=p.Address;
                model.province=p.Province;
                model.city=p.City;
                model.district = p.District;
                model.province_code=p.ProvinceCode;
                model.city_code=p.CityCode;
                model.district_code=p.DistrictCode;
                model.back_deposit =userExpand!=null?userExpand.Ex1:"";
                model.back_account = userExpand!=null?userExpand.Ex2:"";
                returnList.Add(model);
	        }

            var resp = new
            {
                total = totalCount,
                
                rows = returnList

            };
            bllUser.ContextResponse(context, resp);
            }
            catch (Exception ex)
            {
                resp.returnObj = ex.ToString();
                bllUser.ContextResponse(context, resp);
            }
        }

        public class RespUserInfo
        {
            public int id { get; set; }
            public string user_id { get; set; }
            public string company_name { get; set; }
            public string true_name { get; set; }
            public string phone { get; set; }
            public string email { get; set; }
            public string desc { get; set; }
            public long? permission_group_id { get; set; }
            public string permission_group_name { get; set; }
            public string head_img_url { get; set; }
            public string image { get; set; }
            public string ex1 { get; set; }
            public string ex2 { get; set; }
            public string ex3 { get; set; }
            public string ex4 { get; set; }
            public string address { get; set; }
            public string province { get; set; }
            public string city { get; set; }
            public string district { get; set; }
            public string province_code { get; set; }
            public string city_code { get; set; }
            public string district_code { get; set; }
            public string back_deposit { get; set; }
            public string back_account { get; set; }





        }

    }
}