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
    /// WindowTest.xaml 的交互逻辑
    /// </summary>
    public partial class WindowTest : WindowWithSkin
    {
        public WindowTest()
        {
            InitializeComponent();
        }

        bool Loaded = false;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Loaded)
            {
                Application.Current.Resources.MergedDictionaries.Clear();
                //Application.Current.Resources.Clear();
            }
            else
            {
                string SkinFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Skins\\DefaultSkin.xaml");
                if (System.IO.File.Exists(SkinFile))
                {
                    ResourceDictionary rd = new ResourceDictionary();
                    rd.Source = new Uri("Skins/DefaultSkin.xaml", UriKind.RelativeOrAbsolute);

                    Application.Current.Resources.MergedDictionaries.Add(rd);
                    //Application.Current.Resources = rd;
                }
            }

            Loaded = !Loaded;
            this.Title = Loaded.ToString();
        }
    }
}
