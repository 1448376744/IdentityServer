﻿using System.Security.Claims;

namespace IdentityServer.Validation
{
    public interface IScopeParser
    {
        Task<IEnumerable<string>> ParseAsync(ClaimsPrincipal subject);
    }
}