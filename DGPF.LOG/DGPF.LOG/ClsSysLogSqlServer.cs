using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading;

namespace DGPF.LOG
{
   public class ClsSysLogSqlServer
    {
        private static readonly string connStr;
        private static SqlConnection conn;

        /// <summary>
        /// 静态构造函数实例化连接字符串对象
        /// </summary>
        static ClsSysLogSqlServer()
        {
            connStr = GetStrConn();
            conn = new SqlConnection(connStr);
            conn.Open();
        }
        #region MyRegion


        //public static string GetStrConn()
        //{
        //    try
        //    {
        //        StreamReader sr = new StreamReader(System.IO.Directory.GetCurrentDirectory() + "\\log.json", Encoding.Default);
        //        String line;
        //        string jsonobj = "";
        //        while ((line = sr.ReadLine()) != null)
        //        {
        //            jsonobj = jsonobj + line.ToString();
        //        }
        //        LogConn log = JsonConvert.DeserializeObject<LogConn>(jsonobj);
        //        return log.strConn;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}
        #endregion
        /// <summary>
        /// 获取链接字符串
        /// </summary>
        /// <returns></returns>
        public static string GetStrConn()
        {
            using (System.IO.StreamReader file = System.IO.File.OpenText(System.IO.Directory.GetCurrentDirectory() + "\\log.json"))
            {
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    JObject o = (JObject)JToken.ReadFrom(reader);
                    string key = o["SQLSERVER"].ToString();
                    return key;
                }
            }
        }



        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Close()
        {
            if (conn.State != System.Data.ConnectionState.Closed)
            {
                conn.Close();
            }
        }
        public void Info(DateTime ACCESS_TIME, string USER_ID, string USER_NAME, string IP_ADDR, int LOG_TYPE, string LOG_CONTENT, string REMARK)
        {
            LogMod mod = new LogMod();
            mod.ACCESS_TIME = ACCESS_TIME;
            mod.USER_ID = USER_ID;
            mod.USER_NAME = USER_NAME;
            mod.IP_ADDR = IP_ADDR;
            mod.LOG_TYPE = LOG_TYPE;
            mod.LOG_CONTENT = LOG_CONTENT;
            mod.REMARK = REMARK;
            Thread thread = new Thread(ThreadLog);
            thread.Start(mod);
        }
        /// <summary>
        /// 写日志到数据库
        /// </summary>
        /// <param name="md"></param>
        public void ThreadLog(object obj)
        {
            try
            {
                LogMod mod = (LogMod)obj;
                string SQLString = "insert into ts_uidp_loginfo(ACCESS_TIME,USER_ID,USER_NAME,IP_ADDR,LOG_TYPE,LOG_CONTENT,REMARK)"
         + " VALUES(@ACCESS_TIME, @USER_ID, @USER_NAME, @IP_ADDR, @LOG_TYPE, @LOG_CONTENT, @REMARK)";
                SqlParameter[] cmdParms = new SqlParameter[7];
                cmdParms[0] = new SqlParameter("@ACCESS_TIME", mod.ACCESS_TIME == null ? DateTime.Now : mod.ACCESS_TIME);
                cmdParms[1] = new SqlParameter("@USER_ID", mod.USER_ID == null ? "" : mod.USER_ID);
                cmdParms[2] = new SqlParameter("@USER_NAME", mod.USER_NAME == null ? "" : mod.USER_NAME);
                cmdParms[3] = new SqlParameter("@IP_ADDR", mod.IP_ADDR == null ? "" : mod.IP_ADDR);
                cmdParms[4] = new SqlParameter("@LOG_TYPE", mod.LOG_TYPE);
                cmdParms[5] = new SqlParameter("@LOG_CONTENT", mod.LOG_CONTENT == null ? "" : mod.LOG_CONTENT);
                cmdParms[6] = new SqlParameter("@REMARK", mod.REMARK == null ? "" : mod.REMARK);
                if (conn.State != System.Data.ConnectionState.Open)
                {
                    conn = new SqlConnection(connStr);
                    conn.Open();
                }
                using (SqlCommand cmd = new SqlCommand(SQLString, conn))
                {
                    cmd.Parameters.AddRange(cmdParms);
                    cmd.ExecuteNonQuery();//s返回受影响行数
                }
            }
            catch (SqlException e)
            {
                conn.Close();
                throw e;
            }
        }

    }
}
