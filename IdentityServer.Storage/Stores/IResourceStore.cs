﻿using IdentityServer.Models;

namespace IdentityServer.Storage
{
    public interface IResourceStore
    {
        Task<IEnumerable<string>> GetShowInDiscoveryDocumentScopesAsync();
        Task<ResourceCollection> GetResourceByScopesAsync(IEnumerable<string> scopes);
    }
}
