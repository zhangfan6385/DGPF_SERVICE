using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DGPF.BIZModule;
namespace DGPF.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Log")]
    public class LogController : Controller
    {
        LogModule mm = new LogModule();
        /// <summary>
        /// 日志查询
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="USER_NAME"></param>
        /// <param name="FLAG"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        [HttpGet("fetchLogInfoList")]
        public IActionResult fetchLogInfoList(string limit, string page, string USER_NAME, int? LOG_TYPE, string ACCESS_TIME)
        {
            //DGPF.LOG.ClsSysLog aa = new DGPF.LOG.ClsSysLog();
            // aa.Info(DateTime.Now,"sdf","dsf","sdf",2,"sdf","sdf");
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["limit"] = limit;
            d["page"] = page;
            d["USER_NAME"] = USER_NAME;
            d["LOG_TYPE"] = LOG_TYPE;
            d["ACCESS_TIME"] = ACCESS_TIME;
            Dictionary<string, object> res = mm.fetchLogInfoList(d);
            return Json(res);
        }
    }
}