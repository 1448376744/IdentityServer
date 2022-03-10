﻿namespace IdentityServer.Validation
{
    internal class ClientCredentialsGrantValidator : IClientCredentialsGrantValidator
    {
        public Task ValidateAsync(ClientGrantValidationRequest context)
        {
            var resources = context.Request.Resources;
            if (resources.IdentityResources.Any())
            {
                throw new ValidationException(OpenIdConnectErrors.InvalidGrant, "Client cannot request OpenID scopes in client credentials flow");
            }
            return Task.CompletedTask;
        }
    }
}
