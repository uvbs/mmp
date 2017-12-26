using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLPermission.Enums
{
    /// <summary>
    /// 权限系统键值，某些需要支持系统编码的权限可在此配置权限枚举，对应数据库的权限键值字符串
    /// </summary>
    public enum PermissionSysKey
    {
        /// <summary>
        /// 线上分销基础功能，拥有该权限则可以使用线上分销功能
        /// </summary>
        OnlineDistribution,
        /// <summary>
        /// 文章pv权限
        /// </summary>
        IsShowArticlePv,
        /// <summary>
        /// 活动pv权限
        /// </summary>
        IsShowActivityPv,
        /// <summary>
        /// 商品pv权限
        /// </summary>
        IsShowProductPv,
        /// <summary>
        /// 修改用户积分
        /// </summary>
        UpdateMemberScore,
        /// <summary>
        /// 修改用户余额
        /// </summary>
        UpdateMemberBalance,
        /// <summary>
        /// 发送模板消息
        /// </summary>
        SendTtemplatMessage,
        /// <summary>
        /// 移动商机
        /// </summary>
        MobileOpportunities,
        /// <summary>
        /// 保存页面模板
        /// </summary>
        SaveComponentTemplate,
        /// <summary>
        /// 重置会员密码
        /// </summary>
        ResetMemberPwd,
        /// <summary>
        /// 撤单（颂和）
        /// </summary>
        CancelMemberRegister,
        /// <summary>
        /// 更改推荐人
        /// </summary>
        UpdateDistributionOwner,
        /// <summary>
        /// 修改会员资料
        /// </summary>
        UpdateMemberInfo,
        /// <summary>
        /// 修改会员登录手机
        /// </summary>
        UpdateLoginPhone,
        /// <summary>
        /// 锁定会员（禁止升级，转账，报单，提现）
        /// </summary>
        LockMember,
        /// <summary>
        /// 积分导出
        /// </summary>
        ScoreExport,
        /// <summary>
        /// 余额导出
        /// </summary>
        TotalAmountExport,
        /// <summary>
        /// 余额打印
        /// </summary>
        TotalAmountPrint,
        /// <summary>
        /// 提现导出
        /// </summary>
        WithdrawExport,
        /// <summary>
        /// 提现打印
        /// </summary>
        WithdrawPrint,
        /// <summary>
        /// 会员导出
        /// </summary>
        MemberExport,
        /// <summary>
        /// 业绩导出
        /// </summary>
        PerformanceExport,
        /// <summary>
        /// 计算业绩奖
        /// </summary>
        ComputeReward,
        /// <summary>
        /// 会员团队导出
        /// </summary>
        TeamExport,
        /// <summary>
        /// 业绩确认表
        /// </summary>
        PerformanceConfrimExport,
        /// <summary>
        /// 管理奖设置删除
        /// </summary>
        DeletePerformanceSet,
        /// <summary>
        /// 管理奖设置
        /// </summary>
        PerformanceSet,
        /// <summary>
        /// 打款审核
        /// </summary>
        PMS_TRANSFERSAUDIT,
         /// <summary>
        /// 仅支持更新商品库存
        /// </summary>
        PMS_ONLYUPDATEPRODUCTSTOCK,
        /// <summary>
        /// 美帆培训-添加权限
        /// </summary>
        PMS_MFTRAIN_ADD,
        /// <summary>
        /// 美帆培训-编辑权限
        /// </summary>
        PMS_MFTRAIN_UPDATE,
        /// <summary>
        /// 美帆培训-删除权限
        /// </summary>
        PMS_MFTRAIN_DELETE,
        /// <summary>
        /// 美帆培训-启用禁用权限
        /// </summary>
        PMS_MFTRAIN_ENABLE,

        /// <summary>
        /// 美帆活动-添加权限
        /// </summary>
        PMS_MFACTIVITY_ADD,
        /// <summary>
        /// 美帆活动-编辑权限
        /// </summary>
        PMS_MFACTIVITY_UPDATE,
        /// <summary>
        /// 美帆活动-删除权限
        /// </summary>
        PMS_MFACTIVITY_DELETE,
        /// <summary>
        /// 美帆活动-启用禁用权限
        /// </summary>
        PMS_MFACTIVITY_ENABLE,


        

        /// <summary>
        /// 美帆会员卡-添加权限
        /// </summary>
        PMS_MFCARD_ADD,
        /// <summary>
        /// 美帆会员卡-编辑权限
        /// </summary>
        PMS_MFCARD_UPDATE,
        /// <summary>
        /// 美帆会员卡-删除权限
        /// </summary>
        PMS_MFCARD_DELETE,
        /// <summary>
        /// 美帆会员卡-启用禁用权限
        /// </summary>
        PMS_MFCARD_ENABLE,
        /// <summary>
        /// 美帆会员卡-开卡权限
        /// </summary>
        PMS_MFCARD_ADDMYCARD,


        /// <summary>
        /// 美帆竞赛-添加权限
        /// </summary>
        PMS_MFMATCH_ADD,
        /// <summary>
        /// 美帆竞赛-编辑权限
        /// </summary>
        PMS_MFMATCH_UPDATE,
        /// <summary>
        /// 美帆竞赛-删除权限
        /// </summary>
        PMS_MFMATCH_DELETE,
        /// <summary>
        /// 美帆竞赛-启用禁用权限
        /// </summary>
        PMS_MFMATCH_ENABLE,


        /// <summary>
        /// 美帆-添加活动订单权限
        /// </summary>
        PMS_MFACTIVITY_ADDSIGNUP,
        /// <summary>
        /// 美帆-添加竞赛订单权限
        /// </summary>
        PMS_MFMATCH_ADDSIGNUP,
        /// <summary>
        /// 美帆-添加培训订单权限
        /// </summary>
        PMS_MFTRAIN_ADDSIGNUP,

    }
}
