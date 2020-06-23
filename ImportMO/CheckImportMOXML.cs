using System;
using System.Collections;
using System.Linq;
using System.IO;
using System.Xml.Schema;
using System.IO.Compression;

namespace ImportMO
{
    class CheckImportMOXML
    {
        private static MENU menu;
        public string CurFiles;
        public string import_MO_dir = @"import_mo/";
        public IEnumerable XMLfiles;

        public CheckImportMOXML(MENU b)
        {
            menu = b;
        }

        public static void setLog(string str)
        {
            menu.richTextBox1.Text += DateTime.Now.ToString("\n[dd MMMM yyyy HH:mm:ss] ") + str + "\n\n";
        }

        // ЗАПУСК
        public void ImportMO_START()
        {
            ZipExtract();
            CheckXMLfiles();
            DeleteDir();
            CreateZipReport();
            ClearAll();
        }

        public void ZipExtract()
        {
            var files = Directory
                .GetFiles(import_MO_dir, "*.zip")
                .OrderBy(f => File.GetCreationTime(f))
                .Select(f => Path.GetFileNameWithoutExtension(f));

            IEnumerator ie = files.GetEnumerator();
            ie.MoveNext();
            CurFiles = (string)ie.Current;
            setLog(CurFiles);

            string zipPath = import_MO_dir + CurFiles + ".zip";
            string extractPath = import_MO_dir + CurFiles + "/";

            ZipFile.ExtractToDirectory(zipPath, extractPath);


            XMLfiles = Directory
                .GetFiles(import_MO_dir + CurFiles, "*.xml")
                .Select(f => Path.GetFileName(f));

            DirectoryInfo dirRep = new DirectoryInfo("Xml_report/" + CurFiles);

            if (!dirRep.Exists)
            {
                dirRep.Create();
            }
        }

        public void CheckXMLfiles()
        {
            foreach (string item in XMLfiles)
            {
                XmlSchemaSet schemaSet = new XmlSchemaSet();
                schemaSet.Add(null, "XSD/" + item.Substring(0, 2) + ".xsd");
                ValidationShema.ValidateAgainstSchema(import_MO_dir + CurFiles + "/" + item, schemaSet);
                LogXML.SaveXmlLog(Path.GetFileNameWithoutExtension(item)).Save("Xml_report/" + CurFiles + "/" + item);
                setLog(item.Substring(0, 2));
                File.Delete(import_MO_dir + CurFiles + "/" + item);
            }
        }

        public void DeleteDir()
        {
            Directory.Delete(import_MO_dir + CurFiles + "/");
            File.Move(import_MO_dir + CurFiles + ".zip", import_MO_dir + "/Обработано/" + CurFiles + ".zip");
        }

        public void CreateZipReport()
        {
            string zipPath = @"Xml_report/" + CurFiles + ".zip";
            string startPath = @"Xml_report/" + CurFiles + "/";

            if (!File.Exists(zipPath))
            {
                ZipFile.CreateFromDirectory(startPath, zipPath);
            } else {
                File.Delete(zipPath);
                ZipFile.CreateFromDirectory(startPath, zipPath);
            }

            Directory.Delete(startPath, true);
        }

        public void ClearAll()
        {
            ValidationShema.all_err.Clear();
        }
    }
}