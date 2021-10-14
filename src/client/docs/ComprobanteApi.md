# Tribunet.Atv.ApiClient.Api.ComprobanteApi

All URIs are relative to *https://api.comprobanteselectronicos.go.cr/recepcion/v1*

Method | HTTP request | Description
------------- | ------------- | -------------
[**ComprobantesClaveGet**](ComprobanteApi.md#comprobantesclaveget) | **GET** /comprobantes/{clave} | 
[**ComprobantesGet**](ComprobanteApi.md#comprobantesget) | **GET** /comprobantes | 


<a name="comprobantesclaveget"></a>
# **ComprobantesClaveGet**
> Comprobante ComprobantesClaveGet (string clave)



Obtiene el comprobante indicado por la `clave`.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Tribunet.Atv.ApiClient.Api;
using Tribunet.Atv.ApiClient.Client;
using Tribunet.Atv.ApiClient.Model;

namespace Example
{
    public class ComprobantesClaveGetExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://api.comprobanteselectronicos.go.cr/recepcion/v1";
            // Configure OAuth2 access token for authorization: Produccion
            config.AccessToken = "YOUR_ACCESS_TOKEN";
            // Configure OAuth2 access token for authorization: Sandbox
            config.AccessToken = "YOUR_ACCESS_TOKEN";

            var apiInstance = new ComprobanteApi(config);
            var clave = clave_example;  // string | 

            try
            {
                Comprobante result = apiInstance.ComprobantesClaveGet(clave);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ComprobanteApi.ComprobantesClaveGet: " + e.Message );
                Debug.Print("Status Code: "+ e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **clave** | **string**|  | 

### Return type

[**Comprobante**](Comprobante.md)

### Authorization

[Produccion](../README.md#Produccion), [Sandbox](../README.md#Sandbox)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Respuesta correcta completa. |  * X-Ratelimit-Limit - Número de requests permitidos en el período de tiempo actual. <br>  * X-Ratelimit-Remaining - Número de requests restantes en el período de tiempo actual. <br>  * X-Ratelimit-Reset - Tiempo restante antes de que los límites se reinicien (UTC Epoch Seconds). <br>  |
| **404** | Ocurrió un error, no se encuentra el recurso. |  * X-Error-Cause - Muestra la causa del error. <br>  * X-Ratelimit-Limit - Número de requests permitidos en el período de tiempo actual. <br>  * X-Ratelimit-Remaining - Número de requests restantes en el período de tiempo actual. <br>  * X-Ratelimit-Reset - Tiempo restante antes de que los límites se reinicien (UTC Epoch Seconds). <br>  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="comprobantesget"></a>
# **ComprobantesGet**
> List&lt;Comprobante&gt; ComprobantesGet (int? offset = null, int? limit = null, int? emisor = null, string? receptor = null)



Obtiene un resumen de todos los comprobantes electrónicos que ha enviado el obligado tributario ordenado de forma descendente por la fecha.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Tribunet.Atv.ApiClient.Api;
using Tribunet.Atv.ApiClient.Client;
using Tribunet.Atv.ApiClient.Model;

namespace Example
{
    public class ComprobantesGetExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://api.comprobanteselectronicos.go.cr/recepcion/v1";
            // Configure OAuth2 access token for authorization: Produccion
            config.AccessToken = "YOUR_ACCESS_TOKEN";
            // Configure OAuth2 access token for authorization: Sandbox
            config.AccessToken = "YOUR_ACCESS_TOKEN";

            var apiInstance = new ComprobanteApi(config);
            var offset = 10;  // int? | A partir de que posición contar los items a retornar (optional) 
            var limit = 56;  // int? | Cantidad de items a retornar apartir del offset (optional)  (default to 50)
            var emisor = 2003101123456;  // int? | Tipo y número de identificación del emisor. (optional) 
            var receptor = 02003101123456;  // string? | Tipo y número de identificación del receptor. (optional) 

            try
            {
                List<Comprobante> result = apiInstance.ComprobantesGet(offset, limit, emisor, receptor);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ComprobanteApi.ComprobantesGet: " + e.Message );
                Debug.Print("Status Code: "+ e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **offset** | **int?**| A partir de que posición contar los items a retornar | [optional] 
 **limit** | **int?**| Cantidad de items a retornar apartir del offset | [optional] [default to 50]
 **emisor** | **int?**| Tipo y número de identificación del emisor. | [optional] 
 **receptor** | **string?**| Tipo y número de identificación del receptor. | [optional] 

### Return type

[**List&lt;Comprobante&gt;**](Comprobante.md)

### Authorization

[Produccion](../README.md#Produccion), [Sandbox](../README.md#Sandbox)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Respuesta correcta completa. |  * Accept-Range - Muestra la cantidad máxima de items que se le puede solicitar al recurso. <br>  * X-Ratelimit-Limit - Número de requests permitidos en el período de tiempo actual. <br>  * X-Ratelimit-Remaining - Número de requests restantes en el período de tiempo actual. <br>  * X-Ratelimit-Reset - Tiempo restante antes de que los límites se reinicien (UTC Epoch Seconds). <br>  |
| **206** | Respuesta correcta parcial debido al uso de &#x60;offset&#x60; y/o &#x60;limit&#x60;. |  * Accept-Range - Muestra la cantidad máxima de items que se le puede solicitar al recurso. <br>  * Content-Range - Muestra la información del &#x60;offset-limit&#x60;/&#x60;count_all&#x60; <br>  * X-Ratelimit-Limit - Número de requests permitidos en el período de tiempo actual. <br>  * X-Ratelimit-Remaining - Número de requests restantes en el período de tiempo actual. <br>  * X-Ratelimit-Reset - Tiempo restante antes de que los límites se reinicien (UTC Epoch Seconds). <br>  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

