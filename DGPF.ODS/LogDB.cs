using DGPF.UTILITY;
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
            string sql = "select * from ts_uidp_loginfo where 1=1 ";
            if (d["USER_NAME"] != null && d["USER_NAME"].ToString() != "")
            {
                sql += " and USER_NAME like '%" + d["USER_NAME"].ToString() + "%'";
            }
            if (d["LOG_TYPE"] != null && d["LOG_TYPE"].ToString() != "")
            {
                sql += " and LOG_TYPE =" + d["LOG_TYPE"].ToString()+" ";
            }
            if (d["ACCESS_TIME"] != null && d["ACCESS_TIME"].ToString() != "" )
            {
                DateTime date = Convert.ToDateTime(d["ACCESS_TIME"].ToString());
                sql += " and ACCESS_TIME between '"+date.Year+"-"+date.Month+"-"+date.Day+" 00:00:00' and '" + date.Year + "-" + date.Month + "-" + date.Day + " 23:59:59'" ;
                //sql += " and date_format(ACCESS_TIME,'%Y-%m-%d')= date_format('"+ d["ACCESS_TIME"].ToString() + "','%Y-%m-%d')  ";
            }
            sql += " order by ACCESS_TIME desc ";
            return db.GetDataTable(sql);
        }
    }
}
