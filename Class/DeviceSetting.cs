using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Alarmlines
{   
    [XmlRoot("configuration")]
    public class DeviceSetting
    {
        [XmlElement(ElementName = "PortName")]
        public string portName { get; set; }
        [XmlElement(ElementName = "BaudRate")]
        public int baudRate { get; set; }
        [XmlElement(ElementName = "DataBits")]
        public int dataBits { get; set; }
        [XmlElement(ElementName = "Parity")]
        public string parity { get; set; }
        [XmlElement(ElementName = "StopBits")]
        public string stopBits { get; set; }
        // thoi gian query len server de lay thong tin
        [XmlElement(ElementName = "QueryTime")]
        public int queryTime { get; set; }
        //thoi gian leader xac nhan cac loi, trong thoi gian nay khong lam viec query len server ma chi so sanh chuyen bang du lieu
        [XmlElement(ElementName = "ConfirmTime")]
        public int ConfirmTime { get; set; }
        //thoi gian query lai so ngay trong co so du lieu
        [XmlElement(ElementName = "QueryBeforeHours")]
        public int QueryBeforeHours { get; set; }
        //thoi gian query lai so ngay trong co so du lieu
        [XmlElement(ElementName = "Location")]
        public string Location { get; set; }
        public static int ReadXML<Type>(out Type pClass, string pPath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Type));
            using (FileStream stream = new FileStream(pPath, FileMode.Open))
            {
                pClass = (Type)serializer.Deserialize(stream);
            }
            return 0;
        }
        public static int WriteXML<Type>(Type pClass, string pPath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Type));
            using (FileStream stream = new FileStream(pPath, FileMode.Create))
            {
                serializer.Serialize((Stream)stream, pClass);
            }
            return 0;
        }
    }
}
