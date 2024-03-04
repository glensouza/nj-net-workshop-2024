using CarSnapScore.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CarSnapScore.MVC.Data;
using CarSnapScore.Services6;
using System;

namespace CarSnapScore.MVC.Controllers;

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
            
        CarModel? myCar = this.carRepository.GetMyCar();
        if (myCar is not null)
        {
            this.logger.LogInformation("Returning my car to view");
            this.ViewData["CarName"] = myCar.CarName;
            this.ViewData["CarImage"] = myCar.CarImage;
        }
        else
        {
            string carName = this.nameGenerator.GetRandomCarName();
            this.ViewData["CarName"] = carName;
            this.logger.LogInformation("Got Car Name: {0}", carName);

            string carImage = await new CarDoesNotExist().GetPicture();
            this.ViewData["CarImage"] = carImage;
            this.logger.LogInformation("Got Car Image");

            this.carRepository.AddCar(new CarModel() { CarName = carName, CarImage = carImage, IsMe = true });
        }

        return this.View();
    }

    public IActionResult VoteExplorer()
    {
        this.logger.LogInformation("I'm inside the Home Controller, VoteExplorer Action");

        List<VoteModel> votes = this.carRepository.GetAllVotes();

        this.logger.LogInformation("Returning {0} votes to view", votes.Count);
        return this.View(votes);
    }

    public IActionResult Vote()
    {
        this.logger.LogInformation("I'm inside the Home Controller, Vote Action");

        VoteModel? voteDb = this.carRepository.GetVote();
        this.logger.LogInformation("Got vote: {0}", voteDb);

        VoteViewModel vote = new()
        {
            Car1Name = voteDb?.Car1 ?? string.Empty,
            Car2Name = voteDb?.Car2 ?? string.Empty,
            VotesLeft = this.carRepository.GetVotesLeft()
        };

        if (voteDb is null)
        {
            this.logger.LogInformation("voteDb is null, returning empty vote to view");
            return this.View(vote);
        }

        vote.VoteId = voteDb.Id;

        bool order = Random.Shared.Next() > int.MaxValue / 2;
        if (order)
        {
            (vote.Car1Name, vote.Car2Name) = (vote.Car2Name, vote.Car1Name);
        }

        CarModel? car = this.carRepository.GetCarByName(vote.Car1Name);
        vote.Car1Image = car?.CarImage ?? string.Empty;
        vote.Car1Score = car?.Score ?? new CarModel().Score;

        car = this.carRepository.GetCarByName(vote.Car2Name);
        vote.Car2Image = car?.CarImage ?? string.Empty;
        vote.Car2Score = car?.Score ?? new CarModel().Score;

        this.logger.LogInformation("Returning vote to view");

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
        this.logger.LogInformation("I'm inside the Home Controller, Settings Get Action");

        return this.View(new List<CarModel>());
    }

    [HttpPost]
    public async Task<IActionResult> Settings(string a)
    {
        this.logger.LogInformation("I'm inside the Home Controller, Settings Post Action");

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

    [HttpPost]
    public IActionResult ResetMe()
    {
        this.carRepository.ResetMe();
        return this.RedirectToAction("Index");
    }

    public IActionResult ResetVotes()
    {
        this.carRepository.ResetVotes();
        return this.RedirectToAction("VoteExplorer");
    }

    [HttpPost]
    public IActionResult DeleteCar(string carName)
    {
        if (string.IsNullOrEmpty(carName))
        {
            return this.RedirectToAction("Results");
        }

        this.carRepository.DeleteCar(carName);
        return this.RedirectToAction("Results");
    }
}
