﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace GDALAlgorithm
{
    public partial class SoilAggregateToCounty : Form
    {
        public SoilAggregateToCounty()
        {
            InitializeComponent();
        }
        //用来展示结果的datatable,最终绑定到datagridview上
        DataTable dt = AggregateToCounty.CreateSoilDatatable();
        private void btn_calculation_Click(object sender, EventArgs e)
        {
          int date_count = DataBaseOperate.get_DateCount(dT_maize_s.Value, dT_maize_e.Value, "SOILNUTRIENT_PLOT");
          List<string> date_list = DataBaseOperate.get_DateDetail(dT_maize_s.Value, dT_maize_e.Value, "SOILNUTRIENT_PLOT");
          int CropCount = DataBaseOperate.get_CropCount();
          List<string> CropCode_list = DataBaseOperate.get_CropCode();
          int nutrientCount = DataBaseOperate.get_NutrientCount();
          List<string>  nutrient_list = DataBaseOperate.get_NutrientCode();
          //获取Countycount
          int Countycount = AggregateToCounty.getCountyCount();
          //获取具体的CountyCode
          List<string> CountyCode_list = AggregateToCounty.get_CountyCode();

          if (date_count != 0)
          {
              for (int i = 0; i < date_count; i++)//datetime循环
              {
                  for (int j = 0; j < Countycount; j++)//villagecode循环
                    {
                        for (int k = 0; k < CropCount; k++) //crop_count循环
                        {
                            for (int h = 0; h < nutrientCount; h++)
                            {
                                //输入查询限制条件，执行存储过程
                                SqlParameter[] param = new SqlParameter[] {
                                    new SqlParameter("@time",Convert.ToDateTime(date_list[i])),
                                    new SqlParameter("@code",CropCode_list[k]),
                                    new SqlParameter("@countycode",CountyCode_list[j]),
                                    new SqlParameter("@nutrient_code",nutrient_list[h]),
                                    new SqlParameter("@sum_result",SqlDbType.Float)
                                 };
                                param[4].Direction = ParameterDirection.Output;
                                string value = AggregateToCounty.get_SoilCountyValue("calc_county_SOILNUTRIENT", param);
                                if (value != "")
                                {
                                    DataRow row = dt.NewRow();
                                    row["农场"] = DataBaseOperate.getCountyName(CountyCode_list[j]);
                                    row["监测时间"] = Convert.ToDateTime(date_list[i]).ToShortDateString();
                                    row["作物类型"] = DataBaseOperate.get_CropCHName(CropCode_list[k]);
                                    row["养分类型"] = DataBaseOperate.get_NutrientCHName(nutrient_list[h]);
                                    row["汇总结果"] = float.Parse(value);
                                    row["汇总时间"] = DateTime.Now.ToShortDateString();
                                    dt.Rows.Add(row);
                                }
                            }

                        }
                  }
              }
              if (dt.Rows.Count == 0)
              {
                  MessageBox.Show("请检查输入条件或统计数据！");
              }
              else
              {
                  dataGridView1.DataSource = dt;
              }
          }

          else
          {
              MessageBox.Show("此时间段内，没有统计数据！");
          }

          InitDataSet();
        }

        private void btn_Input_Click(object sender, EventArgs e)
        {
            int reslut = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string CountyCode = DataBaseOperate.getCountyCode(dt.Rows[i][0].ToString());
                DateTime MONITORTIME = Convert.ToDateTime(dt.Rows[i][1]);
                string CROP_CODE = DataBaseOperate.get_CropCode(dt.Rows[i][2].ToString());
                string nutrient_code = DataBaseOperate.get_NutrientCode(dt.Rows[i][3].ToString());
                float value = float.Parse(dt.Rows[i][4].ToString());
                SqlParameter[] param = new SqlParameter[] {
                                 new SqlParameter("@COUNTYCODE",CountyCode),
                                new SqlParameter("@MONITORTIME",MONITORTIME),
                                new SqlParameter("@CROP_CODE",CROP_CODE),
                                new SqlParameter("@NUTRIENT_CODE",nutrient_code),
                                new SqlParameter("@SOIL_NUTRIENT",value),
                                new SqlParameter("@RECORDTIME",DateTime.Now.ToShortDateString())
                                 };
                reslut = AggregateToCounty.insert_County_SOILNUTRIENT("insert_County_SOILNUTRIENT", param);
            }
            if (reslut > 0)
            {
                MessageBox.Show("数据库入库成功！");
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ExportDataToExcel.ExportExcel(dt);  
        }

        int pageSize = 0;     //每页显示行数
        int nMax = 0;         //总记录数
        int pageCount = 0;    //页数＝总记录数/每页显示行数
        int pageCurrent = 0;   //当前页号
        int nCurrent = 0;      //当前记录
        private void InitDataSet()
        {
            pageSize = 20;      //设置页面行数
            nMax = dt.Rows.Count;
            pageCount = (nMax / pageSize);    //计算出总页数
            if ((nMax % pageSize) > 0) pageCount++;
            pageCurrent = 1;    //当前页数从1开始
            nCurrent = 0;       //当前记录数从0开始
            LoadData();
        }

        private void LoadData()
        {
            int nStartPos = 0;   //当前页面开始记录行
            int nEndPos = 0;     //当前页面结束记录行
            if (dt.Rows.Count != 0)
            {
                System.Data.DataTable dtTemp = dt.Clone();   //克隆DataTable结构框架

                if (pageCurrent == pageCount)
                {
                    nEndPos = nMax;
                }
                else
                {
                    nEndPos = pageSize * pageCurrent;
                }

                nStartPos = nCurrent;
                lblPageCount.Text = "/" + pageCount.ToString();
                txtCurrentPage.Text = Convert.ToString(pageCurrent);


                //从元数据源复制记录行
                for (int i = nStartPos; i < nEndPos; i++)
                {

                    dtTemp.ImportRow(dt.Rows[i]);
                    nCurrent++;
                }
                bindingSource1.DataSource = dtTemp;
                bindingNavigator1.BindingSource = bindingSource1;
                dataGridView1.DataSource = bindingSource1;
                dataGridView1.Visible = true;
            }
        }

        private void bindingNavigator1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "关闭")
            {
                this.Close();
            }
            if (e.ClickedItem.Text == "上一页")
            {
                pageCurrent--;
                if (pageCurrent <= 0)
                {
                    MessageBox.Show("已经是第一页，请点击“下一页”查看！");
                    return;
                }
                else
                {
                    nCurrent = pageSize * (pageCurrent - 1);
                }
                LoadData();
            }
            if (e.ClickedItem.Text == "下一页")
            {
                pageCurrent++;
                if (pageCurrent > pageCount)
                {
                    MessageBox.Show("已经是最后一页，请点击“上一页”查看！");
                    return;
                }
                else
                {
                    nCurrent = pageSize * (pageCurrent - 1);
                }
                LoadData();
            }
        }
    }
}
