using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alarmlines.Class.WOVerify
{
    interface IWOVerify
    {
        DataTable GetWONeedVerify(List<string> line);
        bool IsExistUPNVerified(string wO, string line);
    }
}
