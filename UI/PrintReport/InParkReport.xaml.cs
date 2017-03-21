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
using CrystalDecisions.CrystalReports.Engine;

namespace UI.PrintReport
{
    /// <summary>
    /// InParkReport.xaml 的交互逻辑
    /// </summary>
    public partial class InParkReport : SFMControls.WindowBase
    {
        public InParkReport()
        {
            InitializeComponent();
        }

        public InParkReport(List<ParkingModel.CarIn> lstCI, ParkingModel.ReportCar rc)
        {
            InitializeComponent();

            try
            {
                //ParkingModel.CrystalReport.CarIn rpt = new ParkingModel.CrystalReport.CarIn();
                ParkingModel.CrystalReport.RptCarIn rpt = new ParkingModel.CrystalReport.RptCarIn();
             
                rpt.SetDataSource(lstCI);

                if (rc != null)
                {
                    //月租
                    ((TextObject)rpt.ReportDefinition.ReportObjects["Text43"]).Text = rc.MthCount.ToString();
                    //免费车
                    ((TextObject)rpt.ReportDefinition.ReportObjects["Text42"]).Text = rc.FreCount.ToString();
                    //临时车
                    ((TextObject)rpt.ReportDefinition.ReportObjects["Text41"]).Text = rc.TmpCount.ToString();
                    //储值车
                    ((TextObject)rpt.ReportDefinition.ReportObjects["Text40"]).Text = rc.StrCount.ToString();
                    //总数
                    ((TextObject)rpt.ReportDefinition.ReportObjects["Text39"]).Text = rc.TotalCount.ToString();

                }
                else
                {
                    ////月租
                    //((TextObject)rpt.ReportDefinition.ReportObjects["Text43"]).Text = "";
                    ////免费车
                    //((TextObject)rpt.ReportDefinition.ReportObjects["Text42"]).Text = "";
                    ////临时车
                    //((TextObject)rpt.ReportDefinition.ReportObjects["Text41"]).Text = "";
                    ////储值车
                    //((TextObject)rpt.ReportDefinition.ReportObjects["Text40"]).Text = "";
                    ////总数
                    //((TextObject)rpt.ReportDefinition.ReportObjects["Text39"]).Text = "";
                }

                CrystalReportViewer1.ViewerCore.ReportSource = rpt;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("InParkReport:" + ex.Message + ":" + ex.StackTrace);
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
