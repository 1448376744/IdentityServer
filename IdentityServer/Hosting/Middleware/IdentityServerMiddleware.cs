﻿using Microsoft.AspNetCore.Http;

namespace IdentityServer.Hosting
{
    internal class IdentityServerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IEndpointRouter _router;

        public IdentityServerMiddleware(
            RequestDelegate next,
            IEndpointRouter router)
        {
            _next = next;
            _router = router;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = _router.Find(context);
            if (endpoint != null)
            {
                await EndpointHandlerAsync(context, endpoint);
            }
            else
            {
                await _next(context);
            }
        }

        private static async Task EndpointHandlerAsync(HttpContext context, IEndpointHandler endpoint)
        {
            try
            {
                var result = await endpoint.ProcessAsync(context);
                await result.ExecuteAsync(context);
            }
            catch (InvalidException ex)
            {
                var result = new BadRequestResult(ex.Error, ex.ErrorDescription);
                await result.ExecuteAsync(context);
            }
        }
    }
}
