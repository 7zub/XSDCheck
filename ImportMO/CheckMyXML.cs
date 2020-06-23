using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml.Schema;

namespace ImportMO
{
    class CheckMyXML
    {
        public static void CheckMyFile(string xsd, string xml)
        {
            XmlSchemaSet myschema = new XmlSchemaSet();

            try
            {
                myschema.Add(null, xsd);
                ValidationShema.ValidateAgainstSchema(xml, myschema);
                LogXML.SaveXmlLog(Path.GetFileNameWithoutExtension(xml)).Save("Xml_report/" + Path.GetFileName(xml));
                Process.Start("explorer.exe", Path.GetDirectoryName(Application.ExecutablePath) + @"\Xml_report");
            }
            catch (Exception e)
            {
                CheckImportMOXML.setLog("Неверный файл XSD-схемы!\n" + e.Message);
            }
        }
    }
}