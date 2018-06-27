using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DGPF.BIZModule;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DGPF.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Org")]
    public class OrgController : Controller
    {
        OrgModule mm = new OrgModule();
        /// <summary>
        /// 查询组织结构
        /// </summary>
        /// <returns></returns>
        [HttpGet("fetchOrgList")]
        public IActionResult fetchOrgList()
        {
            Dictionary<string, object> res = mm.fetchOrgList();
            return Json(res);
        }
        /// <summary>
        /// 新增组织结构
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("createOrgArticle")]
        public IActionResult createOrgArticle([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = mm.createOrgArticle(d);
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
        /// 修改组织结构
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("updateOrgData")]
        public IActionResult updateOrgData([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = mm.updateOrgData(d);
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
        /// 删除组织机构
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("updateOrgArticle")]
        public IActionResult updateOrgArticle([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = mm.updateOrgArticle(d);
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
        /// D.	分配组织结构给用户
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("updateUserOrgArticle")]
        public IActionResult updateUserOrgArticle([FromBody]JObject value) {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = mm.updateUserOrgArticle(d);
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
    }
}