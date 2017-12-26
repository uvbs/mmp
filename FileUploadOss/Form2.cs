using AliOss;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FileUploadOss
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string oldFile1 = textBox1.Text.Trim();
            byte[] fileByte = File.ReadAllBytes(oldFile1);
            string extension = Path.GetExtension(oldFile1).ToLower();
            OssHelper.UploadFileFromByte(OssHelper.GetBucket(""), "oldsite/FileUpload/mixblu/20151012/image/839cc17c-e7e3-4413-bb1a-55880a30c6af.jpg", fileByte, extension);
        }

    }
}
