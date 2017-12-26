using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace Test.Controllers
{
    public class ProductsController: ApiController
    {
        public string GetDef()
        {
            return "hello~";
        }

        public Open.EZRproSDK.Entity.BonusGetResp GetById(string id)
        {
            return new Open.EZRproSDK.Client().GetBonus("000000009", "000000009", "13701969237");
        }

    }
}
