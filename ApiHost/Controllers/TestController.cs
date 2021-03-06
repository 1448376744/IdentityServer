using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiHost.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        [Authorize]
        public async Task<object> GetAsync()
        {
            var client = new HttpClient();

            var disco = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest 
            {
                Address= "https://localhost:7150",
                Policy=new DiscoveryPolicy 
                {
                    ValidateIssuerName =false,
                }
            });
            if (disco.IsError)
                throw new Exception(disco.Error);
            return disco.Raw;
        }
    }
}