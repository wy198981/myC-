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
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;


namespace UI.PrintReport
{
    /// <summary>
    /// CarChargeReport.xaml 的交互逻辑
    /// </summary>
    public partial class CarChargeReport : SFMControls.WindowBase
    {
        public CarChargeReport()
        {
            InitializeComponent();
        }

        public CarChargeReport(List<ParkingModel.CarOut> lstCO, ParkingModel.ReportCar rc)
        {
            InitializeComponent();

            try
            {
                ParkingModel.CrystalReport.RptCarChargeReport rpt = new ParkingModel.CrystalReport.RptCarChargeReport();

                rpt.SetDataSource(lstCO);

                if (rc != null)
                {
                    ((TextObject)rpt.ReportDefinition.ReportObjects["Text53"]).Text = rc.MthCount.ToString();
                    //免费车
                    ((TextObject)rpt.ReportDefinition.ReportObjects["Text52"]).Text = rc.FreCount.ToString();
                    //临时车
                    ((TextObject)rpt.ReportDefinition.ReportObjects["Text51"]).Text = rc.TmpCount.ToString();
                    //月临车
                    ((TextObject)rpt.ReportDefinition.ReportObjects["Text50"]).Text = rc.MtpCount.ToString();
                    //储值车
                    ((TextObject)rpt.ReportDefinition.ReportObjects["Text49"]).Text = rc.StrCount.ToString();
                    //其它车
                    ((TextObject)rpt.ReportDefinition.ReportObjects["Text48"]).Text = rc.OptCount.ToString();
                    //总收费
                    ((TextObject)rpt.ReportDefinition.ReportObjects["TxtSumSF"]).Text = rc.TotalSFJE.ToString("0.0");
                    //总应收费
                    ((TextObject)rpt.ReportDefinition.ReportObjects["TxtSumYSJE"]).Text = rc.TotalYSJE.ToString("0.0");
                    //超时收费
                    ((TextObject)rpt.ReportDefinition.ReportObjects["TxtOSFJE"]).Text = rc.TotalOTSFJE.ToString("0.0");
                }

                CrystalReportViewer1.ViewerCore.ReportSource = rpt;

            }
            catch (Exception ex)
            {
                MessageBox.Show("CarChargeReport:" + ex.Message + ":" + ex.StackTrace);
            }
        }


 
        private DataSet CreatTable(DataTable dt)
        {
            //创建一个Table 和 DataSet
            DataTable table = new DataTable();
            DataSet ds = new DataSet();
            //创建一个列
            DataColumn column1 = new DataColumn();
            column1.DataType = System.Type.GetType("System.String");
            column1.ColumnName = "InOperator";
   
            DataColumn column2 = new DataColumn();
            column2.DataType = System.Type.GetType("System.String");
            column2.ColumnName = "OutOperator";

            DataColumn column3 = new DataColumn();
            column3.DataType = System.Type.GetType("System.String");
            column3.ColumnName = "ChineseName";

            DataColumn column4 = new DataColumn();
            column4.DataType = System.Type.GetType("System.String");
            column4.ColumnName = "CardNO";

            DataColumn column5 = new DataColumn();
            column5.DataType = System.Type.GetType("System.String");
            column5.ColumnName = "CPH";

            DataColumn column6 = new DataColumn();
            column6.DataType = System.Type.GetType("System.DateTime");
            column6.ColumnName = "InTime";

            DataColumn column7 = new DataColumn();
            column7.DataType = System.Type.GetType("System.DateTime");
            column7.ColumnName = "OutTime";

            DataColumn column8 = new DataColumn();
            column8.DataType = System.Type.GetType("System.Decimal");
            column8.ColumnName = "SFJE";

            DataColumn column9 = new DataColumn();
            column9.DataType = System.Type.GetType("System.Decimal");
            column9.ColumnName = "Balance";

            DataColumn column10 = new DataColumn();
            column10.DataType = System.Type.GetType("System.Decimal");
            column10.ColumnName = "YSJE";

            DataColumn column11 = new DataColumn();
            column11.DataType = System.Type.GetType("System.String");
            column11.ColumnName = "FreeReason";

            DataColumn column12 = new DataColumn();
            column12.DataType = System.Type.GetType("System.String");
            column12.ColumnName = "OutGateName";

            //
            //把列添加进去
            table.Columns.Add(column1);
            table.Columns.Add(column2);
            table.Columns.Add(column3);
            table.Columns.Add(column4);
            table.Columns.Add(column5);
            table.Columns.Add(column6);
            table.Columns.Add(column7);
            table.Columns.Add(column8);
            table.Columns.Add(column9);
            table.Columns.Add(column10);
            table.Columns.Add(column11);
            table.Columns.Add(column12);


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //OutOperator
                DataRow row = table.NewRow();
                row["InOperator"] = dt.Rows[i]["InOperator"];
                row["OutOperator"] = "出口通道4";
                row["ChineseName"] = "临时卡";
                row["CardNO"] = dt.Rows[i]["CardNO"];
                row["CPH"] = dt.Rows[i]["CPH"];
                row["InTime"] = dt.Rows[i]["InTime"];
                row["OutTime"] = dt.Rows[i]["OutTime"];
                row["SFJE"] = dt.Rows[i]["SFJE"];
                row["Balance"] = dt.Rows[i]["Balance"];
                row["YSJE"] = dt.Rows[i]["YSJE"];
                row["FreeReason"] = dt.Rows[i]["FreeReason"];
                row["OutGateName"] = dt.Rows[i]["OutGateName"];
                table.Rows.Add(row);
            }
            ds.Tables.Add(table);
            return ds;
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
