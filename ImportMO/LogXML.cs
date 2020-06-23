using System.Xml.Linq;

namespace ImportMO
{
    class LogXML
    {
        public static XDocument SaveXmlLog(string XMLname)
        {
            string oshib_code = "0";
            string report_file_name = "report " + XMLname + ".xml";

            XDocument xdoc = new XDocument();
            XElement base_elem = new XElement("FLK_P");

            base_elem.Add(
                new XElement("FNAME", report_file_name),
                new XElement("FNAME_I", XMLname)
            );

            foreach (string[] elem in ValidationShema.all_err)
            {
                oshib_code = "not defined";
                if (elem[4].Contains("недопустимый дочерний элемент") || elem[4].Contains("Список ожидаемых элементов"))
                {
                    oshib_code = "901";
                }

                if (elem[4].Contains("Фактическая длина меньше значения MinLength") || elem[4].Contains("Фактическая длина больше значения MaxLength"))
                {
                    oshib_code = "902";
                }

                if (elem[4].Contains("не является допустимым значением"))
                {
                    oshib_code = "903";
                }

                XElement PR = new XElement("PR");
                XElement OSHIB = new XElement("OSHIB", oshib_code);
                XElement IM_POL = new XElement("IM_POL", elem[0]);
                XElement BAS_EL = new XElement("BAS_EL", elem[7]);
                XElement N_ZAP = new XElement("N_ZAP", elem[1]);
                XElement SLUCH = new XElement("IDCASE", elem[2]);
                XElement SERV = new XElement("IDSERV", elem[3]);
                XElement COMMENT = new XElement("COMMENT", "Строка: " + elem[5] + ", Позиция: " + elem[6] + ". " + elem[4]);

                PR.Add(OSHIB);
                PR.Add(IM_POL);
                PR.Add(BAS_EL);

                if (elem[1] != null && elem[1] != "[exclude]")
                {
                    PR.Add(N_ZAP);
                    if (elem[2] != null && elem[2] != "[exclude]")
                    {
                        PR.Add(SLUCH);
                        if (elem[3] != null && elem[3] != "[exclude]")
                        {
                            PR.Add(SERV);
                        }
                    }
                }

                PR.Add(COMMENT);
                base_elem.Add(PR);
            }

            xdoc.Add(base_elem);
            //xdoc.Save("Xml_report/" + report_file_name);
            return xdoc;
        }
    }
}