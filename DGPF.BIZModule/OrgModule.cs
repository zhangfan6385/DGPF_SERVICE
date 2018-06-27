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
        public Dictionary<string, object> fetchOrgList() {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = db.fetchOrgList();
                string jsonStr = "";
                if (dt != null && dt.Rows.Count > 0)
                {
                    jsonStr = GetSubMenu("''", dt);
                }
                r["items"] = JsonConvert.DeserializeObject(jsonStr);
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
            d["id"] = CreateOrgId(28);
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
            return db.updateOrgArticle(d);
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
        private string GetSubMenu(string pid, DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            DataRow[] rows = dt.Select("ORG_CODE_UPPER=" + pid);
            if (rows.Length > 0)
            {
                bool isFist = false;
                foreach (DataRow dr in rows)
                {
                    if (isFist)
                        sb.Append(",");
                    isFist = true;
                    string id = dr["ORG_ID"].ToString();
                    sb.Append("{");
                    sb.AppendFormat("\"id\":\"{0}\",", dr["ORG_ID"]);
                    sb.AppendFormat("\"orgCode\":\"{0}\",", dr["ORG_CODE"]);
                    sb.AppendFormat("\"orgName\":\"{0}\",", dr["ORG_NAME"]);
                    sb.AppendFormat("\"parentId\":\"icon_{0}\",", dr["ORG_CODE_UPPER"]);
                    sb.AppendFormat("\"orgNameFull\":\"{0}\",", dr["ORG_NAME_FULL"]);
                    sb.AppendFormat("\"phone\":\"{0}\",", dr["PHONE"]);
                    sb.AppendFormat("\"phoneS\":\"{0}\",", dr["PHONE_S"]);
                    sb.AppendFormat("\"phoneFax\":\"{0}\",", dr["PHONE_FAX"]);
                    sb.AppendFormat("\"orgAddr\":\"{0}\",", dr["ORG_ADDR"]);
                    sb.AppendFormat("\"remark\":\"{0}\"", dr["REMARK"]);
                    sb.Append(",\"children\":[");
                    sb.Append(GetSubMenuSon(id, dt));
                    sb.Append("]");
                    sb.Append("}");
                }
            }
            return sb.ToString();
        }
        private string GetSubMenuSon(string pid, DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            DataRow[] rows = dt.Select("ORG_CODE_UPPER='" + pid+"'");
            if (rows.Length > 0)
            {
                bool isFist = false;
                foreach (DataRow dr in rows)
                {
                    if (isFist)
                        sb.Append(",");
                    isFist = true;
                    string id = dr["ORG_ID"].ToString();
                    sb.Append("{");
                    sb.AppendFormat("\"id\":\"{0}\",", dr["ORG_ID"]);
                    sb.AppendFormat("\"orgCode\":\"{0}\",", dr["ORG_CODE"]);
                    sb.AppendFormat("\"orgName\":\"{0}\",", dr["ORG_NAME"]);
                    sb.AppendFormat("\"parentId\":\"icon_{0}\",", dr["ORG_CODE_UPPER"]);
                    sb.AppendFormat("\"remark\":\"{0}\"", dr["REMARK"]);
                    sb.Append(",\"children\":[");
                    sb.Append(GetSubMenuSon(id, dt));
                    sb.Append("]");
                    sb.Append("}");
                }
            }
            return sb.ToString();
        }
    }
}
