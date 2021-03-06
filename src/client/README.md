# Tribunet.Atv.ApiClient - the C# library for the Atv Api

API de comprobantes electrónicos para la administración tributaria virtual

This C# SDK is automatically generated by the [OpenAPI Generator](https://openapi-generator.tech) project:

- API version: 1.0.0
- SDK version: 1.0.0
- Build package: org.openapitools.codegen.languages.CSharpNetCoreClientCodegen
    For more information, please visit [https://www.hacienda.go.cr/](https://www.hacienda.go.cr/)

<a name="frameworks-supported"></a>
## Frameworks supported
- .NET Core >=1.0
- .NET Framework >=4.6
- Mono/Xamarin >=vNext

<a name="dependencies"></a>
## Dependencies

- [RestSharp](https://www.nuget.org/packages/RestSharp) - 106.11.7 or later
- [Json.NET](https://www.nuget.org/packages/Newtonsoft.Json/) - 12.0.3 or later
- [JsonSubTypes](https://www.nuget.org/packages/JsonSubTypes/) - 1.8.0 or later
- [System.ComponentModel.Annotations](https://www.nuget.org/packages/System.ComponentModel.Annotations) - 5.0.0 or later

The DLLs included in the package may not be the latest version. We recommend using [NuGet](https://docs.nuget.org/consume/installing-nuget) to obtain the latest version of the packages:
```
Install-Package RestSharp
Install-Package Newtonsoft.Json
Install-Package JsonSubTypes
Install-Package System.ComponentModel.Annotations
```

NOTE: RestSharp versions greater than 105.1.0 have a bug which causes file uploads to fail. See [RestSharp#742](https://github.com/restsharp/RestSharp/issues/742).
NOTE: RestSharp for .Net Core creates a new socket for each api call, which can lead to a socket exhaustion problem. See [RestSharp#1406](https://github.com/restsharp/RestSharp/issues/1406).

<a name="installation"></a>
## Installation
Generate the DLL using your preferred tool (e.g. `dotnet build`)

Then include the DLL (under the `bin` folder) in the C# project, and use the namespaces:
```csharp
using Tribunet.Atv.ApiClient.Api;
using Tribunet.Atv.ApiClient.Client;
using Tribunet.Atv.ApiClient.Model;
```
<a name="usage"></a>
## Usage

To use the API client with a HTTP proxy, setup a `System.Net.WebProxy`
```csharp
Configuration c = new Configuration();
System.Net.WebProxy webProxy = new System.Net.WebProxy("http://myProxyUrl:80/");
webProxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
c.Proxy = webProxy;
```

<a name="getting-started"></a>
## Getting Started

```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Tribunet.Atv.ApiClient.Api;
using Tribunet.Atv.ApiClient.Client;
using Tribunet.Atv.ApiClient.Model;

namespace Example
{
    public class Example
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
            catch (ApiException e)
            {
                Debug.Print("Exception when calling ComprobanteApi.ComprobantesClaveGet: " + e.Message );
                Debug.Print("Status Code: "+ e.ErrorCode);
                Debug.Print(e.StackTrace);
            }

        }
    }
}
```

<a name="documentation-for-api-endpoints"></a>
## Documentation for API Endpoints

All URIs are relative to *https://api.comprobanteselectronicos.go.cr/recepcion/v1*

Class | Method | HTTP request | Description
------------ | ------------- | ------------- | -------------
*ComprobanteApi* | [**ComprobantesClaveGet**](docs/ComprobanteApi.md#comprobantesclaveget) | **GET** /comprobantes/{clave} | 
*ComprobanteApi* | [**ComprobantesGet**](docs/ComprobanteApi.md#comprobantesget) | **GET** /comprobantes | 
*RecepcionApi* | [**GetReception**](docs/RecepcionApi.md#getreception) | **GET** /recepcion/{clave} | Obtiene el estado del comprobante indicado por la `clave`
*RecepcionApi* | [**PostReception**](docs/RecepcionApi.md#postreception) | **POST** /recepcion | Recibe el comprobante electrónico o respuesta del receptor.


<a name="documentation-for-models"></a>
## Documentation for Models

 - [Model.Comprobante](docs/Comprobante.md)
 - [Model.ComprobanteEmisor](docs/ComprobanteEmisor.md)
 - [Model.ComprobanteNotasCredito](docs/ComprobanteNotasCredito.md)
 - [Model.RecepcionGetResponse](docs/RecepcionGetResponse.md)
 - [Model.RecepcionPostRequest](docs/RecepcionPostRequest.md)
 - [Model.RecepcionPostRequestEmisor](docs/RecepcionPostRequestEmisor.md)


<a name="documentation-for-authorization"></a>
## Documentation for Authorization

<a name="Produccion"></a>
### Produccion

- **Type**: OAuth
- **Flow**: password
- **Authorization URL**: 
- **Scopes**: N/A

<a name="Sandbox"></a>
### Sandbox

- **Type**: OAuth
- **Flow**: password
- **Authorization URL**: 
- **Scopes**: N/A

