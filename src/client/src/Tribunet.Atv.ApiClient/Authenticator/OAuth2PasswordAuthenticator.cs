using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;

namespace Tribunet.Atv.ApiClient.Authenticator
{
    public class OAuth2PasswordAuthenticator : IAuthenticator
    {
        private readonly OAuth2PasswordAuthenticatorOptions _options;
        private Token _currentToken;

        public OAuth2PasswordAuthenticator(OAuth2PasswordAuthenticatorOptions options)
        {
            _options = options;
            _currentToken = null;
        }

        public void Authenticate(IRestClient client, IRestRequest request)
        {

            if (_currentToken == null)
            {
                // TODO: Try to load token from storage into _currentToken. If not available, _currentToken should be null.
            }

            if (_currentToken == null)
            {
                var idpClient = new RestClient(_options.AccessTokenUrl);
                var idpRequest = new RestRequest(Method.POST);
                idpRequest.AddHeader("cache-control", "no-cache");
                idpRequest.AddHeader("content-type", "application/x-www-form-urlencoded");
                idpRequest.AddParameter("application/x-www-form-urlencoded",
                    $"grant_type=password&username={_options.Username}&password={_options.Password}&client_id={_options.ClientId}&client_secret={_options.ClientSecret}",
                    ParameterType.RequestBody);

                IRestResponse idpResponse = idpClient.Execute(idpRequest);
                if (idpResponse.IsSuccessful)
                {
                    _currentToken = JsonConvert.DeserializeObject<Token>(idpResponse.Content);
                    // TODO: Save token to storage
                }
            }

            
            if (HasExpired(_currentToken))
            {
                // TODO: Retrieve token from Refresh Token Url

                // TODO: Save token to storage
            }

            request.AddOrUpdateParameter(GetAuthenticationParameter(_currentToken.AccessToken));
        }

        private bool HasExpired(Token currentToken)
        {
            // TODO: Compute whether or not the current token has expired
            return false;
        }


        protected Parameter GetAuthenticationParameter(string accessToken)
            => new Parameter("Authorization", $"{_currentToken.TokenType} {accessToken}", ParameterType.HttpHeader);

    }
}
