using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using FirmaXadesNetCore;
using FirmaXadesNetCore.Signature.Parameters;
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
            var comprobanteStream = new MemoryStream();

            // ==========
            // create `comprobante electronico`
            var numeroConsecutivo = new NumeroConsecutivo(
                        tipoComprobante: TipoComprobante.FacturaElectronica,
                        oficinaId: 1,
                        puntoDeVentaId: 1,
                        consecutivoComprobante: Convert.ToInt32(DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                    );
            var claveNumerica = new ClaveNumerica(
                    codigoPais: 506,
                    fechaCreacion: localNow,
                    cedulaEmisor: "110910312",
                    numeracionConsecutivo: numeroConsecutivo,
                    situacion: SituacionDelComprobante.Normal,
                    codigoSeguridad: 1
                );

            ClaveNumerica cn = "50618102100310137332200100001010000002527105558162";
            claveNumerica = cn;
            NumeroConsecutivo nc = "00100001010000002527";
            numeroConsecutivo = nc;

            var comprobanteElectronico = new FacturaElectronica
            {
                //Clave = claveNumerica,
                Clave = cn,
                NumeroConsecutivo = numeroConsecutivo,
                FechaEmision = localNow,
                Emisor = new EmisorType
                {
                    Nombre = "Asociados Dentales Larios Guzman S.A",
                    NombreComercial = "LG DENTAL CARE. Cosmetic and Restorative Center",
                    Identificacion = new IdentificacionType
                    {
                        Tipo = IdentificacionTypeTipo.Item02,
                        Numero = "3101373322",

                    },
                    CorreoElectronico = "drlarios@lgdental.com",
                    Telefono = new TelefonoType
                    {
                        CodigoPais = "506",
                        NumTelefono = "22381229"
                    },
                    Ubicacion = new UbicacionType
                    {
                        Provincia = "4",
                        Canton = "01",
                        Distrito = "03",
                        Barrio = "02",
                        OtrasSenas = "250mts oeste Liceo Samuel Saenz Flores. San Jorge de Heredia"
                    }
                },
                Receptor = new ReceptorType
                {
                    Nombre = "HERBER FERNANDO MADRIGAL BENDLES",
                    Identificacion = new IdentificacionType
                    {
                        Tipo = IdentificacionTypeTipo.Item01,
                        Numero = "109970588"
                    },
                    CorreoElectronico = "nmadrigal@fua.net",
                    Telefono = new TelefonoType
                    {
                        CodigoPais = "506",
                        NumTelefono = "88837555 "
                    },
                    Ubicacion = new UbicacionType
                    {
                        Provincia = "2",
                        Canton = "03",
                        Distrito = "01",
                        OtrasSenas = "Grecia"
                    }
                },
                CondicionVenta = FacturaElectronicaCondicionVenta.Item01,
                MedioPago = new FacturaElectronicaMedioPago[] { FacturaElectronicaMedioPago.Item01 },
                DetalleServicio = new[]
                {
                    new FacturaElectronicaLineaDetalle
                    {
                        NumeroLinea = "1",
                        Codigo= new []{ new CodigoType{ Tipo = CodigoTypeTipo.Item01, Codigo = "07" } },
                        Cantidad = 1,
                        UnidadMedida = UnidadMedidaType.Sp,
                        Detalle = "Profilaxis/Raspado Utrasonico",
                        PrecioUnitario = 50000.00m,
                        SubTotal = 50000.00m,
                        MontoTotal = 50000.00m,
                        Impuesto = new[]
                        {
                            new ImpuestoType { Codigo = ImpuestoTypeCodigo.Item01, Tarifa = 4, Monto = 2000.00m },
                        },
                        MontoTotalLinea = 52000.00m,
                    },
                    new FacturaElectronicaLineaDetalle
                    {
                        NumeroLinea = "1",
                        Codigo= new []{ new CodigoType{ Tipo = CodigoTypeTipo.Item01, Codigo = "07" } },
                        Cantidad = 1,
                        UnidadMedida = UnidadMedidaType.Sp,
                        Detalle = "Profilaxis/Raspado Utrasonico",
                        PrecioUnitario = 50000.00m,
                        SubTotal = 50000.00m,
                        MontoTotal = 50000.00m,
                        Impuesto = new[]
                        {
                            new ImpuestoType { Codigo = ImpuestoTypeCodigo.Item01, Tarifa = 4, Monto = 2000.00m },
                        },
                        MontoTotalLinea = 52000.00m,
                    },
                },
                ResumenFactura = new FacturaElectronicaResumenFactura
                {
                    CodigoMoneda = FacturaElectronicaResumenFacturaCodigoMoneda.CRC,
                    TipoCambio = 1,
                    TotalServGravados = 100000.00m,
                    TotalGravado = 100000.00m,
                    TotalVenta = 100000.00m,
                    TotalVentaNeta = 100000.00m,
                    TotalImpuesto = 4000.00m,
                    TotalComprobante = 100000.00m,
                },
                Otros = new FacturaElectronicaOtros
                {
                    
                },
                Normativa = new FacturaElectronicaNormativa
                {
                    NumeroResolucion = "DGT-R-48-2016",
                    FechaResolucion = "07-10-2016 08:00:00",
                }
            };

            // ==========
            // serializes `comprobante electronico` into XML (represented by a Stream)
            var xmlSerializer = new XmlSerializer(typeof(FacturaElectronica));
            var textWriter = new StreamWriter(comprobanteStream);
            xmlSerializer.Serialize(textWriter, comprobanteElectronico);
            comprobanteStream.Flush();
            comprobanteStream.Seek(0, SeekOrigin.Begin);


            //// Gets XML Text
            //var encoding = System.Text.Encoding.UTF8;
            //var xmlSignedComprobante = encoding.GetString(signedComprobanteStream.ToArray());

            // ==========
            // Sign the XML
            var certificateFilePath = Environment.GetEnvironmentVariable("ATV_XADES_EPES_CERT_FILE_PATH");
            var certificateSubject = Environment.GetEnvironmentVariable("ATV_XADES_EPES_CERT_SUBJECT");
            var certificatePassword = Environment.GetEnvironmentVariable("ATV_XADES_EPES_CERT_PASSWORD");

            X509Certificate2 certificate = (!string.IsNullOrEmpty(certificateFilePath) && File.Exists(certificateFilePath))
                ? GetCertificateFromFilePath(certificateFilePath, certificatePassword)
                : GetCertificateStore(certificateSubject);

            XadesService xadesService = new XadesService();
            SignatureParameters signatureParameters = new SignatureParameters();
            signatureParameters.SignaturePolicyInfo = new SignaturePolicyInfo();
            signatureParameters.SignaturePolicyInfo.PolicyIdentifier =
                "https://tribunet.hacienda.go.cr/docs/esquemas/2016/v4.1/Resolucion_Comprobantes_Electronicos_DGT-R-48-2016.pdf";
            signatureParameters.SignaturePolicyInfo.PolicyHash = "Ohixl6upD6av8N7pEvDABhEL6hM=";
            signatureParameters.SignaturePackaging = SignaturePackaging.ENVELOPED;
            signatureParameters.DataFormat = new DataFormat();
            signatureParameters.Signer = new FirmaXadesNetCore.Crypto.Signer(certificate);
            FirmaXadesNetCore.Signature.SignatureDocument signatureDocument = xadesService.Sign(comprobanteStream, signatureParameters);
            var signedComprobanteStream = new MemoryStream();
            signatureDocument.Save(signedComprobanteStream);
            signedComprobanteStream.Seek(0, SeekOrigin.Begin);

            // ==========
            // validates the XML against XSD
            var xsdValidationResults = Enum.GetNames(typeof(XmlSeverityType)).ToDictionary(n => n, _ => new HashSet<string>(), StringComparer.InvariantCultureIgnoreCase);
            var xmlResourceAssembly = typeof(ModelDataProvider).Assembly;

            void ValidationCallBack(object sender, ValidationEventArgs args)
                => xsdValidationResults[args.Severity.ToString()].Add(args.Message);

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
            var textReader = new System.IO.StreamReader(signedComprobanteStream);
            XmlReader reader = XmlReader.Create(textReader, xmlSettings);

            // Parse the file
            while (reader.Read()) { }
            xmlSettings.ValidationEventHandler -= ValidationCallBack;
            comprobanteStream.Seek(0, SeekOrigin.Begin);
            signedComprobanteStream.Seek(0, SeekOrigin.Begin);

            var hasXsdErrors = xsdValidationResults.Any(kvp => nameof(XmlSeverityType.Error) == kvp.Key && kvp.Value.Count > 0);
            if (hasXsdErrors)
            {
                foreach (var xsdValidationResult in xsdValidationResults)
                    foreach (var message in xsdValidationResult.Value)
                        Debug.WriteLine($"[{xsdValidationResult.Key}] {message}");
                return;
            }

            

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
                comprobanteXml: Convert.ToBase64String(signedComprobanteStream.ToArray())
            );

            var recepcionApiClient = new RecepcionApi(config);
            try
            {
                // Recibe el comprobante electrónico o respuesta del receptor.
                //await recepcionApiClient.PostReceptionAsync(recepcionPostRequest);
                var response = await recepcionApiClient.PostReceptionWithHttpInfoAsync(recepcionPostRequest);
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

    }
}
