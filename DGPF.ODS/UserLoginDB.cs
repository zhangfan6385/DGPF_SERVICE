using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using DGPF.UTILITY;
using Newtonsoft.Json.Linq;

namespace DGPF.ODS
{
    public class UserLoginDB
    {
        DBTool db = new DBTool("");

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string createUserLoginArticle(Dictionary<string, object> d)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into ts_uidp_login (LOGIN_ID,LOGIN_CODE,LOGIN_PASS,LOGIN_REMARK) values(");
            sql.Append(" '");
            sql.Append(d["LOGIN_ID"] == null ? "" : d["LOGIN_ID"].ToString() + "',");
            sql.Append(" '");
            sql.Append(d["LOGIN_CODE"] == null ? "" : d["LOGIN_CODE"].ToString() + "',");
            sql.Append(" '");
            sql.Append(d["LOGIN_PASS"] == null ? "" : d["LOGIN_PASS"].ToString() + "',");
            sql.Append(" '");
            sql.Append(d["LOGIN_REMARK"] == null ? "" : d["LOGIN_REMARK"].ToString() + "')");
            return db.ExecutByStringResult(sql.ToString());
        }
        /// <summary>
        /// 通过LOGIN_ID查询ts_uidp_login
        /// </summary>
        /// <param name="LOGIN_ID"></param>
        /// <returns></returns>
        public DataTable GetUserLoginById(string LOGIN_ID)
        {
            string sql = "select * from ts_uidp_login where LOGIN_ID='" + LOGIN_ID + "'";
            return db.GetDataTable(sql);
        }
        /// <summary>
        /// LOGIN_CODE查询ts_uidp_login
        /// </summary>
        /// <param name="LOGIN_CODE"></param>
        /// <returns></returns>
        public DataTable GetUserLoginByLOGIN_CODE(string LOGIN_CODE)
        {
            string sql = "select * from ts_uidp_login where LOGIN_CODE='" + LOGIN_CODE + "'";
            return db.GetDataTable(sql);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string updateUserLoginData(Dictionary<string, object> d)
        {
            string sql = " update ts_uidp_login set LOGIN_CODE='";
            sql += d["LOGIN_CODE"] == null ? "" : d["LOGIN_CODE"].ToString() + "',";
            sql += " LOGIN_REMARK='";
            sql += d["LOGIN_REMARK"] == null ? "" : d["LOGIN_REMARK"].ToString() + "' ";
            sql += " where LOGIN_ID='";
            sql += d["LOGIN_ID"].ToString() + "'";
            return db.ExecutByStringResult(sql);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string updateUserLoginArticle(Dictionary<string, object> d)
        {
            string sql = " delete from  ts_uidp_login ";
            sql += " where LOGIN_ID='" + d["LOGIN_ID"].ToString() + "'";
            return db.ExecutByStringResult(sql);
        }
        public DataTable fetchUserLoginList(string LOGIN_REMARK, string sort)
        {
            string sql = " select a.*,c.USER_NAME from ts_uidp_login a  ";
            sql += " left join ts_uidp_login_user b on a.LOGIN_ID=b.LOGIN_ID ";
            sql += " left join ts_uidp_userinfo c on c.USER_ID=b.USER_ID ";
            if (!string.IsNullOrEmpty(LOGIN_REMARK))
            {
                sql += " where a.LOGIN_REMARK like '%" + LOGIN_REMARK + "%'";
            }
            if (sort != null && sort == "-LOGIN_ID")
            {
                sql += " order by a.LOGIN_ID desc";
            }
            else
            {
                sql += "order by a.LOGIN_ID";
            }
            return db.GetDataTable(sql);
        }
        /// <summary>
        /// 通过LOGIN_ID查用户信息
        /// </summary>
        /// <param name="LOGIN_ID"></param>
        /// <returns></returns>
        public DataTable fetchUserLoginListById(string LOGIN_ID)
        {
            string sql = " select c.USER_ID,c.USER_CODE,c.USER_NAME,c.USER_DOMAIN from ts_uidp_login a  ";
            sql += "  join ts_uidp_login_user b on a.LOGIN_ID=b.LOGIN_ID ";
            sql += "  join ts_uidp_userinfo c on c.USER_ID=b.USER_ID ";
            sql += " where a.LOGIN_ID ='" + LOGIN_ID + "'";
            sql += " order by a.LOGIN_ID";
            return db.GetDataTable(sql);
        }
        public DataTable fetchUserForLoginList(string USER_NAME, string LOGIN_ID)
        {
            string sql = " select c.USER_ID,c.USER_CODE,c.USER_NAME,c.USER_DOMAIN,a.LOGIN_ID,a.LOGIN_REMARK from ts_uidp_userinfo c  ";
            sql += " left join ts_uidp_login_user b on  c.USER_ID=b.USER_ID ";
            sql += " left join ts_uidp_login a  on a.LOGIN_ID=b.LOGIN_ID where 1=1 ";
            if (!string.IsNullOrEmpty(USER_NAME))
            {
                sql += " and c.USER_NAME like '%" + USER_NAME + "%'";
            }
            if (LOGIN_ID != null && LOGIN_ID != "")
            {
                sql += " AND  a.LOGIN_ID ='" + LOGIN_ID + "'";
            }
            return db.GetDataTable(sql);
        }
        /// <summary>
        /// 分配组织结构给用户
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string updateUserForLoginArticle(Dictionary<string, object> d)
        {
            // string[] array = d["multipleSelection"].ToString().Split(',');
            var array = (JArray)d["arr"];
            string fengefu = "";
            string sql = " insert into ts_uidp_login_user(LOGIN_ID,USER_ID)values ";
            string delSql = "delete from ts_uidp_login_user where  USER_ID in (";
            string userid = "";
            foreach (var item in array)
            {
                userid+= fengefu + "'" + item.ToString() + "'";
                delSql += fengefu + "'" + item.ToString() + "'";
                sql += fengefu + "(";
                sql += "'" + d["LOGIN_ID"].ToString() + "','" + item.ToString() + "'";
                sql += ")";
                fengefu = ",";
            }
            delSql += ")";
            string sqlUser = "select USER_DOMAIN from ts_uidp_userinfo where USER_ID in(" + userid + ")";
            DataTable dt = db.GetDataTable(sqlUser);
            fengefu = "";
            userid = "";
            foreach (DataRow  row  in dt.Rows)
            {
                userid += fengefu  + row[0].ToString();
                fengefu = ",";
            }
            string sqlUpdateUserInfo = "  update ts_uidp_userinfo set ASSOCIATED_ACCOUNT='" + userid + "' where USER_ID='"+ d["LOGIN_ID"].ToString()+"'";
            List<string> list = new List<string>();
            list.Add(delSql);
            list.Add(sql);
            list.Add(sqlUpdateUserInfo);
            return db.Executs(list);
        }
        /// <summary>
        /// 清空
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string deleteUserForLoginArticle(Dictionary<string, object> d)
        {
            var array = (JArray)d["arr"];
            if (array == null || array.Count == 0)
            {
                return "";
            }
            string fengefu = "";
            string delSql = " delete from ts_uidp_login_user where  USER_ID in(";
            foreach (var item in array)
            {
                delSql += fengefu + "'" + item.ToString() + "'";
                fengefu = ",";
            }
            delSql += ")";
            return db.ExecutByStringResult(delSql);
        }

        /// <summary>
        /// 根据usercode查用户信息
        /// </summary>
        /// <param name="userCode"></param>
        /// <returns></returns>
        public DataTable getUserInfoByName(string userCode)
        {
            if (!string.IsNullOrEmpty(userCode))
            {
                string sql = String.Format("select * from ts_uidp_userinfo where USER_DOMAIN='{0}' "
                   , userCode);
                return db.GetDataTable(sql);
            }
            throw new Exception("用户名不能为空！");

        }

        public DataTable getLoginByID(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                string sql = String.Format(@"SELECT ts_uidp_userinfo.* from (
select USER_ID from ts_uidp_login_user
where LOGIN_ID = '{0}'
union
select LOGIN_ID from ts_uidp_login_user
where USER_ID = '{0}'
union
select '{0}'
) tbl
LEFT JOIN
ts_uidp_userinfo
on tbl.USER_ID = ts_uidp_userinfo.USER_ID "
                   , userId);
                return db.GetDataTable(sql);
            }
            throw new Exception("用户ID不能为空！");
        }
    }
}
