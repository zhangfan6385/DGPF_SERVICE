﻿using DGPF.UTILITY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DGPF.ODS
{
   public  class LogDB
    {
        DBTool db = new DBTool("MYSQL");
        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public DataTable fetchLogInfoList(Dictionary<string, object> d)
        {
            string sql = "select ACCESS_TIME,USER_ID,USER_NAME,IP_ADDR,LOG_CONTENT,REMARK, LOG_TYPE,ALARM_LEVEL from ts_uidp_loginfo where 1=1 ";
            if (d["USER_NAME"] != null && d["USER_NAME"].ToString() != "")
            {
                sql += " and USER_NAME like '%" + d["USER_NAME"].ToString() + "%'";
            }
            if (d["LOG_TYPE"] != null && d["LOG_TYPE"].ToString() != "")
            {
                sql += " and LOG_TYPE =" + d["LOG_TYPE"].ToString()+" ";
            }
            if (d["USER_ID"] != null && d["USER_ID"].ToString() != "")
            {
                sql += " and USER_ID like '%" + d["USER_ID"].ToString() + "%'";
            }
            if (d["LOG_CONTENT"] != null && d["LOG_CONTENT"].ToString() != "")
            {
                sql += " and LOG_CONTENT like '%" + d["LOG_CONTENT"].ToString() + "%'";
            }
            if (d["ALARM_LEVEL"] != null && d["ALARM_LEVEL"].ToString() != "")
            {
                sql += " and ALARM_LEVEL =" + d["ALARM_LEVEL"].ToString() + " ";
            }
            if (d["BEGIN_ACCESS_TIME"] != null && d["BEGIN_ACCESS_TIME"].ToString() != "" && d["END_ACCESS_TIME"] == null && d["END_ACCESS_TIME"].ToString() == "")
            {
                DateTime date = Convert.ToDateTime(d["BEGIN_ACCESS_TIME"].ToString());
                //sql += " and ACCESS_TIME > '" + date.Year + "-" + date.Month + "-" + date.Day + " 00:00:00'";
                sql += " and ACCESS_TIME between '" + date.Year + "-" + date.Month + "-" + date.Day + " 00:00:00' and '" + date.Year + "-" + date.Month + "-" + date.Day + " 23:59:59'";
                //sql += " and date_format(ACCESS_TIME,'%Y-%m-%d')= date_format('"+ d["ACCESS_TIME"].ToString() + "','%Y-%m-%d')  ";
            }
            else if (d["END_ACCESS_TIME"] != null && d["END_ACCESS_TIME"].ToString() != "" && d["BEGIN_ACCESS_TIME"] == null && d["BEGIN_ACCESS_TIME"].ToString() == "")
            {
                DateTime date = Convert.ToDateTime(d["END_ACCESS_TIME"].ToString());
                sql += " and ACCESS_TIME < '" + date.Year + "-" + date.Month + "-" + date.Day + " 23:59:59'";

            }
            else if (d["BEGIN_ACCESS_TIME"] != null && d["BEGIN_ACCESS_TIME"].ToString() != "" && d["END_ACCESS_TIME"] != null && d["END_ACCESS_TIME"].ToString() != "")
            {
                DateTime bdate = Convert.ToDateTime(d["BEGIN_ACCESS_TIME"].ToString());
                DateTime edate = Convert.ToDateTime(d["END_ACCESS_TIME"].ToString());
                sql += " and ACCESS_TIME between '"+bdate.Year+"-"+bdate.Month+"-"+bdate.Day+" 00:00:00' and '" + edate.Year + "-" + edate.Month + "-" + edate.Day + " 23:59:59'" ;
                //sql += " and date_format(ACCESS_TIME,'%Y-%m-%d')= date_format('"+ d["ACCESS_TIME"].ToString() + "','%Y-%m-%d')  ";
            }
            sql += " order by ACCESS_TIME desc ";
            return db.GetDataTable(sql);
        }
    }
}
