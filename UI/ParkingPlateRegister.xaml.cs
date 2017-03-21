﻿using System;
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
using System.ComponentModel;
using System.Collections.ObjectModel;
using ParkingCommunication;
using ParkingModel;
using ParkingInterface;
using System.Data;
using System.Printing;

namespace UI
{
  
    /// <summary>
    /// ParkingPlateRegister.xaml 的交互逻辑
    /// </summary>
    public partial class ParkingPlateRegister : SFMControls.WindowBase
    {
        GetServiceData gsd = new GetServiceData();
      

        public delegate void PostponeHandler(Dictionary<string, object> dic = null);
        public delegate void UpdatePersonnelDataHandler(string sUserNO, string sUserName, string sDept);
        public delegate void SelectPersonnelDataHandler(string where);

        public ParkingPlateRegister()
        {
            InitializeComponent();
            
        }

        
        int count = 0;
        private void chkSubSelect_Click(object sender, RoutedEventArgs e)
        {
            CheckBox tb = e.OriginalSource as CheckBox;
            if (tb.IsChecked == true)
            {
                count++;
                string Jihao = tb.Tag.ToString();
                string str1 = sum.Substring(0, Convert.ToInt32(Jihao) - 1); //ab
                string str2 = sum.Substring(Convert.ToInt32(Jihao));
                sum = str1 + "0" + str2;
            }
            else
            {
                count--;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            flag = "add";
            cmbHeader.Text = Model.LocalProvince;
            cmdCardType.SelectedIndex = 0;
            dtpStart.Text = DateTime.Now.ToShortDateString();
            dtpEnd.Text = DateTime.Now.AddMonths(1).ToShortDateString();
            txtCarPlace.Clear();
            txtRemarks.Clear();
            txtUserName.Clear();
            txtMobileNumber.Clear();
            txtAddress.Clear();
            txtMoney.Text="0.00";
            txtCardYJ.Text = "0.00";
            btnSave.IsEnabled = true;

            txtCardYJ.IsEnabled = true;
            txtMoney.IsEnabled = true;
            txtAddress.IsEnabled = true;
            txtMobileNumber.IsEnabled = true;
            dtpEnd.IsEnabled = true;
            dtpStart.IsEnabled = true;
            
            txtUserName.IsEnabled = true;
            cmdCardType.IsEnabled = true;
            dgvCardIssue.SelectedIndex = -1;

            AutoAddNO();
        }

       
        void AutoAddNO()
        {
            //自动卡号和用户编号
            if (chkAutoCardNo.IsChecked == true)
            {
                txtCarNo.IsEnabled = false;
                List<CardIssue> lstCI = gsd.GetAutoCardNo();
                if (lstCI.Count > 0)
                {
                    int max = Convert.ToInt32(lstCI[0].CardNO.Substring(2, 6));
                    txtCarNo.Text = "88" + (max + 1).ToString().PadLeft(6, '0'); // 000012
                }
                else
                {
                    txtCarNo.Text = "88000001";
                }
            }

            if (chkAutoUserNo.IsChecked == true)
            {
                cmbUserNO.IsEnabled = false;
                List<UserInfo> lstUI = gsd.GetAutoUsernoPersonnel();
                if (lstUI.Count > 0)
                {
                    int max = Convert.ToInt32(lstUI[0].UserNO.Substring(1, 5));
                    cmbUserNO.Text = "A" + (max + 1).ToString().PadLeft(5, '0');
                }
                else
                {
                    cmbUserNO.Text = "A00001";
                }
            }
        }


        void Bin(Dictionary<string, object> dic = null)
        {
            dgvCardIssue.ItemsSource = gsd.GetCarChePIss(dic);
        }


        string flag = "";
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                List<ParkJHSet> lstPJS = gsd.GetCCJiHao();


                lvwCtrlNumber.ItemsSource = lstPJS;// 更新所有机号

                ImageBrush berriesBrush = new ImageBrush();
                berriesBrush.ImageSource = new BitmapImage(new Uri(System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, @"Resources\Main0.jpg"), UriKind.Absolute));

                this.Background = berriesBrush;

                for (int i = 0; i < 128; i++)
                {
                    sum += "1";
                }

                CR.BinDic(gsd.GetCardType());
                

                dgvCardIssue.ItemsSource = gsd.GetCarChePIss(); // 更新列表信息
                cmbUserNO.ItemsSource = gsd.GetPersonnel();// 获取用户的No
                cmbUserNO.DisplayMemberPath = "UserNO";
                cmbUserNO.SelectedValuePath = "UserNO";


                List<CardTypeDef> lstCTD = gsd.GetGetFXCardTypeToTrue();//获取车辆类型
                cmdCardType.ItemsSource = lstCTD;
                cmdCardType.DisplayMemberPath = "CardType";
                cmdCardType.SelectedValuePath = "Identifying";

                if (lstCTD != null && lstCTD.Count > 0)
                    cmdCardType.SelectedIndex = 0;

                dtpStart.Text = DateTime.Now.ToShortDateString();
                dtpEnd.Text = DateTime.Now.AddMonths(1).ToShortDateString();

               
                lstShow.ItemsSource = lstPJS;

                chkAutoUserNo.IsChecked = true;
                chkAutoCardNo.IsChecked = true;

                btnAdd.IsEnabled = false;
                btnSave.IsEnabled = true;
                btnCancel.IsEnabled = false;
                btnPrint.IsEnabled = false;
                btnExit.IsEnabled = true;

                AutoAddNO();

                cmbHeader.Text = Model.LocalProvince;
            
                ShowRights();

                flag = "add";
            }
            catch(Exception ex)
            {
                gsd.AddLog(this.Title + ":Window_Loaded", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\n" + "Window_Loaded", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool bInCPH = false;
                string cph = cmbHeader.Text + txtCarNumber.Text;
                if (gsd.GetInCPH(cph) > 0)
                {
                    if (System.Windows.Forms.MessageBox.Show(" 确定注销【场内】车牌:" + cph + "\r\n 系统自动会把此车牌转换为临时车\r\n 并且按照当前时间开始计费", " 提示", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.OK)
                    {
                        string strCarNoNew = CR.GetAutoCPHCardNO(0);
                        gsd.UpdateInCPH(strCarNoNew, txtCarNo.Text.Trim(), cph);
                        bInCPH = true;
                    }
                    else
                    {
                        return;
                    }
                }

                if (bInCPH || System.Windows.Forms.MessageBox.Show("确定注销车牌:" + cph + "  车辆编号:" + txtCarNo.Text, " 提示", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.OK)
                {
                    int relsutint = gsd.UpdateICState(txtCarNo.Text, "5");
                    if (relsutint > 0)
                    {
                        gsd.UpdateOutCard("00000000000000000000000000000000000000000000000000", txtCarNo.Text);
                        // bll.UpdateIDLost(txtCarNo.Text, 0);
                        MessageBox.Show("车牌注销成功!", "btnCancel_Click");
                        dgvCardIssue.ItemsSource = gsd.GetCarChePIss();

                        btnAdd.IsEnabled = true;
                        btnSave.IsEnabled = false;
                        btnCancel.IsEnabled = false;
                        btnPrint.IsEnabled = true;

                    }
                    else
                    {
                        MessageBox.Show("车牌注销失败,请重试!", "btnCancel_Click");
                    }
                }
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":btnCancel_Click", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\n" + "btnCancel_Click", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (flag == "add")
            {
                if (!isAdd)
                {
                    MessageBox.Show("无新增权限", "提示");
                    return;
                }
            }
            else if (flag == "edit")
            {
                List<Rights> lstRs = gsd.GetRightsByName(this.Title, "btnUpdate");
                if (lstRs == null || lstRs.Count == 0)
                {
                    MessageBox.Show("无修改权限", "提示");
                    return;
                }
                else
                {
                    if (!lstRs[0].CanOperate)
                    {
                        MessageBox.Show("无修改权限", "提示");
                        return;
                    }
                }
            }
            try
            {
                CarValidMachine();
                if (cmdCardType.Text.Trim() == "")
                {
                    MessageBox.Show("请选择车辆类型!", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!MachineChead)
                {
                    MessageBox.Show("请选择停车场机号!", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (txtUserName.Text.Trim() == "")
                {
                    MessageBox.Show("请输入人员姓名!", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (txtMoney.Text.Trim() == "")
                {
                    txtMoney.Text = "0";
                }

                if (txtCardYJ.Text.Trim() == "")
                {
                    txtCardYJ.Text = "0";
                }

                CardIssue cardmodel = BinModel();

                if (chkDCWDC.IsChecked == true)
                {
                    string strCPHS = "";
                    string strCPHR = "";
                    foreach (var item in lstCPH.Items)
                    {

                        cardmodel.CPH = item.ToString();
                        strCPHS = strCPHS == "" ? cardmodel.CPH : strCPHS + "," + cardmodel.CPH;
                        List<CardIssue> lstCI = gsd.GetAutoCardNo();
                        if (lstCI.Count > 0)
                        {
                            int cardnomax = Convert.ToInt32(lstCI[0].CardNO.Substring(2, 6));
                            cardmodel.CardNO = "88" + (cardnomax + 1).ToString().PadLeft(6, '0');
                        }
                        else
                        {
                            cardmodel.CardNO = "88000001";
                        }

                        //判断车牌是否重复
                        if (gsd.IfCPHExitsbt(item.ToString()))
                        {
                            if (MessageBox.Show("车牌已重复是否覆盖添加！\n\n【" + item.ToString() + "】", "提示", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.No)
                            {
                                return;
                            }
                        }

                        //判断车牌是否是已入场的临时车,如果是,则把入场记录改为发行卡类  2015-12-26  2016-06-21
                        if (CR.GetCardType(cmdCardType.Text, 0).Substring(0, 3) == "Mth" || CR.GetCardType(cmdCardType.Text, 0).Substring(0, 3) == "Fre" || CR.GetCardType(cmdCardType.Text, 0).Substring(0, 3) == "Str")
                        {
                            int iRet = gsd.GetInRecordIsTmp(item.ToString());   //对比车牌后6位
                            if (iRet > 0)   //有入场记录
                            {
                                if (System.Windows.Forms.MessageBox.Show("该车是【已入场的临时车】，若继续操作，则此车出场时将不按临时车收费！\n请确认是否继续？", "重要提示", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.No)
                                {
                                    return;
                                }
                                if (System.Windows.Forms.MessageBox.Show("该车是【已入场的临时车】，若继续操作，则此车出场时将不按临时车收费！\n请【再次】确认是否继续？", "重要提示", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.No)
                                {
                                    return;
                                }
                                if (System.Windows.Forms.MessageBox.Show("该车是【已入场的临时车】，若继续操作，则此车出场时将不按临时车收费！\n请【最后】确认是否继续？", "重要提示", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.No)
                                {
                                    return;
                                }

                                //则把入场记录改为发行卡类 取车牌后6位对比
                                gsd.UpdateInCPHCardType(item.ToString(), CR.GetCardType(cmdCardType.Text, 0), txtCarNo.Text.Trim());
                                gsd.AddLog("车牌登记", item.ToString() + " 已入场临时车，改为" + cmdCardType.Text.Trim());
                            }
                        }

                        cardmodel.ID = 0;

                        int ret = 0;
                        if (chkAutoCardNo.IsChecked == true)
                        {
                            ret = gsd.PersonnelAddCpdj(GetPersonnel());
                            if (ret > 0)
                            {

                            }
                            else
                            {
                               
                            }
                        }
                        

                        if (ICAddCpDj(cardmodel) > 0)
                        {
                            //int count = gsd.PersonnelAddCpdj(GetPersonnel());


                            strCPHR = strCPHR == "" ? cardmodel.CPH : strCPHR + "," + cardmodel.CPH;
                            gsd.AddLog("车牌登记", item.ToString() + " 车牌添加成功");
                            dgvCardIssue.ItemsSource = gsd.GetCarChePIss();

                            //if (count == 2)
                            //{
                            //    //MessageBox.Show(Language.LanguageXML.GetName("Personnel/p31"), Language.LanguageXML.GetName("CR/Prompt"));
                            //}
                            //else if (count == 3)
                            //{
                            //    //MessageBox.Show(Language.LanguageXML.GetName("Personnel/p32"), Language.LanguageXML.GetName("CR/Prompt"));
                            //}
                            //else if (count == 4)
                            //{
                            //    //MessageBox.Show(Language.LanguageXML.GetName("Personnel/p29"), Language.LanguageXML.GetName("CR/Prompt"));
                            //}
                            //else if (count == 1)
                            //{
                            //    strCPHR = strCPHR == "" ? cardmodel.CPH : strCPHR + "," + cardmodel.CPH;

                            //    dgvCardIssue.ItemsSource = gsd.GetCarChePIss().DefaultView;
                            //    //DgvinDataSource();
                            //}

                        }
                    }

                    MessageBox.Show("添加车牌：【" + strCPHS + "】\n\n成功车牌：【" + strCPHR + "】", "提示", MessageBoxButton.OK, MessageBoxImage.Information);

                    txtCarCount.Text = "";

                    lstCPH.Items.Clear();

                    btnAdd.IsEnabled = true;
                    btnSave.IsEnabled = false;
                    btnCancel.IsEnabled = true;
                    btnPrint.IsEnabled = true;
                    chkDCWDC.IsChecked = false;
                }
                else
                {
                    string cph = cmbHeader.Text + txtCarNumber.Text;
                    if (cph == "")
                    {
                        System.Windows.Forms.MessageBox.Show("请输入车牌!", "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                        return;
                    }

                    if (!CR.CheckUpCPH(cph))
                    {
                        MessageBox.Show("车牌号不规范!请重新输入！\n\n【" + cph + "】会引起车牌数据显示错误", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    //string strCPH = "京津冀晋蒙辽吉黑沪苏浙皖闽赣鲁豫鄂湘粤桂琼渝川贵云藏陕甘青宁新港澳台警使武领学民航";
                    //if (cph != "" && cph.Length != 7)
                    //{
                    //    if (cph.Length == 8 && cph.Substring(0, 2) == "WJ")
                    //    {

                    //    }
                    //    else
                    //    {
                    //        System.Windows.Forms.MessageBox.Show("车牌号不规范!请重新输入！\n\n【" + cph + "】会引起车牌数据显示错误", "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    //        return;
                    //    }
                    //}
                    //else if (cph.Length == 7 && cph.Substring(0, 2) == "WJ" && strCPH.Contains(cph.Substring(2, 1)))
                    //{
                    //    System.Windows.Forms.MessageBox.Show("武警车牌号不规范!请重新输入！\n\n【" + cph + "】会引起车牌数据显示错误", "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    //    return;
                    //}

                    //if (!CR.IsLetterOrFigure(cph.Substring(1)))
                    //{
                    //    System.Windows.Forms.MessageBox.Show("车牌号不规范!请重新输入！\n\n【" + cph + "】会引起车牌数据显示错误", "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    //    return;
                    //}


                    if (flag == "add")
                    {
                        //判断卡片号码是否重复  2016-06-28 th
                        if (gsd.IfCardNOExitsbt(txtCarNo.Text.Trim()))
                        {
                            if (System.Windows.Forms.MessageBox.Show("车辆编号已重复是否覆盖添加！\n\n【" + txtCarNo.Text + "】", "提示", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.No)
                            {
                                return;
                            }
                        }

                        //判断车牌是否重复
                        if (gsd.IfCPHExitsbt(cph))
                        {
                            if (System.Windows.Forms.MessageBox.Show("车牌已重复是否覆盖添加！\n\n【" + cph + "】", "提示", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.No)
                            {
                                return;
                            }

                        }

                        //判断车牌是否是已入场的临时车,如果是,则把入场记录改为发行卡类  2015-12-26  2016-06-21
                        if (CR.GetCardType(cmdCardType.Text, 0).Substring(0, 3) == "Mth" || CR.GetCardType(cmdCardType.Text, 0).Substring(0, 3) == "Fre" || CR.GetCardType(cmdCardType.Text, 0).Substring(0, 3) == "Str")
                        {
                            int iRet = gsd.GetInRecordIsTmp(cph);   //对比车牌后6位
                            if (iRet > 0)   //有入场记录
                            {
                                if (System.Windows.Forms.MessageBox.Show("该车是【已入场的临时车】，若继续操作，则此车出场时将不按临时车收费！\n请确认是否继续？", "重要提示", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.No)
                                {
                                    return;
                                }
                                if (System.Windows.Forms.MessageBox.Show("该车是【已入场的临时车】，若继续操作，则此车出场时将不按临时车收费！\n请【再次】确认是否继续？", "重要提示", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.No)
                                {
                                    return;
                                }
                                if (System.Windows.Forms.MessageBox.Show("该车是【已入场的临时车】，若继续操作，则此车出场时将不按临时车收费！\n请【最后】确认是否继续？", "重要提示", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.No)
                                {
                                    return;
                                }

                                //则把入场记录改为发行卡类 取车牌后6位对比
                                gsd.UpdateInCPHCardType(cph, CR.GetCardType(cmdCardType.Text, 0), txtCarNo.Text.Trim());
                                gsd.AddLog("车牌登记", cph + " 已入场临时车，改为" + cmdCardType.Text.Trim());
                            }
                        }

                        cardmodel.ID = 0;
                    }
                    else if (flag == "edit")
                    {
                        //ID
                        //cardmodel.ID = Convert.ToInt32(ID);
                    }

                    int ret = 0;
                    if (chkAutoCardNo.IsChecked == true)
                    {
                        ret = gsd.PersonnelAddCpdj(GetPersonnel());
                    }
                    else
                    {


                    }

                    if (ICAddCpDj(cardmodel) > 0)
                    {
                        //int count = gsd.PersonnelAddCpdj(GetPersonnel());
                        //if (count == 2)
                        //{
                        //   // MessageBox.Show(Language.LanguageXML.GetName("Personnel/p31"), Language.LanguageXML.GetName("CR/Prompt"));
                        //}
                        //else if (count == 3)
                        //{
                        //    //MessageBox.Show(Language.LanguageXML.GetName("Personnel/p32"), Language.LanguageXML.GetName("CR/Prompt"));
                        //}
                        //else if (count == 4)
                        //{
                        //    //MessageBox.Show(Language.LanguageXML.GetName("Personnel/p29"), Language.LanguageXML.GetName("CR/Prompt"));
                        //}
                        //else if (count == 1)
                        //{
                        //    MessageBox.Show("车牌添加成功!", "提示");
                        //    btnAdd.IsEnabled = true;
                        //    btnSave.IsEnabled = false;
                        //    btnCancel.IsEnabled = true;
                        //    btnPrint.IsEnabled = true;
                        //    txtCarCount.Text = "";

                        //    //DgvinDataSource();
                        //    dgvCardIssue.ItemsSource = gsd.GetCarChePIss().DefaultView;
                        //}

                        dgvCardIssue.ItemsSource = gsd.GetCarChePIss();
                        MessageBox.Show("车牌添加成功!", "提示");
                        btnAdd.IsEnabled = true;
                        btnSave.IsEnabled = false;
                        btnCancel.IsEnabled = true;
                        btnPrint.IsEnabled = true;
                        txtCarCount.Text = "";
                        gsd.AddLog("车牌登记", cph + " 车牌添加成功---车辆号码：" + txtCarNo.Text.Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":btnSave_Click", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\n" + "btnSave_Click", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAddCPH_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string cph = cmbHeader.Text + txtCarNumber.Text;
                if (cph == "")
                {
                    System.Windows.Forms.MessageBox.Show("请输入车牌!", "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return;
                }
                string strCPH = "京津冀晋蒙辽吉黑沪苏浙皖闽赣鲁豫鄂湘粤桂琼渝川贵云藏陕甘青宁新港澳台警使武领学民航";
                if (cph != "" && cph.Length != 7)
                {
                    if (cph.Length == 8 && cph.Substring(0, 2) == "WJ")
                    {

                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("车牌号不规范!请重新输入！\n\n【" + cph + "】会引起车牌数据显示错误", "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                        return;
                    }
                }
                else if (cph.Length == 7 && cph.Substring(0, 2) == "WJ" && strCPH.Contains(cph.Substring(2, 1)))
                {
                    System.Windows.Forms.MessageBox.Show("武警车牌号不规范!请重新输入！\n\n【" + cph + "】会引起车牌数据显示错误", "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    return;
                }

                if (!CR.IsLetterOrFigureNotIO(cph.Substring(1)))
                {
                    System.Windows.Forms.MessageBox.Show("车牌号不规范!请重新输入！\n\n【" + cph + "】会引起车牌数据显示错误", "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    return;
                }

                if (!lstCPH.Items.Contains(cph))
                {
                    lstCPH.Items.Add(cph);
                    lstCPH.SelectedIndex = lstCPH.Items.Count - 1;
                }
                cph = Model.LocalProvince;
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":btnAddCPH_Click", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\n" + "btnAddCPH_Click", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void chkDCWDC_Click(object sender, RoutedEventArgs e)
        {
            if (chkDCWDC.IsChecked == true)
            {
                btnAddCPH.Visibility = Visibility.Visible;
                lstCPH.Visibility = Visibility.Visible;
            }
            else
            {
                btnAddCPH.Visibility = Visibility.Hidden;
                lstCPH.Visibility = Visibility.Hidden;
            }
        }


        string strTemp1;
        bool MachineChead = false;
        string sum = "";


        private void CarValidMachine()
        {
            MachineChead = false;
            sum = "";
            for (int i = 0; i < 128; i++)
                sum += "1";

            for (int i = 0; i < lvwCtrlNumber.Items.Count; i++)
            {
                ListViewItem lv = (ListViewItem)lvwCtrlNumber.ItemContainerGenerator.ContainerFromItem(lvwCtrlNumber.Items[i]);
                if (lv.IsSelected == true)
                {
                    MachineChead = true;
                    var v = lvwCtrlNumber.Items[i] as ParkJHSet;
                    string Jihao = v.CtrlNumber.ToString();
                    string str1 = sum.Substring(0, Convert.ToInt32(Jihao) - 1); //ab
                    string str2 = sum.Substring(Convert.ToInt32(Jihao)); //d
                    sum = str1 + "0" + str2; //abmd
                }
            }

            //string strJihao = "";
            //for (int j = 0; j < sum.Length / 4; j++)
            //{
            //    strJihao += string.Format("{0:X}", Convert.ToInt32(sum.Substring(j * 4, 4), 2));
            //}
            //strTemp1 = strJihao;
        }

        /// <summary>
        /// 绑定Model
        /// </summary>
        /// <returns></returns>
        public CardIssue BinModel()
        {
            CardIssue model = new CardIssue();
            model.CardNO = txtCarNo.Text.Trim().PadLeft(8, '0');
            model.UserNO = cmbUserNO.Text;
            model.CardState = "0";
            model.CardYJ = Convert.ToDecimal(txtCardYJ.Text.Trim() ?? "0");

            model.CardState = "0";
            model.SubSystem = "10000";
            model.CarCardType = CR.GetCardType(cmdCardType.Text.Trim(), 0);
            model.CarIssueDate = DateTime.Now;
            model.CarIssueUserCard = Model.sUserCard;
            model.Balance = Convert.ToDecimal(txtMoney.Text.Trim() ?? "0");

            //model.CarValidEndDate = Convert.ToDateTime(dtpEnd.SelectedDate.Value.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss"));

            model.CarValidEndDate = Convert.ToDateTime(dtpEnd.SelectedDate.Value.ToString("yyyy-MM-dd"));
            model.CarValidStartDate = Convert.ToDateTime(dtpStart.SelectedDate.Value.ToString("yyyy-MM-dd"));
            model.CPH = cmbHeader.Text + txtCarNumber.Text;
            model.CarType = cmbCarType.Text;

            model.CarPlace = txtCarPlace.Text.Trim();

            //保存为2进制数
            model.CarValidMachine = sum;

            model.CarValidZone = "0000000001000000";
            model.CarMemo = txtRemarks.Text.Trim();
           // model.CardIDNO = textBox3.Text.Trim();
            model.IssueUserCard = Model.sUserCard;
            model.IssueDate = DateTime.Now;


            model.HolidayLimited = "0000000";
            
            model.Tempnumber = 0;
            
            model.UserInfo = "";
            model.TimeTeam = "";

            return model;
        }

        public int ICAddCpDj(CardIssue model)
        {
            if (model.ID != 0)
            {
                return gsd.UpdateCPdjfx(model);
            }
            else
            {
                gsd.DeleteFaXing(model.CardNO);
               

                Money money = new Money();
                money.CardNO = model.CardNO;
                money.OptDate = DateTime.Now;
                money.OperatorNO = Model.sUserCard;
                money.SFJE = model.Balance;
              
                money.OptType = "1";

              
                money.NewStartDate = model.CarValidStartDate;
                money.NewEndDate = model.CarValidEndDate;
                money.LastEndDate = model.CarValidStartDate;

                gsd.Add(money);
               
                if (model.CardYJ > 0)
                {
                    money.SFJE = model.CardYJ;
                    money.OptType = "E";
                    gsd.Add(money);
                }

                //延期表无

                //Model.ICVALIDModel ICmodel = new Model.ICVALIDModel();
                //ICmodel.CardNO = model.CardNO;
                //ICmodel.OptDate = DateTime.Now;
                //ICmodel.NewStartDate = model.CarValidStartDate;
                //ICmodel.NewEndDate = model.CarValidEndDate;
                //ICmodel.OperatorCardNO = Model.sUserCard;
                //ICmodel.OptType = "t";
                //gsd.Add(ICmodel);

                //2015-07-29
                if (model.CarCardType == "Opt")
                {
                    gsd.AddOperator(model);  //插入操作员表
                }

                if (model.CarCardType.Substring(0, 3) != "Str")
                {
                    model.Balance = 0;
                }

                return gsd.Addchdj(model);
            }
        }

        private UserInfo GetPersonnel()
        {
            UserInfo PersonnelmModel = new UserInfo();

            PersonnelmModel.UserNO = cmbUserNO.Text.ToString().Trim();
            PersonnelmModel.UserName = txtUserName.Text.ToString().Trim();
            PersonnelmModel.HomeAddress = txtAddress.Text.ToString().Trim();

            PersonnelmModel.WorkTime = DateTime.Now;
            PersonnelmModel.BirthDate = DateTime.Now;

            PersonnelmModel.MobNumber = txtMobileNumber.Text.ToString().Trim();

            //PersonnelmModel.CPH = cmbCarNumber.Text;
            //PersonnelmModel.CarType = cmbCarType.Text;
            //PersonnelmModel.CarPlace = txtCarPlace.Text;
            PersonnelmModel.CarPlaceNo = Convert.ToInt32(txtCarCount.Text == "" ? "1" : txtCarCount.Text);
            return PersonnelmModel;
        }

        private CardIssue BindCardIssue()
        {
            CardIssue model = dgvCardIssue.SelectedItem as CardIssue;
            return model;
        }

        private void dgvCardIssue_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (dgvCardIssue.Items.Count > 0)
                {
                    if (dgvCardIssue.SelectedIndex > -1)
                    {
                        frmPostpone p = new frmPostpone(BindCardIssue(), new PostponeHandler(Bin));
                        p.Owner = this;
                        p.ShowDialog();
                    }
                    //List<Rights> lstRs = gsd.GetRightsByName("车牌登记", "Postponed");
                    //if (lstRs == null || lstRs.Count == 0)
                    //{
                    //    MessageBox.Show("无延期权限", "提示");
                    //    return;
                    //}
                    //else
                    //{
                    //    if (lstRs[0].CanOperate)
                    //    {

                    //    }
                    //    else
                    //    {
                    //        MessageBox.Show("无延期权限", "提示");
                    //        return;
                    //    }
                    //}
                    
                }

            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":dgvCardIssue_MouseDoubleClick", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\n" + "dgvCardIssue_MouseDoubleClick", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtSelectCPH_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void txtSelectUserNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (!((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)))
            {
                e.Handled = true;
            }
        }

        private void txtSelectUserName_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

       
    

        private void txtSelectAddress_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

       

        private void txtSelectCPH_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                string CPH = "";
                if (e.Key == Key.D8 || e.Key == Key.NumPad8)
                {
                    if (txtSelectCPH.Text.Trim().Length > 0)
                    {
                        CPH = txtSelectCPH.Text.Substring(0, txtSelectCPH.Text.Trim().Length - 1);
                    }
                }
                else
                {
                    CPH = txtSelectCPH.Text;
                }
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic["CPH"] = "%" + CPH + "%";
                Bin(dic);
            }
            catch(Exception ex)
            {
                gsd.AddLog(this.Title + ":txtSelectCPH_KeyUp", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\n" + "txtSelectCPH_KeyUp", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

       
     

        private void txtSelectCarPlace_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                string cardNO = "";
                if (e.Key == Key.D8 || e.Key == Key.NumPad8)
                {
                    if (txtSelectCarPlace.Text.Trim().Length > 0)
                    {
                        cardNO = txtSelectCarPlace.Text.ToString().Substring(0, txtSelectCarPlace.Text.Trim().Length - 1);
                    }
                }
                //else if (e.KeyChar == 13)
                //{
                //    cardNO = txtCardNO.Text;
                //}
                else
                {
                    cardNO = txtSelectCarPlace.Text;
                }
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic["CarPlace"] = "%" + cardNO + "%";
                Bin(dic);
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":txtSelectCarPlace_KeyUp", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\n" + "txtSelectCarPlace_KeyUp", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtSelectUserName_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                string UserName = "";

                if (e.Key == Key.D8 || e.Key == Key.NumPad8)
                {
                    if (txtSelectUserName.Text.Trim().Length > 0)
                    {
                        UserName = txtSelectUserName.Text.ToString().Substring(0, txtSelectUserName.Text.Trim().Length - 1);
                    }
                }
                //else if (e.Key == 13)
                //{
                //    UserName = textBox3.Text;
                //}
                else
                {
                    UserName = txtSelectUserName.Text;
                }
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic["UserName"] = "%" + UserName + "%";
                Bin(dic);
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":txtSelectUserName_KeyUp", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\n" + "txtSelectUserName_KeyUp", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtSelectUserNo_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                string UserNo = "";
               
                if (e.Key == Key.D8 || e.Key == Key.NumPad8)
                {
                    if (txtSelectUserNo.Text.Trim().Length > 0)
                    {
                        UserNo = txtSelectUserNo.Text.ToString().Substring(0, txtSelectUserNo.Text.Trim().Length - 1);
                    }
                }

                else
                {
                    UserNo = txtSelectUserNo.Text;
                }
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic["UserNo"] = "%" + UserNo + "%";
                Bin(dic);
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":txtSelectUserNo_KeyUp", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\n" + "txtUserNo_KeyPress", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtSelectAddress_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                string sHome = "";

                if (e.Key == Key.D8 || e.Key == Key.NumPad8)
                {
                    if (txtSelectAddress.Text.Trim().Length > 0)
                    {
                        sHome = txtSelectAddress.Text.ToString().Substring(0, txtSelectAddress.Text.Trim().Length - 1);
                    }
                }
                //else if (e.KeyChar == 13)
                //{
                //    sHome = txtHome.Text;
                //}
                else
                {
                    sHome = txtSelectAddress.Text;
                }
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic["HomeAddress"] = "%" + sHome + "%";
                Bin(dic);
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":txtSelectAddress_KeyUp", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\n" + "txtSelectAddress_KeyUp", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                //MessageBox.Show(ex.Message + "\r\n" + "txtHome_KeyPress", Language.LanguageXML.GetName("CR/D6"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvCardIssue_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgvCardIssue.Items.Count > 0)
            {
                if (dgvCardIssue.SelectedIndex >-1)
                {
                    flag = "edit";
                    CardIssue ci = (dgvCardIssue.SelectedItem) as CardIssue;
                    txtUserName.Text = ci.UserName;
                    //cmbCarNumber.Text = ci.CPH;

                    cmbHeader.Text = ci.CPH.Substring(0, 1);
                    txtCarNumber.Text = ci.CPH.Substring(1);

                    cmbUserNO.Text = ci.UserNO;
                    cmdCardType.Text = ci.CardType;
                    txtCarNo.Text = ci.CardNO;
                    dtpStart.Text = ci.CarValidStartDate.ToShortDateString();
                    dtpEnd.Text = ci.CarValidEndDate.ToShortDateString();
                    txtMobileNumber.Text = ci.MobNumber;
                    txtCarPlace.Text = ci.CarPlace;
                    txtCarCount.Text = ci.CarPlaceNo.ToString();
                    cmbCarType.Text = ci.CarType;
                    txtAddress.Text = ci.HomeAddress;
                    txtCardYJ.Text = ci.CardYJ.ToString();
                    txtMoney.Text = ci.Balance.ToString();

                    SetLtcheaked(ci.CarValidMachine);

                    btnAdd.IsEnabled = true;
                    btnCancel.IsEnabled = isDelete;
                    //btnCancel.IsEnabled = true;
                    btnPrint.IsEnabled = false;
                    cmdCardType.IsEnabled = false;
                    dtpStart.IsEnabled = false;
                    dtpEnd.IsEnabled = false;
                    cmbUserNO.IsEnabled = false;
                    txtMoney.IsEnabled = false;
                    txtCardYJ.IsEnabled = false;
                }
            }
        }

        void SetLtcheaked(string carvalidmachinestr)
        {
            StringBuilder builder = new StringBuilder();
            //机号绑定控件
            foreach (char c in carvalidmachinestr)
            {
                builder.Append(c.ToString());
            }

            for (int i = 0; i < lvwCtrlNumber.Items.Count; i++)
            {
                ListViewItem lv = (ListViewItem)lvwCtrlNumber.ItemContainerGenerator.ContainerFromItem(lvwCtrlNumber.Items[i]);
                lv.IsSelected = false;
            }

            for (int i = 0; i < builder.ToString().Length; i++)
            {
                if (builder.ToString().Substring(i, 1) == "0")
                {
                    for (int j = 0; j < lvwCtrlNumber.Items.Count; j++)
                    {
                        var v = lvwCtrlNumber.Items[j] as ParkJHSet;
                        if (v.CtrlNumber == (i + 1))
                        {
                            ListViewItem lv = (ListViewItem)lvwCtrlNumber.ItemContainerGenerator.ContainerFromItem(lvwCtrlNumber.Items[j]);
                            //Border dr = (Border)lv.Template.FindName("bdColor", lv);
                            //dr.Background = new SolidColorBrush(Colors.DarkGreen);
                            lv.IsSelected = true;
                        }
                    }
                }
            }

            
        }

        //void GetPrint(RichTextBox rTxt, System.Drawing.Printing.PrintPageEventArgs e)
        //{
        //    int iPrint = 0;
        //    //打印信息处理	
        //    //获得绘制对象
        //    System.Drawing.Graphics g = e.Graphics;
        //    //一页中的行数
        //    float LinePages = 0;
        //    //待绘文本的纵向坐标
        //    float YPos = 0;
        //    //行计数
        //    int count = 0;
        //    //左边界
        //    float LeftMargin = 0;
        //    //顶边界
        //    float topMargin = 0;
        //    //字符串流
        //    String line = null;
        //    //根据页面的高度和字体的高度计算
        //    //一页中可以打印的行数
        //    LinePages = e.MarginBounds.Height / this.;
        //    if (iPrint >= this.richTextBox1.Lines.Length - 1)
        //    {
        //        e.HasMorePages = false;
        //        return;
        //    }
        //    //每次从文件中读取一行并打印		
        //    while (count < LinePages && ((line = rTxt.Lines[iPrint++]) != null))
        //    {
        //        //计算这一行的显示位置
        //        YPos = topMargin + (count * this.Font.GetHeight(g));
        //        //绘制文本
        //        g.DrawString(line, this.Font, Brushes.Black, LeftMargin, YPos, new StringFormat());
        //        //行数增加
        //        count++;
        //        if (iPrint > rTxt.Lines.Length - 1)
        //            break;
        //    }
        //}


        public PrintQueue GetPrinter(string printerName = null)
        {
            try
            {
                PrintQueue selectedPrinter = null;
                if (!string.IsNullOrEmpty(printerName))
                {
                    var printers = new LocalPrintServer().GetPrintQueues();
                    selectedPrinter = printers.FirstOrDefault(p => p.Name == printerName);
                }
                else
                {
                    selectedPrinter = LocalPrintServer.GetDefaultPrintQueue();
                }
                return selectedPrinter;
            }
            catch(Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        } 

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<BillPrintSet> lstBPS = gsd.GetPrint();
                FlowDocument doc = new FlowDocument();
                Paragraph ph = new Paragraph();

                ph.Inlines.Add(new Run("        停车场初次发行收费票据"));
                if (lstBPS.Count > 0 && lstBPS != null)
                {
                    if (lstBPS[0].Title != "")
                    {
                        ph.Inlines.Add(new Run("\r\n" + "        " + lstBPS[0].Title));
                    }
                    if (lstBPS[0].FTitle != "")
                    {
                        ph.Inlines.Add(new Run("\r\n" + "        " + lstBPS[0].FTitle));
                    }
                }
                ph.Inlines.Add(new Run("\r\n-----------------------------------"));
                doc.Blocks.Add(ph);
                doc.Blocks.Add(new Paragraph(new Run("车辆编号:" + txtCarNo.Text)));
                doc.Blocks.Add(new Paragraph(new Run("车辆类型:" + cmdCardType.Text)));
                doc.Blocks.Add(new Paragraph(new Run("车牌号码:" + cmbHeader.Text + txtCarNumber.Text)));
                doc.Blocks.Add(new Paragraph(new Run("有效起日:" + dtpStart.Text)));
                doc.Blocks.Add(new Paragraph(new Run("有效止日:" + dtpEnd.Text)));
                doc.Blocks.Add(new Paragraph(new Run("交纳金额:" + txtMoney.Text + "元")));
                doc.Blocks.Add(new Paragraph(new Run("车辆押金:" + txtCardYJ.Text + "元")));
                doc.Blocks.Add(new Paragraph(new Run("操作员姓名:" + Model.sUserName)));
                doc.Blocks.Add(new Paragraph(new Run("日期:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))));

                Paragraph ph1 = new Paragraph();
                ph1.Inlines.Add(new Run("-----------------------------------"));
                if (lstBPS.Count > 0 && lstBPS != null)
                {
                    if (lstBPS[0].Footer != "")
                    {
                        ph1.Inlines.Add(new Run("\r\n" + lstBPS[0].Footer));
                    }
                }

                doc.Blocks.Add(ph1);
                rtbPrint.Document = doc;
                var printDialog = new PrintDialog();
                printDialog.PrintQueue = GetPrinter();
                printDialog.PrintDocument(((IDocumentPaginatorSource)rtbPrint.Document).DocumentPaginator, "A Flow Document");
            }
            catch (Exception ex)
            {
                gsd.AddLog("车牌登记:btnPrint_Click", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\n" + "btnPrint_Click", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

      

        private void chkAutoUserNo_Click(object sender, RoutedEventArgs e)
        {
            if (chkAutoUserNo.IsChecked == true)
            {
                AutoAddNO();
            }
            else
            {
                chkAutoUserNo.IsEnabled = true;
                cmbUserNO.IsEnabled = true;
            }
        }

        private void chkAutoCardNo_Click(object sender, RoutedEventArgs e)
        {
            if (chkAutoCardNo.IsChecked == true)
            {
                AutoAddNO();
            }
            else
            {
                txtCarNo.IsEnabled = true;
                chkAutoCardNo.IsEnabled = true;
            }
        }


        bool isAdd = false;
        bool isDelete = false;

        void ShowRights()
        {
            GetUiAllRightButton((this.Content as Grid).Children);// ???
            long pid = gsd.GetIDByName(this.Title, "CmdView");
            List<RightsItem> lstRI = new List<RightsItem>();
            if (lstRightButton == null || lstRightButton.Count == 0)
            {
                return;
            }
            foreach (var v in lstRightButton) // 这里的权限，是只有当前的几个FromName的按钮吗?
            {
                List<Rights> lstRs = gsd.GetRightsByName(v.FormName, v.ItemName);// 在xml只有定义了一部分的Item name;
                if (lstRs.Count > 0)
                {
                    //v.Visibility = lstRs[0].CanView == true ? Visibility.Visible : Visibility.Hidden;
                    v.IsEnabled = lstRs[0].CanOperate;
                    if (v.ItemName == "btnAdd")
                    {
                        isAdd = lstRs[0].CanOperate;
                    }
                    if (v.ItemName == "btnDelete")
                    {
                        isDelete = lstRs[0].CanOperate;
                    }
                }
                else
                {
                    //lstRI.Add(new RightsItem() { FormName = v.FormName, ItemName = v.ItemName, Category = "车场", Description = v.Content.ToString(), PID = pid });
                }
            }
            
            //if (lstRI.Count > 0)
            //{
            //    gsd.SetRightsItem(lstRI);
            //}
        }
        


        List<SFMControls.ButtonSfm> lstRightButton = new List<SFMControls.ButtonSfm>();
        private void GetUiAllRightButton(UIElementCollection uiControls)
        {
            foreach (UIElement element in uiControls)
            {
                if (element is SFMControls.ButtonSfm)
                {
                    SFMControls.ButtonSfm current = element as SFMControls.ButtonSfm;
                    if (current.ItemName != "" && current.FormName != "" && current.ItemName != null && current.FormName != "")
                    {
                        lstRightButton.Add(current);
                    }
                }
                else if (element is Grid)
                {
                    GetUiAllRightButton((element as Grid).Children);
                }
                else if (element is Expander)
                {
                    if ((element as Expander).Content is StackPanel)
                    {
                        StackPanel sa = (element as Expander).Content as StackPanel;
                        GetUiAllRightButton(sa.Children);
                    }
                    else if ((element as Expander).Content is Grid)
                    {
                        Grid sa = (element as Expander).Content as Grid;
                        GetUiAllRightButton(sa.Children);
                    }
                }
                else if (element is StackPanel)
                {
                    GetUiAllRightButton((element as StackPanel).Children);
                }
                else if (element is ScrollViewer)
                {
                    StackPanel sp = (element as ScrollViewer).Content as StackPanel;
                    GetUiAllRightButton(sp.Children);
                }
                else if (element is TabControl)
                {
                    foreach (UIElement Pageelment in (element as TabControl).Items)
                    {
                        TabItem tabtemp = (TabItem)Pageelment;

                        Grid gd = tabtemp.Content as Grid;
                        GetUiAllRightButton(gd.Children);
                    }
                }
                else if (element is GroupBox)
                {
                    GroupBox tabtemp = (GroupBox)element;
                    Grid gd = tabtemp.Content as Grid;
                    if (gd != null)
                        GetUiAllRightButton(gd.Children);
                }
            }
        }

        private void txtCarCount_KeyDown(object sender, KeyEventArgs e)
        {
            if (!((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)))
            {
                e.Handled = true;
            }
        }

        public List<T> GetChildObjects<T>(DependencyObject obj, Type typename) where T : FrameworkElement
        {
            DependencyObject child = null;
            List<T> childList = new List<T>();

            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);

                if (child is T && (((T)child).GetType() == typename))
                {
                    childList.Add((T)child);
                }
                childList.AddRange(GetChildObjects<T>(child, typename));
            }
            return childList;
        }


        //private void cmbCarNumber_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    List<TextBox> LstTxtBox = GetChildObjects<TextBox>(cmbCarNumber, typeof(TextBox));
        //    if (LstTxtBox.Count > 0)
        //    {
        //        int pos = 0;
        //        pos = LstTxtBox[0].SelectionStart;
        //        LstTxtBox[0].Text = cmbCarNumber.Text.ToUpper();
        //        LstTxtBox[0].Select(pos, 0);
        //    }
        //}

        private void chkAllSelect_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < lvwCtrlNumber.Items.Count; i++)
            {
                ListViewItem lv = (ListViewItem)lvwCtrlNumber.ItemContainerGenerator.ContainerFromItem(lvwCtrlNumber.Items[i]);
                lv.IsSelected = chkAllSelect.IsChecked.Value;
            }
        }

        private void btnAddPerson_Click(object sender, RoutedEventArgs e)
        {
            frmPersonnelAdd fpa = new frmPersonnelAdd(new UpdatePersonnelDataHandler(UpdateData));
            fpa.Owner = this;
            fpa.ShowDialog();
        }


        private void UpdateData(string strUserNO, string strUserName, string strDept)
        {
            try
            {
                cmbUserNO.ItemsSource = gsd.GetPersonnel();
                if (cmbUserNO.Items.Count > 0)
                {
                    cmbUserNO.DisplayMemberPath = "UserNO";
                    cmbUserNO.SelectedValue = "UserNO";
                    cmbUserNO.SelectedIndex = 0;
                }
                cmbUserNO.Text = strUserNO;
                txtUserName.Text = strUserName;
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":人事信息", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\n人事信息", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            frmPersonnelSelect select = new frmPersonnelSelect(new SelectPersonnelDataHandler(SelectData));
            select.Owner = this;
            select.ShowDialog();
        }



        List<UserInfo> lstUserInfo = new List<UserInfo>();

        private void SelectData(string where)
        {
            try
            {
                if (where != "")
                {
                    lstUserInfo = gsd.GetPersonnel(where);
                    cmbUserNO.ItemsSource = lstUserInfo;
                    if (cmbUserNO.Items.Count > 0)
                    {
                        cmbUserNO.DisplayMemberPath = "UserNO";
                        cmbUserNO.SelectedValuePath = "UserNO";
                        cmbUserNO.SelectedIndex = 0;
                    }
                }
                else
                {
                    lstUserInfo = gsd.GetPersonnel();
                    cmbUserNO.ItemsSource = lstUserInfo;
                    if (cmbUserNO.Items.Count > 0)
                    {
                        cmbUserNO.DisplayMemberPath = "UserNO";
                        cmbUserNO.SelectedValuePath = "UserNO";
                        cmbUserNO.SelectedIndex = 0;
                    }
                }

                if (cmbUserNO.SelectedIndex > -1)
                {
                    var vr = cmbUserNO.SelectedItem as UserInfo;
                    if (vr.UserNO != null && vr.UserNO.Length > 0)
                    {
                        List<CardIssue> lstCI = gsd.GetModel(vr.UserNO);
                        if (lstCI.Count > 0)
                        {
                            txtUserName.Text = lstCI[0].UserName;
                            //cmbCarNumber.Text = lstCI[0].CPH;

                            cmbHeader.Text = lstCI[0].CPH.Substring(0, 1);
                            txtCarNumber.Text = lstCI[0].CPH.Substring(1);

                            cmbUserNO.Text = lstCI[0].UserNO;
                            cmdCardType.Text = lstCI[0].CardType;
                            txtCarNo.Text = lstCI[0].CardNO;
                            dtpStart.Text = lstCI[0].CarValidStartDate.ToShortDateString();
                            dtpEnd.Text = lstCI[0].CarValidEndDate.ToShortDateString();
                            txtMobileNumber.Text = lstCI[0].MobNumber;
                            txtCarPlace.Text = lstCI[0].CarPlace;
                            txtCarCount.Text = lstCI[0].CarPlaceNo.ToString();
                            cmbCarType.Text = lstCI[0].CarType;
                            txtAddress.Text = lstCI[0].HomeAddress;
                            txtCardYJ.Text = lstCI[0].CardYJ.ToString();
                            txtMoney.Text = lstCI[0].Balance.ToString();

                            SetLtcheaked(lstCI[0].CarValidMachine);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gsd.AddLog(this.Title + ":SelectData", ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show(ex.Message + "\r\nSelectData", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtCarNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.I || e.Key == Key.O)
            {
                e.Handled = true;
            }
        }

        private void cmbUserNO_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //try
            //{
            //    string value = cmbUserNO.SelectedValue.ToString();
            //    if (cmbUserNO.SelectedItem != null)
            //    {
            //        var vr = cmbUserNO.SelectedItem as UserInfo;
            //        if (vr.UserNO != null && vr.UserNO.Length > 0)
            //        {
            //            List<CardIssue> lstCI = gsd.GetModel(vr.UserNO);
            //            if (lstCI.Count > 0)
            //            {
            //                txtUserName.Text = lstCI[0].UserName ?? "";
            //                txtCarPlace.Text = lstCI[0].CarPlace ?? "";
            //                cmbCarType.Text = lstCI[0].CarType ?? "";
            //                cmbHeader.Text = (lstCI[0].CPH ?? Model.LocalProvince).Substring(0, 1);
            //                txtCarNumber.Text = (lstCI[0].CPH ?? "").Length > 0 ? lstCI[0].CPH.Substring(1) : "";
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    gsd.AddLog(this.Title + ":cmbUserNO_SelectionChanged", ex.Message + "\r\n" + ex.StackTrace);
            //    MessageBox.Show(ex.Message + "\r\ncmbUserNO_SelectionChanged", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
        }
      
    }
}
