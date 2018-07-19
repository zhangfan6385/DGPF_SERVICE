using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DGPF.BIZModule;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http.Internal;
using System.Data;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace DGPF.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("Org")]
    public class OrgController : WebApiBaseController
    {
        OrgModule mm = new OrgModule();
        /// <summary>
        /// 查询组织结构
        /// </summary>
        /// <returns></returns>
        [HttpGet("fetchOrgList")]
        public IActionResult fetchOrgList()
        {
            UserModule user = new UserModule();
            string Admin = user.getAdminCode();
            bool isAdmin = UserId.Equals(Admin);
            Dictionary<string, object> res = mm.fetchOrgList(isAdmin);
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
        /// <summary>
        /// 清空用户角色
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("deleteUserOrgArticle")]
        public IActionResult deleteUserOrgArticle([FromBody]JObject value)
        {
            Dictionary<string, object> d = value.ToObject<Dictionary<string, object>>();
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                string b = mm.deleteUserOrgArticle(d);
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
        [Consumes("multipart/form-data")]//此处为新增
        
        public IActionResult UploadExcelFiles([FromForm] IFormCollection formCollection)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                FormFileCollection fileCollection = (FormFileCollection)formCollection.Files;
                IFormFileCollection dd= Request.Form.Files;
                IFormFile file = fileCollection[0];
                string path = file.FileName;
                //HttpFileCollection
                Stream aa = file.OpenReadStream();
                Stream BB = dd[0].OpenReadStream();
                string modePath = System.IO.Directory.GetCurrentDirectory()+"\\ExcelModel\\组织结构模板.xlsx";//原始文件
                string mes = "";
                DataTable dt = new DataTable();
                UTILITY.ExcelTools tool = new UTILITY.ExcelTools();
                tool.GetDataTable( aa, path,modePath,ref mes, ref dt);
                return Json(r);
            }
            catch (Exception ex)
            {
                r["code"] = -1;
                r["message"] = ex.Message;
            }
            return Json(r);
        }
        /// <summary>
        /// 导入excel(无用)
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> FileSave()
        {
            try
            {
                var date = Request;
                var files = Request.Form.Files;
                long size = files.Sum(f => f.Length);
                string webRootPath = _hostingEnvironment.WebRootPath;
                string contentRootPath = _hostingEnvironment.ContentRootPath;
                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {

                        string fileExt = Path.GetExtension(formFile.FileName); //文件扩展名，含“.”
                        long fileSize = formFile.Length; //获得文件大小，以字节为单位
                        string newFileName = System.Guid.NewGuid().ToString() + fileExt; //随机生成新的文件名
                        var filePath = contentRootPath + "\\files\\" + newFileName;
                        string modePath = System.IO.Directory.GetCurrentDirectory() + "\\ExcelModel\\组织结构模板.xlsx";//原始文件
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                            string aa = mm.UploadOrgFile(filePath);
                        }
                    }
                }
                return Ok(new { count = files.Count, size });
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        /// <summary>
        /// 导入excel
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [HttpPost("uploadOrgArticle")]
        public IActionResult PostFile([FromForm] IFormCollection formCollection)
        {
            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                FormFileCollection fileCollection = (FormFileCollection)formCollection.Files;
                foreach (IFormFile file in fileCollection)
                {
                    StreamReader reader = new StreamReader(file.OpenReadStream());
                    String content = reader.ReadToEnd();
                    String name = file.FileName;
                    String filename = System.IO.Directory.GetCurrentDirectory() + "\\Files\\" +Guid.NewGuid()+ name;
                    if (System.IO.File.Exists(filename))
                    {
                        System.IO.File.Delete(filename);
                    }
                    using (FileStream fs = System.IO.File.Create(filename))
                    {
                        // 复制文件
                        file.CopyTo(fs);
                        // 清空缓冲区数据
                        fs.Flush();
                    }
                    r["message"] = mm.UploadOrgFile(filename);
                    if (r["message"].ToString() != "")
                    {
                        r["code"] = -1;
                    }
                    else {
                        r["code"] = 2000;
                    }
                    Json(r);
                }
            }
            catch (Exception ex)
            {
                r["code"] = -1;
                r["message"] = ex.Message;
            }
           
            return Json(r);
        }

        private readonly IHostingEnvironment _hostingEnvironment;

        public OrgController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
    }
}