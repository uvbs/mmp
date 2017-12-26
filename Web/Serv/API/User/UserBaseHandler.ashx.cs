
namespace ZentCloud.JubitIMP.Web.Serv.API.User
{
    /// <summary>
    /// UserBaseHandler 的摘要说明
    /// </summary>
    public class UserBaseHandler : BaseHandler
    {
        /// <summary>
        /// 用户业务逻辑
        /// </summary>
        protected BLLJIMP.BLLUser bll = new BLLJIMP.BLLUser();
        /// <summary>
        /// 基于用户扩展的商家业务员逻辑
        /// </summary>
        protected BLLJIMP.BLLSaller bllSaller = new BLLJIMP.BLLSaller();
        /// <summary>
        /// 用户扩展信息
        /// </summary>
        protected BLLJIMP.BLLUserExpand bllUserExpand = new BLLJIMP.BLLUserExpand();
    }
}