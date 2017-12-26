using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API.File
{
    [Serializable]
    public class ExportCache
    {
        public string FileName { get; set; }
        public MemoryStream Stream { get; set; }
    }
}
