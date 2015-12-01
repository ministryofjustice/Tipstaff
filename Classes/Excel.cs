using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Tipstaff.Models;
using System.Reflection;

namespace Tipstaff
{
    class Excel_TS
    {
        int maxColumns=0;
        List<XElement> rows;
        XElement data;

        XNamespace mainNamespace = "urn:schemas-microsoft-com:office:spreadsheet";
        XNamespace o = "urn:schemas-microsoft-com:office:office";
        XNamespace x = "urn:schemas-microsoft-com:office:excel";
        XNamespace ss = "urn:schemas-microsoft-com:office:spreadsheet";
        XNamespace html = "http://www.w3.org/TR/REC-html40";

        public Excel_TS()
        {
            rows = new List<XElement>();
        }

        public XDocument CreateXMLfile<T>(List<T> dataToAdd)
        {
            //Create XML from scratch
            XDocument xdoc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), new XProcessingInstruction("mso-application", "progid=\"Excel.Sheet\""));
            XElement workbook = new XElement(mainNamespace + "Workbook",
                new XAttribute(XNamespace.Xmlns + "html", html),
                new XAttribute(XName.Get("ss", "http://www.w3.org/2000/xmlns/"), ss),
                new XAttribute(XName.Get("o", "http://www.w3.org/2000/xmlns/"), o),
                new XAttribute(XName.Get("x", "http://www.w3.org/2000/xmlns/"), x),
                new XAttribute(XName.Get("xmlns", ""), mainNamespace)
            );
            XElement worksheet = new XElement(mainNamespace + "Worksheet",
                    new XAttribute(ss + "Name", "SystemConfig")
                ); // close Worksheet
            GetRows(dataToAdd);
            XElement table = new XElement(mainNamespace + "Table",
                        new XAttribute(ss + "ExpandedColumnCount", 1),
                        new XAttribute(ss + "ExpandedRowCount", rows.Count)
                    ); //close table
            int curRow=1;
            foreach (var row in rows)
            {
                var cols = row.Descendants(mainNamespace + "Data").Count();
                var title = prepTitle(row.Descendants(mainNamespace + "Data").First().Value);
                table.Add(row);
                //names.Add(new XElement(mainNamespace + "NamedRange",
                        //new XAttribute(ss + "Name", title),
                        //new XAttribute(ss + "RefersTo", string.Format("=SystemConfig!R{0}C2:R{0}C{1}", curRow, cols))));
                curRow++;
            }
            worksheet.Add(table);
            workbook.Add(worksheet);
            xdoc.Add(workbook);
            //xdoc.Save(@"H:\temp.xls");
            return xdoc;
        }

        public int AddRow()
        {
            return 0;
        }

        private void GetRows<T>(List<T> dataToAdd)
        {
            foreach (var item in dataToAdd)
            {

                if (item.GetType().BaseType == typeof(ChildAbduction))
                {
                    ChildAbduction CA = item as ChildAbduction;
                    
                    var dataRow = new XElement(mainNamespace + "Row",
                            new XElement(mainNamespace + "Cell",
                            new XElement(mainNamespace + "Data", new XAttribute(ss + "Type", "String"), CA.UniqueRecordID))
                    );
                    rows.Add(dataRow);
                }
            }
        }

        private string prepTitle(string value)
        {
            string res = value.Replace(" ", "_").Replace("(", "_").Replace(")", "_").Replace("&","_"); 
            return res;
        }
    }
}
