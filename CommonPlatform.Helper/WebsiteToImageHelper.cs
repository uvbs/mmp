
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace CommonPlatform.Helper
{
    public class WebsiteToImageHelper
    {
        private Bitmap m_Bitmap;
        private string m_Url;
        public WebsiteToImageHelper(string url)
        {
            // Without file 
            m_Url = url;
        }
        public Bitmap Generate()
        {
            // Thread 
            var m_thread = new Thread(_Generate);
            m_thread.SetApartmentState(ApartmentState.STA);
            m_thread.Start();
            m_thread.Join(10000);
            return m_Bitmap;
        }
        private void _Generate()
        {
            var browser = new WebBrowser { ScrollBarsEnabled = false };
            //browser.Width = 375;
            //browser.Height = 667;

            browser.Width = 553;
            browser.Height = 986;
            browser.Navigate(m_Url);
            browser.DocumentCompleted += WebBrowser_DocumentCompleted;
            while (browser.ReadyState != WebBrowserReadyState.Complete)
            {
                Application.DoEvents();
            }
            browser.Dispose();
        }
        private void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            // Capture 
            var browser = (WebBrowser)sender;
            browser.ClientSize = new Size(browser.Document.Body.ScrollRectangle.Width, browser.Document.Body.ScrollRectangle.Bottom);
            browser.ScrollBarsEnabled = false;
            m_Bitmap = new Bitmap(browser.Document.Body.ScrollRectangle.Width, browser.Document.Body.ScrollRectangle.Bottom);
            browser.BringToFront();
            browser.DrawToBitmap(m_Bitmap, browser.Bounds);
        }
    }
}