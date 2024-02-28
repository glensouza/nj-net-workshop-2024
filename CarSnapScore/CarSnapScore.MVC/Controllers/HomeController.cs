using CarSnapScore.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CarSnapScore.Services6;

namespace CarSnapScore.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly NameGenerator nameGenerator;
        private readonly CarDoesNotExist carDoesNotExist;

        public HomeController(ILogger<HomeController> logger, 
            NameGenerator nameGenerator, 
            CarDoesNotExist carDoesNotExist)
        {
            this.logger = logger;
            this.nameGenerator = nameGenerator;
            this.carDoesNotExist = carDoesNotExist;
        }

        public async Task<IActionResult> Index()
        {
            this.logger.LogInformation("I'm inside the Home Controller, Index Action");
            
            string carName = this.nameGenerator.GetRandomCarName();
            this.ViewData["CarName"] = carName;
            this.logger.LogInformation("Got Car Name: {0}", carName);

            string carImage = await new CarDoesNotExist().GetPicture();
            this.ViewData["CarImage"] = carImage;
            this.logger.LogInformation("Got Car Image: {0}", carImage);

            return this.View();
        }

        public IActionResult Vote()
        {
            return this.View();
        }

        public IActionResult Results()
        {
            return this.View();
        }

        public IActionResult Settings()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Settings(string a)
        {
            List<CarViewModel> carList = new();

            for (int i = 0; i < 5; i++)
            {
                string carName = this.nameGenerator.GetRandomCarName();
                string carImage = await this.carDoesNotExist.GetPicture();
                this.logger.LogInformation("Got car name: {0}", carName);
                carList.Add(new CarViewModel { CarName = carName, CarImage = carImage });
            }

            // TODO: Add it to a database

            this.logger.LogInformation("passed {0} cars to view", carList.Count);
            return this.View(carList);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
