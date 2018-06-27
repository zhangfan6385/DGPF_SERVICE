using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DGPF.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/WebApiBase")]
    public class WebApiBaseController : Controller
    {
        public string ClientIp = "";
        public string UserId = "";
        public string UserName = "";
        public string accessToken = "";
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                Microsoft.Extensions.Primitives.StringValues AccessToken;//获取header中某一项的值
                context.HttpContext.Request.Headers.TryGetValue("X-Token", out AccessToken);
                //根据实际需求进行具体实现
                accessToken = AccessToken;
                if (accessToken == "")
                {
                    context.Result = new ObjectResult(new { code = 50008, msg = "没有找到X-Token" });
                }
                string userId = DGPF.UTILITY.AccessTokenTool.GetUserId(AccessToken);
                BIZModule.UserModule mm = new BIZModule.UserModule();
                string admin = mm.getAdminCode();
                if (userId == admin)
                {
                    UserName = "系统超级管理员";
                }
                else
                {
                    UserName = mm.getUserInfoByUserId(userId).USER_NAME;
                }
                DGPF.UTILITY.Message mes = DGPF.UTILITY.AccessTokenTool.IsInValidUser(userId, AccessToken,admin);
                if (mes.code != 2000)
                {
                    context.Result = new ObjectResult(mes);
                }
                
                
                UserId = userId;
                ClientIp = Extension.GetClientUserIp(Request.HttpContext);
            }
            catch (Exception ex)
            {
                context.Result = new ObjectResult(new { code = -1, msg = "验证token时程序出错", result = ex.Message });
            }

        }

    }
}