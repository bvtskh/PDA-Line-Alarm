using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Alarmlines
{
    public class ErrorInfo
    {
        public int ErrorCODE
        {
            get;
            set;
        }

        public string ErrorContent
        {
            get;
            set;
        }
        public string ErrorContentVN
        {
            get;
            set;
        }

        public int Alarm
        {
            get;
            set;
        }

        public static void WriteCSV(string path, List<ErrorInfo> lstPosition)
        {
            using (StreamWriter file = new StreamWriter(path, false))
            {
                foreach (var p in lstPosition)
                {
                    string csvData;
                    csvData = string.Join(",", p.ErrorCODE, p.ErrorContent, p.ErrorContentVN,p.Alarm);
                    file.WriteLine(csvData);
                }
            }
        }

        public static List<ErrorInfo> ReadCSVErr(string FilePath)
        {
            List<ErrorInfo> lstError_infor = new List<ErrorInfo>();
            if (File.Exists(FilePath))
            {
                File.ReadAllLines(FilePath).ToList()
                    .ForEach(delegate (string r)
                    {
                        string[] array = r.Split(',');
                        ErrorInfo item = new ErrorInfo
                        {
                            ErrorCODE = Convert.ToInt32(array[0]),
                            ErrorContent = array[1],
                            ErrorContentVN = array[2],
                            Alarm = Convert.ToInt16(array[3])
                        };
                        lstError_infor.Add(item);
                    });
            }
            return lstError_infor;
        }
    }
}
