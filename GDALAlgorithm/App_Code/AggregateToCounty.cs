﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace GDALAlgorithm
{
     public class AggregateToCounty
    {
        /// <summary>
        /// 获取到County的汇总值
        /// </summary>
        /// <param name="procedure_name"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string get_CountyValue(String procedure_name, SqlParameter[] param)
        {
            SqlConnection con = DataBaseOperate.getSqlCon();
            SqlCommand cmd = DataBaseOperate.getSqlCmd(procedure_name, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddRange(param);
            cmd.ExecuteNonQuery();
            string value = param[3].Value.ToString();
            con.Close();
            return value;
        }


        /// <summary>
        /// 获取农场Code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static int getCountyCount()
        {
            string strsql = "select count(distinct QXCode) from COUNTY";
            SqlConnection con = DataBaseOperate.getSqlCon();
            SqlCommand cmd = DataBaseOperate.getSqlCmd(strsql, con);
            int count = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            return count;
        }

        /// <summary>
        /// 获取CountyCode
        /// </summary>
        /// <param name="plotid"></param>
        /// <returns></returns>
        public static List<string> get_CountyCode()
        {
            List<string> list = new List<string>();
            string strsql = "select distinct QXCode from COUNTY";
            SqlConnection con = DataBaseOperate.getSqlCon();
            SqlCommand cmd = DataBaseOperate.getSqlCmd(strsql, con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(reader[0].ToString());
            }
            con.Close();
            return list;
        }

        //创建普通类型表
        public static DataTable CreateDatatable()
        {
            DataTable dt_VillageReslut = new DataTable();
            DataColumn dt_colum1 = new DataColumn();
            dt_colum1.ColumnName = "农场";
            dt_VillageReslut.Columns.Add(dt_colum1);
            DataColumn dt_colum2 = new DataColumn();
            dt_colum2.ColumnName = "监测时间";
            dt_VillageReslut.Columns.Add(dt_colum2);
            DataColumn dt_colum3 = new DataColumn();
            dt_colum3.ColumnName = "作物类型";
            dt_VillageReslut.Columns.Add(dt_colum3);
            DataColumn dt_colum4 = new DataColumn();
            dt_colum4.ColumnName = "汇总结果";
            dt_VillageReslut.Columns.Add(dt_colum4);
            DataColumn dt_colum5 = new DataColumn();
            dt_colum5.ColumnName = "汇总时间";
            dt_VillageReslut.Columns.Add(dt_colum5);
            return dt_VillageReslut;
        }

        /// <summary>
        /// 汇总到County
        /// </summary>
        /// <param name="VILLAGECODE"></param>
        /// <param name="MONITORTIME"></param>
        /// <param name="CROP_CODE"></param>
        /// <param name="SOIL_NUTRIENT"></param>
        /// <returns></returns>
        public static int insert_County(String procedure_name, SqlParameter[] param)
        {
            SqlConnection con = DataBaseOperate.getSqlCon();
            SqlCommand cmd = DataBaseOperate.getSqlCmd(procedure_name, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddRange(param);
            int reslut = cmd.ExecuteNonQuery();
            con.Close();
            return reslut;
        }

        /// <summary>
        /// 获取到Town的汇总值
        /// </summary>
        /// <param name="procedure_name"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string get_SoilCountyValue(String procedure_name, SqlParameter[] param)
        {
            SqlConnection con = DataBaseOperate.getSqlCon();
            SqlCommand cmd = DataBaseOperate.getSqlCmd(procedure_name, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddRange(param);
            cmd.ExecuteNonQuery();
            string value = param[4].Value.ToString();
            con.Close();
            return value;
        }

        /// <summary>
        /// 汇总到Town_SOILNUTRIENT
        /// </summary>
        /// <param name="VILLAGECODE"></param>
        /// <param name="MONITORTIME"></param>
        /// <param name="CROP_CODE"></param>
        /// <param name="SOIL_NUTRIENT"></param>
        /// <returns></returns>
        public static int insert_County_SOILNUTRIENT(String procedure_name, SqlParameter[] param)
        {
            SqlConnection con = DataBaseOperate.getSqlCon();
            SqlCommand cmd = DataBaseOperate.getSqlCmd(procedure_name, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddRange(param);
            int reslut = cmd.ExecuteNonQuery();
            con.Close();
            return reslut;
        }

        //创建SOIL类型表
        public static DataTable CreateSoilDatatable()
        {
            DataTable dt_VillageReslut = new DataTable();
            DataColumn dt_colum1 = new DataColumn();
            dt_colum1.ColumnName = "农场";
            dt_VillageReslut.Columns.Add(dt_colum1);
            DataColumn dt_colum2 = new DataColumn();
            dt_colum2.ColumnName = "监测时间";
            dt_VillageReslut.Columns.Add(dt_colum2);
            DataColumn dt_colum3 = new DataColumn();
            dt_colum3.ColumnName = "作物类型";
            dt_VillageReslut.Columns.Add(dt_colum3);
            DataColumn dt_colum4 = new DataColumn();
            dt_colum4.ColumnName = "养分类型";
            dt_VillageReslut.Columns.Add(dt_colum4);
            DataColumn dt_colum5 = new DataColumn();
            dt_colum5.ColumnName = "汇总结果";
            dt_VillageReslut.Columns.Add(dt_colum5);
            DataColumn dt_colum6 = new DataColumn();
            dt_colum6.ColumnName = "汇总时间";
            dt_VillageReslut.Columns.Add(dt_colum6);
            return dt_VillageReslut;
        }
     }
}
