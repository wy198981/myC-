using ParkingInterface;
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

namespace UI
{
    /// <summary>
    /// ParkingHoliday.xaml 的交互逻辑
    /// </summary>
    public partial class ParkingHoliday : Window
    {
        GetServiceData gsd = new GetServiceData();
        public ParkingHoliday()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ImageBrush berriesBrush = new ImageBrush();
            berriesBrush.ImageSource = new BitmapImage(new Uri(System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, @"Resources\Main0.jpg"), UriKind.Absolute));

            this.Background = berriesBrush;

            GetBin();
        }

        private void GetBin()
        {
            lstHoliday.Items.Clear();
            List<ParkingModel.Holiday> lstHd = gsd.GetHoliday("Holiday");

            if (lstHd != null && lstHd.Count > 0)
            {
                foreach (var v in lstHd)
                {
                    lstHoliday.Items.Add(v.Dates.ToString("yyyy-MM-dd"));
                }
            }

            lstWork.Items.Clear();

            lstHd = gsd.GetHoliday("WorkDay");

            if (lstHd != null && lstHd.Count > 0)
            {
                foreach (var v in lstHd)
                {
                    lstWork.Items.Add(v.Dates.ToString("yyyy-MM-dd"));
                }
            }
        }

        private void btnDeleteHoliday_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstHoliday.SelectedItem != null && lstHoliday.SelectedItem.ToString() != "")
                {
                    if (MessageBox.Show("是否删除假日：" + lstHoliday.SelectedItem, "提示", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        gsd.DeleteHoliday(Convert.ToDateTime(lstHoliday.SelectedItem), "Holiday");
                        GetBin();
                    }

                }
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":btnHolidayDelete_Click", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnAddHoliday_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!lstWork.Items.Contains(dtHoliday.Text))
                {

                    gsd.AddHoliday(dtHoliday.SelectedDate.Value, "Holiday");
                    GetBin();
                }
                else
                {
                    MessageBox.Show("该日期已经设置工作日", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":btnHolidayADD_Click", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnAddWorkDay_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!lstWork.Items.Contains(dtWork.Text))
                {
                    gsd.AddHoliday(Convert.ToDateTime(dtWork.Text), "WorkDay");
                    GetBin();
                }
                else
                {
                    MessageBox.Show("该日期已经设置假日", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":btnAddWork_Click", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnDeleteWorkDay_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstWork.SelectedItem != null && lstWork.SelectedItem.ToString() != "")
                {
                    if (MessageBox.Show("是否删除工作日：" + lstWork.SelectedItem, "提示", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        gsd.DeleteHoliday(Convert.ToDateTime(lstWork.SelectedItem), "WorkDay");
                        GetBin();
                    }
                }
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":btnDelWork_Click", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        
    }
}
