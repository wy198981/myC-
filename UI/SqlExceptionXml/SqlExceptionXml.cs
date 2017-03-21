using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace UI.SqlExceptionXml
{
    public class SqlExceptionXml
    {
        public static string strURL = @"SqlExceptionXml/SqlException.xml";
        /// <summary>
        /// 保存丢失数据信息
        /// </summary>
        /// <param name="InOut">机号</param>
        /// <param name="strCardNO">读取的字符串</param>
        public static void SaveCardNO(string JiHao,string strCardNO)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(strURL);
            XmlNode root = xmlDoc.SelectSingleNode("Items");//查找<bookstore>
            XmlElement xe1 = xmlDoc.CreateElement("item");//创建一个<book>节点
            xe1.SetAttribute("JiHao", JiHao);//设置该节点genre属性
            xe1.SetAttribute("CardNOs", strCardNO);//设置该节点ISBN属性
            root.AppendChild(xe1);//添加到<bookstore>节点中
            xmlDoc.Save(strURL);
        }
        /// <summary>
        /// 读取丢失数据
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string,string> GetCardNOs()
        {
          
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(strURL);
            XmlNode xn = xmlDoc.SelectSingleNode("Items");

            XmlNodeList xnl = xn.ChildNodes;
            Dictionary<string, string> list = new Dictionary<string,string>();
           // int i = 0;
            foreach (XmlNode xnf in xnl) 
            {

                XmlElement xe = (XmlElement)xnf;
                list[xe.GetAttribute("JiHao")] = xe.GetAttribute("CardNOs");
               // Console.WriteLine(xe.GetAttribute("InOut"));//显示属性值 
               // Console.WriteLine(xe.GetAttribute("CardNOs"));
               // i++;
            }
            return list;
        }
        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="strCardNO"></param>
        public static void DeleteCardNOs(string strCardNO)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(strURL);
           // XmlNode xnItem = xmlDoc.SelectSingleNode("Items");
            XmlNodeList xnl = xmlDoc.SelectSingleNode("Items").ChildNodes;
            foreach (XmlNode xn in xnl)
            {
                XmlElement xe = (XmlElement)xn;
                if (xe.GetAttribute("CardNOs") == strCardNO)
                {
                    xe.RemoveAll();
                }
            }
            xmlDoc.Save(strURL);
        }
    }
}
