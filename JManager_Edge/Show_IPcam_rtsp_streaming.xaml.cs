
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Annotations;
using System.Windows.Forms;
using System.Collections.Specialized;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;


namespace JManager_Edge
{
    /// <summary>
    /// Show_IPcam_rtsp_streaming.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Show_IPcam_rtsp_streaming : System.Windows.Window
    {
        string RTSP = "";
        VideoCapture v = null;

        public Show_IPcam_rtsp_streaming(string rtspaddr)
        {
            InitializeComponent();
            RTSP = rtspaddr;
        }

        public void aaa()
        {
            VideoCapture capture = new VideoCapture();
            capture.Open(RTSP);

            using (Mat image = new Mat())
            {
                while (true)
                {
                    if (!capture.Read(image))
                    {
                        Cv2.WaitKey();
                    }

                    if (image.Size().Width > 0 && image.Size().Height > 0)
                    {
                        rtsp_display.Source = image;
                    }

                    if (Cv2.WaitKey(1) >= 0)
                        break;

                }
            }
        }
    }
}
