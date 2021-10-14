using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using FirmaXadesNetCore;
using FirmaXadesNetCore.Crypto;
using FirmaXadesNetCore.Signature.Parameters;
using FirmaXadesNetCore.Utils;
using Tribunet.Atv.ApiClient.Api;
using Tribunet.Atv.ApiClient.Authenticator;
using Tribunet.Atv.ApiClient.Client;
using Tribunet.Atv.ApiClient.Model;
using Tribunet.Atv.Models.FacturaElectronica_V_4_2;
using Tribunet.Atv.Services;

namespace Tribunet.Atv.TerminalApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var localNow = DateTime.Now;
            var utcNow = DateTime.UtcNow;
            var comprobanteStream = new System.IO.MemoryStream();

            // ==========
            // create `comprobante electronico`
            var comprobanteElectronico = new Models.FacturaElectronica_V_4_2.FacturaElectronica
            {
                Clave = new ClaveNumerica(
                    codigoPais: 506,
                    fechaCreacion: localNow,
                    cedulaEmisor: "110910312",
                    numeracionConsecutivo: new NumeroConsecutivo(
                        tipoComprobante: TipoComprobante.FacturaElectronica,
                        oficinaId: 1,
                        puntoDeVentaId: 1,
                        consecutivoComprobante: Convert.ToInt32(DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                    ),
                    situacion: SituacionDelComprobante.Normal,
                    codigoSeguridad: 1
                ),
                Emisor = new EmisorType
                {
                    Identificacion = new IdentificacionType
                    {
                        Tipo = IdentificacionTypeTipo.Item01,
                        Numero = "110910312",
                    }
                },
                Receptor = new ReceptorType
                {
                    Identificacion = new IdentificacionType
                    {
                        Tipo = IdentificacionTypeTipo.Item01,
                        Numero = "1997588"
                    }
                }
            };

            // ==========
            // serializes `comprobante electronico` into XML (represented by a Stream)
            var xmlSerializer = new XmlSerializer(typeof(Models.FacturaElectronica_V_4_2.FacturaElectronica));
            var textWriter = new StreamWriter(comprobanteStream);
            xmlSerializer.Serialize(textWriter, comprobanteElectronico);
            comprobanteStream.Flush();
            comprobanteStream.Seek(0, SeekOrigin.Begin);

            // ==========
            // TODO: Sign the XML
            //XadesService xadesService = new XadesService();
            //SignatureParameters parametros = new SignatureParameters();

            //parametros.SignaturePolicyInfo = new SignaturePolicyInfo();
            //parametros.SignaturePolicyInfo.PolicyIdentifier = "http://www.facturae.es/politica_de_firma_formato_facturae/politica_de_firma_formato_facturae_v3_1.pdf";
            //parametros.SignaturePolicyInfo.PolicyHash = "Ohixl6upD6av8N7pEvDABhEL6hM=";
            //parametros.SignaturePackaging = SignaturePackaging.ENVELOPED;
            //parametros.DataFormat = new DataFormat();
            //parametros.DataFormat.MimeType = "text/xml";
            //parametros.SignerRole = new SignerRole();
            //parametros.SignerRole.ClaimedRoles.Add("emisor");

            //using (parametros.Signer = new Signer(CertUtil.VerifyCertificate()))
            //{
            //    using (FileStream fs = new FileStream(ficheroFactura, FileMode.Open))
            //    {
            //        var docFirmado = xadesService.Sign(fs, parametros);

            //    }
            //}


            // ==========
            // validates the XML
            var xdsValidationResult = Enum.GetNames(typeof(XmlSeverityType)).ToDictionary(n => n, _ => new HashSet<string>(), StringComparer.InvariantCultureIgnoreCase);
            var xmlResourceAssembly = typeof(ModelDataProvider).Assembly;

            void ValidationCallBack(object sender, ValidationEventArgs args)
                => xdsValidationResult[args.Severity.ToString()].Add(args.Message);

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

            // Parse the file
            while (reader.Read()) { }
            xmlSettings.ValidationEventHandler -= ValidationCallBack;

            // Gets XML Text
            //var encoding = System.Text.Encoding.UTF8;
            //var xmlComprobante = encoding.GetString(comprobanteStream.ToArray());

            // ==========
            // TODO: Sends the `comprobante electronico` 

            Configuration config = new Configuration
            {
                BasePath = "https://api.comprobanteselectronicos.go.cr/recepcion-sandbox/v1",
                OAuth2PasswordAuthenticatorOptions = new OAuth2PasswordAuthenticatorOptions(
                    accessTokenUrl: Environment.GetEnvironmentVariable("ATV_OAUTH2_ACCESS_TOKEN_URL"),
                    refreshTokenUrl: Environment.GetEnvironmentVariable("ATV_OAUTH2_REFRESH_TOKEN_URL"),
                    username: Environment.GetEnvironmentVariable("ATV_OAUTH2_USERNAME"),
                    password: Environment.GetEnvironmentVariable("ATV_OAUTH2_PASSWORD"),
                    clientId: Environment.GetEnvironmentVariable("ATV_OAUTH2_CLIENT_ID")
                )
            };
            GlobalConfiguration.Instance.OAuth2PasswordAuthenticatorOptions = config.OAuth2PasswordAuthenticatorOptions;

            var recepcionPostRequest = new RecepcionPostRequest
            (
                clave: comprobanteElectronico.Clave,
                fecha: utcNow.ToRfc3339String(),
                emisor: new RecepcionPostRequestEmisor(
                    tipoIdentificacion: comprobanteElectronico.Emisor.Identificacion.Tipo.GetXmlEnumName(),
                    numeroIdentificacion: comprobanteElectronico.Emisor.Identificacion.Numero
                ),
                receptor: new RecepcionPostRequestEmisor(
                    tipoIdentificacion: comprobanteElectronico.Receptor.Identificacion.Tipo.GetXmlEnumName(),
                    numeroIdentificacion: comprobanteElectronico.Receptor.Identificacion.Numero
                ),
                comprobanteXml: Convert.ToBase64String(comprobanteStream.ToArray())
            );

            var recepcionApiClient = new RecepcionApi(config);
            try
            {
                // Recibe el comprobante electrónico o respuesta del receptor.
                await recepcionApiClient.PostReceptionAsync(recepcionPostRequest);
            }
            catch (ApiException e)
            {
                Debug.WriteLine("Exception when calling RecepcionApi.PostReception: " + e.Message);
                Debug.WriteLine("Status Code: " + e.ErrorCode);
                Debug.WriteLine(e.StackTrace);
            }

            // ==========
            // TODO:  Checks status of the sent `comprobante electronico`
            Debug.WriteLine("Hello World!");
        }


    }
}
