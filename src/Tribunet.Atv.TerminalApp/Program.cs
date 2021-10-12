using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Org.BouncyCastle.Crypto.Agreement;
using Tribunet.Atv.Services;

namespace Tribunet.Atv.TerminalApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // generates comprobante electronico
            var facturaElectronica = new Models.FacturaElectronica_V_4_2.FacturaElectronica();
            facturaElectronica.Clave =
                new ModelDataProvider.Clave(ModelDataProvider.Clave.tipoComprobante.FacturaElectronica);

            // serializes comprobante electronico into XML
            var xmlSerializer = new XmlSerializer(typeof(Models.FacturaElectronica_V_4_2.FacturaElectronica));
            var sb = new StringBuilder();
            var textWriter = new StringWriter(sb);
            xmlSerializer.Serialize(textWriter, facturaElectronica);

            // validates against XSD

            XmlSchemaSet schemas = new XmlSchemaSet();
            schemas.Add(XmlSchema.Read(System.IO.File.OpenRead(
                @"D:\projects\github\tribunet\src\Tribunet.Atv\Resources\FacturaElectronica_V.4.2.xsd"
                ), OnXmlReaderSettingsValidationEventHandler));
            var xmlReaderSettings = new XmlReaderSettings();
            xmlReaderSettings.DtdProcessing = DtdProcessing.Parse;
            xmlReaderSettings.ValidationType = ValidationType.DTD;
            xmlReaderSettings.Schemas.Add(schemas);
            xmlReaderSettings.Schemas.Add(
                "https://www.w3.org/TR/2008/REC-xmldsig-core-20080610/xmldsig-core-schema.xsd",
                @"D:\projects\github\tribunet\src\Tribunet.Atv\Resources\xmldsig-core-schema.xsd");
            xmlReaderSettings.ValidationType = ValidationType.Schema;
            xmlReaderSettings.ValidationEventHandler += OnXmlReaderSettingsValidationEventHandler;
            var textReader = new StringReader(sb.ToString());
            XmlReader xmlReader = XmlReader.Create(textReader, xmlReaderSettings);
            while (xmlReader.Read()) { }


            Console.WriteLine("Hello World!");
        }

        private static void OnXmlReaderSettingsValidationEventHandler(object sender, ValidationEventArgs e)
        {
            if (e.Severity == XmlSeverityType.Warning)
            {
                Console.Write("WARNING: ");
                Console.WriteLine(e.Message);
            }
            else if (e.Severity == XmlSeverityType.Error)
            {
                Console.Write("ERROR: ");
                Console.WriteLine(e.Message);
            }
        }
    }
}
