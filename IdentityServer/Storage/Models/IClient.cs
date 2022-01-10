﻿namespace IdentityServer.Models
{
    public interface IClient
    {
        string ClientId { get; }
        string ClientSecret { get; }
        string ClientName { get; }
        string Description { get; }
        string ClientUri { get; }
        bool Enabled { get; }
        bool IncludeJwtId { get; }
        int? AccessTokenLifetime { get; }
        int IdentityTokenLifetime { get; }
        AccessTokenType AccessTokenType { get; }
        IReadOnlyCollection<ISecret> ClientSecrets { get; }
        IReadOnlyCollection<string> AllowedGrantTypes { get; }
        IReadOnlyCollection<string> AllowedSigningAlgorithms { get; }
    }
}
