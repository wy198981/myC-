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
using ParkingModel;
using ParkingInterface;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Data;

namespace UI
{
    /// <summary>
    /// frmPersonnelAdd.xaml 的交互逻辑
    /// </summary>
    public partial class frmPersonnelAdd : SFMControls.WindowBase
    {
        GetServiceData gsd = new GetServiceData();
        ParkingPlateRegister.UpdatePersonnelDataHandler CPHDJfxPersonnelDataHandler;

      
        #region 导入函数
        /// <summary>
        /// 获取系统安装的摄像头列表
        /// </summary>
        /// <returns>返回格式：0|USB视频设备|1|USB摄像头，其中0或1是摄像头ID</returns>

        [DllImport("VedioCapture.dll")]
        public extern static IntPtr GetCameraList();

        /// <summary>
        /// 设置打开的摄像头ID
        /// </summary>
        /// <param name="value">GetCameraList中列举的摄像头ID</param>
        /// <returns>返回0，设置成功</returns>
        [DllImport("VedioCapture.dll")]
        private extern static int SetCameraID(int value);
        /// <summary>
        /// 打开SetCameraID中设置的摄像头
        /// </summary>
        /// <param name="Handle">要显示视频的控件</param>
        /// <returns>摄像头句柄</returns>
        [DllImport("VedioCapture.dll")]
        private extern static IntPtr StartCamera(IntPtr Handle);
        /// <summary>
        /// 截取BMP格式的视频图像
        /// </summary>
        /// <param name="hWndC">摄像头句柄</param>
        /// <param name="cFileName">截取的bmp文件名，或则-1（代表内存图片）</param>
        /// <returns>如果cFileName传入文件名，返回0，截取成功；如果cFileName传入-1，返回bmp图片的句柄（请使用后注意释放）</returns>
        [DllImport("VedioCapture.dll")]
        private extern static int SaveBmp(IntPtr hWndC, string cFileName);

        /// <summary>
        /// 关闭摄像头
        /// </summary>
        /// <param name="hWndC">摄像头句柄</param>
        /// <returns>返回0，设置成功</returns>
        [DllImport("VedioCapture.dll")]
        public extern static int CloseCamera(IntPtr hWndC);
        #endregion


        public frmPersonnelAdd()
        {
            InitializeComponent();
        }

        public frmPersonnelAdd(ParkingPlateRegister.UpdatePersonnelDataHandler _CPHDJfxPersonnelDataHandler)
        {
            InitializeComponent();
            CPHDJfxPersonnelDataHandler = _CPHDJfxPersonnelDataHandler;
        }

        private void WindowBase_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                cmbDeptName.ItemsSource = gsd.GetDepartment();
                cmbDeptName.DisplayMemberPath = "DeptName";
                cmbDeptName.SelectedValuePath = "ID";
                if (cmbDeptName.Items.Count > 0)
                    cmbDeptName.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":PersonnelAdd_Load", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\nPersonnelAdd_Load", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtTelephone_KeyDown(object sender, KeyEventArgs e)
        {
            if (!((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.Subtract))
            {
                e.Handled = true;
            }
        }

        private void txtPhone_KeyDown(object sender, KeyEventArgs e)
        {
            if (!((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)))
            {
                e.Handled = true;
            }
        }

        private void txtIDCard_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyboardDevice.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
            {
                e.Handled = true;
            }


            if (!((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key >= Key.A && e.Key <= Key.Z) || e.Key == Key.LeftShift || e.Key == Key.RightShift))
            {
                e.Handled = true;
            }
        }

        private void txtCarPosCount_KeyDown(object sender, KeyEventArgs e)
        {
            if (!((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)))
            {
                e.Handled = true;
            }
        }

        private void txtCredentials_KeyDown(object sender, KeyEventArgs e)
        {
            if (!((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.Subtract))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// 人员图片
        /// </summary>
        byte[] PersonPhoto = null;
        /// <summary>
        /// 车图片
        /// </summary>
        byte[] CarPhoto = null;

        public UserInfo GetPersonnel()
        {
            UserInfo ui = new UserInfo();
            ui.UserNO = txtUserNO.Text.ToString().Trim();
            ui.UserName = txtUserName.Text.ToString().Trim();
            if (rbtMan.IsChecked == true) { ui.Sex = (rbtMan.Content ?? "男").ToString(); }
            else { ui.Sex = (rbtWoman.Content ?? "女").ToString(); }
            ui.HomeAddress = txtHomeAddress.Text.ToString().Trim();
            if (cmbDeptName.Items.Count > 0)
                ui.DeptID = Convert.ToInt32(cmbDeptName.SelectedValue.ToString());
            ui.DeptName = cmbDeptName.Text;
            ui.Job = cmbJob.Text;
            ui.WorkTime = dpBirthDate.SelectedDate;
            ui.BirthDate = dpWorkDate.SelectedDate;
            ui.IDCard = txtIDCard.Text.ToString().Trim();
            if (rbtMarried.IsChecked == true) { ui.MaritalStatus = (rbtMarried.Content ?? "已婚").ToString(); }
            else { ui.MaritalStatus = (rbtUnmarried.Content ?? "未婚").ToString(); }
            ui.TelNumber = txtTelephone.Text.ToString().Trim();
            ui.MobNumber = txtPhone.Text.ToString().Trim();
            //ui.ZipCode=txtZipCode.
            ui.NativePlace = txtNativePlace.Text.ToString().Trim();
            ui.Skill = txtCredentials.Text.ToString().Trim();
            ui.CarPlaceNo = Convert.ToInt32(txtCarPosCount.Text == "" ? "1" : txtCarPosCount.Text);
            //ui.CarPlaceNo = Convert.ToInt32(cmdCarPlace.Text == "" ? "0" : cmdCarPlace.Text);
    
            //车图  人图

            return ui;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string strUserNO = txtUserNO.Text;
                string strUserName = txtUserName.Text;
                string strDept = cmbDeptName.Text;

                if (cmbCPH.Text != "")
                {
                    if (!CR.CheckUpCPH(cmbCPH.Text))
                    {
                        MessageBox.Show("车牌号不规范!请重新输入！\n\n【" + cmbCPH.Text + "】会引起车牌数据显示错误", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                }

                int ret = gsd.PersonnelAdd(GetPersonnel(), PersonPhoto, cmbCPH.Text, CarPhoto);
                if (ret == 5)
                {
                    MessageBox.Show("编号不能为空!", "提示信息", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                else if (ret == 3)
                {
                    MessageBox.Show("编号已存在!", "提示信息", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                else if (ret == 4)
                {
                    MessageBox.Show("用户名不能为空!", "提示信息", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                else
                {
                    gsd.AddLog(this.Title, "人员编号:" + txtUserNO.Text + "人事信息添加成功!车位个数添加为:" + txtCarPosCount.Text);
                    MessageBox.Show("添加成功!", "提示");
                }
                if (CPHDJfxPersonnelDataHandler != null)
                {
                    CPHDJfxPersonnelDataHandler(strUserNO, strUserName, strDept);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示");
                gsd.AddLog(this.Title + ":btnSave_Click", ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        private void btnCarPic_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog fileDialog = new System.Windows.Forms.OpenFileDialog();
            //筛选
            fileDialog.Filter = "图片(*.jpg)|*.jpg|图片(*.png)|*.png|图片(*.bmp)|*.bmp|All files (*.*)|*.*";
            if (fileDialog.ShowDialog().Equals(System.Windows.Forms.DialogResult.OK))
            {
                System.IO.FileInfo file = new System.IO.FileInfo(fileDialog.FileName);
                long size = file.Length / 1024 / 1024;//文件大小。byte
                if (size > 2)
                {
                    MessageBox.Show("图片不能超过2M！", "提示");
                    return;
                }
                //显示选择文件
                //MessageBox.Show(fileDialog.FileName);
                System.Drawing.Image img = System.Drawing.Image.FromFile(fileDialog.FileName);

                //CarPhotoUrl = fileDialog.FileName;
                Bitmap bmp = new Bitmap(img, new System.Drawing.Size(ptrCar.Width, ptrCar.Height));
                ptrCar.Image = bmp;

                MemoryStream output = new MemoryStream();
                ptrCar.Image.Save(output, System.Drawing.Imaging.ImageFormat.Jpeg);

                CarPhoto = output.GetBuffer();

                img.Dispose();
            }
        }

        private void btnPeoplePic_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog fileDialog = new System.Windows.Forms.OpenFileDialog();
            //筛选
            fileDialog.Filter = "图片(*.jpg)|*.jpg|图片(*.png)|*.png|图片(*.bmp)|*.bmp|All files (*.*)|*.*";
            if (fileDialog.ShowDialog().Equals(System.Windows.Forms.DialogResult.OK))
            {
                FileInfo file = new FileInfo(fileDialog.FileName);
                long size = file.Length / 1024 / 1024;//文件大小。byte
                if (size > 2)
                {
                    MessageBox.Show("图片不能超过2M！", "提示");
                    return;
                }
                //显示选择文件
                //MessageBox.Show(fileDialog.FileName);
                System.Drawing.Image img = System.Drawing.Image.FromFile(fileDialog.FileName);
                //PersonPhotoUrl = fileDialog.FileName;
                Bitmap bmp = new Bitmap(img, new System.Drawing.Size(ptrPerson.Width, ptrPerson.Height));
                ptrPerson.Image = bmp;

                MemoryStream output = new MemoryStream();
                ptrPerson.Image.Save(output, System.Drawing.Imaging.ImageFormat.Jpeg);

                PersonPhoto = output.GetBuffer();

                img.Dispose();
                // bmp.Dispose();
            }
        }

        private void btVedio_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //this.Location = new Point(300, 200);
                listView1.Items.Clear();
                IntPtr ptrret = GetCameraList();
                string str = Marshal.PtrToStringAnsi(ptrret);
                string[] strdev = str.Split('|');
                if (strdev.Length < 2) return;
                int i = 0;
                //ListViewItem lvi = null;

                DataTable dt = new DataTable();
                dt.Columns.Add("ID", typeof(string));
                dt.Columns.Add("Name", typeof(string));


                while (i < strdev.Length)
                {
                    if (strdev[i] != "")
                    {
                        dt.Rows.Add(strdev[i], strdev[i + 1]);
                        //lvi = new ListViewItem(strdev[i]);
                        //lvi.SubItems.Add(strdev[i + 1]);
                        //listView1.Items.Add(lvi);
                    }
                    i = i + 2;
                }
                listView1.ItemsSource = dt.DefaultView;

                int Result = SetCameraID(0);
                //this.listView1.Select();
                //listView1.Items[0].Selected = true;
                if (Result != 0)
                {
                    MessageBox.Show("设备打开失败");
                    return;
                }
                camerah = StartCamera(ptrCarmera.Handle);

                this.Width = 1300;

                this.Left = (SystemParameters.WorkArea.Width - this.Width) / 2;
                //this.Top = SystemParameters.WorkArea.Height - 720;
            }
            catch
            {
                MessageBox.Show("打开摄像头失败!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }          
        }

        private void btnColose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

       // bool IfPaizhao = false;
        IntPtr camerah = IntPtr.Zero;


        private void btnPhotograph_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (camerah != IntPtr.Zero)
                {
                    //传入文件路径
                    // SaveBmp(camerah, AppPath + DateTime.Now.ToString("yyyyMMddHHmmss") + "ss.bmp");
                    //传入-1，返回内存图片
                    IntPtr iptr = (IntPtr)SaveBmp(camerah, "-1");
                    Bitmap bmp = Bitmap.FromHbitmap(iptr);
                    ptrPerson.Image = GetNewImage(Bitmap.FromHbitmap(iptr), ptrPerson.Width, ptrPerson.Height);

                    MemoryStream output = new MemoryStream();
                    ptrPerson.Image.Save(output, System.Drawing.Imaging.ImageFormat.Jpeg);
                    PersonPhoto = output.GetBuffer();

                   // IfPaizhao = true;

                    //CR.UpdateAppConfig("PzDevc", listView1.SelectedItems[0].Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private System.Drawing.Image GetNewImage(Bitmap bmap, int newWidth, int newHeight)
        {
            System.Drawing.Image oldImg = bmap;
            System.Drawing.Image newImg = oldImg.GetThumbnailImage(newWidth, newHeight, null, IntPtr.Zero); // 对原图片进行缩放 
            return newImg;
        }


        private void btnExitPhotograph_Click(object sender, RoutedEventArgs e)
        {
            this.Width = 700;
            this.Left = (SystemParameters.WorkArea.Width - this.Width) / 2;
            CloseCamera(camerah);
        }

        private void listView1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listView1.SelectedItems == null || listView1.SelectedItems.Count == 0) return;

            var vr = listView1.SelectedItem as DataRowView;

            string DevIndex = vr["ID"].ToString();
            //string DevIndex = listView1.SelectedItems[0].Text;
            CloseCamera(camerah);
            int Result = SetCameraID(int.Parse(DevIndex));
            if (Result != 0)
            {
                MessageBox.Show("设备打开失败");
                return;
            }
            camerah = StartCamera(ptrCarmera.Handle);
        }

        private void ptrPerson_Click(object sender, EventArgs e)
        {
            if (PersonPhoto != null)
            {
                frmCamPoto frmCam = new frmCamPoto(PersonPhoto);
                frmCam.Owner = this;
                frmCam.ShowDialog();
            }
        }

        private void txtUserNO_KeyDown(object sender, KeyEventArgs e)
        {
            if (!((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)))
            {
                e.Handled = true;
            }
        }

    }
}
