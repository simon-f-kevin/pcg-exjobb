using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace StorageSystem
{
    public class StorageHandler
    {

        public StorageHandler()
        {

        }

        //private string NameHandler()
        //{
        //    XDocument doc = XDocument.Load(@"D:\\database.xml");
        //    var elem = doc.Descendants("Count");
        //    int value;
        //    XElement tempEl;
        //    foreach(var el in elem)
        //    {
        //        value = int.Parse(el.Value);
        //        value += 1;
        //        tempEl = new XElement("Count", value);
        //    }
        //    doc.Element("Count").Add(tempEl);
        //    return null;
        //}

        public void SaveStatsToStorage(string playerName, int score)
        {
            if (!File.Exists(@"D:\\database.xml") || File.ReadAllText(@"D:\\database.xml").Equals(""))
            {
                XDocument tempDoc = new XDocument(new XElement("AllPlayers"));
                tempDoc.Add(new XElement("Count"));
                using (XmlTextWriter writer = new XmlTextWriter(@"D:\\database.xml", null)) {
                    writer.Formatting = Formatting.Indented;
                    tempDoc.Save(writer);
                };
                
            }
            try
            {
                XDocument doc = XDocument.Load(@"D:\\database.xml");
                XElement element = CreateXElementFromData(playerName, score);
                doc.Element("AllPlayers").Add(element);
                doc.Save(@"D:\\database.xml");
            }
            catch (Exception ex)
            {
                //Handle exception here
            }
        }

        private XElement CreateXElementFromData(string playerName, int score)
        {
            XElement element = new XElement("Player",
                new XElement("Name", playerName),
                new XElement("Score", score));

            return element;
        }
    }
}
