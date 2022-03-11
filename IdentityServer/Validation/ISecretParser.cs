﻿using Microsoft.AspNetCore.Http;

namespace IdentityServer.Validation
{
    public interface ISecretParser
    {
        string AuthenticationMethod { get; }
        Task<ParsedCredentials> ParseAsync(HttpContext context);
    }
}
