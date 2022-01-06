﻿using IdentityModel;
using IdentityServer.Configuration;
using IdentityServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer.Application
{
    internal class TokenRequestValidator
        : ITokenRequestValidator
    {
        private readonly IdentityServerOptions _options;
        private readonly IServiceProvider _services;

        public TokenRequestValidator(
            IdentityServerOptions options,
            IServiceProvider services)
        {
            _options = options;
            _services = services;
        }

        public async Task<ValidationResult> ValidateAsync(IClient client, HttpContext content)
        {
            var parameters = await content.Request.ReadFormAsNameValueCollectionAsync();
            var grantType = parameters.Get(OidcConstants.TokenRequest.GrantType);
            if (string.IsNullOrWhiteSpace(grantType))
            {
                return ValidationResult.Error("Grant type is missing");
            }
            if (grantType.Length > _options.InputLengthRestrictions.GrantType)
            {
                return ValidationResult.Error("Grant type is too long");
            }
            if (!client.AllowedGrantTypes.Contains(grantType))
            {
                return ValidationResult.Error("Client not authorized for client credentials flow, check the AllowedGrantTypes setting");
            }
            ValidationResult validationResult;
            if (grantType == GrantType.ClientCredentials)
            {
                var grantRequest = new ClientCredentialsGrantValidationRequest(client);
                var grantValidator = _services.GetRequiredService<IClientCredentialsGrantValidator>();
                validationResult = await grantValidator.ValidateAsync(grantRequest);
            }
            else if (grantType == GrantType.ResourceOwnerPassword)
            {
                var username = parameters.Get(OidcConstants.TokenRequest.UserName);
                if (string.IsNullOrWhiteSpace(username))
                {
                    return ValidationResult.Error("'username' is required");
                }
                var password = parameters.Get(OidcConstants.TokenRequest.Password);
                if (string.IsNullOrWhiteSpace(password))
                {
                    return ValidationResult.Error("'password' is required");
                }
                if (username.Length > _options.InputLengthRestrictions.UserName ||
                    password.Length > _options.InputLengthRestrictions.Password)
                {
                    return ValidationResult.Error(OidcConstants.TokenErrors.InvalidGrant);
                }
                var grantRequest = new ResourceOwnerPasswordGrantRequest(client, username, password);
                var grantValidator = _services.GetRequiredService<IResourceOwnerPasswordGrantValidator>();
                validationResult = await grantValidator.ValidateAsync(grantRequest);
            }
            else
            {
                var grantRequest = new ExtensionGrantRequest(client);
                var grantValidator = _services.GetRequiredService<IExtensionGrantValidator>();
                validationResult = await grantValidator.ValidateAsync(grantRequest);
            }
            if (validationResult.IsError)
            {
                return validationResult;
            }
            return ValidationResult.Success();
        }
    }
}
