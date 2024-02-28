using CarSnapScore.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarSnapScore.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NameGeneratorController : ControllerBase
    {
        private readonly ILogger<NameGeneratorController> logger;
        private readonly NameGenerator nameGenerator;

        public NameGeneratorController(ILogger<NameGeneratorController> logger, NameGenerator nameGenerator)
        {
            this.logger = logger;
            this.nameGenerator = nameGenerator;
        }

        [HttpGet(Name = "GetCarName")]
        public string Get()
        {
            this.logger.LogInformation("Getting Car Name");
            string carName = this.nameGenerator.GetRandomCarName();
            this.logger.LogInformation("Generated car name: {0}", carName);
            return carName;
        }
    }
}
