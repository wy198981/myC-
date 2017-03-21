using ParkingCommunication.CameraSDK.ZNYKT5;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// WhiteList_Form.xaml 的交互逻辑
    /// </summary>
    public partial class WhiteList_Form : SFMControls.WindowBase
    {
        public WhiteList_Form()
        {
            InitializeComponent();
        }

        private int m_hLPRClient = 0;
        private DataTable dt;
        private VzClientSDK.VZLPRC_WLIST_QUERY_CALLBACK m_wlistCB = null;
        public IntPtr hwndMain;

        private const int MSG_PLATE_INFO = 0x902;

        private void WindowBase_Loaded(object sender, RoutedEventArgs e)
        {
            //this.dgList.Columns.Add("车牌 ID");
            //this.dgList.Columns.Add("车牌号");
            //this.dgList.Columns.Add("是否启用");
            //this.dgList.Columns.Add("过期时间");
            //this.dgList.Columns.Add("是否报警");
            //this.dgList.View = System.Windows.Forms.View.Details;
            //dgList.FullRowSelect = true;
            //this.dgList.GridLines = true;
            //dgList.LabelEdit = true;
            // Allow the user to rearrange columns.
            //dgList.CanUserReorderColumns = true;
            //dgList.Columns[0].Width = 70;
            //dgList.Columns[1].Width = 90;
            //dgList.Columns[2].Width = 70;
            //dgList.Columns[3].Width = 155;
            //dgList.Columns[4].Width = 70;

            dt = new DataTable();
            dt.Columns.Add("CardNO", typeof(string));
            dt.Columns.Add("CPH", typeof(string));
            dt.Columns.Add("EndDate", typeof(string));
            dt.Columns.Add("IsEnabled", typeof(int));
            dt.Columns.Add("IsAlarm", typeof(int));

            dgList.IsReadOnly = true;
            dgList.AutoGenerateColumns = false;
            dgList.ItemsSource = dt.DefaultView;

            //hwndMain = this.Handle;
            hwndMain = ((HwndSource)PresentationSource.FromVisual(dgList)).Handle;
        }

        public void Setm_hLPRClient(int m_hLPRClient_)
        {
            m_hLPRClient = m_hLPRClient_;
            //SearchText();
        }


        private void addbt_Click(object sender, RoutedEventArgs e)
        {
            //             WhiteListAdd_Form listform = new WhiteListAdd_Form();
            //             listform.Setm_hLPRClient(m_hLPRClient);
            //             listform.StartPosition = FormStartPosition.CenterScreen;
            //             listform.GetForm2(this);
            //             listform.Show();
        }

        private void changebt_Click(object sender, RoutedEventArgs e)
        {
            int index = 0;
            DataRow drSelected = null;

            WhiteListChange_Form listform = new WhiteListChange_Form();
            listform.Setm_hLPRClient(m_hLPRClient);

            if (null != this.dgList.SelectedItem)
            {
                index = this.dgList.SelectedIndex;
                drSelected = (dgList.SelectedItem as DataRowView).Row;
            }
            else
            {
                return;
            }


            int bEnable = Int32.Parse(drSelected["IsEnabled"].ToString());
            int bAlarm = Int32.Parse(drSelected["IsAlarm"].ToString());
            listform.GetForm2(this);
            listform.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            listform.Show();
            listform.UpdatePalate(UInt32.Parse(drSelected["CardNO"].ToString()),
                drSelected["CPH"].ToString(),
                bEnable == 0 ? false : true,
                drSelected["EndDate"].ToString(),
                bAlarm == 0 ? false : true
                );
        }

        private void deletebt_Click(object sender, RoutedEventArgs e)
        {
            int index = -1;
            DataRow drSelected = null;

            if (null == this.dgList.SelectedItem)
            {
                return;
            }

            //index = this.dgList.SelectedItems[0].Index;
            drSelected = (dgList.SelectedItem as DataRowView).Row;

            string strPlateID = drSelected["CPH"].ToString();
            VzClientSDK.VzLPRClient_WhiteListDeleteVehicle(m_hLPRClient, strPlateID);
            ShowDeleteResult(strPlateID);
        }
        private delegate void ShowDeleteCrossThread();
        //查询结果显示
        public void ShowDeleteResult(String strPlateID)
        {
            ShowDeleteCrossThread DeleteDelegate = delegate()
            {
                int index = 0;
                DataRow drSelected = null;

                if (null != this.dgList.SelectedItem)
                {
                    //index = this.dgList.SelectedItems[0].Index;
                    drSelected = (dgList.SelectedItem as DataRowView).Row;
                    drSelected.Delete();
                }
            };
            dgList.Dispatcher.Invoke(DeleteDelegate);
        }
        private delegate void ClearListViewThread();
        //查询清除显示
        public void ClearListView()
        {
            ClearListViewThread ClearDelegate = delegate()
            {
                //dgList.Items.Clear();
                //dgList.Clear();
                dt.Rows.Clear();
            };
            dgList.Dispatcher.Invoke(ClearDelegate);
        }

        private void OnWlistQueryResult(VzClientSDK.VZLPRC_WLIST_CB_TYPE type, IntPtr pLP, IntPtr pCustomer, IntPtr pUserData)
        {
            if (pLP != IntPtr.Zero)
            {
                ShowSearchResult(pLP);
            }
            if (pCustomer != IntPtr.Zero)
            {
                VzClientSDK.VZ_LPR_WLIST_CUSTOMER wlistCustomer = (VzClientSDK.VZ_LPR_WLIST_CUSTOMER)Marshal.PtrToStructure(pCustomer, typeof(VzClientSDK.VZ_LPR_WLIST_CUSTOMER));
            }
        }

        private delegate void ShowSearchCrossThread();
        //查询结果显示
        public void ShowSearchResult(IntPtr pLP)
        {
            VzClientSDK.VZ_LPR_WLIST_VEHICLE wlistVehicle = (VzClientSDK.VZ_LPR_WLIST_VEHICLE)Marshal.PtrToStructure(pLP, typeof(VzClientSDK.VZ_LPR_WLIST_VEHICLE));
            ShowSearchCrossThread SearchDelegate = delegate()
            {
                //ListViewItem item = new ListViewItem();
                //item.SubItems[0].Text = (wlistVehicle.uVehicleID.ToString());
                //item.SubItems.Add(wlistVehicle.strPlateID.ToString());
                //item.SubItems.Add(wlistVehicle.bEnable.ToString());

                ////item.SubItems.Add(wlistVehicle.uVehicleID.ToString());
                //item.SubItems.Add(wlistVehicle.struTMOverdule.nYear.ToString() + "-" + wlistVehicle.struTMOverdule.nMonth.ToString()
                //    + "-" + wlistVehicle.struTMOverdule.nMDay + " " + wlistVehicle.struTMOverdule.nHour.ToString()
                //    + ":" + wlistVehicle.struTMOverdule.nMin.ToString() + ":" + wlistVehicle.struTMOverdule.nSec.ToString());

                //item.SubItems.Add(wlistVehicle.bAlarm.ToString());
                //dgList.Items.Add(item);

                DataRow drNew;

                drNew = dt.NewRow();
                drNew["CardNO"] = wlistVehicle.uVehicleID.ToString();
                drNew["CPH"] = wlistVehicle.strPlateID.ToString();
                drNew["EndDate"] = string.Format("{0}-{1}-{2} {3}:{4}:{5}", wlistVehicle.struTMOverdule.nYear, wlistVehicle.struTMOverdule.nMonth, wlistVehicle.struTMOverdule.nMDay,
                    wlistVehicle.struTMOverdule.nHour, wlistVehicle.struTMOverdule.nMin, wlistVehicle.struTMOverdule.nSec);
                drNew["IsEnabled"] = wlistVehicle.bEnable;
                drNew["IsAlarm"] = wlistVehicle.bAlarm;
                dt.Rows.Add(drNew);
            };

            dgList.Dispatcher.Invoke(SearchDelegate);
        }

        public void SearchText()
        {

            ClearListView();
            // 设置一体机白名单结果回调
            m_wlistCB = new VzClientSDK.VZLPRC_WLIST_QUERY_CALLBACK(OnWlistQueryResult);
            VzClientSDK.VzLPRClient_WhiteListSetQueryCallBack(m_hLPRClient, m_wlistCB, IntPtr.Zero);

            VzClientSDK.VZ_LPR_WLIST_LIMIT limit = new VzClientSDK.VZ_LPR_WLIST_LIMIT();
            VzClientSDK.VZ_LPR_WLIST_LOAD_CONDITIONS conditions = new VzClientSDK.VZ_LPR_WLIST_LOAD_CONDITIONS();
            limit.limitType = VzClientSDK.VZ_LPR_WLIST_LIMIT_TYPE.VZ_LPR_WLIST_LIMIT_TYPE_RANGE;

            //limit.pRangeLimit = null;
            // 起始范围的定义
            VzClientSDK.VZ_LPR_WLIST_RANGE_LIMIT rangLimit = new VzClientSDK.VZ_LPR_WLIST_RANGE_LIMIT();
            rangLimit.startIndex = startPage * 50;
            rangLimit.count = 50;

            limit.pRangeLimit = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VzClientSDK.VZ_LPR_WLIST_RANGE_LIMIT)));
            Marshal.StructureToPtr(rangLimit, limit.pRangeLimit, true);

            conditions.pLimit = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VzClientSDK.VZ_LPR_WLIST_LIMIT)));
            Marshal.StructureToPtr(limit, conditions.pLimit, true);
            //conditions.pSortType = null;

            VzClientSDK.VZ_LPR_WLIST_SEARCH_CONSTRAINT searchConstraint = new VzClientSDK.VZ_LPR_WLIST_SEARCH_CONSTRAINT();
            searchConstraint.key = "PlateID";
            searchConstraint.search_string = searchtext.Text.ToString();
            VzClientSDK.VZ_LPR_WLIST_SEARCH_WHERE searchWhere = new VzClientSDK.VZ_LPR_WLIST_SEARCH_WHERE();
            searchWhere.pSearchConstraints = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VzClientSDK.VZ_LPR_WLIST_SEARCH_CONSTRAINT)));
            Marshal.StructureToPtr(searchConstraint, searchWhere.pSearchConstraints, true);
            searchWhere.searchConstraintCount = 1;
            searchWhere.searchType = VzClientSDK.VZ_LPR_WLIST_SEARCH_TYPE.VZ_LPR_WLIST_SEARCH_TYPE_LIKE;

            conditions.pSearchWhere = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VzClientSDK.VZ_LPR_WLIST_SEARCH_WHERE)));
            Marshal.StructureToPtr(searchWhere, conditions.pSearchWhere, true);

            int nRecordCount = 0;
            int ret0 = VzClientSDK.VzLPRClient_WhiteListGetVehicleCount(m_hLPRClient, ref nRecordCount, ref searchWhere);

            int ret = VzClientSDK.VzLPRClient_WhiteListLoadVehicle(m_hLPRClient, ref conditions);

            if (nRecordCount > 50)
            {
                if ((nRecordCount - startPage * 50) < 50)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        button3.IsEnabled = false;
                    });
                }
                else
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        button3.IsEnabled = true;
                    });
                }

                //int iYS = nRecordCount % 50;
                //if (iYS > 0)
                //{
                //    label1.Text = ((nRecordCount / 50) + 1).ToString();
                //}
                //else
                //{
                //    label1.Text = (nRecordCount / 50).ToString();
                //}
            }
            //else
            //{
            //    this.Dispatcher.Invoke(() =>
            //    {
            //        button3.IsEnabled = false;
            //        //label1.Text = "1";
            //        lblStep.Content = string.Format("{0} / 1", startPage + 1);
            //    });
            //}

            this.Dispatcher.Invoke(() =>
            {
                int TotalPage = (nRecordCount / 50) + (nRecordCount % 50 > 0 ? 1 : 0);

                lblStep.Content = string.Format("{0} / {1}", (startPage >= TotalPage ? TotalPage : startPage + 1), TotalPage);
                button2.IsEnabled = startPage > 0;
                button3.IsEnabled = startPage < TotalPage - 1;
            });

            Marshal.FreeHGlobal(conditions.pLimit);
            Marshal.FreeHGlobal(searchWhere.pSearchConstraints);
            Marshal.FreeHGlobal(conditions.pSearchWhere);

        }
        int startPage = 0;
        private void searchbt_Click(object sender, RoutedEventArgs e)
        {
            //label4.Text = (startPage + 1) + "页";
            startPage = 0;
            SearchText();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("确认删除所有脱机车牌", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                VzClientSDK.VzLPRClient_WhiteListClearCustomersAndVehicles(m_hLPRClient);

                SearchText();
            }

        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            //label4.Text = startPage + "页";
            if (startPage == 0)
            {
                button2.IsEnabled = false;
                return;
            }
            startPage--;
            SearchText();
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            startPage++;
            if (startPage > 0)
            {
                button2.IsEnabled = true;
            }
            SearchText();
            //label4.Text = (startPage + 1) + "页";
        }
    }
}