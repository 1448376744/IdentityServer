﻿using IdentityServer.Models;

namespace IdentityServer.Storage
{
    internal class InMemoryClientStore : IClientStore
    {
        private readonly IEnumerable<IClient> _clients;

        public InMemoryClientStore(IEnumerable<IClient> clients)
        {
            _clients = clients;
        }

        public Task<IClient?> FindClientByIdAsync(string clientId)
        {
            var client = _clients
                .Where(a => a.ClientId == clientId)
                .FirstOrDefault();
            return Task.FromResult(client);
        }
    }
}
