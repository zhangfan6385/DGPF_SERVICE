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
namespace DGPF.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/LogIn")]
    public class LogInController : Controller
    {
       
        [HttpGet("login")]
        public IActionResult LogIn(string username, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    return Json(new { code = -1, message = "用户名或密码不能为空！" });
                }
                UserModule mm = new UserModule();
                DGPF.BIZModule.Models.ts_uidp_userinfo mode = mm.getUserInfoByUserCode(username);
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
                DGPF.UTILITY.AccessTokenTool.InsertToken(userId,accessToken,DateTime.Now.AddHours(1));
                DataTable dtUserOrg = mm.GetUserOrg(mode.USER_ID);
                //  string strUserOrg = JsonConvert.SerializeObject(dtUserOrg);
                return Json(new { code = 2000, message = "", AccessToken = accessToken, data = dtUserOrg });
            }
            catch (Exception ex)
            {
                return Json(new { code = -1, message = "登录时程序发生错误"+ex.Message});

            }
            
        }
    }
}