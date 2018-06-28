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
       
        [HttpPost("login")]
        public IActionResult LogIn([FromBody]JObject value)
        {
            try
            {
                Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
                string username = d["username"]==null?"" : d["username"].ToString();
                string password = d["password"]==null?"": d["password"].ToString();
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    return Json(new { code = -1, message = "用户名或密码不能为空！" });
                }
                UserModule mm = new UserModule();
                string admin = mm.getAdminCode();
                string pass = mm.getAdminPass();
                if ((username==admin)&&(password==pass)) {
                    string accessToken = AccessTokenTool.GetAccessToken(admin);
                    DGPF.UTILITY.AccessTokenTool.DeleteToken(admin);
                    DGPF.UTILITY.AccessTokenTool.InsertToken(admin, accessToken, DateTime.Now.AddHours(1));
                    return Json(new { code = 2000, message = "", token = accessToken,roleLevel = "admin" });
                }
                else {
                    DGPF.BIZModule.Models.ts_uidp_userinfo mode = mm.getUserInfoByLogin(username, d["userDomain"].ToString());
                    if (mode == null)
                    {
                        return Json(new { code = -1, message = "此用户不存在！" });
                    }
                    if (password != mode.USER_PASS)
                    {
                        return Json(new { code = -1, message = "密码错误！" });
                    }
                    string userId = mode.USER_ID;
                    string accessToken = AccessTokenTool.GetAccessToken(userId);
                    DGPF.UTILITY.AccessTokenTool.DeleteToken(userId);
                    DGPF.UTILITY.AccessTokenTool.InsertToken(userId, accessToken, DateTime.Now.AddHours(1));
                    DataTable dtUserOrg = mm.GetUserOrg(mode.USER_ID);
                    return Json(new { code = 2000, message = "", token = accessToken, orgList = dtUserOrg, roleLevel = "" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = -1, message = "登录时程序发生错误"+ex.Message});

            }
            
        }
    }
}