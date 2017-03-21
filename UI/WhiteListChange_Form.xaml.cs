using ParkingCommunication.CameraSDK.ZNYKT5;
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
    /// WhiteListChange_Form.xaml 的交互逻辑
    /// </summary>
    public partial class WhiteListChange_Form : SFMControls.WindowBase
    {
        public WhiteListChange_Form()
        {
            InitializeComponent();
        }

        private int m_hLPRClient = 0;
        private uint Update_lVehicleID;
        WhiteList_Form form2;

        public void GetForm2(WhiteList_Form form2_)
        {
            form2 = form2_;
        }
        public void UpdatePalate(uint lVehicleID, string PlateID, bool bEnable,
            string strOverdule, bool bAlarm)
        {
            Update_lVehicleID = lVehicleID;
            if (string.Compare(strOverdule, " ") == 0)
                ShowView(PlateID, bEnable, "2015年01月01日 00:00:00", bAlarm);
            else
                ShowView(PlateID, bEnable, strOverdule, bAlarm);
        }

        private delegate void ShowThread();
        //显示所选信息
        public void ShowView(string PlateID, bool bEnable,
            string strOverdule, bool bAlarm)
        {
            ShowThread ShowDelegate = delegate()
            {
                strPalatID.Text = PlateID;
                if (bEnable)
                    isenable.IsChecked = true;
                if (bAlarm)
                    isalarm.IsChecked = true;
                //DateTime dt1 = Convert.ToDateTime(strOverdule);
                DateTime dte = Convert.ToDateTime(strOverdule);
                datalist.Text = dte.ToString();
            };
            this.Dispatcher.Invoke(ShowDelegate);
        }

        public void Setm_hLPRClient(int m_hLPRClient_)
        {
            m_hLPRClient = m_hLPRClient_;
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            if (null == datalist.SelectedDate)
            {
                MessageBox.Show("请选择过期时间");
                return;
            }

            VzClientSDK.VZ_LPR_WLIST_VEHICLE wlistVehicle = new VzClientSDK.VZ_LPR_WLIST_VEHICLE();
            wlistVehicle.uVehicleID = Update_lVehicleID;
            if (isalarm.IsChecked ?? false)
                wlistVehicle.bAlarm = 1;
            else
                wlistVehicle.bAlarm = 0;
            if (isenable.IsChecked ?? false)
                wlistVehicle.bEnable = 1;
            else
                wlistVehicle.bEnable = 0;
            wlistVehicle.uCustomerID = 1;
            wlistVehicle.strPlateID = strPalatID.Text.ToString();
            wlistVehicle.struTMOverdule.nHour = Int16.Parse(datalist.SelectedDate.Value.Hour.ToString());
            wlistVehicle.struTMOverdule.nMin = Int16.Parse(datalist.SelectedDate.Value.Minute.ToString());
            wlistVehicle.struTMOverdule.nSec = Int16.Parse(datalist.SelectedDate.Value.Second.ToString());
            wlistVehicle.struTMOverdule.nYear = Int16.Parse(datalist.SelectedDate.Value.Year.ToString());
            wlistVehicle.struTMOverdule.nMonth = Int16.Parse(datalist.SelectedDate.Value.Month.ToString());
            wlistVehicle.struTMOverdule.nMDay = Int16.Parse(datalist.SelectedDate.Value.Day.ToString());
            wlistVehicle.bUsingTimeSeg = 1;
            wlistVehicle.bEnableTMOverdule = 1;

            VzClientSDK.VzLPRClient_WhiteListUpdateVehicleByID(m_hLPRClient, ref wlistVehicle);
            // 
            form2.SearchText();

            this.Close();
        }
    }
}
