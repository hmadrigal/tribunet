# Tribunet.Atv.ApiClient.Model.RecepcionGetResponse

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Clave** | **string** |  | 
**Fecha** | **string** | Fecha de la factura en formato [yyyy-MM-dd&#39;T&#39;HH:mm:ssZ] como se define en [http://tools.ietf.org/html/rfc3339#section-5.6] (date-time). | 
**IndEstado** | **string** |  | 
**CallbackUrl** | **string** | URL utilizado para que Hacienda envíe la respuesta de aceptación o rechazo. Muestra el URL que fue enviado por el obligado tributario. | [optional] 
**RespuestaXml** | **string** | Respuesta de aceptación o rechazo en XML firmada por el Ministerio de Hacienda utilizando XAdES-XL. El texto del XML debe convertirse a un byte array y codificarse en Base64. El mapa de caracteres a utilizar en el XML y en la codificación Base64 es UTF8. | [optional] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

