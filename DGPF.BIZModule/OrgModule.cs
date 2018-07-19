using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using DGPF.ODS;
using Newtonsoft.Json;
namespace DGPF.BIZModule
{
   public class OrgModule
    {
        OrgDB db = new OrgDB();
        public Dictionary<string, object> fetchOrgList(bool isAdmin) {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = db.fetchOrgList();
                string jsonStr = "";
                if (dt != null && dt.Rows.Count > 0)
                {
                    jsonStr = GetSubMenu("", dt,isAdmin);
                }
                r["items"] = JsonConvert.DeserializeObject("[" + jsonStr+"]");
                r["code"] = 2000;
                r["message"] = "查询成功";
            }
            catch (Exception ex)
            {
                r["items"] = null;
                r["code"] = -1;
                r["message"] = ex.Message;
            }
            return r;
        }
        public string createOrgArticle(Dictionary<string, object> d)
        {
            d["id"] = Guid.NewGuid().ToString();//CreateOrgId(28);
            return db.createOrgArticle(d);
        }

        /// <summary>
        /// 系统自动生成orgId
        /// </summary>
        /// <returns></returns>
        public string CreateOrgId(int CreateOrgIdcount)
        {
            string OrgId = string.Empty;
            DataTable dt = new DataTable();
            OrgId = GenerateCheckCode(CreateOrgIdcount);
            dt = db.GetOrgById(OrgId);
            while (dt != null && dt.Rows.Count > 0)
            {
                OrgId = GenerateCheckCode(CreateOrgIdcount);
                dt = db.GetOrgById(OrgId);
            }
            return OrgId;
        }
        /// <summary>
        /// 
        /// </summary>
        private int rep = 0;
        /// 
        /// 生成随机字母字符串(数字字母混和)
        /// 
        /// 待生成的位数
        /// 生成的字母字符串
        private string GenerateCheckCode(int codeCount)
        {
            string str = string.Empty;
            long num2 = DateTime.Now.Ticks + this.rep;
            this.rep++;
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> this.rep)));
            for (int i = 0; i < codeCount; i++)
            {
                char ch;
                int num = random.Next();
                if ((num % 2) == 0)
                {
                    ch = (char)(0x30 + ((ushort)(num % 10)));
                }
                else
                {
                    ch = (char)(0x41 + ((ushort)(num % 0x1a)));
                }
                str = str + ch.ToString();
            }
            return str;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string updateOrgData(Dictionary<string, object> d)
        {
            return db.updateOrgData(d);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string updateOrgArticle(Dictionary<string, object> d)
        {
            if (d["id"]==null) {
                return "无组织机构id";
            }
            DataTable dt = db.fetchOrgList();
            string strIds = getOrgIds(d["id"].ToString(), dt);
            if (strIds != null && strIds != "")
            {
                strIds += ",'" + d["id"].ToString() + "'";
            }
            else {
                strIds = "'" + d["id"].ToString() + "'";
            }
            return db.updateOrgArticle(strIds);
        }
        /// <summary>
        /// 所有子节点的id  逗号隔开
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string getOrgIds(string pid,DataTable dt) {
            StringBuilder sb = new StringBuilder();
            DataRow[] rows = dt.Select("ORG_CODE_UPPER='" + pid + "'");
            if (rows.Length > 0)
            {
                bool isFist = false;
                foreach (DataRow dr in rows)
                {
                    if (isFist)
                        sb.Append(",");
                    isFist = true;
                    string id = dr["ORG_ID"].ToString();
                    sb.AppendFormat("'{0}'", dr["ORG_ID"] == null ? "" : dr["ORG_ID"]);
                    sb.Append(getOrgIds(id, dt));
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// D.	分配组织结构给用户
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string updateUserOrgArticle(Dictionary<string, object> d) {
            return db.updateUserOrgArticle(d);
        }

        /// <summary>
        /// 递归调用生成无限级别
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        private string GetSubMenu(string pid, DataTable dt,bool isAdmin)
        {
            StringBuilder sb = new StringBuilder();
            DataRow[] rows = dt.Select("ORG_CODE_UPPER='" + pid+"'");
            if (rows.Length > 0)
            {
                bool isFist = false;
                foreach (DataRow dr in rows)
                {
                    if (isAdmin)
                    {
                        if (dr["ISINVALID"].ToString() == "0")
                        {
                            dr["ORG_NAME"] = dr["ORG_NAME"] == null ? "" : dr["ORG_NAME"] + "(无效)";
                        }
                    }
                    else
                    {
                        if (dr["ISINVALID"].ToString() == "0")
                        {
                            continue;
                        }
                    }
                    if (isFist)
                        sb.Append(",");
                    isFist = true;
                    string id = dr["ORG_CODE"].ToString();
                    sb.Append("{");
                    sb.AppendFormat("\"id\":\"{0}\",", dr["ORG_ID"]==null?"": dr["ORG_ID"]);
                    sb.AppendFormat("\"orgCode\":\"{0}\",", dr["ORG_CODE"]==null?"":dr["ORG_CODE"]);
                    sb.AppendFormat("\"orgName\":\"{0}\",", dr["ORG_NAME"]==null?"": dr["ORG_NAME"]);
                    sb.AppendFormat("\"parentId\":\"{0}\",", dr["ORG_CODE_UPPER"]==null?"":dr["ORG_CODE_UPPER"]);
                    sb.AppendFormat("\"ISINVALID\":\"{0}\",", dr["ISINVALID"] ==null?"":dr["ISINVALID"]);
                    sb.AppendFormat("\"remark\":\"{0}\"", dr["REMARK"]==null?"" : dr["REMARK"]);
                    sb.Append(",\"children\":[");
                    sb.Append(GetSubMenu(id, dt, isAdmin));
                    sb.Append("]");
                    sb.Append("}");
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// 清空用户组织机构
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string deleteUserOrgArticle(Dictionary<string, object> d) {
            return db.deleteUserOrgArticle(d);
        }
        public string UploadOrgFile(string filePath) {
            string modePath = System.IO.Directory.GetCurrentDirectory() + "\\ExcelModel\\组织结构模板.xlsx";//原始文件
            string path = filePath;//原始文件
            string mes = "";
            DataTable dt = new DataTable();
            UTILITY.ExcelTools tool = new UTILITY.ExcelTools();
            tool.GetDataTable(System.IO.File.OpenRead(path), path, modePath, ref mes, ref dt);

            if (dt==null||dt.Rows.Count==0) {
                return "空数据，导入失败！";
            }
            DataView dv = new DataView(dt);
            if (dt.Rows.Count!=dv.ToTable(true, "组织机构编码").Rows.Count) {
                return "组织机构编码存在重复数据，导入失败！";
            }
            string fengefu = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(" insert into ts_uidp_org (ORG_ID,ORG_CODE,ORG_NAME,ORG_CODE_UPPER,ISINVALID,ISDELETE,REMARK) values ");
            foreach (DataRow row in dt.Rows)
            {
                sb.Append(fengefu+"('"+Guid.NewGuid().ToString()+"',");
                sb.Append("'" + getString(row["组织机构编码"])+ "',");
                sb.Append("'" + getString(row["组织机构名称"]) + "',");
                sb.Append("'" + getString(row["上级组织机构编码"]) + "',");
                if (row["是否有效"] != null && row["是否有效"].ToString() == "是")
                {
                    sb.Append("'1',");
                }
                else {
                    sb.Append("'0',");
                }
                sb.Append("'1',");
                sb.Append("'" + getString(row["备注"]) + "')");
                fengefu = ",";
            }
            return db.UploadOrgFile(sb.ToString());
        }
        public string getString(object obj) {
            if (obj==null) {
                return "";
            }
            return obj.ToString();
        }
    }
}
