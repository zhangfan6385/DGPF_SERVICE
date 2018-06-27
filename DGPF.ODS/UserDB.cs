﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using DGPF.UTILITY;
namespace DGPF.ODS
{
    public class UserDB
    {
        DBTool db = new DBTool("MYSQL");
        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public DataTable fetchUserList(Dictionary<string, object> d)
        {
            string sql = "select a.*,b.ORG_NAME from ts_uidp_userinfo a ";
            sql += " left join ts_uidp_org_user c on c.USER_ID=a.USER_ID ";
            sql += " left join ts_uidp_org b on b.ORG_ID=c.ORG_ID  ";
            sql += " where 1=1 ";
            if (d["USER_NAME"] != null && d["USER_NAME"].ToString() != "")
            {
                sql += " and a.USER_NAME like '%" + d["USER_NAME"].ToString() + "%'";
            }
            if (d["FLAG"] != null && d["FLAG"].ToString() != "")
            {
                sql += " and a.FLAG=" + d["FLAG"].ToString();
            }
            if (d["sort"] != null && d["sort"].ToString() != "" && d["sort"].ToString() == "-USER_ID")
            {
                sql += " order by a.USER_ID DESC";
            }
            else {
                sql += " order by a.USER_ID ASC";
            }
            return db.GetDataTable(sql);
        }
        public string createUserArticle(Dictionary<string, object> d)
        {
            string col = "";
            string val = "";
            foreach (var v in d)
            {
                col += "," + v.Key;
                if (v.Value.GetType().ToString()== "System.Int64") {
                    val += "," + v.Value + "";
                }
                else
                {
                    val += ",'" + v.Value + "'";
                }
            }
            if (col != "")
            {
                col = col.Substring(1);
            }
            if (val != "")
            {
                val = val.Substring(1);
            }
            string sql = "INSERT INTO ts_uidp_userinfo(" + col + ") VALUES(" + val + ") ;";
            return db.ExecutByStringResult(sql);
        }

        public string updateUserArticle(Dictionary<string, object> d)
        {
            string sql = "delete FROM ts_uidp_userinfo where USER_ID='" + d["USER_ID"].ToString() + "' ;";

            return db.ExecutByStringResult(sql);
        }
        public string updateUserFlag(Dictionary<string, object> d)
        {
            string sql = "update  ts_uidp_userinfo set FLAG="+d["FLAG"] +" where USER_ID='" + d["USER_ID"].ToString() + "' ;";

            return db.ExecutByStringResult(sql);
        }
        public string updatePasswordData(Dictionary<string, object> d)
        {
            string sql = "update  ts_uidp_userinfo set USER_PASS=" + d["newpassword"].ToString() + " where USER_ID='" + d["userid"].ToString() + "' and USER_PASS='"+d["password"].ToString() + "' ;";

            return db.ExecutByStringResult(sql);
        }
        public DataTable IsInvalidPassword(Dictionary<string, object> d)
        {
            string sql = "select * from  ts_uidp_userinfo  where USER_ID='" + d["userid"].ToString() + "' and USER_PASS='" + d["password"].ToString() + "' ;";

            return db.GetDataTable(sql);
        }

        public string updateUserData(Dictionary<string, object> d) {
            StringBuilder sb = new StringBuilder();
            sb.Append(" update ts_uidp_userinfo set ");
            sb.Append(" USER_CODE='" + d["USER_CODE"] + "', ");
            sb.Append(" USER_NAME='" + d["USER_NAME"] + "', ");
            sb.Append(" USER_ALIAS='" + d["USER_ALIAS"] + "', ");
            sb.Append(" USER_PASS='" + d["USER_PASS"] + "', ");
            sb.Append(" PHONE_MOBILE='" + d["PHONE_MOBILE"] + "', ");
            sb.Append(" PHONE_OFFICE='" + d["PHONE_OFFICE"] + "', ");
            sb.Append(" PHONE_ORG='" + d["PHONE_ORG"] + "', ");
            sb.Append(" USER_EMAIL='" + d["USER_EMAIL"] + "', ");
            sb.Append(" EMAIL_OFFICE='" + d["EMAIL_OFFICE"] + "', ");
            sb.Append(" USER_IP='" + d["USER_IP"] + "', ");
            sb.Append(" FLAG=" + d["FLAG"] + ", ");
            sb.Append(" USER_SEX="+d["USER_SEX"]+",");
            sb.Append(" USER_DOMAIN='" + d["USER_DOMAIN"] + "', ");
            sb.Append(" REMARK='" + d["REMARK"] + "' ");
            sb.Append(" where USER_ID='" + d["USER_ID"].ToString() + "' ");
            return db.ExecutByStringResult(sb.ToString());
        }
        public DataTable  GetUserInfoByUserId(string userId)
        {
            string sql = "select * from ts_uidp_userinfo where USER_ID='" + userId + "' ";
            return db.GetDataTable(sql);
        }
        public DataTable GetUserInfoByUserCode(string userCode,string userid)
        {
            string sql = "select * from ts_uidp_userinfo where USER_CODE='" + userCode + "' ";
            if (!string.IsNullOrEmpty(userid))
            {
                sql += " and USER_ID!='" + userid + "'";
            }
            return db.GetDataTable(sql);
        }
        public DataTable GetUserInfoBylogin(string username, string userDomain)
        {
            string sql = "select * from ts_uidp_userinfo where ";
            if (userDomain== "userDomain") {
                sql += " USER_DOMAIN = '" + username + "' ";
            }
            else
            {//userDomain=user
                sql += " USER_CODE = '" + username + "' ";
            }
            return db.GetDataTable(sql);
        }
        public DataTable GetUserInfoByUSER_DOMAIN(string USER_DOMAIN,string userid)
        {
            string sql = "select * from ts_uidp_userinfo where USER_DOMAIN='" + USER_DOMAIN + "' ";
            if (!string.IsNullOrEmpty(userid)) {
                sql += " and USER_ID!='"+userid+"'";
            }
            return db.GetDataTable(sql);
        }
        /// <summary>
        /// 获取管理员账号
        /// </summary>
        /// <returns></returns>
        public string getAdminCode() {
            string sqluser = "SELECT conf_value from ts_uidp_config where conf_code= 'Admin_Code'";
            return  db.GetString(sqluser);
        }/// <summary>
        /// 获取管理员密码
        /// </summary>
        /// <returns></returns>
        public string getAdminPass()
        {
            string sqluser = "SELECT conf_value from ts_uidp_config where conf_code= 'Admin_Password'";
            return db.GetString(sqluser);
        }
        /// <summary>
        /// 根据userid获取组织机构信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataTable GetUserOrg(string userId) {
            string sql = "select a.ORG_ID orgId ,a.ORG_NAME orgName, a.ORG_CODE orgCode from ts_uidp_org a ";
            sql += " join ts_uidp_org_user b on a.ORG_ID=b.ORG_ID ";
            sql += " where b.USER_ID='"+userId+"'; ";
            return db.GetDataTable(sql);

        }
        /// <summary>
        /// 根据userid获取用户和角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataTable GetUserAndGroup(string userId)
        {
            string sql = "select a.*,c.* from  ts_uidp_userinfo a ";
            sql += " left join ts_uidp_group_user b on a.USER_ID=b.USER_ID ";
            sql += " left join ts_uidp_groupinfo c on   c.GROUP_ID=b.GROUP_ID ";
            sql += " where a.USER_ID='" + userId + "'; ";
            return db.GetDataTable(sql);

        }
        /// <summary>
        /// 查询用户信息（包括组织机构）
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public DataTable fetchUserOrgList(Dictionary<string, object> d)
        {
            string sql = "select a.*,b.ORG_ID orgId,b.ORG_CODE,b.ORG_NAME orgName from ts_uidp_userinfo a";
            sql += " left join ts_uidp_org_user c on c.USER_ID=a.USER_ID ";
            sql += " left join ts_uidp_org b on b.ORG_ID=c.ORG_ID  where 1=1 ";
            if (d["USER_NAME"] != null && d["USER_NAME"].ToString() != "")
            {
                sql += " and a.USER_NAME like '%" + d["USER_NAME"].ToString() + "%'";
            }
            if (d["FLAG"] != null && d["FLAG"].ToString() != "")
            {
                sql += " and a.FLAG=" + d["FLAG"].ToString();
            }
            if (d["sort"] != null && d["sort"].ToString() != "" && d["sort"].ToString() == "-USER_ID")
            {
                sql += " order by a.USER_ID DESC";
            }
            else
            {
                sql += " order by a.USER_ID ASC";
            }
            if (d["orgId"] != null) {
                 sql = "select a.*,b.ORG_ID orgId,b.ORG_CODE,b.ORG_NAME orgName from ts_uidp_userinfo a";
                sql += "  join ts_uidp_org_user c on c.USER_ID=a.USER_ID ";
                sql += "  join ts_uidp_org b on b.ORG_ID=c.ORG_ID  where 1=1 ";
                sql += " and a.USER_ID in (select a.USER_ID from ts_uidp_userinfo a";
                sql += "  join ts_uidp_org_user c on c.USER_ID=a.USER_ID ";
                sql += "  join ts_uidp_org b on b.ORG_ID=c.ORG_ID  where 1=1 ";
                if (d["USER_NAME"] != null && d["USER_NAME"].ToString() != "")
                {
                    sql += " and a.USER_NAME like '%" + d["USER_NAME"].ToString() + "%'";
                }
                if (d["FLAG"] != null && d["FLAG"].ToString() != "")
                {
                    sql += " and a.FLAG=" + d["FLAG"].ToString();
                }
                if (d["orgId"] != null)
                {
                    sql += " and b.ORG_ID='" + d["orgId"].ToString()+"' ";
                }
                sql += " ) ";
                if (d["sort"] != null && d["sort"].ToString() != "" && d["sort"].ToString() == "-USER_ID")
                {
                    sql += " order by a.USER_ID DESC";
                }
                else
                {
                    sql += " order by a.USER_ID ASC";
                }
            }
            return db.GetDataTable(sql);
        }
        /// <summary>
        /// 查询用户信息包括角色信息
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public DataTable fetchUserRoleList(Dictionary<string, object> d)
        {
            string sql = "select a.*,b.GROUP_ID roleId,b.GROUP_NAME groupName from ts_uidp_userinfo a";
            sql += " left join ts_uidp_group_user c on c.USER_ID=a.USER_ID ";
            sql += " left join ts_uidp_groupinfo b on b.GROUP_ID=c.GROUP_ID where 1=1 ";
            if (d["USER_NAME"] != null && d["USER_NAME"].ToString() != "")
            {
                sql += " and a.USER_NAME like '%" + d["USER_NAME"].ToString() + "%'";
            }
            if (d["FLAG"] != null && d["FLAG"].ToString() != "")
            {
                sql += " and a.FLAG=" + d["FLAG"].ToString();
            }
            if (d["sort"] != null && d["sort"].ToString() != "" && d["sort"].ToString() == "-USER_ID")
            {
                sql += " order by a.USER_ID desc";
            }
            else
            {
                sql += " order by a.USER_ID asc";
            }
            //如果roleid不为空时，只差有roleid 的
            if (d["roleId"] != null && d["roleId"].ToString() != "")
            {
                sql = "select a.*,b.GROUP_ID roleId,b.GROUP_NAME groupName from ts_uidp_userinfo a";
                sql += "  join ts_uidp_group_user c on c.USER_ID=a.USER_ID ";
                sql += "  join ts_uidp_groupinfo b on b.GROUP_ID=c.GROUP_ID where 1=1 ";

                sql += " and a.USER_ID IN ( select a.USER_ID from ts_uidp_userinfo a";
                sql += "  join ts_uidp_group_user c on c.USER_ID=a.USER_ID ";
                sql += "  join ts_uidp_groupinfo b on b.GROUP_ID=c.GROUP_ID where 1=1 ";
                if (d["USER_NAME"] != null && d["USER_NAME"].ToString() != "")
                {
                    sql += " and a.USER_NAME like '%" + d["USER_NAME"].ToString() + "%'";
                }
                if (d["FLAG"] != null && d["FLAG"].ToString() != "")
                {
                    sql += " and a.FLAG=" + d["FLAG"].ToString();
                }
                if (d["roleId"] != null && d["roleId"].ToString() != "")
                {
                    sql += " and b.GROUP_ID='" + d["roleId"].ToString() + "' ";
                }
                sql += ")";
                if (d["sort"] != null && d["sort"].ToString() != "" && d["sort"].ToString() == "-USER_ID")
                {
                    sql += " order by a.USER_ID desc";
                }
                else
                {
                    sql += " order by a.USER_ID asc";
                }
            }

            return db.GetDataTable(sql);
        }
    }
}