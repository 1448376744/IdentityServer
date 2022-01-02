﻿using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using IdentityModel;
using IdentityServer.Configuration;
using IdentityServer.Storage;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Application
{
    internal class DefaultTokenCreationService
        : ITokenCreationService
    {
        private readonly ISystemClock _clock;
        private readonly ILogger _logger;
        private readonly IdentityServerOptions _options;
        private readonly ISigningCredentialStore _credentials;

        public DefaultTokenCreationService(
            ISystemClock clock,
            ISigningCredentialStore credentials,
            IdentityServerOptions options,
            ILogger<DefaultTokenCreationService> logger)
        {
            _clock = clock;
            _credentials = credentials;
            _options = options;
            _logger = logger;
        }

        public async Task<string> CreateTokenAsync(Token token)
        {
            var header = await CreateHeaderAsync(token);
            var payload = await CreatePayloadAsync(token);
            var jwt = new JwtSecurityToken(header, payload);
            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(jwt);
        }

        private async Task<JwtHeader> CreateHeaderAsync(Token token)
        {
            var credential = await _credentials.GetSigningCredentialsAsync(token.AllowedSigningAlgorithms);

            if (credential == null)
            {
                throw new InvalidOperationException("No signing credential is configured. Can't create JWT token");
            }

            var header = new JwtHeader(credential);

            // emit x5t claim for backwards compatibility with v4 of MS JWT library
            if (credential.Key is X509SecurityKey x509Key)
            {
                var cert = x509Key.Certificate;
                if (_clock.UtcNow.UtcDateTime > cert.NotAfter)
                {
                    _logger.LogWarning("Certificate {subjectName} has expired on {expiration}", cert.Subject, cert.NotAfter.ToString(CultureInfo.InvariantCulture));
                }

                header["x5t"] = Base64Url.Encode(cert.GetCertHash());
            }

            if (token.Type == OidcConstants.TokenTypes.AccessToken)
            {
                if (!string.IsNullOrWhiteSpace(_options.AccessTokenJwtType))
                {
                    header["typ"] = _options.AccessTokenJwtType;
                }
            }

            return header;
        }
    
        private Task<JwtPayload> CreatePayloadAsync(Token token)
        {
            var notbefore = _clock.UtcNow.UtcDateTime;
            var expires = _clock.UtcNow.UtcDateTime
                .AddSeconds(token.Lifetime);

            var payload = new JwtPayload(
                token.Issuer,
                null,
                null,
                notbefore,
                expires);

            foreach (var aud in token.Audiences)
            {
                payload.AddClaim(new Claim(JwtClaimTypes.Audience, aud));
            }

            return Task.FromResult(payload);
        }
    }
}
