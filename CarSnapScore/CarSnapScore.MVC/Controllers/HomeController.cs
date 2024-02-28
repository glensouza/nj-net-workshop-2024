﻿using CarSnapScore.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CarSnapScore.MVC.Data;
using CarSnapScore.Services6;

namespace CarSnapScore.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly NameGenerator nameGenerator;
        private readonly CarDoesNotExist carDoesNotExist;
        private readonly CarRepository carRepository;

        public HomeController(ILogger<HomeController> logger, 
            NameGenerator nameGenerator, 
            CarDoesNotExist carDoesNotExist,
            CarRepository carRepository)
        {
            this.logger = logger;
            this.nameGenerator = nameGenerator;
            this.carDoesNotExist = carDoesNotExist;
            this.carRepository = carRepository;
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
            VoteModel? voteDb = this.carRepository.GetVote();
            this.logger.LogInformation("Got vote: {0}", voteDb);

            VoteViewModel vote = new()
            {
                Car1Name = voteDb?.Car1 ?? string.Empty,
                Car2Name = voteDb?.Car2 ?? string.Empty,
                VotesLeft = this.carRepository.GetVotesLeft()
            };

            if (voteDb is not null)
            {
                vote.VoteId = voteDb.Id;

                CarModel? car = this.carRepository.GetCarByName(vote.Car1Name);
                vote.Car1Image = car?.CarImage ?? string.Empty;
                vote.Car1Score = car?.Score ?? 0;

                car = this.carRepository.GetCarByName(vote.Car2Name);
                vote.Car2Image = car?.CarImage ?? string.Empty;
                vote.Car2Score = car?.Score ?? 0;
            }

            return this.View(vote);
        }

        [HttpPost]
        public IActionResult Vote(string voteId, string winner)
        {
            this.logger.LogInformation("Vote: {0} has winnder {1}", voteId, winner);
            this.carRepository.VoteForWinner(voteId, winner);
            return this.RedirectToAction("Vote");
        }

        public IActionResult Results()
        {
            List<CarModel> cars = this.carRepository.GetAllCars();
            this.logger.LogInformation("Got {0} cars", cars.Count);
            return this.View(cars);
        }

        public IActionResult Settings()
        {
            return this.View(new List<CarModel>());
        }

        [HttpPost]
        public async Task<IActionResult> Settings(string a)
        {
            List<CarModel> carList = new();

            for (int i = 0; i < 5; i++)
            {
                string carName = this.nameGenerator.GetRandomCarName();
                string carImage = await this.carDoesNotExist.GetPicture();
                this.logger.LogInformation("Got car name: {0}", carName);
                CarModel car = new() { CarName = carName, CarImage = carImage };
                carList.Add(car);
                this.carRepository.AddCar(car);
            }

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
