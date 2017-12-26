using System.Web;
using System.Linq;

namespace ZentCloud.JubitIMP.Web.Serv.API.Car.Model
{
    /// <summary>
    /// 获取汽车品牌
    /// </summary>
    public class BrandList : CarBaseHandler
    {
       
        public override void ProcessRequest(HttpContext context)
        {
            int totalCount = 0;
            var dataList = bll.QueryBrand(out totalCount, bll.WebsiteOwner, 10000, 1, "", true);
            resp.isSuccess = true;

            resp.returnObj = new
            {
                totalCount = totalCount,
                list = dataList.Select(p => new {
                    carBrandId = p.CarBrandId,
                    carBrandName = p.CarBrandName,
                    firstLetter = p.FirstLetter,
                    brandImg = p.BrandImg,
                    isCurrBuyCarBrand = p.IsCurrBuyCarBrand,
                    isCurrServiceCarBrand = p.IsCurrServiceCarBrand
                })
            };

            bll.ContextResponse(context, resp);
        }
        
    }
}