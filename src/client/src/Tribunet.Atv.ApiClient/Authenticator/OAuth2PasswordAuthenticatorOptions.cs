using System;

namespace Tribunet.Atv.ApiClient.Authenticator
{
    public class OAuth2PasswordAuthenticatorOptions
    {
        public string AccessTokenUrl { get; }
        public string RefreshTokenUrl { get; }
        public string Username { get; }
        public string Password { get; }
        public string ClientId { get; }
        public string ClientSecret { get; }
        public string Scope { get; }

        public OAuth2PasswordAuthenticatorOptions(
            string accessTokenUrl,
            string refreshTokenUrl,
            string username,
            string password,
            string clientId,
            string clientSecret = null,
            string scope = null
        )
        {
            _ = accessTokenUrl ?? throw new ArgumentNullException(nameof(accessTokenUrl));
            //_ = refreshTokenUrl ?? throw new ArgumentNullException(nameof(refreshTokenUrl));
            _ = username ?? throw new ArgumentNullException(nameof(username));
            _ = password ?? throw new ArgumentNullException(nameof(password));
            _ = clientId ?? throw new ArgumentNullException(nameof(clientId));

            if (!Uri.IsWellFormedUriString(accessTokenUrl, UriKind.Absolute))
                throw new ArgumentOutOfRangeException(nameof(accessTokenUrl));

            if (string.IsNullOrWhiteSpace(refreshTokenUrl) && !Uri.IsWellFormedUriString(refreshTokenUrl, UriKind.Absolute))
                throw new ArgumentOutOfRangeException(nameof(refreshTokenUrl));

            AccessTokenUrl = accessTokenUrl;
            RefreshTokenUrl = refreshTokenUrl ?? string.Empty;
            Username = username;
            Password = password;
            ClientId = clientId;
            ClientSecret = clientSecret ?? string.Empty;
            Scope = scope ?? string.Empty;
        }

    }
}