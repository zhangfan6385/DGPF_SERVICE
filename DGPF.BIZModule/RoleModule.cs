using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using DGPF.ODS;
using Newtonsoft.Json;

namespace DGPF.BIZModule
{
   public class RoleModule
    {
        RoleDB db = new RoleDB();
        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> fetchRoleList(Dictionary<string, object> d)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = db.GetRoleBySYS_CODE(d);
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
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string createRoleArticle(Dictionary<string, object> d)
        {
            d["id"] = CreateId(28);
            return db.createRoleArticle(d);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string updateRoleData(Dictionary<string, object> d)
        {
            return db.updateRoleData(d);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string updateRoleArticle(Dictionary<string, object> d)
        {
            return db.updateRoleArticle(d);
        }
        /// <summary>
        /// 系统自动生成orgId
        /// </summary>
        /// <returns></returns>
        public string CreateId(int CreateOrgIdcount)
        {
            string roleId = string.Empty;
            DataTable dt = new DataTable();
            roleId = GenerateCheckCode(CreateOrgIdcount);
            dt = db.GetRoleById(roleId);
            while (dt != null && dt.Rows.Count > 0)
            {
                roleId = GenerateCheckCode(CreateOrgIdcount);
                dt = db.GetRoleById(roleId);
            }
            return roleId;
        }

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
                    string id = dr["GROUP_ID"].ToString();
                    sb.Append("{");
                    sb.AppendFormat("\"id\":\"{0}\",", dr["ORG_ID"]);
                    sb.AppendFormat("\"parentId\":\"icon_{0}\",", dr["ORG_CODE_UPPER"]);
                    sb.AppendFormat("\"groupName\":\"{0}\",", dr["ORG_NAME_FULL"]);
                    sb.AppendFormat("\"groupCode\":\"{0}\",", dr["PHONE"]);
                    sb.AppendFormat("\"sysCode\":\"{0}\",", dr["PHONE_S"]);
                    sb.AppendFormat("\"remark\":\"{0}\",", dr["PHONE_FAX"]);
                    sb.Append(",\"children\":[");
                    sb.Append(GetSubMenu(id, dt));
                    sb.Append("]");
                    sb.Append("}");
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// D.	分配角色给用户
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string updateUserRoleArticle(Dictionary<string, object> d)
        {
            return db.updateUserRoleArticle(d);
        }
    }
}
