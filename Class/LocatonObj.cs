using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Alarmlines
{
    public class LocatonObj
    {
        public string name {get; set;}
        public List<string> lstline
        {
            get;
            set;
        }

        public static void WriteCSV(string path, List<LocatonObj> lst)
        {
            using (StreamWriter streamWriter = new StreamWriter(path, append: false))
            {
                string value = "Name," + "ListLine";
                streamWriter.WriteLine(value);
                foreach (var item in lst)
                {
                    string t = "";
                    foreach (var s in item.lstline) {
                        t += s + ",";
                    }
                    string value2 = item.name + "," + t;
                    streamWriter.WriteLine(value2);
                }
            }
        }

        public static List<LocatonObj> ReadCSV(string FilePath)
        {
            List<LocatonObj> lst = new List<LocatonObj>();
            if (File.Exists(FilePath))
            {
                File.ReadAllLines(FilePath).Skip(1).ToList()
                    .ForEach(delegate (string r)
                    {
                        string[] array = r.Split(',');
                        LocatonObj item = new LocatonObj
                        {
                            name = array[0],
                            lstline = array.Skip(1).ToList()
                        };
                        lst.Add(item);
                    });
            }
            return lst;
        }

        public static string getname(string code, List<LocatonObj> lst)
        {
            foreach (var p in lst)
            {
                if (p.lstline.Contains(code) == true) return p.name;
            }
            return "";
        }
    }
}
