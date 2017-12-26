using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 游戏信息
    /// </summary>
    public class GameInfo : ZCBLLEngine.ModelTable
    {
       /// <summary>
       /// 游戏标识
       /// </summary>
       public int AutoID { get; set; }
       /// <summary>
       /// 游戏名称
       /// </summary>
       public string GameName { get; set; }
        /// <summary>
        /// 游戏图片
        /// </summary>
       public string GameImage { get; set; }
       /// <summary>
       /// 游戏描述
       /// </summary>
       public string GameDesc { get; set; }
       /// <summary>
       /// 游戏代码
       /// </summary>
       public string GameCode { get; set; }
       /// <summary>
       /// 游戏排序
       /// </summary>
       public int GameSort { get; set; }

       /// <summary>
       /// ViewPort
       /// </summary>
       public string GameViewPort { get; set; }


    }
}
