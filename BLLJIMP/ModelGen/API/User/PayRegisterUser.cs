using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API.User
{
    [Serializable]
    public class PayRegisterUser
    {
        public string userid { get; set; }
        public int level { get; set; }
        public string levelname { get; set; }
        public string phone { get; set; }
        public string spreadid { get; set; } //推荐人编号
        public string truename { get; set; }
        public string idcard { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string district { get; set; }
        public string town { get; set; }
        public string provinceCode { get; set; }
        public string cityCode { get; set; }
        public string districtCode { get; set; }
        public string townCode { get; set; }
        public string regIP { get; set; }
        public string password { get; set; }
        public string flow_key { get; set; }
        public string ex1 { get; set; }
        public string content { get; set; }
        public string files { get; set; }
        /// <summary>
        /// V1 V2 类型
        /// </summary>
        public string vType { get; set; }
    }
}
