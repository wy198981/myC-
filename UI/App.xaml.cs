using ParkingInterface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace UI
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        static System.Threading.Mutex mutex;

        protected override void OnStartup(StartupEventArgs e)
        {
            mutex = new System.Threading.Mutex(true, System.Windows.Forms.Application.ProductName);
            if (!mutex.WaitOne(0, false))
            {
                MessageBox.Show("程序已启动!");
                Environment.Exit(0);
                //Application.Current.Shutdown();
                //这里用什么方式 显示主窗体呢，
            }
            //else
            //{
            //    mutex.ReleaseMutex();
            //    //启动主程序
            //}
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message, "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            GetServiceData gsd = new GetServiceData();
            gsd.AddLog("App:", e.Exception.Message + "\r\n" + e.Exception.StackTrace);
            e.Handled = true;
        }
    }
}
