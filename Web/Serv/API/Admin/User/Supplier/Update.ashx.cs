using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.User.Supplier
{
    /// <summary>
    /// Update 的摘要说明
    /// </summary>
    public class Update : BaseHandlerNeedLoginAdminNoAction
    {

        public void ProcessRequest(HttpContext context)
        {
            
            string id=context.Request["id"];
            string companyName = context.Request["companyName"];
            string trueName = context.Request["trueName"];
            string phone = context.Request["phone"];
            string email = context.Request["email"];
            string description = context.Request["description"];
            string permissionGroupId = context.Request["permissionGroupId"];
            string headImage = context.Request["headImage"];
            string image = context.Request["image"];
            string ex1 = context.Request["ex1"];
            string ex2 = context.Request["ex2"];//供应商代码
            string ex3=context.Request["ex3"];
            string ex4 = context.Request["ex4"];
            string address = context.Request["address"];
            string backDeposit = context.Request["backDeposit"];
            string backAccount = context.Request["backAccount"];
            string province = context.Request["privince"];
            string city = context.Request["city"];
            string district = context.Request["district"];
            string provinceCode = context.Request["province_code"];
            string cityCode = context.Request["city_code"];
            string districtCode = context.Request["district_code"];
            string msg = "";
            apiResp.status = bllUser.UpdateSupplier(id, companyName, trueName, phone, email, description, permissionGroupId, headImage, image, ex1, ex2, ex3, ex4, out msg, address
                , province, provinceCode, city, cityCode, district, districtCode, backDeposit, backAccount);
            apiResp.msg = msg;
            bllUser.ContextResponse(context, apiResp);
        }


    }
}