using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DGPF.DB;
using Newtonsoft.Json;
namespace DGPF.UTILITY
{
    public class DBTool
    {
        public  readonly string strMySqlCon;//"server=localhost;user id=root;pwd=root;database=uidp;SslMode=none;charset=UTF8";
        public  IDataBase db;
        public DBTool(string DBType)
        {
            try
            {
                strMySqlCon = GetStrConn(DBType);
                db = new ClsDBFactory(DBType, strMySqlCon).DataBase;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// 查询获取dataset
        /// </summary>
        /// <param name="sql"></param>
        public  DataSet GetDataSet(Dictionary<string, string> data)
        {
            try
            {
                db.Open();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                foreach (var item in data)
                {
                    db.Fill(dt, item.Value);
                    dt.TableName = item.Key;
                    ds.Tables.Add(dt);
                    dt.Clear();
                }
                db.Close();
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally {
                db.Close();
            }
        }
        /// <summary>
        /// 查询获取datatable
        /// </summary>
        /// <param name="sql"></param>
        public  DataTable GetDataTable(string sql)
        {
            try
            {
                db.Open();
                DataTable dt = new DataTable();
                db.Fill(dt, sql);
                db.Close();
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally {
                db.Close();
            }
        }
        /// <summary>
        /// 查询获取string
        /// </summary>
        /// <param name="sql"></param>
        public  string GetString(string sql)
        {
            try
            {
                db.Open();
                DataTable dt = new DataTable();
                db.Fill(dt, sql);
                db.Close();
                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt.Rows[0][0].ToString();
                }
                return "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally {
                db.Close();
            }
        }
        /// <summary>
        /// 执行sql方法;
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public bool Execut(string sql)
        {
            try
            {
                db.Open();
                DataTable dt = new DataTable();
                int rows = db.ExecuteSQL(sql);
                db.Close();
                if (rows > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally {
                db.Close();
            }
        }
        /// <summary>
        /// 执行sql方法;
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string ExecutByStringResult(string sql)
        {
            try
            {
                db.Open();
                DataTable dt = new DataTable();
                int rows = db.ExecuteSQL(sql);
                db.Close();
                if (rows > 0)
                {
                    return "";
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
            finally
            {
                db.Close();
            }
        }
        /// <summary>
        /// 执行多语句sql方法;
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public bool Executs(List<string> sql)
        {
            try
            {
                db.Open();
                db.BeginTransaction();
                foreach (var item in sql)
                {
                    db.ExecuteSQL(item);
                }
                db.Commit();
                db.Close();
                return true;
            }
            catch (Exception ex)
            {
                db.Rollback();
                return false;
            }
            finally {
                db.Close();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private  string GetStrConn(string ConnType)
        {
            try
            {
                StreamReader sr = new StreamReader(System.IO.Directory.GetCurrentDirectory() + "\\DBConfig.json", Encoding.Default);
                String line;
                string jsonobj = "";
                while ((line = sr.ReadLine()) != null)
                {
                    jsonobj = jsonobj + line.ToString();
                }
                DBConn dbConn = JsonConvert.DeserializeObject<DBConn>(jsonobj);
                System.Reflection.PropertyInfo[] properties = dbConn.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                foreach (System.Reflection.PropertyInfo item in properties)
                {
                    string name = item.Name;
                    object value = item.GetValue(dbConn, null);
                    if (name==ConnType)
                    {
                        return value.ToString();
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
public class DBConn
    {
        public string MYSQL { get; set; }
        public string SQLSERVER { get; set; }
        public string ORACLE { get; set; }
    }
}
