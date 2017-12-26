using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv
{
    /// <summary>
    /// 汽车相关模块BaseHandler
    /// </summary>
    public class CarBaseHandler : BaseHandler
    {
        protected BLLJIMP.BLLCarLibrary bll = new BLLJIMP.BLLCarLibrary();

        protected BLLJIMP.BLLUserExpand bllUserEx = new BLLJIMP.BLLUserExpand();

        protected BLLJIMP.BLLCommRelation bllCommRelation = new BLLJIMP.BLLCommRelation();
    }
}