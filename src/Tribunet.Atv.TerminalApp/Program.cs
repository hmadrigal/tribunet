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
            var xadesService = new XadesService();
            xadesService.
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


        #region XAdES-EPES Signer
        // 1. - Seleccion del certificado
        public X509Certificate2 ElegirCertificado()
        {
            X509Store store = new X509Store(StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            X509Certificate2Collection certificates = store.Certificates;
            X509Certificate2Collection foundCertificates = certificates.Find(X509FindType.FindByTimeValid, DateTime.Now, false);
            X509Certificate2 cert = 
                foundCertificates
                .OfType<X509Certificate2>()
                .Where(x => x.Subject == "CN=NEOTECNOLOGIAS SOCIEDAD ANONIMA, OU=CPJ, O=PERSONA JURIDICA, C=CR, G=NEOTECNOLOGIAS SOCIEDAD ANONIMA, SN=\"\", SERIALNUMBER=CPJ-3-101-408861")
                .First();
            return cert;
        }

        public string PreviaXadesEpes(string path)
        {
            error = "true";
            try
            {
                X509Certificate2 certificado = new X509Certificate2();
                certificado = ElegirCertificado();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.PreserveWhitespace = true;

                xmlDoc.Load(path);
                xmlDoc = FirmarXadesEPES(xmlDoc, certificado);
                xmlDoc.Save(path);
            }
            catch (Exception ex) { error = ex.ToString(); }
            return error;
        }

        // 2. - Ejecuto los siguientes metodos :
        private XmlDocument FirmarXadesEPES(XmlDocument xmlDoc, X509Certificate2 certificate)
        {

            XadesSignedXml signedXml = new XadesSignedXml(xmlDoc);
            signedXml.Signature.Id = "SignatureId";
            signedXml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigExcC14NTransformUrl;
            signedXml.SignedInfo.SignatureMethod = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";

            string URI = "http://uri.etsi.org/01903/v1.3.2#";
            XmlElement qualifyingPropertiesRoot = xmlDoc.CreateElement("xades", "QualifyingProperties", URI);
            qualifyingPropertiesRoot.SetAttribute("Target", "#SignatureId", URI);

            XmlElement signaturePropertiesRoot = xmlDoc.CreateElement("xades", "SignedProperties", URI);
            signaturePropertiesRoot.SetAttribute("Id", "SignedPropertiesId", URI);

            XmlElement SignedSignatureProperties = xmlDoc.CreateElement("xades", "SignedSignatureProperties", URI);

            XmlElement timestamp = xmlDoc.CreateElement("xades", "SigningTime", URI);
            timestamp.InnerText = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); //2011-09-05T09:11:24.268Z
            SignedSignatureProperties.AppendChild(timestamp);

            XmlElement SigningCertificate = xmlDoc.CreateElement("xades", "SigningCertificate", URI);
            XmlElement Cert = xmlDoc.CreateElement("xades", "Cert", URI);
            XmlElement CertDigest = xmlDoc.CreateElement("xades", "CertDigest", URI);
            SHA1 cryptoServiceProvider = new SHA1CryptoServiceProvider();
            byte[] sha1 = cryptoServiceProvider.ComputeHash(certificate.RawData);

            XmlElement DigestMethod = xmlDoc.CreateElement("ds", "DigestMethod", URI);

            DigestMethod.SetAttribute("Algorithm", SignedXml.XmlDsigSHA1Url);
            XmlElement DigestValue = xmlDoc.CreateElement("ds", "DigestValue", URI);
            DigestValue.InnerText = Convert.ToBase64String(sha1);
            CertDigest.AppendChild(DigestMethod);
            CertDigest.AppendChild(DigestValue);
            Cert.AppendChild(CertDigest);

            XmlElement IssuerSerial = xmlDoc.CreateElement("xades", "IssuerSerial", URI);
            XmlElement X509IssuerName = xmlDoc.CreateElement("ds", "X509IssuerName", "http://www.w3.org/2000/09/xmldsig#");
            X509IssuerName.InnerText = certificate.IssuerName.Name;
            XmlElement X509SerialNumber = xmlDoc.CreateElement("ds", "X509SerialNumber", "http://www.w3.org/2000/09/xmldsig#");
            X509SerialNumber.InnerText = certificate.SerialNumber;
            IssuerSerial.AppendChild(X509IssuerName);
            IssuerSerial.AppendChild(X509SerialNumber);
            Cert.AppendChild(IssuerSerial);

            SigningCertificate.AppendChild(Cert);
            SignedSignatureProperties.AppendChild(SigningCertificate);

            signaturePropertiesRoot.AppendChild(SignedSignatureProperties);
            qualifyingPropertiesRoot.AppendChild(signaturePropertiesRoot);

            // /////////////////////////////////
            XmlElement SignaturePolicyIdentifier = xmlDoc.CreateElement("xades", "SignaturePolicyIdentifier", URI);
            SignedSignatureProperties.AppendChild(SignaturePolicyIdentifier);

            XmlElement SignaturePolicyId = xmlDoc.CreateElement("xades", "SignaturePolicyId", URI);
            SignaturePolicyIdentifier.AppendChild(SignaturePolicyId);

            XmlElement SigPolicyId = xmlDoc.CreateElement("xades", "SigPolicyId", URI);
            SignaturePolicyId.AppendChild(SigPolicyId);

            XmlElement Identifier = xmlDoc.CreateElement("xades", "Identifier", URI);
            Identifier.InnerText = "https://tribunet.hacienda.go.cr/docs/esquemas/2016/v4.1/Resolucion_Comprobantes_Electronicos_DGT-R-48-2016.pdf";
            SigPolicyId.AppendChild(Identifier);

            XmlElement SigPolicyHash = xmlDoc.CreateElement("xades", "SigPolicyHash", URI);
            SignaturePolicyId.AppendChild(SigPolicyHash);

            DigestMethod = xmlDoc.CreateElement("ds", "DigestMethod", URI);
            DigestMethod.SetAttribute("Algorithm", "http://www.w3.org/2001/04/xmlenc#sha256");
            DigestValue = xmlDoc.CreateElement("ds", "DigestValue", URI);
            byte[] shaCertificate = { 0xf1, 0x48, 0x03, 0x50, 0x5c, 0x33, 0x64, 0x29, 0x07, 0x84, 0x43, 0xca, 0x79, 0x6e, 0x59, 0xcc, 0xac, 0xf5, 0x85, 0x4c };
            DigestValue.InnerText = Convert.ToBase64String(shaCertificate);
            SigPolicyHash.AppendChild(DigestMethod);
            SigPolicyHash.AppendChild(DigestValue);

            XmlElement SignedDataObjectProperties = xmlDoc.CreateElement("xades", "SignedDataObjectProperties", URI);
            XmlElement DataObjectFormat = xmlDoc.CreateElement("xades", "DataObjectFormat", URI);
            DataObjectFormat.SetAttribute("ObjectReference", "#r-id-1");
            signaturePropertiesRoot.AppendChild(SignedDataObjectProperties);
            SignedDataObjectProperties.AppendChild(DataObjectFormat);
            XmlElement MimeType = xmlDoc.CreateElement("xades", "MimeType", URI);
            MimeType.InnerText = "application/octet-stream";
            DataObjectFormat.AppendChild(MimeType);
            // /////////////////////////////////////////////////////////////

            DataObject dataObject = new DataObject
            {
                Data = qualifyingPropertiesRoot.SelectNodes("."),
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
            XmlDsigXPathTransform XPathTransform = CreateXPathTransform("ValorPath");
            reference2.AddTransform(XPathTransform);
            reference2.DigestMethod = "http://www.w3.org/2001/04/xmlenc#sha256";
            reference2.AddTransform(new XmlDsigExcC14NTransform());
            signedXml.AddReference(reference2);

            //Reference 2
            reference2 = new Reference();
            // reference2.Id = "R2";
            reference2.Type = "http://uri.etsi.org/01903#SignedProperties";
            reference2.Uri = "";
            // reference2.AddTransform(new XmlDsigExcC14NTransform());
            XPathTransform = CreateXPathTransform("ValorPath");
            // reference2.AddTransform(XPathTransform);
            reference2.DigestMethod = "http://www.w3.org/2001/04/xmlenc#sha256";
            reference2.AddTransform(new XmlDsigExcC14NTransform());
            signedXml.AddReference(reference2);

            signedXml.ComputeSignature();

            // Get the XML representation of the signature and save
            // it to an XmlElement object.
            XmlElement xmlDigitalSignature = signedXml.GetXml();

            xmlDoc.DocumentElement.AppendChild(xmlDoc.ImportNode(xmlDigitalSignature, true));

            bool checkSign = signedXml.CheckSignature();

            //return xmlDoc.OuterXml;
            return xmlDoc;

        }
        private static XmlDsigXPathTransform CreateXPathTransform(string XPathString)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement xPathElem = doc.CreateElement("XPath");
            xPathElem.InnerText = XPathString;
            XmlDsigXPathTransform xForm = new XmlDsigXPathTransform();
            xForm.LoadInnerXml(xPathElem.SelectNodes("."));
            return xForm;
        }


        #endregion



    }
}
