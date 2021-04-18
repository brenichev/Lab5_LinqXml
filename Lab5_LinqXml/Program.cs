using System;
using System.IO;
using System.Xml.Linq;
using System.Linq;

namespace Lab5_LinqXml
{
    class Program
    {
        static string path = @"C:\Users\breni\source\repos\Lab5_LinqXml\XmlFiles\";
        static void Main(string[] args)
        {
            Task1();
            Task2();
            Task3();
            Task4();
            Task5();
        }

        static void Task1()
        {
            XDocument res;
            XElement root = new XElement("root");
            using (FileStream fs = new FileStream(@$"{path}Task1.txt", FileMode.Open))
            {
                StreamReader sr = new StreamReader(fs);
                while (!sr.EndOfStream)
                {
                    root.Add(new XElement("line", from word in sr.ReadLine().Trim().Split(' ' , StringSplitOptions.RemoveEmptyEntries) orderby word select new XElement("word", word)));
                }
            }
            res = new XDocument(root);
            res.Save(@$"{path}Task1Result.xml");
        }

        static void Task2()
        {
            XDocument xdoc = XDocument.Load(@$"{path}Task2.xml");
            var lvl2 = xdoc.Root.Elements().Elements().Nodes().OfType<XText>().Select(n => new { name = n.Parent.Name.LocalName, val = n.Value });
            Console.WriteLine(lvl2.Count() + "\n");
            foreach (var n in lvl2)
            {
                Console.WriteLine(n.name + " " + n.val);
            }           
        }

        static void Task3()
        {
            XDocument xdoc = new XDocument(XDocument.Load(@$"{path}Task3.xml"));
            xdoc.Root.DescendantNodes().Where(x => x is XComment && (x.Parent == xdoc.Root || x.Parent.Parent == xdoc.Root)).Remove();
            xdoc.Save(@$"{path}Task3Result.xml");
        }

        static void Task4()
        {
            XDocument xdoc = new XDocument(XDocument.Load(@$"{path}Task4.xml"));

            var withatr= xdoc.Root.Elements().Where(x => x.HasAttributes);
            foreach(var val in withatr)
            {
                val.ReplaceAttributes(val.Attributes().Select(atr => new XElement(atr.Name, atr.Value)));
            }

            xdoc.Save(@$"{path}Task4Result.xml");
        }

        static void Task5()
        {
            XDocument xdoc = new XDocument(XDocument.Load(@$"{path}Task5.xml"));
            string atrName = "realnum";
            foreach (var item in xdoc.Descendants())
            {
                if (!item.HasElements) continue;
                double? min = null;
                foreach (var elem in item.Descendants())
                {
                    if (!elem.HasAttributes) continue;
                    foreach (var atr in elem.Attributes())
                    {
                        if (atr.Name.ToString() == atrName && (min == null || double.Parse(atr.Value) < min)) min = double.Parse(atr.Value.ToString());
                    }
                }
                item.SetAttributeValue("min", min);
            }

            xdoc.Save(@$"{path}Task5Result.xml");
        }

    }
}
