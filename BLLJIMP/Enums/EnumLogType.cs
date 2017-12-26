using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Enums
{
    /// <summary>
    /// 日志模块类型
    /// </summary>
    public enum EnumLogType
    {
        /// <summary>
        /// 登录
        /// </summary>
        Login,
        /// <summary>
        /// 业务分销模块
        /// </summary>
        DistributionOffLine,
        /// <summary>
        /// 文章
        /// </summary>
        Article,
        /// <summary>
        /// 活动
        /// </summary>
        Activity,
        /// <summary>
        /// 商城
        /// </summary>
        Mall,
        /// <summary>
        /// 系统功能
        /// </sumary>
        System,
        /// <summary>
        /// 移动网站
        /// </summary>
        Website,
        /// <summary>
        /// 会员
        /// </summary>
        Member,
        /// <summary>
        /// 营销中心
        /// </summary>
        Marketing,
        /// <summary>
        /// 服务网点
        /// </summary>
        Outlets,
        /// <summary>
        /// Sh会员
        /// </summary>
        ShMember,
        /// <summary>
        /// 手机授权，覆盖WXUnionID
        /// </summary>
        OAuthBind
    }


    public enum EnumLogTypeAction
    {

        /// <summary>
        /// 导出
        /// </summary>
        Export,
        /// <summary>
        /// 删除
        /// </summary>
        Delete,
        /// <summary>
        /// 修改
        /// </summary>
        Update,
        /// <summary>
        /// 添加
        /// </summary>
        Add,
        /// <summary>
        /// 登入
        /// </summary>
        SignIn,
        /// <summary>
        /// 配置
        /// </summary>
        Config
    }
}
