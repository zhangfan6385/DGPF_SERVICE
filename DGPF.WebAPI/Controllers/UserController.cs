using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using DGPF.BIZModule;
using DGPF.LOG;
namespace DGPF.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/User")]
    public class UserController : WebApiBaseController
    {
        UserModule mm = new UserModule();
        ClsSysLog log = new ClsSysLog();
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
        [HttpGet("Info")]
        public IActionResult Info(string token) {
            BIZModule.Models.ts_uidp_userinfo mode= mm.getUserInfoByToken(token);
            return Json(new { code=2000,message="",data=Newtonsoft.Json.JsonConvert.SerializeObject(mode)});
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
    }
}