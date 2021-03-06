namespace IdentityServer.Endpoints
{
    public interface IDiscoveryResponseGenerator
    {
        Task<JwkDiscoveryGeneratorResponse> CreateJwkDiscoveryDocumentAsync();
        Task<DiscoveryGeneratorResponse> GetDiscoveryDocumentAsync(string issuer, string baseUrl);
    }
}
