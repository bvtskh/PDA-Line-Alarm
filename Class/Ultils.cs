using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alarmlines.Class
{
    public class Ultils
    {
        public static void WriteRegistry(string keyName, string content)
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\SUPPORT_WIP\LineAlarm\Configs");
            if (!string.IsNullOrEmpty(keyName))
            {
                key.SetValue(keyName, content);
                key.Close();
            }
        }
        public static string GetValueRegistryKey(string keyName)
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\SUPPORT_WIP\LineAlarm\Configs");
            string value = null;
            if (key != null)
            {
                if (key.GetValue(keyName) != null)
                {
                    value = key.GetValue(keyName).ToString();
                    key.Close();
                    return value;
                }
            }

            return null;
        }
    }
}
