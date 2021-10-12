using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Tribunet.Atv.Services;

namespace Tribunet.Atv.TerminalApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var comprobanteStream = new System.IO.MemoryStream();

            // generates comprobante electronico
            var facturaElectronica = new Models.FacturaElectronica_V_4_2.FacturaElectronica();
            facturaElectronica.Clave =
                new ModelDataProvider.Clave(ModelDataProvider.Clave.tipoComprobante.FacturaElectronica);

            // serializes comprobante electronico into XML
            var xmlSerializer = new XmlSerializer(typeof(Models.FacturaElectronica_V_4_2.FacturaElectronica));
            var textWriter = new StreamWriter(comprobanteStream);
            xmlSerializer.Serialize(textWriter, facturaElectronica);
            comprobanteStream.Flush();
            comprobanteStream.Seek(0, SeekOrigin.Begin);

            // Gets XML Text
            var encoding = System.Text.Encoding.UTF8;
            var xmlComprobante = encoding.GetString(comprobanteStream.ToArray());

            // validates against XSD
            var xmlResourceAssembly = typeof(ModelDataProvider).Assembly;
            static void ValidationCallBack(object sender, ValidationEventArgs args)
            {
                if (args.Severity == XmlSeverityType.Warning)
                    Console.WriteLine("\tWarning: Matching schema not found.  No validation occurred." + args.Message);
                else
                    Console.WriteLine("\tValidation error: " + args.Message);

            }
            var xmlSettings = new XmlReaderSettings();
            xmlSettings.ValidationType = ValidationType.Schema;
            xmlSettings.DtdProcessing = DtdProcessing.Parse;
            xmlSettings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
            xmlSettings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
            xmlSettings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            xmlSettings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);


            XmlSchemaSet xmlSchemaSet = default;
            Stream xmlSchemaStream = default;

            xmlSchemaSet = new XmlSchemaSet();
            xmlSchemaStream = xmlResourceAssembly.GetManifestResourceStream("Tribunet.Atv.Resources.FacturaElectronica_V.4.2.xsd");
            xmlSchemaSet.Add("https://tribunet.hacienda.go.cr/docs/esquemas/2017/v4.2/facturaElectronica", XmlReader.Create(xmlSchemaStream));
            xmlSettings.Schemas.Add(xmlSchemaSet);


            xmlSchemaSet = new XmlSchemaSet();
            xmlSchemaStream = xmlResourceAssembly.GetManifestResourceStream("Tribunet.Atv.Resources.xmldsig-core-schema.xsd");
            xmlSchemaSet.Add("http://www.w3.org/2000/09/xmldsig#", XmlReader.Create(xmlSchemaStream, new XmlReaderSettings()
            {
                DtdProcessing = DtdProcessing.Parse
            }));
            xmlSettings.Schemas.Add(xmlSchemaSet);

            // Create the XmlReader object.
            var textReader = new System.IO.StreamReader(comprobanteStream);
            XmlReader reader = XmlReader.Create(textReader, xmlSettings);

            // Parse the file. 
            while (reader.Read()) ;


            Console.WriteLine("Hello World!");
        }


    }
}
