using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;

namespace ImportMO
{
    class ValidationShema
    {
        public static Stack<string> stack = new Stack<string>();
        public static IList<string[]> all_err = new List<String[]>();

        public static string current_el;
        public static int depth;
        public static string valueZAP;
        public static string valueSLUCH;
        public static string valueSERV;

        public static void ValidateAgainstSchema(string XMLSourceDocument, XmlSchemaSet validatingSchemas)
        {
            if (validatingSchemas == null)
            {
                throw new ArgumentNullException("Серьезная ошибка! Схема не загружена.");
            }

            ValidationHandler handler = new ValidationHandler();

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.CloseInput = true;
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationEventHandler += new ValidationEventHandler(handler.HandleValidationError);
            settings.Schemas.Add(validatingSchemas);
            settings.ValidationFlags =
                XmlSchemaValidationFlags.ReportValidationWarnings |
                XmlSchemaValidationFlags.ProcessIdentityConstraints |
                XmlSchemaValidationFlags.ProcessInlineSchema |
                XmlSchemaValidationFlags.ProcessSchemaLocation;

            try
            {
                using (XmlReader validatingReader = XmlReader.Create(XMLSourceDocument, settings))
                {
                    while (validatingReader.Read())
                    {
                        if (validatingReader.NodeType == XmlNodeType.Element)
                        {
                            current_el = validatingReader.Name;

                            if (depth == validatingReader.Depth)
                            {
                                stack.Push(validatingReader.Name);
                                depth++;
                            }

                            if (current_el == "N_ZAP")
                            {
                                validatingReader.Read();
                                valueZAP = validatingReader.Value;
                            }

                            if (current_el == "IDCASE")
                            {
                                validatingReader.Read();
                                valueSLUCH = validatingReader.Value;
                            }

                            if (current_el == "IDSERV")
                            {
                                validatingReader.Read();
                                valueSERV = validatingReader.Value;
                            }
                        }
                        else if (validatingReader.NodeType == XmlNodeType.EndElement)
                        {
                            stack.Pop();
                            depth--;

                            if (current_el == "ZAP")
                            {
                                valueZAP = "[exclude]";
                            }

                            if (current_el == "Z_SL")
                            {
                                valueSLUCH = "[exclude]";
                            }

                            if (current_el == "USL")
                            {
                                valueSERV = "[exclude]";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //setLog(ex.Data.ToString())
            }

            if (handler.MyValidationErrors.Count > 0)
            {
                foreach (String[] item in handler.MyValidationErrors)
                {
                    all_err.Add(item);
                }
            }
            else
            {
                CheckImportMOXML.setLog("Успешно прошел проверку!");
            }
        }
    }
}