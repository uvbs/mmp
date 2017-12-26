using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.customize.HaiMa
{
    public partial class SignUp : HaiMaBase
    {
        /// <summary>
        /// 销售顾问 A 题库
        /// </summary>
       public List<string> QeuestionA = new List<string>();
        /// <summary>
        /// 销售顾问 B题库
        /// </summary>
       public List<string> QeuestionB = new List<string>();
        /// <summary>
        /// 销售经理题库
        /// </summary>
      public  List<string> QeuestionC = new List<string>();
        /// <summary>
        /// 市场经理题库
        /// </summary>
      public  List<string> QeuestionD = new List<string>();
        /// <summary>
        /// 业务逻辑
        /// </summary>
        BLLJIMP.BLLHaiMa bllHaiMa = new BLLJIMP.BLLHaiMa();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (bllHaiMa.IsReg(CurrentUserInfo))
            {
                if (CurrentUserInfo.UserType.Equals(2))// 2表示是销售店人员
                {

                    if (!string.IsNullOrEmpty(CurrentUserInfo.Ex5))
                    {
                        Response.Redirect("MySignUp.aspx");
                    }
                    //初始化题库

                    //初始化题库
                    switch (CurrentUserInfo.Postion)
                    {
                        case "销售顾问":
                            //A题库
                            QeuestionA.Add("M6锐动前脸");
                            QeuestionA.Add("M6锐利侧面");
                            QeuestionA.Add("M6锐劲尾部");
                            QeuestionA.Add("M6精雕内饰工艺");

                            QeuestionA.Add("M6智能科技");
                            QeuestionA.Add("M6舒适的驾乘空间");
                            QeuestionA.Add("M61.5T发动机");
                            QeuestionA.Add("M6CVT变速箱");

                            QeuestionA.Add("M66速手动");
                            QeuestionA.Add("M6轿跑平台");
                            QeuestionA.Add("M6多重主动的安全系统");
                            QeuestionA.Add("M6全时周密的被动安全系统");
                            QeuestionA.Add("M6绿色环保工艺");
                            QeuestionA.Add("S5虎鲨造型的仿生设计");
                            QeuestionA.Add("S5-SFD碰撞主动控制");
                            QeuestionA.Add("S5中控集成");
                            QeuestionA.Add("S5镀铬水波侧裙边");
                            QeuestionA.Add("S5-360°智能驾驶辅助系统");
                            QeuestionA.Add("S5平视娱乐导航显示");
                            QeuestionA.Add("S5环保搪塑工艺");
                            QeuestionA.Add("S5空腔注蜡及底盘过滤");
                            QeuestionA.Add("S5-40分贝的剧院级静逸体验");
                            QeuestionA.Add("S5日间行车灯");
                            QeuestionA.Add("S5-5星碰撞安全");
                            QeuestionA.Add("S5-6安全气囊");
                            QeuestionA.Add("S5-Human-touch 人性驾乘");
                            QeuestionA.Add("S5-Can-bus系统");
                            QeuestionA.Add("S5一体冲压车门");
                            QeuestionA.Add("S5-S-drive多路况驾驶系统");
                            QeuestionA.Add("M3新一代A级国际标准化平台");
                            QeuestionA.Add("M3后排腿部空间");
                            QeuestionA.Add("M3全系标配的倒车雷达、四轮碟刹、中央扶手");
                            QeuestionA.Add("M3轻量化全铝发动机");
                            QeuestionA.Add("M3超低风阻系数");
                            QeuestionA.Add("M3悬架加装横向稳定杆");
                            QeuestionA.Add("M3VVT与CVT的双V技术黄金组合");
                            QeuestionA.Add("M3可溃缩式转向柱及踏板");
                            QeuestionA.Add("M3-ESP车身动态电子稳定系统");
                            QeuestionA.Add("M3五星碰撞安全");
                            QeuestionA.Add("M3-6安全气囊");
                            QeuestionA.Add("M3隔音棉的应用");
                            QeuestionA.Add("M3最佳弯轴比");
                            QeuestionA.Add("M3拱形后备箱格挡");
                            QeuestionA.Add("Moofun对车况、中控、灯光、门等信息的管理");
                            QeuestionA.Add("Moofun车辆体检与自检、警情信息的上报，以及空中升级");
                            QeuestionA.Add("Moofun车辆远程控制功能");
                            QeuestionA.Add("Moofun车辆预约保养维修与服务热线功能");
                            QeuestionA.Add("Moofun娱乐、互动、资讯、手机车机互联等功能");
                            QeuestionA.Add("Moofun 24H call center");
                            QeuestionA.Add("Moofun SOS");
                            QeuestionA.Add("Moofun一键语音");
                            QeuestionA.Add("Moofun车辆定位及车库寻车");


                            //A题库
                            //B题库
                            QeuestionB.Add("郑州海马品牌不如北京现代瑞纳知名度高");
                            QeuestionB.Add("郑州海马品牌不如上汽大众桑塔纳知名度高");
                            QeuestionB.Add("郑州海马品牌不如上汽别克赛欧知名度高");
                            QeuestionB.Add("郑州海马产品质量不如一汽海马产品质量好");
                            QeuestionB.Add("郑州海马产品优惠力度不如一汽海马力度大");
                            QeuestionB.Add("郑州海马的发动机质量不如三菱发动机质量好");
                            QeuestionB.Add("海马汽车做工比较粗糙，没有合资车品质高");
                            QeuestionB.Add("海马采用的是马自达不要的老技术");
                            QeuestionB.Add("M3外观不如k2好看");
                            QeuestionB.Add("M3油耗为何比赛欧高");
                            QeuestionB.Add("M3性价比不如捷达高");
                            QeuestionB.Add("M3后备箱空间小");
                            QeuestionB.Add("M3内部空间没有桑塔纳大");
                            QeuestionB.Add("M3优惠价格不如福美来力度大");
                            QeuestionB.Add("M3比比亚迪价格高那么多，值么？");
                            QeuestionB.Add("他们都说艾瑞泽3比海马M3性价比高");
                            QeuestionB.Add("日产阳光才不到六万，M3为何这么贵？");
                            QeuestionB.Add("S5优惠价格不如骑士力度大");
                            QeuestionB.Add("S5离地间隙太小");
                            QeuestionB.Add("S5内部空间不如长城H6大");
                            QeuestionB.Add("大家都说s5异响很严重");
                            QeuestionB.Add("一汽海马骑士才不到八万，S5凭什么这么贵？");
                            QeuestionB.Add("S5导航屏太高了，遮挡视线");
                            QeuestionB.Add("S5大灯进水");
                            QeuestionB.Add("M6外观不如昂克赛拉好看");
                            QeuestionB.Add("M6性价比不如轩逸高");
                            QeuestionB.Add("M6油耗太高了");
                            QeuestionB.Add("M6内饰采用硬质塑料，不环保");
                            QeuestionB.Add("M6尾部不好看");
                            QeuestionB.Add("M6不如科鲁兹操控性好");
                            QeuestionB.Add("M6不如卡罗拉知名度高");
                            QeuestionB.Add("日产轩逸才不到八万，M6太贵，买不起");
                            QeuestionB.Add("车太贵，再便宜两千元，我马上交钱");
                            QeuestionB.Add("试乘试驾邀约话术");
                            QeuestionB.Add("客户进展厅寒暄破冰话术");
                            QeuestionB.Add("1.5T的发动机不如2.0自然吸气的");
                            QeuestionB.Add("涡轮增压发动机后期保养成本高");
                            QeuestionB.Add("CVT不如手动挡有驾驶感觉");
                            QeuestionB.Add("CVT油耗高");
                            //B题库
                            break;
                        case "销售经理":
                            QeuestionC.Add("客户初次进店，如何教新销售顾问破冰？");
                            QeuestionC.Add("当客户与销售顾问纠结于价格问题时，如何破局？");
                            QeuestionC.Add("如果出现因新客户使用不当而产生的投诉，如何处理？");
                            QeuestionC.Add("怎样才能更快的提升展厅成交率？");
                            break;
                        case "市场经理":
                            QeuestionD.Add("在媒体选择时，怎样甄别适合自己媒体？");
                            QeuestionD.Add("执行店头活动时，如何控场防止客户离开？");
                            QeuestionD.Add("网电微线上营销如何与线下活动有机结合？");
                            QeuestionD.Add("当前形势下如何提升售后回厂？");
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    Response.Write("只有销售店人员才可以报名");
                    Response.End();
                }


            }
            else
            {
                Response.Redirect("Reg.aspx");
            }


        }
    }
}