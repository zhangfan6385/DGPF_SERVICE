﻿using DGPF.ODS;
using DGPF.UTILITY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DGPF.BIZModule
{
    public class ConfModule
    {
        ConfDB db = new ConfDB();
        /// <summary>
        /// 登录读取配置信息
        /// </summary>
        /// <param name="d">CONF_NAME：配置项Code</param>
        /// <returns></returns>
        public Dictionary<string, object> loginConfig(Dictionary<string, object> d)
        {

            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {
                DataTable dt = db.loginConfig(d);
                r["total"] = dt.Rows.Count;
                var lst = KVTool.TableToListDic(dt);
                r["sysname"] = lst[0];
                List<object> typeList = new List<object>();
                for (int i = 1; i < lst.Count; i++)
                {
                    object obj = new { key = lst[i]["CONF_NAME"], user_code = lst[i]["CONF_CODE"] };
                    typeList.Add(obj);
                }
                r["itemtype"] = typeList;
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
        public Dictionary<string, object> fetchConfigList(Dictionary<string ,object> d)
        {

            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {

                int limit = d["limit"] == null ? 100 : int.Parse(d["limit"].ToString());
                int page = d["page"] == null ? 1 : int.Parse(d["page"].ToString());

                DataTable dt = db.fetchConfigList(d);
                r["total"] = dt.Rows.Count;
                //r["items"] = KVTool.TableToListDic(KVTool.GetPagedTable(dt, page,limit));
                r["items"] = KVTool.RowsToListDic(dt, d);
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

        public string createConfigArticle(Dictionary<string, object> d)
        {
            return db.createConfigArticle(d);
        }


        public string updateConfigData(Dictionary<string, object> d)
        {
            return db.updateConfigData(d);
        }
        public string updateConfigArticle(Dictionary<string, object> d)
        {
            return db.updateConfigArticle(d);
        }

        
    }
}
