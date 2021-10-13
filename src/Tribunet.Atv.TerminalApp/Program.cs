using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Tribunet.Atv.ApiClient.Api;
using Tribunet.Atv.ApiClient.Model;
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
                new ClaveNumerica(
                    codigoPais: 506,
                    fechaCreacion: DateTime.Now,
                    cedulaEmisor: "110910312",
                    numeracionConsecutivo: new NumeroConsecutivo(
                        tipoComprobante: TipoComprobante.FacturaElectronica,
                        oficinaId: 1,
                        puntoDeVentaId: 1,
                        consecutivoComprobante: 0
                        ),
                    situacion: SituacionDelComprobante.Normal,
                    codigoSeguridad: 1
                );

            // serializes comprobante electronico into XML
            var xmlSerializer = new XmlSerializer(typeof(Models.FacturaElectronica_V_4_2.FacturaElectronica));
            var textWriter = new StreamWriter(comprobanteStream);
            xmlSerializer.Serialize(textWriter, facturaElectronica);
            comprobanteStream.Flush();
            comprobanteStream.Seek(0, SeekOrigin.Begin);

            // TODO: Sign the XML

            // validates against XSD
            var xdsValidationResult = Enum.GetNames(typeof(XmlSeverityType)).ToDictionary(n => n, _ => new HashSet<string>(), StringComparer.InvariantCultureIgnoreCase);
            var xmlResourceAssembly = typeof(ModelDataProvider).Assembly;
            void ValidationCallBack(object sender, ValidationEventArgs args)
            {
                xdsValidationResult[args.Severity.ToString()].Add(args.Message);

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
            xmlSettings.ValidationEventHandler += ValidationCallBack;


            XmlSchemaSet xmlSchemaSet = default;
            Stream xmlSchemaStream = default;

            xmlSchemaSet = new XmlSchemaSet();
            xmlSchemaStream = xmlResourceAssembly.GetManifestResourceStream("Tribunet.Atv.Resources.FacturaElectronica_V_4_2.xsd");
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
            while (reader.Read()) { }
            xmlSettings.ValidationEventHandler -= ValidationCallBack;

            // Gets XML Text
            //var encoding = System.Text.Encoding.UTF8;
            //var xmlComprobante = encoding.GetString(comprobanteStream.ToArray());

            // TODO: Send the Comprobante electronico (XML) using ATV Api Client
            IRecepcionApi recepcionApi = new RecepcionApi();
            recepcionApi.PostReception(new RecepcionPostRequest
            (
                clave: facturaElectronica.Clave,
                fecha: DateTime.Now.ToRfc3339String(),
                emisor: new RecepcionPostRequestEmisor
                {
                    TipoIdentificacion = 
                },
                receptor: new RecepcionPostRequestEmisor
                {

                },
                comprobanteXml: Convert.ToBase64String(comprobanteStream.ToArray())
            ));
            Console.WriteLine("Hello World!");
        }


    }
}
