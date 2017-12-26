using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 游戏广告
    /// </summary>
    public class GameAdvertPlan : ZCBLLEngine.ModelTable
    {
      /// <summary>
      /// 自动标识
      /// </summary>
      public int AutoID { get; set; }
      /// <summary>
      /// 任务名称
      /// </summary>
      public string PlanName { get; set; }
      /// <summary>
      /// 游戏ID
      /// </summary>
      public int GameID { get; set; }
      /// <summary>
      /// IP
      /// </summary>
      public int IP { get; set; }
      /// <summary>
      ///  PV
      /// </summary>
      public int PV { get; set; }
      /// <summary>
      /// 广告点击数
      /// </summary>
      public int AdvertClickCount { get; set; }

      /// <summary>
      /// 网站所有者
      /// </summary>
      public string WebsiteOwner { get; set; }


      /// <summary>
      /// 广告图片
      /// </summary>
      public string AdvertImage1 { get; set; }

      /// <summary>
      /// 广告链接
      /// </summary>
      public string AdvertUrl1 { get; set; }

      /// <summary>
      /// 广告图片
      /// </summary>
      public string AdvertImage2 { get; set; }

      /// <summary>
      /// 广告链接
      /// </summary>
      public string AdvertUrl2 { get; set; }
      /// <summary>
      /// 广告图片
      /// </summary>
      public string AdvertImage3 { get; set; }

      /// <summary>
      /// 广告链接
      /// </summary>
      public string AdvertUrl3 { get; set; }


      /// <summary>
      /// 游戏名称
      /// </summary>
      public string GameName
      {
          get
          {
              string result = "";
              try
              {
                  BllGame bllGame = new BllGame();
                  result = bllGame.GetSingleGameInfo(GameID).GameName;

              }
              catch { }
              return result;
          }
      }




    }
}
