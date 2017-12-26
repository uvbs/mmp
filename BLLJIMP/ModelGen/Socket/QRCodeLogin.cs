using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.Socket
{
    [Serializable]
    public class QRCodeLogin
    {
        public string redisKey { get; set; }
        //0 等待登录  1登录成功 2登录失败
        public int status { get; set; }
        public string msg { get; set; }
    }
}
