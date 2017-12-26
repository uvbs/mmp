using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API.User
{

    [Serializable]
    public class UserCenterField
    {
        public Field avatar { get; set; }
        public Field truename { get; set; }
        public Field sex { get; set; }
        public Field phone { get; set; }
        public Field email { get; set; }
        public Field address { get; set; }
        public Field identitycardphoto { get; set; }
        public Field businessintelligencecertificatephoto { get; set; }
        public Field bankcard { get; set; }
        public Field qrcode { get; set; }
        public Field forgetpassword { get; set; }
        public Field loginout { get; set; }
    }
}
