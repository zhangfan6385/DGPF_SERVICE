using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DGPF.BIZModule;
using DGPF.UTILITY;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DGPF.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("LogIn")]
    public class LogInController : Controller
    {
        DGPF.LOG.SysLog log = new LOG.SysLog();

        [HttpPost("login")]
        public IActionResult loginByUsernames([FromBody]JObject value)
        {
            string userId = "";
            string userName = "";
            try
            {
                Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
                string username = d["username"] == null ? "" : d["username"].ToString();
                string password = d["password"] == null ? "" : d["password"].ToString();
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    return Json(new { code = -1, message = "用户名或密码不能为空！" });
                }


                UserModule mm = new UserModule();
                userId = mm.getAdminCode();
                string pass = mm.getAdminPass();
                if ((username == userId) && (password == pass))
                {
                    userName = "系统超级管理员";
                    string accessToken = AccessTokenTool.GetAccessToken(userId);
                    DGPF.UTILITY.AccessTokenTool.DeleteToken(userId);
                    DGPF.UTILITY.AccessTokenTool.InsertToken(userId, accessToken, DateTime.Now.AddHours(1));

                    log.Info(DateTime.Now, userId, userName, Extension.GetClientUserIp(Request.HttpContext), 2, "LogIn", "");
                    return Json(new {
                        code = 2000,
                        message = "",
                        token = accessToken,
                        orgList = new DataTable(),
                        userList = new DataTable(),
                        roleLevel = "admin" });
                }
                else
                {
                    UserLoginModule um = new UserLoginModule();
                   DataTable dt= um.GetUserLoginByLOGIN_CODE(username);
                    if (dt == null||dt.Rows.Count==0)
                    {
                        return Json(new { code = -1, message = "此用户不存在！" });
                    }
                    if (password != dt.Rows[0]["LOGIN_PASS"].ToString())
                    {
                        return Json(new { code = -1, message = "密码错误！" });
                    }
                    userId = username;
                    userName = dt.Rows[0]["LOGIN_REMARK"].ToString();
                    string accessToken = AccessTokenTool.GetAccessToken(userId);
                    DGPF.UTILITY.AccessTokenTool.DeleteToken(userId);
                    DGPF.UTILITY.AccessTokenTool.InsertToken(userId, accessToken, DateTime.Now.AddHours(1));
                    DataTable dtUser = um.fetchUserLoginListById(dt.Rows[0]["LOGIN_ID"].ToString());
                    log.Info(DateTime.Now, userId, userName, Extension.GetClientUserIp(Request.HttpContext), 2, "LogIn", "");
                    return Json(new {
                        code = 2000,
                        message = "",
                        token = accessToken,
                        orgList = new DataTable(),
                        userList = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(dtUser)),
                        roleLevel = "" });
                }
            }
            catch (Exception ex)
            {
                log.Info(DateTime.Now, userId, userName, Extension.GetClientUserIp(Request.HttpContext), 1, "LogIn", ex.Message.Length > 120 ? ex.Message.Substring(0, 100) : ex.Message);
                return Json(new { code = -1, message = "登录时程序发生错误" + ex.Message });

            }

        }
        
        #region 备用


        //[HttpPost("login")]
        //public IActionResult LogIn([FromBody]JObject value)
        //{
        //    string userId = "";
        //    string userName = "";
        //    try
        //    {
        //        Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
        //        string username = d["username"]==null?"" : d["username"].ToString();
        //        string password = d["password"]==null?"": d["password"].ToString();
        //        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        //        {
        //            return Json(new { code = -1, message = "用户名或密码不能为空！" });
        //        }
        //        UserModule mm = new UserModule();
        //         userId = mm.getAdminCode();
        //        string pass = mm.getAdminPass();
        //        if ((username== userId) &&(password==pass)) {
        //            userName = "系统超级管理员";
        //            string accessToken = AccessTokenTool.GetAccessToken(userId);
        //            DGPF.UTILITY.AccessTokenTool.DeleteToken(userId);
        //            DGPF.UTILITY.AccessTokenTool.InsertToken(userId, accessToken, DateTime.Now.AddHours(1));

        //            log.Info(DateTime.Now, userId, userName, Extension.GetClientUserIp(Request.HttpContext), 2, "LogIn", "");
        //            return Json(new { code = 2000, message = "", token = accessToken,roleLevel = "admin" });
        //        }
        //        else {
        //            DGPF.BIZModule.Models.ts_uidp_userinfo mode = mm.getUserInfoByLogin(username, d["userDomain"].ToString());
        //            if (mode == null)
        //            {
        //                return Json(new { code = -1, message = "此用户不存在！" });
        //            }
        //            if (password != mode.USER_PASS)
        //            {
        //                return Json(new { code = -1, message = "密码错误！" });
        //            }
        //            userId = mode.USER_ID;
        //            userName = mode.USER_NAME;
        //            string accessToken = AccessTokenTool.GetAccessToken(userId);
        //            DGPF.UTILITY.AccessTokenTool.DeleteToken(userId);
        //            DGPF.UTILITY.AccessTokenTool.InsertToken(userId, accessToken, DateTime.Now.AddHours(1));
        //            DataTable dtUserOrg = mm.GetUserOrg(mode.USER_ID);
        //            log.Info(DateTime.Now, userId, mode.USER_NAME, Extension.GetClientUserIp(Request.HttpContext), 2, "LogIn", "");
        //            return Json(new { code = 2000, message = "", token = accessToken, orgList = dtUserOrg, roleLevel = "" });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Info(DateTime.Now, userId, userName, Extension.GetClientUserIp(Request.HttpContext), 1, "LogIn", ex.Message.Length>120?ex.Message.Substring(0,100):ex.Message);
        //        return Json(new { code = -1, message = "登录时程序发生错误"+ex.Message});

        //    }

        //}
        #endregion
    }
}