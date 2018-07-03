using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using DGPF.BIZModule;
using DGPF.LOG;
using System.Data;
using Newtonsoft.Json;

namespace DGPF.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("User")]
    public class UserController : WebApiBaseController
    {
        UserModule mm = new UserModule();
        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="USER_NAME"></param>
        /// <param name="FLAG"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        // GET api/values
        [HttpGet("fetchUserList")]
        public IActionResult fetchUserList(string limit, string page, string USER_NAME,int? FLAG,string sort)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["limit"] = limit;
            d["page"] = page;
            d["USER_NAME"] = USER_NAME;
            d["FLAG"] = FLAG;
            d["sort"] = sort;
            Dictionary<string, object> res = mm.fetchUserList(d);
            return Json(res);
        }
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("createUserArticle")]
        public IActionResult createUserArticle([FromBody]JObject value) {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                
                string userId = mm.CreateUserId(16);//生成16位userid
                d["USER_ID"] = userId;
                string b = mm.createUserArticle(d);
                if (b == "")
                {
                    r["message"] = "成功";
                    r["code"] = 2000;
                }
                else
                {
                    r["code"] = -1;
                    r["message"] = b;
                }
            }
            catch (Exception e)
            {
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return Json(r);
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("updateUserArticle")]
        public IActionResult updateUserArticle([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = mm.updateUserArticle(d);
                if (b == "")
                {
                    r["message"] = "成功";

                    r["code"] = 2000;
                }
                else
                {
                    r["code"] = -1;
                    r["message"] = b;
                }

            }
            catch (Exception e)
            {
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return Json(r);
        }
        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("updateUserData")]
        public IActionResult updateUserData([FromBody]JObject value)
        {

            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();

            Dictionary<string, object> r = new Dictionary<string, object>();


            try
            {
                string b = mm.updateUserData(d);
                if (b == "")
                {
                    r["message"] = "成功";

                    r["code"] = 2000;
                }
                else
                {
                    r["code"] = -1;
                    r["message"] = b;
                }

            }
            catch (Exception e)
            {
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return Json(r);


        }
        /// <summary>
        /// 激活或者锁用户
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("updateUserFlag")]
        public IActionResult updateUserFlag([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = mm.updateUserFlag(d);
                if (b == "")
                {
                    r["message"] = "成功";

                    r["code"] = 2000;
                }
                else
                {
                    r["code"] = -1;
                    r["message"] = b;
                }

            }
            catch (Exception e)
            {
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return Json(r);
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("updatePasswordData")]
        public IActionResult updatePasswordData([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = mm.updatePasswordData(d);
                if (b == "")
                {
                    r["message"] = "成功";
                    r["code"] = 2000;
                }
                else
                {
                    r["code"] = -1;
                    r["message"] = b;
                }

            }
            catch (Exception e)
            {
                r["code"] = -1;
                r["message"] = e.Message;
            }
            return Json(r);
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpPost("Info")]
        public IActionResult Info([FromBody]JObject value) {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                if (d["userId"].ToString() ==mm.getAdminCode()) {
                    DGPF.LOG.SysLog log = new LOG.SysLog();
                    log.Info(DateTime.Now, d["userId"].ToString(), "系统超级管理员", ClientIp, 0, "info", "");
                    return Json(new {
                        code = 2000,
                        message = "",
                        roles = JsonConvert.DeserializeObject("['admin']") ,
                        name = "系统超级管理员",
                        userCode = d["userId"].ToString(),
                        token = DGPF.UTILITY.AccessTokenTool.GetAccessToken(d["userId"].ToString()),
                        introduction = "",
                        avatar = "",
                        sysCode = "1",
                        sysName = mm.getSysName(),
                        userId = d["userId"].ToString(),
                        userSex = 0,
                        departCode = "",
                        departName = ""
                    });
                }
                string token = DGPF.UTILITY.AccessTokenTool.GetAccessToken(d["userId"].ToString());
                DataTable dt = mm.GetUserAndOrgByUserId(d["userId"].ToString());
                if (dt != null && dt.Rows.Count > 0) { 
                    string _name = dt.Rows[0]["USER_NAME"] == null ? "" : dt.Rows[0]["USER_NAME"].ToString();
                    string _userCode= dt.Rows[0]["USER_CODE"] == null ? "" : dt.Rows[0]["USER_CODE"].ToString();
                    string _userId= dt.Rows[0]["USER_ID"] == null ? "" : dt.Rows[0]["USER_ID"].ToString();
                    int _userSex= Convert.ToInt32(dt.Rows[0]["USER_SEX"].ToString());
                    string _deptCode = dt.Rows[0]["ORG_CODE"] == null ? "" : dt.Rows[0]["ORG_CODE"].ToString();
                    string _deptName = dt.Rows[0]["ORG_NAME"] == null ? "" : dt.Rows[0]["ORG_NAME"].ToString();
                    DGPF.LOG.SysLog log = new LOG.SysLog();
                    log.Info(DateTime.Now, d["userId"].ToString(), _name, ClientIp, 0, "info", "");
                    return Json(new {
                        code = 2000,
                        message = "",
                        roles = new Dictionary<string, object>(),
                        token = token,
                        introduction = "",
                        avatar = "",
                        name = _name,
                        userCode = _userCode,
                        sysCode = "1",
                        sysName = mm.getSysName(),
                        userId = _userId,
                        userSex = _userSex,
                        departCode = _deptCode,
                        departName = _deptName
                    });
                }
                return Json(new {
                    code = 2000,
                    message = "",
                    roles = "",
                    name = "",
                    userCode ="",
                    token = token,
                    introduction = "",
                    avatar = "",
                    sysCode = "1",
                    sysName = mm.getSysName(),
                    userId = "",
                    userSex =0 ,
                    departCode = "",
                    departName = ""
                });
            }
            catch (Exception ex)
            {
                r["code"] = -1;
                r["message"] = ex.Message;
            }
            return Json(r);
        }
        /// <summary>
        /// 查询用户信息(包括角色信息)
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="USER_NAME"></param>
        /// <param name="FLAG"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        // GET api/values
        [HttpGet("fetchUserOrgList")]
        public IActionResult fetchUserOrgList(string limit, string page, string USER_NAME, int? FLAG, string sort, string orgId)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["limit"] = limit;
            d["page"] = page;
            d["USER_NAME"] = USER_NAME;
            d["FLAG"] = FLAG;
            d["sort"] = sort;
            d["orgId"] = orgId;
            Dictionary<string, object> res = mm.fetchUserOrgList(d);
            return Json(res);
        }
        /// <summary>
        /// 查询用户信息(包括角色信息)
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="USER_NAME"></param>
        /// <param name="FLAG"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        // GET api/values
        [HttpGet("fetchUserRoleList")]
        public IActionResult fetchUserRoleList(string limit, string page, string USER_NAME, int? FLAG, string sort, string roleId)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["limit"] = limit;
            d["page"] = page;
            d["USER_NAME"] = USER_NAME;
            d["FLAG"] = FLAG;
            d["sort"] = sort;
            d["roleId"] = roleId;
            Dictionary<string, object> res = mm.fetchUserRoleList(d);
            return Json(res);
        }
       // / <summary>
        /// 弹窗查询
        /// </summary>
        /// <returns></returns>
        [HttpGet("fetchUserForLoginList")]
        public IActionResult fetchUserForLoginLists(string limit, string page, string USER_NAME = "", string LOGIN_ID = "")
        {
            UserLoginModule mm = new UserLoginModule();
            Dictionary<string, object> res = mm.fetchUserForLoginList(limit, page, USER_NAME, LOGIN_ID);
            return Json(res);
        }
        #region MyRegion
        ///// <summary>
        ///// 获取用户信息
        ///// </summary>
        ///// <param name=""></param>
        ///// <returns></returns>
        //[HttpPost("Info")]
        //public IActionResult Info([FromBody]JObject value)
        //{
        //    Dictionary<string, object> r = new Dictionary<string, object>();
        //    try
        //    {
        //        if (UserId == mm.getAdminCode())
        //        {
        //            string[] arr = new string[1];
        //            arr[0] = "";
        //            return Json(new { code = 2000, message = "", roles = JsonConvert.DeserializeObject("['admin']"), name = "系统超级管理员", userCode = UserId, token = accessToken, introduction = "", avatar = "", sysCode = "1", sysName = mm.getSysName(), userId = UserId, userSex = 0 });
        //        }
        //        Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
        //        string _token = d["token"] == null ? "" : d["token"].ToString();
        //        string departcode = d["departCode"] == null ? "" : d["departCode"].ToString();
        //        DataTable dt = mm.getUserAndGroupgByToken(_token);
        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            string[] role = new string[dt.Rows.Count];
        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                role[i] = dt.Rows[i]["GROUP_NAME"] == null ? "" : dt.Rows[i]["GROUP_NAME"].ToString();
        //            }
        //            string _name = dt.Rows[0]["USER_NAME"] == null ? "" : dt.Rows[0]["USER_NAME"].ToString();
        //            string _userCode = dt.Rows[0]["USER_CODE"] == null ? "" : dt.Rows[0]["USER_CODE"].ToString();
        //            string _userId = dt.Rows[0]["USER_ID"] == null ? "" : dt.Rows[0]["USER_ID"].ToString();
        //            int _userSex = Convert.ToInt32(dt.Rows[0]["USER_SEX"].ToString());
        //            return Json(new { code = 2000, message = "", roles = role, name = _name, userCode = _userCode, token = _token, introduction = "", avatar = "", sysCode = "1", sysName = mm.getSysName(), userId = _userId, userSex = _userSex });
        //        }
        //        return Json(new { code = 2000, message = "", roles = "", name = "", userCode = "", token = _token, introduction = "", avatar = "", sysCode = "", sysName = mm.getSysName(), userId = "", userSex = 0 });
        //    }
        //    catch (Exception ex)
        //    {
        //        r["code"] = -1;
        //        r["message"] = ex.Message;
        //    }
        //    return Json(r);
        //}
        #endregion

    }
}