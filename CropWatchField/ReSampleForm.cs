﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using GdalAlg;

namespace CropWatchField
{
    public partial class ReSampleForm : Form
    {
        public ReSampleForm()
        {
            InitializeComponent();
           
        }

        /// <summary>
        /// 输出类型数据绑定，从XML文件中获取
        /// </summary>
        public void ComBoxDataBind(string strVectorFile)
        {
            char[] strVectorFileList = strVectorFile.ToCharArray();
            StringBuilder sSBuilder = new StringBuilder(300);
            GdalAlgInterface.GetVectorFields(strVectorFileList, sSBuilder, 300);
            //MessageBox.Show("sSBuilder=" + sSBuilder);
            List<string> list = StringFormater.getListByStringSplit(sSBuilder.ToString());
            this.cbx_FormatType.DataSource = list;


        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_cancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// InputFileselect,mutiply
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_InPutFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();　//创建一个OpenFileDialog 
            dlg.Filter = "(*.shp)|*.shp|(*.*)|*.*";
            dlg.Multiselect = true;//设置属性为多选
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                this.txt_ImageInput.Text = dlg.FileName;
            }
            string sVectorInput = this.txt_ImageInput.Text.Trim();
            ComBoxDataBind(sVectorInput);
            this.txt_ImageOutPath.Text = Path.GetDirectoryName(sVectorInput);
        }

        /// <summary>
        /// ImageOutPathSelect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ImageOutPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.txt_ImageOutPath.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btn_ok_Click(object sender, EventArgs e)
        { 
            #region 输入与输出路径条件判断
            if (this.txt_ImageInput.Text.Equals(""))
            {
                MessageBox.Show("请选择输入矢量文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //if (this.cbx_FormatType.SelectedValue.ToString().Equals("请选择"))
            if (this.cbx_FormatType.Text.Equals(""))
            {
                MessageBox.Show("请选择转换字段！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string sResolution = this.txt_resolution.Text;
            if (sResolution.Equals(""))
            {
                MessageBox.Show("请输入分辨率数值！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int NResolution = 30;
            bool BResolution = int.TryParse(sResolution, out NResolution);

            if (!BResolution)
            {
                MessageBox.Show("请输入数字！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (this.txt_ImageOutPath.Text.Equals(""))
            {
                MessageBox.Show("请选择输出路径！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            #endregion
            this.btn_ok.Enabled = false;
            #region 界面参数获取
            string strVectorFile = txt_ImageInput.Text.Trim();
            //string strField = this.cbx_FormatType.SelectedValue.ToString();
            string strField = this.cbx_FormatType.Text.Trim();
            string strRasterFile = this.txt_ImageOutPath.Text.Trim();
            //string sFileName = FileManage.getFileName(strVectorFile);
            string sFileName = Path.GetFileNameWithoutExtension(strVectorFile);
            strRasterFile = strRasterFile + "\\" + sFileName+".tif";


            #endregion

            #region 调用转换算法
            //声明进度信息回调函数
            ProgressFunc pd = new ProgressFunc(this.TermProgress1);
            IntPtr p = new IntPtr(0);
            int ire = 0;
            try
            {
                char[] strVectorFileList = strVectorFile.ToCharArray();

                char[] strRasterFileList = strRasterFile.ToCharArray();

                char[] strFieldList = strField.ToCharArray();

                ire = GdalAlgInterface.ShpRasterize(strVectorFileList, strRasterFileList, NResolution, 2, 0, strFieldList, "GTiff", pd, p);

                MessageBox.Show("矢量转栅格完毕", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.btn_ok.Enabled = true;
                this.btn_OpenOutPut.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            #endregion

        }

        #region
        //进度信息回调函数
        static int nLastTick = -1;
        public int TermProgress1(double dfComplete, char[] strMessage, IntPtr Data)
        {

            //Console.Write("dfComplete==={0}", dfComplete + "\n");
            int nThisTick = (int)(dfComplete * 40.0);

            nThisTick = Math.Min(40, Math.Max(0, nThisTick));
            //Console.Write("nThisTick==={0}", nThisTick + "\n");
            // Have we started a new progress run?
            if (nThisTick < nLastTick && nLastTick >= 39)
                nLastTick = -1;

            if (nThisTick <= nLastTick)
                return 1;

            while (nThisTick > nLastTick)
            {
                nLastTick++;
                if (nLastTick % 4 == 0)
                    Console.Write("{0}", (nLastTick / 4) * 10);
                else
                    Console.Write(".");
            }

            if (nThisTick == 40)
                Console.Write(" - done.\n");
            else
                Console.Write("");

            return 1;
        }
        #endregion

        /// <summary>
        /// 打开输出文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_OpenOutPut_Click(object sender, EventArgs e)
        {
            string sPath = this.txt_ImageOutPath.Text.Trim();
            System.Diagnostics.Process.Start("explorer.exe",sPath);
        }
    }
}
