using DGPF.UTILITY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DGPF.ODS
{
    public class MenuDB
    {
        DBTool db = new DBTool("MYSQL");
        public DataTable fetchMenuList(Dictionary<string,object> sysCode)
        {
          return  db.GetDataTable("select * from ts_uidp_config where SYS_CODE='"+ sysCode ["sysCode"] + "'");
        }
    }
}
