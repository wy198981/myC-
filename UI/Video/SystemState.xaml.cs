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
using SFMControls;
using ParkingCommunication.CameraSDK.ZNYKT5;

namespace UI.Video
{
    /// <summary>
    /// SystemState.xaml 的交互逻辑
    /// </summary>
    public partial class SystemState : SFMControls.WindowBase
    {
        private int m_hLPRClient = 0;

        public SystemState()
        {
            InitializeComponent();
        }

        public SystemState(int hLPRClient)
        {
            InitializeComponent();
            m_hLPRClient = hLPRClient;
        }

        private void WindowBase_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void btnPartSet_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("确认恢复摄像机部分参数?", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                if (MessageBox.Show("确认恢复摄像机部分参数?", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    if (MessageBox.Show("确认恢复摄像机部分参数?", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        int iRst = VzClientSDK.VzLPRClient_RestoreConfig(m_hLPRClient);
                        if (iRst == 0)
                        {
                            MessageBox.Show("摄像机部分参数恢复成功", "提示");
                        }
                    }
                }
            }
        }

        private void btnAllSet_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("确认恢复摄像机所有参数?", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                if (MessageBox.Show("确认恢复摄像机所有参数?", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    if (MessageBox.Show("确认恢复摄像机所有参数?", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        int iRst = VzClientSDK.VzLPRClient_RestoreConfig(m_hLPRClient);
                        if (iRst == 0)
                        {
                            MessageBox.Show("摄像机所有参数恢复成功", "提示");
                        }
                    }
                }
            }
        }

        private void btnRead_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog fileDialog = new System.Windows.Forms.OpenFileDialog();
            fileDialog.Filter = "升级文件(*.bin)|*.bin|All files (*.*)|*.*";
            if (fileDialog.ShowDialog().Equals(System.Windows.Forms.DialogResult.OK))
            {
                txtFileName.Text = fileDialog.FileName;
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (txtFileName.Text.Trim() == "")
            {
                MessageBox.Show("请选择升级文件？", "提示");
                return;
            }
            if (MessageBox.Show("确认升级摄像机程序？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                VzClientSDK.VzLPRClient_Update(m_hLPRClient, txtFileName.Text);
            }
        }
    }
}
