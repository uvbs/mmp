using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Enums
{
    /// <summary>
    /// Project项目表 类型，数据库直接存字符串
    /// </summary>
    public enum ProjectType
    {
        /// <summary>
        /// 线下分销，目前该模块已暂停使用
        /// </summary>
        DistributionOffLine,

        /// <summary>
        /// 分公司申请
        /// 
        /// 字段：Ex1 省份、Ex2 省份代码、Ex3 城市、Ex4 城市代码、Ex5 地区、 Ex6 地区代码、联系人（手填 默认填自己的）、联系人姓名（手填 默认填自己的）
        /// </summary>
        CompanyBranchApply,

        /// <summary>
        /// 分公司推荐
        /// 
        /// 字段：Ex1 省份、Ex2 省份代码、Ex3 城市、Ex4 城市代码、Ex5 地区、 Ex6 地区代码、联系人（手填 默认填自己的）、联系人姓名（手填 默认填自己的）
        /// </summary>
        CompanyBranchRecommend,

        /// <summary>
        /// 房源推荐，
        /// 状态：审批进度，通过、未通过，
        /// 二级分类：新房NewHouse、二手房SecondHandHouse
        /// 字段：项目名称（上传新房、上传二手房）、项目介绍（项目概况）、联系人（开发商联系人）、联系手机（开发商联系手机）、
        ///       Ex1 楼盘名称、Ex2 楼盘所在省份、Ex3 楼盘所在省份代码、Ex4 城市、Ex5 城市代码、Ex6 地区、 Ex7 地区代码、Ex8 提交者姓名、Ex9 提交者手机、
        ///       Ex10 是否有征信报告 1 0、Ex11 是否有抵押 1 0、Ex12 是否有诉讼 1 0、Ex13 土地用途 商业 住宅 商住两用、Ex14 二级分类 新房NewHouse 二手房SecondHandHouse
        ///       
        /// </summary>
        HouseRecommend,

        /// <summary>
        /// 预约看房，
        /// 状态：是否成功，是、否
        /// 字段：项目名称（固定 预约看房）、联系人（看房人姓名 手填 默认当前用户）、联系手机（手填 默认当前用户）、备注、提交者信息、初始状态 待审核、Ex1 楼盘ID、Ex2 楼盘名称、Ex3 预约看房时间
        /// 
        /// </summary>
        HouseAppointment,

        /// <summary>
        /// 推荐购房顾客，
        /// 状态：进度，全款、分期、放弃，
        /// 字段：项目名称(固定 推荐购房顾客)、联系人（购房客户姓名）、联系手机、项目介绍（备注）、提交者信息、初始状态 待审核、Ex1 楼盘ID、Ex2 楼盘名称
        /// 
        /// </summary>
        HouseBuyerRecommend

    }
}
