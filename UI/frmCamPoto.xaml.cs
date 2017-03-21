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
using System.IO;

namespace UI
{
    /// <summary>
    /// frmCamPoto.xaml 的交互逻辑
    /// </summary>
    public partial class frmCamPoto : SFMControls.WindowBase
    {
        byte[] ImgBys = null;

        public frmCamPoto()
        {
            InitializeComponent();
        }

        public frmCamPoto(byte[] Imabyts)
        {
            InitializeComponent();
            ImgBys = Imabyts;
        }

        private void WindowBase_Loaded(object sender, RoutedEventArgs e)
        {
            if (ImgBys != null)
            {
                MemoryStream ms1 = new MemoryStream(ImgBys);
                ptrPerson.Image =System.Drawing.Image.FromStream(ms1);//显示
            }
        }

        private void ptrPerson_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
