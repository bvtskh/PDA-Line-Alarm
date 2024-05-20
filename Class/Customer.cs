using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Alarmlines
{
    public class CustomerObj
    {
        public List<string> code
        {
            get;
            set;
        }
        public string name
        {
            get;
            set;
        }

        public static void WriteCSV(string path, List<CustomerObj> lst)
        {
            using (StreamWriter streamWriter = new StreamWriter(path, append: false))
            {
                string value = "Name," + "ListLine";
                streamWriter.WriteLine(value);
                foreach (var item in lst)
                {
                    string t = "";
                    foreach (var s in item.code)
                    {
                        t += s + ",";
                    }
                    string value2 = item.name + "," + t;
                    streamWriter.WriteLine(value2);
                }
            }
        }

        public static List<CustomerObj> ReadCSV(string FilePath)
        {
            List<CustomerObj> lst = new List<CustomerObj>();
            if (File.Exists(FilePath))
            {
                File.ReadAllLines(FilePath).Skip(1).ToList()
                    .ForEach(delegate (string r)
                    {
                        string[] array = r.Split(',');
                        CustomerObj item = new CustomerObj
                        {
                            name = array[0],
                            code = array.Skip(1).ToList()
                        };
                        lst.Add(item);
                    });
            }
            return lst;
        }

        public static string getname(string code, List<CustomerObj> lst)
        {
            foreach (var p in lst)
            {
                if (p.code.Contains(code) == true) return p.name;
            }
            return "";
        }
    }
}
