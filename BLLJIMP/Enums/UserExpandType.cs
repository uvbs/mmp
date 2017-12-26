using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Enums
{
    /// <summary>
    /// 用户扩展类型：入库为枚举字符串
    /// </summary>
    public enum UserExpandType
    {
        /// <summary>
        /// 用户座机
        /// </summary>
        UserTel,
        /// <summary>
        /// 用户公司地址
        /// </summary>
        UserCompanyAddress,
        /// <summary>
        /// 用户收件地址
        /// </summary>
        UserReceiveAddress,
        /// <summary>
        /// 用户个人介绍
        /// </summary>
        UserIntroduction,
        /// <summary>
        /// 课程用户
        /// </summary>
        UserOpenCreate,
        /// <summary>
        /// 用户是否是VIP
        /// </summary>
        UserIsVip,
        /// <summary>
        /// 律师执业证编号
        /// </summary>
        LawyerLicenseNo,
        /// <summary>
        /// 律师执业证
        /// </summary>
        LawyerLicensePhoto,
        /// <summary>
        /// 身份证正面
        /// </summary>
        IDPhoto1,
        /// <summary>
        /// 身份证反面
        /// </summary>
        IDPhoto2,
        /// <summary>
        /// 是否公开信息 0隐藏 1公开
        /// </summary>
        IsSHowInfo,
        /// <summary>
        /// 身份证号码
        /// </summary>
        IdCardNo,
        /// <summary>
        /// 车主信息：ex1 我的车型、ex2 车牌号码、ex3 vin号、ex4 车辆上牌时间（yyyy-MM-dd）、ex5 驾照领取时间 (yyyy-MM-dd)、ex6 驾照类型
        /// </summary>
        CarOwnerInfo,
        /// <summary>
        /// 上次投保时间：datavalue 投保时间
        /// </summary>
        LastInsuranceDate,
        /// <summary>
        /// 学籍信息  userid=身份证号码,  DataValue=姓名,  ex1=类型 eg:成人,  ex2=教育程度 eg:本科,  ex3=毕业的时间 eg:2006,  ex4=结果 eg:毕业,   ex5=学校 eg:浙江大学,      ex6=专业名称 eg:英语,  ex7=注册日期 eg:2004
        /// </summary>
        StudentStatus,
        /// <summary>
        /// 保养提醒:datavalue 保养提醒时间
        /// </summary>
        ServiceRemind,
        /// <summary>
        /// 开票资料
        /// value发票类型 ex1公司名称 ex2信用代码 ex3开户银行 ex4银行帐号 ex5公司注册地址 ex6公司电话 ex7一般纳税人资格证书(照片链接)
        /// </summary>
        InvoicingInformation,
        /// <summary>
        /// 银行信息
        /// Ex1 开户行 Ex2银行账号
        /// </summary>
        BankInfo,
    }
}
