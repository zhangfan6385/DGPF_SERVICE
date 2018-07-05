using DGPF.UTILITY;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DGPF.ODS
{
   public class OrgDB
    {
        DBTool db = new DBTool("MYSQL");
        /// <summary>
        /// 新增组织结构
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string createOrgArticle(Dictionary<string, object> d)
        {
            string sql = "INSERT INTO ts_uidp_org(ORG_ID,ORG_CODE,ORG_NAME,ORG_CODE_UPPER,ORG_NAME_FULL,ORG_ADDR,PHONE,PHONE_S,PHONE_FAX,REMARK) VALUES(";
            sql += "'" + GetIsNullStr(d["id"]) + "',";
            sql += "'" + GetIsNullStr(d["orgCode"])+"',";
            sql += "'" + GetIsNullStr(d["orgName"]) + "',";
            sql += "'" + GetIsNullStr(d["parentId"]) + "',";
            sql += "'" + GetIsNullStr(d["orgNameFull"])+ "',";
            sql += "'" + GetIsNullStr(d["orgAddr"] )+ "',";
            sql += "'" + GetIsNullStr(d["phone"])  + "',";
            sql += "'" + GetIsNullStr(d["phoneS"])  + "',";
            sql += "'" + GetIsNullStr(d["phoneFax"]) + "',";
            sql += "'" + GetIsNullStr(d["remark"]) + "')";
            return db.ExecutByStringResult(sql);
        }
        public string GetIsNullStr(object obj) {
            if (obj == null)
            {
                return "";
            }
            else {
                return obj.ToString();
            }
        }
        /// <summary>
        /// 修改组织结构
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string updateOrgData(Dictionary<string, object> d)
        {
            string sql = "update  ts_uidp_org set ";
            sql += " ORG_CODE='" + GetIsNullStr(d["orgCode"]) + "'," ;
            sql += " ORG_NAME='" + GetIsNullStr(d["orgName"] ) + "',";
            sql += " ORG_CODE_UPPER='" + GetIsNullStr(d["parentId"])+ "',";
            sql += " ORG_NAME_FULL='" + GetIsNullStr(d["orgNameFull"]) + "',";
            sql += " ORG_ADDR='" + GetIsNullStr(d["orgAddr"]) + "',";
            sql += " PHONE='" + GetIsNullStr(d["phone"]) + "',";
            sql += " PHONE_S='" + GetIsNullStr(d["phoneS"]) + "',";
            sql += " PHONE_FAX='" + GetIsNullStr(d["phoneFax"]) + "',";
            sql += " REMARK='" + GetIsNullStr(d["remark"]) + "'";
            sql += " where ORG_ID='" + d["id"].ToString() + "' ;";

            return db.ExecutByStringResult(sql);
        }
        /// <summary>
        /// 删除组织机构
        /// </summary>
        /// <param name="d">带单引号 逗号分隔的id字符串</param>
        /// <returns></returns>
        public string updateOrgArticle(string strid)
        {
            string sql = "delete FROM ts_uidp_org where ORG_ID in(" + strid + ")";

            return db.ExecutByStringResult(sql);
        }
        /// <summary>
        /// 分配组织结构给用户
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string updateUserOrgArticle(Dictionary<string, object> d)
        {
            // string[] array = d["multipleSelection"].ToString().Split(',');
            var array =(JArray) d["arr"];
            string fengefu = "";
            string sql = " insert into ts_uidp_org_user(ORG_ID,USER_ID)values ";
            string delSql = "delete from ts_uidp_org_user where  USER_ID in (";
            foreach (var item in array)
            {
                delSql += fengefu + "'" +item.ToString()+ "'";
                sql +=fengefu+ "(";
                sql += "'"+d["orgId"].ToString()+"','" + item.ToString()+"'" ;
                sql += ")";
                fengefu = ",";
            }
            delSql += ")";
            List<string> list = new List<string>();
            list.Add(delSql);
            list.Add(sql);
            return db.Executs(list);
        }
        /// <summary>
        /// 通过组织结构id查询
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public DataTable GetOrgById(string orgId) {
            string sql = "select * FROM ts_uidp_org where ORG_ID='" + orgId + "' ;";
            return db.GetDataTable(sql);
        }
        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        public DataTable fetchOrgList() {
            string sql = "select * FROM ts_uidp_org ";
            return db.GetDataTable(sql);
        }
        /// <summary>
        /// 清空用户组织机构
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string deleteUserOrgArticle(Dictionary<string, object> d)
        {
            var array = (JArray)d["arr"];
            if (array == null || array.Count == 0)
            {
                return "";
            }
            string fengefu = "";
            string delSql = " delete from ts_uidp_org_user where  USER_ID in(";
            foreach (var item in array)
            {
                delSql += fengefu + "'" + item.ToString() + "'";
                fengefu = ",";
            }
            delSql += ")";
            return db.ExecutByStringResult(delSql);
        }
    }
}
