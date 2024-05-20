using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Threading.Tasks;

namespace Alarmlines
{
    public class ModifyCSV
    {
        public static void WriteCSVModel(string path, List<PDA_ErrorHistory> lstPosition)
        {
            int step = 0;
            using (StreamWriter file = new StreamWriter(path, false))
            {
                string header = string.Join(",", "ID", "Error Time", "Line", "Model", "WO", "Partcode", "ErrorContent", "OperatorCode", "Customers", "Location");

                file.WriteLine(header);
                foreach (var p in lstPosition)
                {
                    string csvData;
                    csvData = string.Join(",", p.id, p.ErrorTime, p.Line, p.Model, p.WO, p.PartCode, p.ErrorContent, p.OperatorCode, p.Customer, p.Location);
                    step++;
                    file.WriteLine(csvData);
                }
            }
        }

        /// <summary>
        /// Thiết lập các thông số cổng com
        /// </summary>
        /// <param name="comport"></param>
        /// <param name="setting"></param>

        //public static List<position> ReadCsv(string path)
        //{
        //    if (File.Exists(path) == false)
        //    {
        //        MessageBox.Show("Not Exist File. Please select again");
        //        return null;
        //    }
        //    List<position> result = File.ReadLines(path).Skip(1).Select(r => position.FromCsv(r)).ToList();
        //    return result;
        //}

        public static List<PDA_ErrorHistory> ReadCsv(string FilePath)
        {
            //List<PDA_ErrorHistory> lstposition = new List<PDA_ErrorHistory>();
            //if (File.Exists(FilePath))
            //{
            //    File.ReadAllLines(FilePath).Skip(1).ToList()
            //        .ForEach(delegate (string r)
            //        {
            //            string[] col = r.Split(',');
            //            position item = new position
            //            {
            //                step = Convert.ToUInt16(col[0]),
            //                X = Convert.ToDouble(col[1]),
            //                Y = Convert.ToDouble(col[2]),
            //                Z = Convert.ToDouble(col[3]),
            //                Moving_Mode = col[4],
            //                action = col[5],
            //                gcodefile = col[6],
            //                //angle = col[7],
            //                output = col[7],
            //                result = col[8]
            //            };
            //            lstposition.Add(item);
            //        });
            //}
            return null;// lstposition;
        }
    }
}
