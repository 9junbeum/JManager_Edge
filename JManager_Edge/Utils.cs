using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Emgu.CV;
using Emgu.CV.CvEnum;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections;
using System.Drawing;

namespace JManager_Edge
{
    public static class Utils
    {

        [DllImport("gdi32")]
        private static extern int DeleteObject(IntPtr o);

        /// <summary>
        /// Convert an IImage to a WPF BitmapSource. The result can be used in the Set Property of Image.Source
        /// </summary>
        /// <param name="image">The Emgu CV Image</param>
        /// <returns>The equivalent BitmapSource</returns>

        public static BitmapSource ToBitmapSource(IInputArray image)
        {
            try
            {
                using (InputArray ia = image.GetInputArray())
                using (Mat m = ia.GetMat())
                using (System.Drawing.Bitmap source = m.ToBitmap())
                {
                    IntPtr ptr = source.GetHbitmap(); //obtain the Hbitmap

                    BitmapSource bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                        ptr,
                        IntPtr.Zero,
                        Int32Rect.Empty,
                        System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

                    DeleteObject(ptr); //release the HBitmap
                    return bs;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public static List<float[]> ArrayTo2DList(Array array)
        {
            IEnumerator en = array.GetEnumerator();
            int rows = array.GetLength(0);
            int cols = array.GetLength(1);
            List<float[]> result = new List<float[]>();
            List<float> t = new List<float>();
            for (int i = 0; i < rows; i++)
            {
                t.Clear();
                for (int j = 0; j < cols; j++)
                {
                    t.Add(float.Parse(array.GetValue(i, j).ToString()));
                }
                result.Add(t.ToArray());
            }
            return result;
        }
        public class BBox
        {
            public int ImageWidth = 0;
            public int ImageHeight = 0;
            public float CenterX = 0f;
            public float CenterY = 0f;
            public float Width = 0f;
            public float Height = 0f;
            public float Score = 0f;
            public int ClassId = 0;
            public PointF LeftTop
            {
                get => new PointF(CenterX - (Width / 2f), CenterY - (Height / 2f));
            }
            public PointF RightBottom
            {
                get => new PointF(CenterX + (Width / 2f), CenterY + (Height / 2f));
            }

            public RectangleF Rect
            {
                get => new RectangleF(CenterX - (Width / 2f), CenterY - (Height / 2f), CenterX + (Width / 2f), CenterY + (Height / 2f));
            }
            public override string ToString()
            {
                return $"cls : {ClassId}, score={Score}, box={Rect.ToString()}";
            }

        }


    }
}
