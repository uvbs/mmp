using System;

namespace ZentCloud.BLLJIMP.Model.Weixin
{
    public class RequestMessageLocation : RequestMessageBase, IRequestMessageBase
    {
        public double Location_X { get; set; }
        public double Location_Y { get; set; }
        public int Scale { get; set; }
        public string Label { get; set; }
    }
}
