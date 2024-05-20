using Alarmlines.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alarmlines.Class
{
    public class LocationHelper
    {
        public static string[] GetLocation()
        {
            using (DXContext context = new DXContext())
            {
                string sql = @"SELECT GroupID
                              FROM [SMT].[dbo].[Location]
                              WHERE GroupID is not null
                              GROUP BY GroupID";
                return context.Database.SqlQuery<string>(sql).ToArray();
            }
        }

        public static string[] GetLocationByArea(string AreaFilter)
        {
            using (DXContext context = new DXContext())
            {
                string sql = @"SELECT GroupID
                              FROM [SMT].[dbo].[Location]
                              WHERE GroupID is not null and LocationId='" + AreaFilter + "' GROUP BY GroupID";
                return context.Database.SqlQuery<string>(sql).ToArray();
            }
        }
    }
}
