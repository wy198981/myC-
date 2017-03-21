using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace UI.PrintReport
{
    /// <summary>
    /// WorkJiaoJieReport.xaml 的交互逻辑
    /// </summary>
    public partial class WorkJiaoJieReport : SFMControls.WindowBase
    {
        public WorkJiaoJieReport()
        {
            InitializeComponent();
        }

        public WorkJiaoJieReport(List<ParkingModel.Handover> lstHandover)
        {
            InitializeComponent();

            try
            {
                ParkingModel.CrystalReport.JiaoJieReport rpt = new ParkingModel.CrystalReport.JiaoJieReport();
                rpt.SetDataSource(lstHandover);
                CrystalReportViewer1.ViewerCore.ReportSource = rpt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // 设置全屏
            this.WindowState = System.Windows.WindowState.Normal;
            this.WindowStyle = System.Windows.WindowStyle.SingleBorderWindow;
            this.ResizeMode = System.Windows.ResizeMode.CanResizeWithGrip;
            //this.Topmost = true;

            this.Left = 0.0;
            this.Top = 0.0;
            this.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            this.Height = System.Windows.SystemParameters.PrimaryScreenHeight;
        }
    }
}
