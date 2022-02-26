﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Services
{
    internal class RefreshTokenService : IRefreshTokenService
    {
        private readonly ISystemClock _clock;
        private readonly IIdGenerator _idGenerator;
        private readonly IRefreshTokenStore _refreshTokenStore;

        public RefreshTokenService(
            ISystemClock clock,
            IIdGenerator idGenerator,
            IRefreshTokenStore refreshTokenStore)
        {
            _clock = clock;
            _idGenerator = idGenerator;
            _refreshTokenStore = refreshTokenStore;
        }

        public async Task<string> CreateRefreshTokenAsync(AccessToken token, int lifetime)
        {
            var id = _idGenerator.GeneratorId();
            var nowTime = _clock.UtcNow.UtcDateTime;
            var refreshToken = new RefreshToken(
                id, 
                token,
                lifetime,
                nowTime);
            await _refreshTokenStore.AddAsync(refreshToken);
            return Base64UrlEncoder.Encode(id);
        }

        public async Task DeleteRefreshTokenAsync(RefreshToken refreshToken)
        {
            await _refreshTokenStore.RevomeAsync(refreshToken);
        }

        public async Task<RefreshToken?> GetRefreshTokenAsync(string id)
        {
            id = Base64UrlEncoder.Decode(id);
            return await _refreshTokenStore.FindByIdAsync(id);
        }
    }
}
