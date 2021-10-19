# Tribunet.Atv.ApiClient.Api.RecepcionApi

All URIs are relative to *https://api.comprobanteselectronicos.go.cr/recepcion/v1*

Method | HTTP request | Description
------------- | ------------- | -------------
[**GetReception**](RecepcionApi.md#getreception) | **GET** /recepcion/{clave} | Obtiene el estado del comprobante indicado por la &#x60;clave&#x60;
[**PostReception**](RecepcionApi.md#postreception) | **POST** /recepcion | Recibe el comprobante electrónico o respuesta del receptor.


<a name="getreception"></a>
# **GetReception**
> RecepcionGetResponse GetReception (string clave)

Obtiene el estado del comprobante indicado por la `clave`

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Tribunet.Atv.ApiClient.Api;
using Tribunet.Atv.ApiClient.Client;
using Tribunet.Atv.ApiClient.Model;

namespace Example
{
    public class GetReceptionExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://api.comprobanteselectronicos.go.cr/recepcion/v1";
            // Configure OAuth2 access token for authorization: Produccion
            config.AccessToken = "YOUR_ACCESS_TOKEN";
            // Configure OAuth2 access token for authorization: Sandbox
            config.AccessToken = "YOUR_ACCESS_TOKEN";

            var apiInstance = new RecepcionApi(config);
            var clave = clave_example;  // string | 

            try
            {
                // Obtiene el estado del comprobante indicado por la `clave`
                RecepcionGetResponse result = apiInstance.GetReception(clave);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling RecepcionApi.GetReception: " + e.Message );
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

[**RecepcionGetResponse**](RecepcionGetResponse.md)

### Authorization

[Produccion](../README.md#Produccion), [Sandbox](../README.md#Sandbox)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Respuesta correcta. |  * X-Ratelimit-Limit - Número de requests permitidos en el período de tiempo actual. <br>  * X-Ratelimit-Remaining - Número de requests restantes en el período de tiempo actual. <br>  * X-Ratelimit-Reset - Tiempo restante antes de que los límites se reinicien (UTC Epoch Seconds). <br>  |
| **404** | Ocurrió un error, no se encuentra el recurso. |  * X-Error-Cause - Muestra la causa del error. <br>  * X-Ratelimit-Limit - Número de requests permitidos en el período de tiempo actual. <br>  * X-Ratelimit-Remaining - Número de requests restantes en el período de tiempo actual. <br>  * X-Ratelimit-Reset - Tiempo restante antes de que los límites se reinicien (UTC Epoch Seconds). <br>  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postreception"></a>
# **PostReception**
> void PostReception (RecepcionPostRequest? recepcionPostRequest = null)

Recibe el comprobante electrónico o respuesta del receptor.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Tribunet.Atv.ApiClient.Api;
using Tribunet.Atv.ApiClient.Client;
using Tribunet.Atv.ApiClient.Model;

namespace Example
{
    public class PostReceptionExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://api.comprobanteselectronicos.go.cr/recepcion/v1";
            // Configure OAuth2 access token for authorization: Produccion
            config.AccessToken = "YOUR_ACCESS_TOKEN";
            // Configure OAuth2 access token for authorization: Sandbox
            config.AccessToken = "YOUR_ACCESS_TOKEN";

            var apiInstance = new RecepcionApi(config);
            var recepcionPostRequest = new RecepcionPostRequest?(); // RecepcionPostRequest? |  (optional) 

            try
            {
                // Recibe el comprobante electrónico o respuesta del receptor.
                apiInstance.PostReception(recepcionPostRequest);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling RecepcionApi.PostReception: " + e.Message );
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
 **recepcionPostRequest** | [**RecepcionPostRequest?**](RecepcionPostRequest?.md)|  | [optional] 

### Return type

void (empty response body)

### Authorization

[Produccion](../README.md#Produccion), [Sandbox](../README.md#Sandbox)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **201** | Se recibió correctamente el comprobante electrónico o respuesta del receptor, queda pendiente la validación del mismo y el envío de la respuesta de parte de Hacienda. |  * X-Ratelimit-Limit - Número de requests permitidos en el período de tiempo actual. <br>  * X-Ratelimit-Remaining - Número de requests restantes en el período de tiempo actual. <br>  * X-Ratelimit-Reset - Tiempo restante antes de que los límites se reinicien (UTC Epoch Seconds). <br>  |
| **400** | Ocurrió algún error de validación. |  * X-Error-Cause - Muestra la causa del error. <br>  * validation-exception - Indica si es un error con el &#x60;body&#x60; enviado del entity. <br>  * X-Ratelimit-Limit - Número de requests permitidos en el período de tiempo actual. <br>  * X-Ratelimit-Remaining - Número de requests restantes en el período de tiempo actual. <br>  * X-Ratelimit-Reset - Tiempo restante antes de que los límites se reinicien (UTC Epoch Seconds). <br>  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

