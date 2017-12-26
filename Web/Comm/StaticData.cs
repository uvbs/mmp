using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.Comm
{
    public class StaticData
    {
        private static BLLKeyValueData bllKeyValueData = new BLLKeyValueData();
        // 微信编辑器数据
        public static object zcWxEditorData;
        public static void InitKeyValueData()
        {
            zcWxEditorData = bllKeyValueData.GetCateKeyValueData("WeixinKindeditor");
        }

        public static void InitStaticData()
        {
            InitKeyValueData();
        }

    }
}