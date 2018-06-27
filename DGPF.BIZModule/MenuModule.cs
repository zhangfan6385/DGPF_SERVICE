using DGPF.ODS;
using DGPF.UTILITY;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DGPF.BIZModule
{
    public class MenuModule
    {
        MenuDB db = new MenuDB();
        public List<Dictionary<string, object>> fetchMenuList(Dictionary<string ,object> sysCode)
        {
            List<Dictionary<string, object>> d =KVTool.TableToListDic(db.fetchMenuList(sysCode));

            return d;
        }

    }
}
