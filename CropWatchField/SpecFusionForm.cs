﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace CropWatchField
{
    public partial class SpecFusionForm : Form
    {
        string sOutQuaFile = "false";
        public SpecFusionForm()
        {
            InitializeComponent();
            //禁用打开文件路径
            this.btn_OpenOutputPath.Visible = false;
            //初始化窗口大小下拉菜单
            //融合方法
            List<string> cbxString = new List<string>();
            cbxString.Add(string.Format("Weighted Average"));
            cbxString.Add(string.Format("High Pass Filter"));
            cbxString.Add(string.Format("Wavelet Transform"));
            cbxString.Add(string.Format("Brovey"));
            cbxString.Add(string.Format("GS"));
            cbxString.Add(string.Format("PCA"));
            cbxString.Add(string.Format("SFIM"));
            cbxString.Add(string.Format("SVR"));
            cbxString.Add(string.Format("FIHS"));
            cbxString.Add(string.Format("GIHSA"));
            cbxString.Add(string.Format("PBIM"));
            this.cbx_Method.DataSource = cbxString;
            this.cbx_Method.SelectedIndex = 0;
            //高通滤波器
            List<string> cbxString2 = new List<string>();
            cbxString2.Add(string.Format("Sobel"));
            cbxString2.Add(string.Format("Robert"));
            cbxString2.Add(string.Format("Prewitt"));
            cbxString2.Add(string.Format("Laplacian1"));
            cbxString2.Add(string.Format("Laplacian2"));
            cbxString2.Add(string.Format("Log"));
            this.cbx_Filter.DataSource = cbxString2;
            this.cbx_Filter.SelectedIndex = 0;
            //高频融合规则
            List<string> cbxString3 = new List<string>();
            cbxString3.Add(string.Format("Replace"));
            cbxString3.Add(string.Format("Maximum"));
            cbxString3.Add(string.Format("Direction Contrast"));
            cbxString3.Add(string.Format("Average Gradient"));
            cbxString3.Add(string.Format("Area Energy"));
            this.cbx_Hr.DataSource = cbxString3;
            this.cbx_Hr.SelectedIndex = 0;
            //低频融合规则
            List<string> cbxString4 = new List<string>();
            cbxString4.Add(string.Format("Replace"));
            cbxString4.Add(string.Format("Weighted Average"));
            cbxString4.Add(string.Format("Weighted Average or Choose"));
            this.cbx_Lr.DataSource = cbxString4;
            this.cbx_Lr.SelectedIndex = 0;
            //分解层数
            List<string> cbxString5 = new List<string>();
            cbxString5.Add(string.Format("2"));
            cbxString5.Add(string.Format("3"));
            cbxString5.Add(string.Format("4"));
            this.cbx_Level.DataSource = cbxString5;
            this.cbx_Level.SelectedIndex = 0;
            //小波基
            List<string> cbxString6 = new List<string>();
            cbxString6.Add(string.Format("Haar"));
            cbxString6.Add(string.Format("Daubechies"));
            this.cbx_Basis.DataSource = cbxString6;
            this.cbx_Basis.SelectedIndex = 0;
        }

        private void btn_InPutFile_Low_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();　//创建一个OpenFileDialog 
            dlg.Filter = "(*.tif)|*.tif|(*.*)|*.*";
            dlg.Multiselect = true;//设置属性为多选
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                this.TextBox_LowPath.Text = Path.GetDirectoryName(dlg.FileName);
                string str = " ";
                for (int i = 0; i < dlg.FileNames.Length; i++)//根据数组长度定义循环次数
                {
                    str = Path.GetFileName(dlg.FileNames.GetValue(i).ToString());//获取文件文件名
                    ListViewItem item = new ListViewItem() { Text = str };
                    this.listView_Low.Items.Add(item);
                }
            }
        }

        private void btn_InPutFile_High_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();　//创建一个OpenFileDialog 
            dlg.Filter = "(*.tif)|*.tif|(*.*)|*.*";
            dlg.Multiselect = true;//设置属性为多选
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                this.TextBox_HighPath.Text = Path.GetDirectoryName(dlg.FileName);
                string str = " ";
                for (int i = 0; i < dlg.FileNames.Length; i++)//根据数组长度定义循环次数
                {
                    str = Path.GetFileName(dlg.FileNames.GetValue(i).ToString());//获取文件文件名
                    ListViewItem item = new ListViewItem() { Text = str };
                    this.listView_High.Items.Add(item);
                }
            }
        }

        private void btn_delete_Low_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView_Low.SelectedItems)
            {
                listView_Low.Items.Remove(item);
            }
        }

        private void btn_Up_Low_Click(object sender, EventArgs e)
        {
            //可选择多个项目同时上移
            //未选择任何项目
            if (listView_Low.SelectedItems.Count == 0)
            {
                return;
            }
            listView_Low.BeginUpdate();
            //选中的第一个项目不是第一个项目时才能进行上移操作
            if (listView_Low.SelectedItems[0].Index > 0)
            {
                foreach (ListViewItem item in listView_Low.SelectedItems)
                {
                    ListViewItem itemSelected = item;
                    int indexSexlectedItem = item.Index;
                    listView_Low.Items.RemoveAt(indexSexlectedItem);
                    listView_Low.Items.Insert(indexSexlectedItem - 1, itemSelected);
                }
            }
            listView_Low.EndUpdate();
            if (listView_Low.Items.Count > 0 && listView_Low.SelectedItems.Count > 0)
            {
                listView_Low.Focus();
                listView_Low.SelectedItems[0].Focused = true;
                listView_Low.SelectedItems[0].EnsureVisible();
            }
        }

        private void btn_Down_Low_Click(object sender, EventArgs e)
        {
            //可选择多个项目同时下移
            //未选择任何项目
            if (listView_Low.SelectedItems.Count == 0)
            {
                return;
            }
            listView_Low.BeginUpdate();
            //选中的第一个项目不是最后一个项目时才能进行上移操作
            int indexMaxSelectedItem = listView_Low.SelectedItems[listView_Low.SelectedItems.Count - 1].Index;//选中的最后一个项目索引
            if (indexMaxSelectedItem < listView_Low.Items.Count - 1)
            {
                for (int i = listView_Low.SelectedItems.Count - 1; i >= 0; i--)
                {
                    ListViewItem itemSelected = listView_Low.SelectedItems[i];
                    int indexSelected = itemSelected.Index;
                    listView_Low.Items.RemoveAt(indexSelected);
                    listView_Low.Items.Insert(indexSelected + 1, itemSelected);
                }
            }
            listView_Low.EndUpdate();
            if (listView_Low.Items.Count > 0 && listView_Low.SelectedItems.Count > 0)
            {
                listView_Low.Focus();
                listView_Low.SelectedItems[listView_Low.SelectedItems.Count - 1].Focused = true;
                listView_Low.SelectedItems[listView_Low.SelectedItems.Count - 1].EnsureVisible();
            }
        }

        private void btn_delete_High_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView_High.SelectedItems)
            {
                listView_High.Items.Remove(item);
            }
        }

        private void btn_Up_High_Click(object sender, EventArgs e)
        {
            //可选择多个项目同时上移
            //未选择任何项目
            if (listView_High.SelectedItems.Count == 0)
            {
                return;
            }
            listView_High.BeginUpdate();
            //选中的第一个项目不是第一个项目时才能进行上移操作
            if (listView_High.SelectedItems[0].Index > 0)
            {
                foreach (ListViewItem item in listView_High.SelectedItems)
                {
                    ListViewItem itemSelected = item;
                    int indexSexlectedItem = item.Index;
                    listView_High.Items.RemoveAt(indexSexlectedItem);
                    listView_High.Items.Insert(indexSexlectedItem - 1, itemSelected);
                }
            }
            listView_High.EndUpdate();
            if (listView_High.Items.Count > 0 && listView_High.SelectedItems.Count > 0)
            {
                listView_High.Focus();
                listView_High.SelectedItems[0].Focused = true;
                listView_High.SelectedItems[0].EnsureVisible();
            }
        }

        private void btn_Down_High_Click(object sender, EventArgs e)
        {
            //可选择多个项目同时下移
            //未选择任何项目
            if (listView_High.SelectedItems.Count == 0)
            {
                return;
            }
            listView_High.BeginUpdate();
            //选中的第一个项目不是最后一个项目时才能进行上移操作
            int indexMaxSelectedItem = listView_High.SelectedItems[listView_High.SelectedItems.Count - 1].Index;//选中的最后一个项目索引
            if (indexMaxSelectedItem < listView_High.Items.Count - 1)
            {
                for (int i = listView_High.SelectedItems.Count - 1; i >= 0; i--)
                {
                    ListViewItem itemSelected = listView_High.SelectedItems[i];
                    int indexSelected = itemSelected.Index;
                    listView_High.Items.RemoveAt(indexSelected);
                    listView_High.Items.Insert(indexSelected + 1, itemSelected);
                }
            }
            listView_High.EndUpdate();
            if (listView_High.Items.Count > 0 && listView_High.SelectedItems.Count > 0)
            {
                listView_High.Focus();
                listView_High.SelectedItems[listView_High.SelectedItems.Count - 1].Focused = true;
                listView_High.SelectedItems[listView_High.SelectedItems.Count - 1].EnsureVisible();
            }
        }

        private void cbx_Method_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sMethod = this.cbx_Method.SelectedValue.ToString();
            if (sMethod == "PBIM")
            {

                this.panel_Filter.Visible = false;
                this.panel_Wavelet.Visible = false;
                this.panel_PBIM.Visible = true;
            }
            else if (sMethod == "Wavelet Transform")
            {
                this.panel_PBIM.Visible = false;
                this.panel_Filter.Visible = false;
                this.panel_Wavelet.Visible = true;
            }
            else if (sMethod == "High Pass Filter")
            {
                this.panel_PBIM.Visible = false;
                this.panel_Wavelet.Visible = false;
                this.panel_Filter.Visible = true;
            }
            else
            {
                this.panel_PBIM.Visible = false;
                this.panel_Filter.Visible = false;
                this.panel_Wavelet.Visible = false;
            }
        }

        private void btn_OutputPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.TextBox_OutputPath.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {

            #region 输入与输出路径条件判断
            if (this.TextBox_LowPath.Text.Equals(""))
            {
                MessageBox.Show("请选择输入低空间分辨率影像！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (this.TextBox_HighPath.Text.Equals(""))
            {
                MessageBox.Show("请选择输入低空间分辨率影像！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (this.TextBox_OutputPath.Text.Equals(""))
            {
                MessageBox.Show("请选择输出影像路径！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (this.listView_Low.Items.Count != this.listView_High.Items.Count)
            {
                MessageBox.Show("输入的高低分辨率影像文件数不一致！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (this.cbx_Method.SelectedValue.ToString() == "PBIM" && this.TextBox_HLRatio.Text.Equals(""))
            {
                MessageBox.Show("请输入低高空间分辨率影像分辨率比值！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            #endregion
            this.btn_OK.Enabled = false;
            #region 界面参数获取
            //低空间分辨率影像
            List<string> list_Low = new List<string>();
            foreach (ListViewItem item in this.listView_Low.Items)
            {
                string s = this.TextBox_LowPath.Text + "\\" + item.SubItems[0].Text.Trim();
                list_Low.Add(s);
            }
            string[] sFilename_Low = (string[])list_Low.ToArray();
            //高空间分辨率影像
            List<string> list_High = new List<string>();
            foreach (ListViewItem item in this.listView_High.Items)
            {
                string s = this.TextBox_HighPath.Text + "\\" + item.SubItems[0].Text.Trim();
                list_High.Add(s);
            }
            string[] sFilename_High = (string[])list_High.ToArray();
            //输出路径
            string sFilename_Output = this.TextBox_OutputPath.Text.Trim();
            //方法
            string sMethod = this.cbx_Method.SelectedValue.ToString();
            //参数
            string[] sPara = new string[] { "", "", "", "", "", "" };
            if (sMethod == "PBIM")
            {
                sPara[0] = this.TextBox_HLRatio.Text.ToString();
            }
            else if (sMethod == "High Pass Filter")
            {
                sPara[1] = this.cbx_Filter.SelectedValue.ToString();
            }
            else if (sMethod == "Wavelet Transform")
            {
                sPara[2] = this.cbx_Basis.SelectedValue.ToString();
                sPara[3] = this.cbx_Hr.SelectedValue.ToString();
                sPara[4] = this.cbx_Lr.SelectedValue.ToString();
                sPara[5] = this.cbx_Level.SelectedValue.ToString();
            }
            #endregion
            #region 调用IDL程序
            //IDLSav的路径
            string sIDLSavPath = FileManage.getApplicatonPath();
            sIDLSavPath = sIDLSavPath + "IDLSav\\SpecFusion.pro";
            COM_IDL_connectLib.COM_IDL_connectClass oCom = new COM_IDL_connectLib.COM_IDL_connectClass();
            try
            {
                //初始化
                oCom.CreateObject(0, 0, 0);
                //参数设置

                oCom.SetIDLVariable("FilenameLow", sFilename_Low);
                oCom.SetIDLVariable("FilenameHigh", sFilename_High);
                oCom.SetIDLVariable("OutputPath", sFilename_Output);

                oCom.SetIDLVariable("OutQuaFile", sOutQuaFile);
                oCom.SetIDLVariable("para", sPara);
                oCom.SetIDLVariable("Method", sMethod);
                //编译idl功能源码
                oCom.ExecuteString(".compile -v '" + sIDLSavPath + "'");
                //oCom.ExecuteString("restore,\'" + sIDLSavPath + "\'");
                oCom.ExecuteString("SpecFusion,FilenameLow,FilenameHigh,OutputPath,Method,OutQuaFile,para,Message=Message");
                object objArr = oCom.GetIDLVariable("Message");
                //返回错误消息
                if (objArr != null)
                {
                    MessageBox.Show(objArr.ToString());
                    oCom.DestroyObject();
                    this.btn_OK.Enabled = true;
                    return;
                }
                oCom.DestroyObject();
                MessageBox.Show("光谱融合完毕", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.btn_OK.Enabled = true;
                this.btn_OpenOutputPath.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            #endregion
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rab_Yes_CheckedChanged(object sender, EventArgs e)
        {
            sOutQuaFile = "true";
        }

        private void rab_No_CheckedChanged(object sender, EventArgs e)
        {
            sOutQuaFile = "";
        }

        private void btn_OpenOutputPath_Click(object sender, EventArgs e)
        {
            string sFilename = this.TextBox_OutputPath.Text.Trim();
            //string sPath = Path.GetDirectoryName(sFilename);
            System.Diagnostics.Process.Start("explorer.exe", sFilename);
        }
    }
}
