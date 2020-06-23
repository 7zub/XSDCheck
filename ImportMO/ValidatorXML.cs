using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;

namespace ImportMO
{
    class ValidationHandler
    {
        private IList<string[]> myValidationErrors = new List<String[]>();
        public IList<string[]> MyValidationErrors { get { return this.myValidationErrors; } }

        public void HandleValidationError(object sender, ValidationEventArgs ve)
        {
            if (ve.Severity == XmlSeverityType.Error || ve.Severity == XmlSeverityType.Warning)
            {
                CheckImportMOXML.setLog(string.Join(", ", ValidationShema.stack));
                CheckImportMOXML.setLog(ValidationShema.current_el);

                string[] err_el =
                {
                    ValidationShema.current_el,
                    ValidationShema.valueZAP,
                    ValidationShema.valueSLUCH,
                    ValidationShema.valueSERV,
                    ve.Exception.Message,
                    ve.Exception.LineNumber.ToString(),
                    ve.Exception.LinePosition.ToString(),
                    ValidationShema.stack.First()
                };

                myValidationErrors.Add(err_el);
            }
        }
    }
}