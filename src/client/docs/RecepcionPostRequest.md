# Tribunet.Atv.ApiClient.Model.RecepcionPostRequest

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Clave** | **string** |  | 
**Fecha** | **string** | Fecha de la factura en formato [yyyy-MM-dd&#39;T&#39;HH:mm:ssZ] como se define en [http://tools.ietf.org/html/rfc3339#section-5.6] (date-time). | 
**Emisor** | [**RecepcionPostRequestEmisor**](RecepcionPostRequestEmisor.md) |  | 
**Receptor** | [**RecepcionPostRequestEmisor**](RecepcionPostRequestEmisor.md) |  | [optional] 
**CallbackUrl** | **string** | URL utilizado para que Hacienda envíe la respuesta de aceptación o rechazo, se va a enviar un mensaje JSON, igual al que se define en recepcionGetItem, por medio de un canal HTTP/HTTPS utilizando POST. | [optional] 
**ConsecutivoReceptor** | **string** | Numeración consecutiva de los mensajes de confirmación. Este atributo es obligatorio en caso de ser un mensaje de confirmación del receptor. | [optional] 
**ComprobanteXml** | **string** | Comprobante electrónico XML firmado por el obligado tributario utilizando XAdES-EPES. El texto del XML debe convertirse a un byte array y codificarse en Base64. El mapa de caracteres a utilizar en el XML y en la codificación Base64 es UTF8. | 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

