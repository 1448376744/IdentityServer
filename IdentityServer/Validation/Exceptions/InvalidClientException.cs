﻿namespace IdentityServer.Validation
{
    public class InvalidClientException : InvalidException
    {
        public InvalidClientException(string errorDescription)
            : base(OpenIdConnectTokenErrors.InvalidClient, errorDescription)
        {

        }
    }
}