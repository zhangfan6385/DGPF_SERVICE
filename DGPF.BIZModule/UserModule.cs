using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using DGPF.ODS;
using DGPF.UTILITY;
namespace DGPF.BIZModule
{
    public class UserModule
    {
        UserDB db = new UserDB();
        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public Dictionary<string, object> fetchUserList(Dictionary<string, object> d)
        {

            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {

                int limit = d["limit"] == null ? 100 : int.Parse(d["limit"].ToString());
                int page = d["page"] == null ? 1 : int.Parse(d["page"].ToString());

                DataTable dt = db.fetchUserList(d);
                r["total"] = dt.Rows.Count;
                r["items"] = KVTool.TableToListDic(KVTool.GetPagedTable(dt, page, limit));
                r["code"] = 2000;
                r["message"] = "查询成功";
            }
            catch (Exception e)
            {
                r["total"] = 0;
                r["items"] = null;
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }

        public string createUserArticle(Dictionary<string, object> d)
        {
            return db.createUserArticle(d);
        }
        public string updateUserArticle(Dictionary<string, object> d)
        {
            return db.updateUserArticle(d);
        }
        public string updateUserFlag(Dictionary<string, object> d)
        {
            return db.updateUserFlag(d);
        }
        public string updatePasswordData(Dictionary<string, object> d)
        {
            return db.updatePasswordData(d);
        }
        public string updateUserData(Dictionary<string,object> d){
            return db.updateUserData(d);
        }
        public DGPF.BIZModule.Models.ts_uidp_userinfo getUserInfoByUserId(string userId) {
            DataTable dt = db.GetUserInfoByUserId(userId);
            DGPF.BIZModule.Models.ts_uidp_userinfo mod = new Models.ts_uidp_userinfo();
            if (dt!=null&&dt.Rows.Count>0) {
                mod = DataRowToModel(dt.Rows[0]);
            }
            return mod;
        }
        public DGPF.BIZModule.Models.ts_uidp_userinfo getUserInfoByUserCode(string userCode)
        {
            DataTable dt = db.GetUserInfoByUserCode(userCode);
            DGPF.BIZModule.Models.ts_uidp_userinfo mod = new Models.ts_uidp_userinfo();
            if (dt != null && dt.Rows.Count > 0)
            {
                mod = DataRowToModel(dt.Rows[0]);
            }
            return mod;
        }
        public DGPF.BIZModule.Models.ts_uidp_userinfo getUserInfoByToken(string token) {
            string userid = AccessTokenTool.GetUserId(token);
            return getUserInfoByUserId(userid);
        }
        /// <summary>
        /// 根据userid 获取用户组织机构信息列表
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public DataTable GetUserOrg(string userId)
        {
            return db.GetUserOrg(userId);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public DGPF.BIZModule.Models.ts_uidp_userinfo DataRowToModel(DataRow row)
        {
            DGPF.BIZModule.Models.ts_uidp_userinfo model = new DGPF.BIZModule.Models.ts_uidp_userinfo();
            if (row != null)
            {
                if (row["USER_ID"] != null)
                {
                    model.USER_ID = row["USER_ID"].ToString();
                }
                if (row["USER_CODE"] != null)
                {
                    model.USER_CODE = row["USER_CODE"].ToString();
                }
                if (row["USER_NAME"] != null)
                {
                    model.USER_NAME = row["USER_NAME"].ToString();
                }
                if (row["USER_ALIAS"] != null)
                {
                    model.USER_ALIAS = row["USER_ALIAS"].ToString();
                }
                if (row["USER_PASS"] != null)
                {
                    model.USER_PASS = row["USER_PASS"].ToString();
                }
                if (row["PHONE_MOBILE"] != null)
                {
                    model.PHONE_MOBILE = row["PHONE_MOBILE"].ToString();
                }
                if (row["PHONE_OFFICE"] != null)
                {
                    model.PHONE_OFFICE = row["PHONE_OFFICE"].ToString();
                }
                if (row["PHONE_ORG"] != null)
                {
                    model.PHONE_ORG = row["PHONE_ORG"].ToString();
                }
                if (row["USER_EMAIL"] != null)
                {
                    model.USER_EMAIL = row["USER_EMAIL"].ToString();
                }
                if (row["EMAIL_OFFICE"] != null)
                {
                    model.EMAIL_OFFICE = row["EMAIL_OFFICE"].ToString();
                }
                if (row["USER_IP"] != null)
                {
                    model.USER_IP = row["USER_IP"].ToString();
                }
                if (row["REG_TIME"] != null && row["REG_TIME"].ToString() != "")
                {
                    model.REG_TIME = DateTime.Parse(row["REG_TIME"].ToString());
                }
                if (row["FLAG"] != null && row["FLAG"].ToString() != "")
                {
                    model.FLAG = int.Parse(row["FLAG"].ToString());
                }
                if (row["USER_DOMAIN"] != null)
                {
                    model.USER_DOMAIN = row["USER_DOMAIN"].ToString();
                }
                if (row["REMARK"] != null)
                {
                    model.REMARK = row["REMARK"].ToString();
                }
            }
            return model;
        }
        /// <summary>
        /// 系统自动生成userid
        /// </summary>
        /// <returns></returns>
        public string CreateUserId(int userIdCount) {
            string userId = string.Empty;
            DataTable dt=new DataTable();
            userId = GenerateCheckCode(userIdCount);
            dt = db.GetUserInfoByUserId(userId);
            while (dt!=null&&dt.Rows.Count>0) {
                userId = GenerateCheckCode(userIdCount); 
                dt = db.GetUserInfoByUserId(userId);
            }
            return userId;
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
        /// 查询用户信息(包含组织结构)
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public Dictionary<string, object> fetchUserOrgList(Dictionary<string, object> d)
        {

            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {

                int limit = d["limit"] == null ? 100 : int.Parse(d["limit"].ToString());
                int page = d["page"] == null ? 1 : int.Parse(d["page"].ToString());

                DataTable dt = db.fetchUserList(d);
                r["total"] = dt.Rows.Count;
                r["items"] = KVTool.TableToListDic(KVTool.GetPagedTable(dt, page, limit));
                r["code"] = 2000;
                r["message"] = "查询成功";
            }
            catch (Exception e)
            {
                r["total"] = 0;
                r["items"] = null;
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }
        /// <summary>
        /// 查询用户信息（包含角色信息）
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public Dictionary<string, object> fetchUserRoleList(Dictionary<string, object> d)
        {

            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {

                int limit = d["limit"] == null ? 100 : int.Parse(d["limit"].ToString());
                int page = d["page"] == null ? 1 : int.Parse(d["page"].ToString());

                DataTable dt = db.fetchUserRoleList(d);
                r["total"] = dt.Rows.Count;
                r["items"] = KVTool.TableToListDic(KVTool.GetPagedTable(dt, page, limit));
                r["code"] = 2000;
                r["message"] = "查询成功";
            }
            catch (Exception e)
            {
                r["total"] = 0;
                r["items"] = null;
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return r;
        }
    }
}
