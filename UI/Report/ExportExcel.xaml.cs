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
using ParkingInterface;


namespace UI.Report
{
    /// <summary>
    /// ExportExcel.xaml 的交互逻辑
    /// </summary>
    public partial class ExportExcel : SFMControls.WindowBase
    {

        #region Fields
        GetServiceData gsd = new GetServiceData();
        DataGrid dgvData;
        DataTable dtHasAllLeft, dtHasAllRight;
        #endregion

        public ExportExcel()
        {
            InitializeComponent();
        }

        public ExportExcel(DataGrid dgv)
        {
            InitializeComponent();
            dgvData = dgv;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // dtHasAllLeft = ds.Tables[2];
                dtHasAllLeft = new DataTable("cart");
                DataColumn dc1 = new DataColumn("Id", Type.GetType("System.String"));
                DataColumn dc2 = new DataColumn("Name", Type.GetType("System.String"));
                dtHasAllLeft.Columns.Add(dc1);
                dtHasAllLeft.Columns.Add(dc2);
                for (int count = 0; count < dgvData.Columns.Count; count++)
                {
                    DataRow dr = dtHasAllLeft.NewRow();
                    dr["Id"] = dgvData.Columns[count].SortMemberPath;
                    dr["Name"] = dgvData.Columns[count].Header;
                    dtHasAllLeft.Rows.Add(dr);
                }
                dtHasAllRight = dtHasAllLeft.Clone();
                lstData.ItemsSource = dtHasAllLeft.DefaultView;
                lstData.DisplayMemberPath = "Name";
                lstData.SelectedValuePath = "Id";

                lstSelectData.ItemsSource = dtHasAllRight.DefaultView;
                lstSelectData.DisplayMemberPath = "Name";
                lstSelectData.SelectedValuePath = "Id";
            }
            catch(Exception ex)
            {
                gsd.AddLog(this.Title + ":ExportExcel_Load", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\nExportExcel_Load", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnRight_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstData.SelectedItems.Count > 0)
                {
                    String str = lstData.SelectedValue.ToString();

                    //lstData.SetSelected(lstData.SelectedIndex, false);

                    //lstData.c

                    foreach (DataRow dr in dtHasAllLeft.Rows)
                    {
                        if (dr["ID"].ToString() == str)
                        {
                            DataRow drNew = dtHasAllRight.NewRow();
                            drNew.ItemArray = dr.ItemArray;
                            dtHasAllLeft.Rows.Remove(dr);
                            dtHasAllRight.Rows.Add(drNew);
                            lstSelectData.ItemsSource = dtHasAllRight.DefaultView;
                            lstSelectData.DisplayMemberPath = "Name";
                            lstSelectData.SelectedValuePath = "Id";
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":btnRight_Click", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\nbtnRight_Click", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAllRight_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (DataRowView _Row in lstData.Items)
                {
                    string name = _Row.Row[lstData.DisplayMemberPath].ToString();
                    string id = _Row.Row[lstData.SelectedValuePath].ToString();
                    DataRow dr = dtHasAllRight.NewRow();
                    dr["Id"] = id;
                    dr["Name"] = name;
                    dtHasAllRight.Rows.Add(dr);
                }
                dtHasAllLeft.Clear();
                lstData.ItemsSource = dtHasAllLeft.DefaultView;
                lstData.DisplayMemberPath = "Name";
                lstData.SelectedValuePath = "Id";
                lstSelectData.ItemsSource = dtHasAllRight.DefaultView;
                lstSelectData.DisplayMemberPath = "Name";
                lstSelectData.SelectedValuePath = "Id";
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":btnAllRight_Click", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\nbtnAllRight_Click", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnLeft_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstSelectData.SelectedItems.Count > 0)
                {
                    String str = lstSelectData.SelectedValue.ToString();

                    //lstSelectData.SetSelected(lstSelectData.SelectedIndex, false);
                    
                    foreach (DataRow dr in dtHasAllRight.Rows)
                    {
                        if (dr["ID"].ToString() == str)
                        {
                            DataRow drNew = dtHasAllLeft.NewRow();
                            drNew.ItemArray = dr.ItemArray;
                            dtHasAllRight.Rows.Remove(dr);
                            dtHasAllLeft.Rows.Add(drNew);

                            lstData.ItemsSource = dtHasAllLeft.DefaultView;
                            lstData.DisplayMemberPath = "Name";
                            lstData.SelectedValuePath = "Id";
                            break;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                gsd.AddLog(this.Title + ":btnLeft_Click", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\nbtnLeft_Click", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAllLeft_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (DataRowView _Row in lstSelectData.Items)
                {
                    string name = _Row.Row[lstSelectData.DisplayMemberPath].ToString();
                    string id = _Row.Row[lstSelectData.SelectedValuePath].ToString();
                    DataRow dr = dtHasAllLeft.NewRow();
                    dr["Id"] = id;
                    dr["Name"] = name;
                    dtHasAllLeft.Rows.Add(dr);
                }
                dtHasAllRight.Clear();
                lstSelectData.ItemsSource = dtHasAllRight.DefaultView;
                lstSelectData.DisplayMemberPath = "Name";
                lstSelectData.SelectedValuePath = "Id";
                lstData.ItemsSource = dtHasAllLeft.DefaultView;
                lstData.DisplayMemberPath = "Name";
                lstData.SelectedValuePath = "Id";
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":btnAllLeft_Click", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\nbtnAllLeft_Click", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            DataTableSort(dtHasAllRight, lstSelectData.SelectedIndex, "up");
        }

        private void btnDown_Click(object sender, RoutedEventArgs e)
        {
            DataTableSort(dtHasAllRight, lstSelectData.SelectedIndex, "down");
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveAsExcel(dgvData);
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":btnExport_Click", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\nbtnExport_Click", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        public void DataTableSort(System.Data.DataTable dt, int index, String sort)
        {
            if (index >= 0)
            {
                DataTable dtm = dt.Clone();
                DataRow drCurrent = dt.Rows[index];
                DataRow drm = dtm.NewRow();
                switch (sort)
                {
                    case "up":
                        {
                            if (index > 0)
                            {
                                DataRow drLast = dt.Rows[index - 1];
                                drm.ItemArray = dt.Rows[index].ItemArray;
                                dt.Rows[index].ItemArray = dt.Rows[index - 1].ItemArray;
                                dt.Rows[index - 1].ItemArray = drm.ItemArray;
                               // lstSelectData.SetSelected(index - 1, true);
                                lstSelectData.SelectedIndex = index - 1;
                            }

                        } break;
                    case "down":
                        {
                            if (index + 1 < lstSelectData.Items.Count)
                            {
                                DataRow drNext = dt.Rows[index + 1];
                                drm.ItemArray = dt.Rows[index].ItemArray;
                                dt.Rows[index].ItemArray = dt.Rows[index + 1].ItemArray;
                                dt.Rows[index + 1].ItemArray = drm.ItemArray;
                                lstSelectData.SelectedIndex = index + 1;
                            }
                        } break;
                }
            }
        }


        private void SaveAsExcel(DataGrid dgvAgeWeekSex)
        {
            try
            {
                System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
                saveFileDialog.Filter = "Execl files (*.xls)|*.xls";
                saveFileDialog.FilterIndex = 0;
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.CreatePrompt = false;
                saveFileDialog.Title = "Export Excel File To";
                saveFileDialog.CheckFileExists = false;
                saveFileDialog.FileName = DateTime.Now.ToString("yyyy-MM-dd HHmmss");
                if (saveFileDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;


                AppLibrary.WriteExcel.XlsDocument doc = new AppLibrary.WriteExcel.XlsDocument();
                doc.FileName = saveFileDialog.FileName;
                string SheetName = string.Empty;
                //记录条数
                int mCount = 1;
                //每个SHEET的数量
                // int inv = 1;
                //计算当前多少个SHEET
                int k = 1;//Convert.ToInt32(Math.Round(Convert.ToDouble(mCount / inv))) + 1;


                SheetName = "停车卡片信息";
                AppLibrary.WriteExcel.Worksheet sheet = doc.Workbook.Worksheets.Add(SheetName);
                AppLibrary.WriteExcel.Cells cells = sheet.Cells;


             
                //第一行表头
                int cellrows = 1;
                foreach (DataRowView _Row in lstSelectData.Items)
                {
                    cells.Add(1, cellrows, _Row.Row[lstSelectData.DisplayMemberPath]);

                    for (int i = 0; i < dgvAgeWeekSex.Columns.Count; i++)
                    {
                        if (dgvAgeWeekSex.Columns[i].Header.ToString() == _Row.Row[lstSelectData.DisplayMemberPath].ToString())
                        {
                            for (int j = 0; j < dgvAgeWeekSex.Items.Count; j++)
                            {
                                var selectItem = dgvAgeWeekSex.Items[j] as DataRowView;
                                string header = _Row.Row[lstData.SelectedValuePath].ToString();
                                string value = selectItem[header].ToString() ?? "";
                                cells.Add(j + 2, cellrows, value);
                            }
                        }
                    }
                    cellrows++;
                }
                doc.Save(saveFileDialog.FileName, true);
            }

            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                
            }
        }
    }
}
