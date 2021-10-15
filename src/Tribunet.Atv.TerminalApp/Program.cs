using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using FirmaXadesNetCore;
using FirmaXadesNetCore.Crypto;
using FirmaXadesNetCore.Signature.Parameters;
using FirmaXadesNetCore.Utils;
using Microsoft.Xades;
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
            var certificateFilePath = Environment.GetEnvironmentVariable("ATV_XADES_EPES_CERT_FILE_PATH");
            var certificateSubject = Environment.GetEnvironmentVariable("ATV_XADES_EPES_CERT_SUBJECT");
            var certificatePassword = Environment.GetEnvironmentVariable("ATV_XADES_EPES_CERT_PASSWORD");
            
            X509Certificate2 certificate = (!string.IsNullOrEmpty(certificateFilePath) && File.Exists(certificateFilePath))
                ? GetCertificateFromFilePath(certificateFilePath, certificatePassword)
                : GetCertificateStore(certificateSubject);
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.PreserveWhitespace = true;
            xmlDocument.Load(comprobanteStream);
            var signedXmlDocument = SignUsingXadesEPES(xmlDocument, certificate);
            var signedComprobanteStream = new MemoryStream();
            signedXmlDocument.WriteTo(XmlWriter.Create(signedComprobanteStream));
            signedComprobanteStream.Seek(0, SeekOrigin.Begin);


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

        private static X509Certificate2 GetCertificateFromFilePath(string filePath, string password)
        {
            X509Certificate2 cert = new X509Certificate2(filePath, password, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);
            //RSACryptoServiceProvider crypt = (RSACryptoServiceProvider)cert.PrivateKey;
            return cert;
        }


        #region XAdES-EPES Signer

        // 1. - Seleccion del certificado
        public static X509Certificate2 GetCertificateStore(string subject)
        {
            X509Store store = new X509Store(StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            X509Certificate2Collection allCertificates = store.Certificates;
            X509Certificate2Collection nonExpiredCertificates = allCertificates.Find(X509FindType.FindByTimeValid, DateTime.Now, false);
            X509Certificate2 selectedCertificate =
                nonExpiredCertificates
                .OfType<X509Certificate2>()
                .FirstOrDefault(x => x.Subject == subject);

            return selectedCertificate;
        }

        // 2. - Ejecuto los siguientes metodos :
        private static XmlDocument SignUsingXadesEPES(XmlDocument xmlDocument, X509Certificate2 certificate)
        {

            XadesSignedXml signedXml = new XadesSignedXml(xmlDocument);
            signedXml.Signature.Id = "SignatureId";
            signedXml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigExcC14NTransformUrl;
            signedXml.SignedInfo.SignatureMethod = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";

            string URI = "http://uri.etsi.org/01903/v1.3.2#";
            XmlElement qualifyingPropertiesRootElement = xmlDocument.CreateElement("xades", "QualifyingProperties", URI);
            qualifyingPropertiesRootElement.SetAttribute("Target", "#SignatureId", URI);

            XmlElement signaturePropertiesRootElement = xmlDocument.CreateElement("xades", "SignedProperties", URI);
            signaturePropertiesRootElement.SetAttribute("Id", "SignedPropertiesId", URI);

            XmlElement signedSignaturePropertiesElement = xmlDocument.CreateElement("xades", "SignedSignatureProperties", URI);

            XmlElement timestampElement = xmlDocument.CreateElement("xades", "SigningTime", URI);
            timestampElement.InnerText = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); //2011-09-05T09:11:24.268Z
            signedSignaturePropertiesElement.AppendChild(timestampElement);

            XmlElement signingCertificateElement = xmlDocument.CreateElement("xades", "SigningCertificate", URI);
            XmlElement certElement = xmlDocument.CreateElement("xades", "Cert", URI);
            XmlElement certDigestElement = xmlDocument.CreateElement("xades", "CertDigest", URI);
            SHA1 cryptoServiceProvider = new SHA1CryptoServiceProvider();
            byte[] sha1 = cryptoServiceProvider.ComputeHash(certificate.RawData);

            XmlElement digestMethodElement = xmlDocument.CreateElement("ds", "DigestMethod", URI);

            digestMethodElement.SetAttribute("Algorithm", SignedXml.XmlDsigSHA1Url);
            XmlElement digestValueElement = xmlDocument.CreateElement("ds", "DigestValue", URI);
            digestValueElement.InnerText = Convert.ToBase64String(sha1);
            certDigestElement.AppendChild(digestMethodElement);
            certDigestElement.AppendChild(digestValueElement);
            certElement.AppendChild(certDigestElement);

            XmlElement issuerSerialElement = xmlDocument.CreateElement("xades", "IssuerSerial", URI);
            XmlElement x509IssuerNameElement = xmlDocument.CreateElement("ds", "X509IssuerName", "http://www.w3.org/2000/09/xmldsig#");
            x509IssuerNameElement.InnerText = certificate.IssuerName.Name;
            XmlElement x509SerialNumberElement = xmlDocument.CreateElement("ds", "X509SerialNumber", "http://www.w3.org/2000/09/xmldsig#");
            x509SerialNumberElement.InnerText = certificate.SerialNumber;
            issuerSerialElement.AppendChild(x509IssuerNameElement);
            issuerSerialElement.AppendChild(x509SerialNumberElement);
            certElement.AppendChild(issuerSerialElement);

            signingCertificateElement.AppendChild(certElement);
            signedSignaturePropertiesElement.AppendChild(signingCertificateElement);

            signaturePropertiesRootElement.AppendChild(signedSignaturePropertiesElement);
            qualifyingPropertiesRootElement.AppendChild(signaturePropertiesRootElement);

            // /////////////////////////////////
            XmlElement signaturePolicyIdentifierElement = xmlDocument.CreateElement("xades", "SignaturePolicyIdentifier", URI);
            signedSignaturePropertiesElement.AppendChild(signaturePolicyIdentifierElement);

            XmlElement signaturePolicyIdElement = xmlDocument.CreateElement("xades", "SignaturePolicyId", URI);
            signaturePolicyIdentifierElement.AppendChild(signaturePolicyIdElement);

            XmlElement sigPolicyIdElement = xmlDocument.CreateElement("xades", "SigPolicyId", URI);
            signaturePolicyIdElement.AppendChild(sigPolicyIdElement);

            XmlElement identifierElement = xmlDocument.CreateElement("xades", "Identifier", URI);
            identifierElement.InnerText = "https://tribunet.hacienda.go.cr/docs/esquemas/2016/v4.1/Resolucion_Comprobantes_Electronicos_DGT-R-48-2016.pdf";
            sigPolicyIdElement.AppendChild(identifierElement);

            XmlElement sigPolicyHashElement = xmlDocument.CreateElement("xades", "SigPolicyHash", URI);
            signaturePolicyIdElement.AppendChild(sigPolicyHashElement);

            digestMethodElement = xmlDocument.CreateElement("ds", "DigestMethod", URI);
            digestMethodElement.SetAttribute("Algorithm", "http://www.w3.org/2001/04/xmlenc#sha256");
            digestValueElement = xmlDocument.CreateElement("ds", "DigestValue", URI);
            byte[] shaCertificate = { 0xf1, 0x48, 0x03, 0x50, 0x5c, 0x33, 0x64, 0x29, 0x07, 0x84, 0x43, 0xca, 0x79, 0x6e, 0x59, 0xcc, 0xac, 0xf5, 0x85, 0x4c };
            digestValueElement.InnerText = Convert.ToBase64String(shaCertificate);
            sigPolicyHashElement.AppendChild(digestMethodElement);
            sigPolicyHashElement.AppendChild(digestValueElement);

            XmlElement signedDataObjectPropertiesElement = xmlDocument.CreateElement("xades", "SignedDataObjectProperties", URI);
            XmlElement dataObjectFormatElement = xmlDocument.CreateElement("xades", "DataObjectFormat", URI);
            dataObjectFormatElement.SetAttribute("ObjectReference", "#r-id-1");
            signaturePropertiesRootElement.AppendChild(signedDataObjectPropertiesElement);
            signedDataObjectPropertiesElement.AppendChild(dataObjectFormatElement);
            XmlElement mimeTypeElement = xmlDocument.CreateElement("xades", "MimeType", URI);
            mimeTypeElement.InnerText = "application/octet-stream";
            dataObjectFormatElement.AppendChild(mimeTypeElement);
            // /////////////////////////////////////////////////////////////

            DataObject dataObject = new DataObject
            {
                Data = qualifyingPropertiesRootElement.SelectNodes("."),
            };

            signedXml.AddObject(dataObject);

            signedXml.SigningKey = certificate.PrivateKey;

            KeyInfo keyInfo = new KeyInfo();
            KeyInfoX509Data keyInfoX509Data = new KeyInfoX509Data(certificate, X509IncludeOption.ExcludeRoot);
            keyInfo.AddClause(keyInfoX509Data);
            signedXml.KeyInfo = keyInfo;

            //Reference 1
            Reference reference2 = new Reference();
            reference2.Id = "R1";
            reference2.Type = "http://uri.etsi.org/01903#SignedProperties";
            reference2.Uri = "";
            XmlDsigXPathTransform xPathTransform = CreateXPathTransform("ValorPath");
            reference2.AddTransform(xPathTransform);
            reference2.DigestMethod = "http://www.w3.org/2001/04/xmlenc#sha256";
            reference2.AddTransform(new XmlDsigExcC14NTransform());
            signedXml.AddReference(reference2);

            //Reference 2
            reference2 = new Reference();
            // reference2.Id = "R2";
            reference2.Type = "http://uri.etsi.org/01903#SignedProperties";
            reference2.Uri = "";
            // reference2.AddTransform(new XmlDsigExcC14NTransform());
            xPathTransform = CreateXPathTransform("ValorPath");
            // reference2.AddTransform(XPathTransform);
            reference2.DigestMethod = "http://www.w3.org/2001/04/xmlenc#sha256";
            reference2.AddTransform(new XmlDsigExcC14NTransform());
            signedXml.AddReference(reference2);

            signedXml.ComputeSignature();

            // Get the XML representation of the signature and save
            // it to an XmlElement object.
            XmlElement xmlDigitalSignatureElement = signedXml.GetXml();

            xmlDocument.DocumentElement.AppendChild(xmlDocument.ImportNode(xmlDigitalSignatureElement, true));

            bool checkSign = signedXml.CheckSignature();

            //return xmlDoc.OuterXml;
            return xmlDocument;

        }
        private static XmlDsigXPathTransform CreateXPathTransform(string xPathString)
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlElement xPathElement = xmlDocument.CreateElement("XPath");
            xPathElement.InnerText = xPathString;
            XmlDsigXPathTransform xForm = new XmlDsigXPathTransform();
            xForm.LoadInnerXml(xPathElement.SelectNodes("."));
            return xForm;
        }


        //var xadesService = new XadesService();
        //var signatureParameters = new SignatureParameters();

        //signatureParameters.SignaturePolicyInfo = new SignaturePolicyInfo();
        //signatureParameters.SignaturePolicyInfo.PolicyIdentifier = "http://www.facturae.es/politica_de_firma_formato_facturae/politica_de_firma_formato_facturae_v3_1.pdf";
        //signatureParameters.SignaturePolicyInfo.PolicyHash = "Ohixl6upD6av8N7pEvDABhEL6hM=";
        //signatureParameters.SignaturePackaging = SignaturePackaging.ENVELOPED;
        //signatureParameters.DataFormat = new DataFormat();
        //signatureParameters.DataFormat.MimeType = "text/xml";
        //signatureParameters.SignerRole = new SignerRole();
        //signatureParameters.SignerRole.ClaimedRoles.Add("emisor");

        //using (signatureParameters.Signer = new Signer(CertUtil.VerifyCertificate()))
        //{
        //    using (FileStream fs = new FileStream(ficheroFactura, FileMode.Open))
        //    {
        //        var docFirmado = xadesService.Sign(fs, signatureParameters);

        //    }
        //}
        #endregion



    }
}
