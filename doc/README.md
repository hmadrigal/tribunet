# Administración Tributaria Virtual

La administracion Tributaria Virtual o ATV, es parte del esfuerzo del Ministerio de Hacienda de Costa Rica para digitalizar el proceso de generar facturas en CR. 

ATV permite enviar `comprobante electronicos` por medio de su Web API. Los `comprobante electronicos` son documentos XML, que deben ser firmados utilizando certificados digitales.

Para la implementacion de un cliente, requiere la habilidad de crear documentos XML validos, luego firmarlos utilizando un certificado digital. Una vez listo, el documento firmado debe enviarse ha ha hacienda utilizando eu Web API.

Los `comprobante electronicos` pueden ser de diferene diferente tipos:
- Mensaje Hacienda
- Factura electronica
- Mensaje Receptor
- Nota de Credito
- Nota de Debito
- Tiquete Electronico

Asi mismo estos documentos tienen diferentes versiones. En la pagina [Estructuras XML para Facturación Electrónica](https://tribunet.hacienda.go.cr/FormatosYEstructurasXML.jsp) o en [Anexos y Estructuras](https://www.hacienda.go.cr/ATV/ComprobanteElectronico/frmAnexosyEstructuras.aspx#) es posible ver las versiones de documentos.


El Web API esta basado en JSON  y utiliza como autenticacion OAuth 2.0 usando Client Credentials. 

## Comprobante Electronicos


Auto Generated Code
### Generate Classes based on XSD files
xsd .\Resources\FacturaElectronica_V.4.2.xsd  /classes /l:CS /n:Tribunet.Atv.Models.FacturaElectronica_V_4_2 /o:.\Models\FacturaElectronica_V_4_2
xsd .\Resources\MensajeHacienda_V4.2.xsd  /classes /l:CS /n:Tribunet.Atv.Models.MensajeHacienda_V4_2 /o:.\Models\MensajeHacienda_V4_2
xsd .\Resources\MensajeReceptor_4.2.xsd  /classes /l:CS /n:Tribunet.Atv.Models.MensajeReceptor_4_2 /o:.\Models\MensajeReceptor_4_2
xsd .\Resources\NotaCreditoElectronica_V4.2.xsd  /classes /l:CS /n:Tribunet.Atv.Models.NotaCreditoElectronica_V4_2 /o:.\Models\NotaCreditoElectronica_V4_2
xsd .\Resources\NotaDebitoElectronica_V4.2.xsd  /classes /l:CS /n:Tribunet.Atv.Models.NotaDebitoElectronica_V4_2 /o:.\Models\NotaDebitoElectronica_V4_2
xsd .\Resources\TiqueteElectronico_V4.2.xsd  /classes /l:CS /n:Tribunet.Atv.Models.TiqueteElectronico_V4_2 /o:.\Models\TiqueteElectronico_V4_2

### Generando API Client based on OpenAPI document

#### `csharp-netcore`
docker run --rm -v D:\projects\github\tribunet\src\Tribunet.Atv:/local -v D:\projects\github\tribunet\doc:/docs -v D:\projects\github\tribunet\src:/src openapitools/openapi-generator-cli generate -i /docs/atv-1.0.0-openapi-3.0.1.yml -g csharp-netcore -o /src/client/ --package-name Tribunet.Atv.ApiClient --additional-properties=netCoreProjectFile=true,nullableReferenceTypes=true,targetFramework=netstandard2.1
#### `csharp`
docker run --rm -v D:\projects\github\tribunet\src\Tribunet.Atv:/local -v D:\projects\github\tribunet\doc:/docs -v D:\projects\github\tribunet\src:/src openapitools/openapi-generator-cli generate -i /docs/atv-1.0.0-openapi-3.0.1.yml -g csharp-netcore -o /src/client/ --package-name Tribunet.Atv.ApiClient --additional-properties=netCoreProjectFile=true,targetFramework=netstandard2.1

## References

- Herramientas
  - [openapi-generator-cli help](https://openapi-generator.tech/docs/usage/)
  - [FirmaXadesNetCore GitHub](https://github.com/newverdun/FirmaXadesNetCore)
  - [FirmaXadesNetCore Nuget](https://www.nuget.org/packages/FirmaXadesNetCore/ )
  - [Create XAdES-EPES Factura Electrónica Signature](https://www.example-code.com/csharp/xades_epes_factura_electronica_cr.asp)
  - [Xades.NetCore](https://github.com/pgiacomo69/Xades.NetCore)
  - [openapi-generator](https://openapi-generator.tech/)
  - [Swagger Editor](https://editor.swagger.io)
  - [OpenAPI.Tools](https://openapi.tools/)
  - [JSON to YAML](https://onlineyamltools.com/convert-json-to-yaml)
  - [Visual XSD](http://visualxsd.com)
  - [Microsoft XSD](https://docs.microsoft.com/en-us/dotnet/standard/serialization/xml-schema-def-tool-gen)
  - [mermaid](https://mermaid-js.github.io)
  - [mermaid live editor](https://mermaid.live/edit#eyJjb2RlIjoiZ3JhcGggVERcbiAgICBBW0NocmlzdG1hc10gLS0-fEdldCBtb25leXwgQihHbyBzaG9wcGluZylcbiAgICBCIC0tPiBDe0xldCBtZSB0aGlua31cbiAgICBDIC0tPnxPbmV8IERbTGFwdG9wXVxuICAgIEMgLS0-fFR3b3wgRVtpUGhvbmVdXG4gICAgQyAtLT58VGhyZWV8IEZbZmE6ZmEtY2FyIENhcl1cbiAgIiwibWVybWFpZCI6IntcbiAgXCJ0aGVtZVwiOiBcImRhcmtcIlxufSIsInVwZGF0ZUVkaXRvciI6dHJ1ZSwiYXV0b1N5bmMiOnRydWUsInVwZGF0ZURpYWdyYW0iOnRydWV9)
- Documentacion en Hacienda de Costa Rica 
  - [NUEVAS FUNCIONALIDADES EN ATV PARA ELABORACIÓN DE COMPROBANTES ELECTRÓNICOS](https://www.hacienda.go.cr/contenido/14050-nuevas-funcionalidades-en-atv-para-elaboracion-de-comprobantes-electronicos)
  - [Anexos y Estructuras](https://www.hacienda.go.cr/ATV/ComprobanteElectronico/frmAnexosyEstructuras.aspx#)
  - [Estructuras XML para Facturación Electrónica](https://tribunet.hacienda.go.cr/FormatosYEstructurasXML.jsp#)