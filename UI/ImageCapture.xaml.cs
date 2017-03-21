using ParkingModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace UI
{
    /// <summary>
    /// ImageCapture.xaml 的交互逻辑
    /// </summary>
    public partial class ImageCapture : SFMControls.WindowBase
    {
        public ImageCapture()
        {
            InitializeComponent();
        }

        public Bitmap bmp;
        IntPtr camerah = IntPtr.Zero;

        #region 导入函数
        /// <summary>
        /// 设置打开的摄像头ID
        /// </summary>
        /// <param name="value">GetCameraList中列举的摄像头ID</param>
        /// <returns>返回0，设置成功</returns>
        [DllImport("VedioCapture.dll")]
        private extern static int SetCameraID(int value);
        /// <summary>
        /// 打开SetCameraID中设置的摄像头
        /// </summary>
        /// <param name="Handle">要显示视频的控件</param>
        /// <returns>摄像头句柄</returns>
        [DllImport("VedioCapture.dll")]
        private extern static IntPtr StartCamera(IntPtr Handle);
        /// <summary>
        /// 截取BMP格式的视频图像
        /// </summary>
        /// <param name="hWndC">摄像头句柄</param>
        /// <param name="cFileName">截取的bmp文件名，或则-1（代表内存图片）</param>
        /// <returns>如果cFileName传入文件名，返回0，截取成功；如果cFileName传入-1，返回bmp图片的句柄（请使用后注意释放）</returns>
        [DllImport("VedioCapture.dll")]
        private extern static int SaveBmp(IntPtr hWndC, string cFileName);
        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                int Result = SetCameraID(0);
                if (Result != 0)
                {
                    MessageBox.Show("设备打开失败");
                    return;
                }
                //camerah = StartCamera(((HwndSource)PresentationSource.FromVisual(grdVideo)).Handle);
                camerah = StartCamera(picVideo.Handle);
            }
            catch (Exception ex)
            {
                MessageBox.Show("设备打开失败: " + ex.Message);
            }
        }

        private void btnCapture_Click(object sender, RoutedEventArgs e)
        {
            if (camerah != IntPtr.Zero)
            {
                //传入文件路径
                // SaveBmp(camerah, AppPath + DateTime.Now.ToString("yyyyMMddHHmmss") + "ss.bmp");
                //传入-1，返回内存图片
                IntPtr iptr = (IntPtr)SaveBmp(camerah, "-1");

                bmp = Bitmap.FromHbitmap(iptr);

                imgCapture.Source = SFMControls.ControlHelper.HbitmapToSource(iptr);
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            //if (bmp != null)
            //{
            //    if (Model.sImageSavePath.Substring(Model.sImageSavePath.Length - 1) != @"\")
            //    {
            //        Model.sImageSavePath = Model.sImageSavePath + @"\";
            //    }
            //    ImagePath = Model.sImageSavePath + DateTime.Now.ToString("yyyyMMdd") + @"\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";

            //    // ImagePathstr = AppPath + "Person_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".bmp";
            //    bmp.Save(ImagePath);
            //    //Parking.ParkingMonitoring.File = ImagePathstr;
            //    bmp.Dispose();
            //}
            this.DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
