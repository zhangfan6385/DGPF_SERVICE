using DGPF.ODS;
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
        public Dictionary<string, object> fetchConfigList(Dictionary<string ,object> d)
        {

            Dictionary<string, object> r = new Dictionary<string, object>();
            try
            {

                int limit = d["limit"] == null ? 100 : int.Parse(d["limit"].ToString());
                int page = d["page"] == null ? 1 : int.Parse(d["page"].ToString());

                DataTable dt = db.fetchConfigList(d);
                r["total"] = dt.Rows.Count;
                r["items"] = KVTool.TableToListDic(KVTool.GetPagedTable(dt, page,limit));
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
