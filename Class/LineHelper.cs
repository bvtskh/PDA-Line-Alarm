using Alarmlines.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZedGraph;

namespace Alarmlines.Class
{
    public class LineHelper
    {
        public static string GetGroup(string lineID)
        {
            using (DXContext context = new DXContext())
            {
                var entity = context.Locations.FirstOrDefault(r => r.LineId == lineID);
                return entity == null ? string.Empty : entity.GroupID;
            }
        }
        public static List<DB.Location> GetLocations()
        {
            using (DXContext context = new DXContext())
            {
                var res = context.Locations.ToList();
                return res;
            }
        }
    }
}
