﻿namespace IdentityServer.ResponseGenerators
{
    public class TokenResponseGenerator : ITokenResponseGenerator
    {
        private readonly ITokenService _tokenService;

        private readonly IRefreshTokenService _refreshTokenService;

        public TokenResponseGenerator(
            IServerUrl serverUrl,
            ITokenService tokenService,
            IRefreshTokenService refreshTokenService)
        {
            _tokenService = tokenService;
            _refreshTokenService = refreshTokenService;
        }

        public async Task<TokenResponse> ProcessAsync(TokenRequest request)
        {
            (string accessToken, string? refreshToken) = await CreateAccessTokenAsync(request);

            var scope = string.Join(",", request.Scopes);

            var response = new TokenResponse()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenLifetime = request.Client.AccessTokenLifetime,
                Scope = scope,
            };

            return response;
        }

        private async Task<(string accessToken, string? refreshToken)> CreateAccessTokenAsync(TokenRequest request)
        {
            var token = await _tokenService.CreateAccessTokenAsync(request);

            var accessToken = await _tokenService.CreateSecurityTokenAsync(token);
            if (request.Resources.OfflineAccess)
            {
                var refreshToken = await _refreshTokenService.CreateRefreshTokenAsync(token, request.Client.RefreshTokenLifetime);
                return (accessToken, refreshToken);
            }

            return (accessToken, null);
        }
    }
}