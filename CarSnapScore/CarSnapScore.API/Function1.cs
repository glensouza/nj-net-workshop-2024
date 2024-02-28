using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CarSnapScore.API
{
    public class Function1
    {
        private readonly ILogger<Function1> logger;

        public Function1(ILogger<Function1> logger)
        {
            this.logger = logger;
        }

        [Function("Function1")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            this.logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
