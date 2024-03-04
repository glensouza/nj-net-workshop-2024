using CarSnapScore.MVC.Models;
using CarSnapScore.Services6;

namespace CarSnapScore.MVC.Data;

public class CarRepository
{
    private readonly CarSnapScoreContext carContext = new();

    public CarModel? GetMyCar()
    {
        return this.carContext.Cars.FirstOrDefault(s => s.IsMe);
    }

    public List<CarModel> GetAllCars()
    {
        return this.carContext.Cars.ToList() ?? new List<CarModel>();
    }

    public CarModel? GetCarByName(string carName)
    {
        return this.carContext.Cars.FirstOrDefault(s => s.CarName == carName);
    }

    public void AddCar(CarModel entity)
    {
        // Check unique name
        if (this.GetCarByName(entity.CarName) is not null)
        {
            return;
        }

        // Add Votes
        List<CarModel> cars = this.GetAllCars();
        foreach (CarModel car in cars.Where(s => s.CarName != entity.CarName))
        {
            VoteModel vote = new()
            {
                Car1 = entity.CarName,
                Car2 = car.CarName
            };
            this.carContext.Votes.Add(vote);
            this.carContext.SaveChanges();
        }

        this.carContext.Cars.Add(entity);
        this.carContext.SaveChanges();
    }

    public int GetVotesLeft()
    {
        int voteCount = this.carContext.Votes.Count(s => string.IsNullOrEmpty(s.Winner));
        return voteCount;
    }

    public VoteModel? GetVote()
    {
        List<VoteModel> votes = this.carContext.Votes.Where(s => string.IsNullOrEmpty(s.Winner)).ToList();
        if (votes.Count == 0)
        {
            return null;
        }

        votes.Shuffle();
        VoteModel? vote = votes.FirstOrDefault();
        return vote;
    }

    public void VoteForWinner(string voteId, string winner)
    {
        VoteModel? vote = this.carContext.Votes.FirstOrDefault(s => s.Id == voteId);
        if (vote is null)
        {
            return;
        }

        CarModel? winningCar = this.GetCarByName(winner);
        CarModel? losingCar = this.GetCarByName(vote.Car1 == winner ? vote.Car2 : vote.Car1);
        if (winningCar is null || losingCar is null)
        {
            return;
        }

        (double, double) scores = EloCalculator.CalculateElo(winningCar.Score, losingCar.Score);
        winningCar.Score += scores.Item1;
        winningCar.Wins++;
        this.carContext.SaveChanges();
        losingCar.Score += scores.Item2;
        losingCar.Losses++;
        this.carContext.SaveChanges();

        vote.Winner = winner;
        vote.Score = scores.Item1;
        this.carContext.SaveChanges();
    }

    public List<VoteModel> GetAllVotes()
    {
        return this.carContext.Votes.ToList() ?? new List<VoteModel>();
    }

    public void ResetMe()
    {
        CarModel? myCar = this.GetAllCars().FirstOrDefault(s => s.IsMe);
        if (myCar is null)
        {
            return;
        }

        myCar.IsMe = false;
        this.carContext.SaveChanges();
    }

    public void ResetVotes()
    {
        List<VoteModel> votes = this.GetAllVotes();
        foreach (VoteModel vote in votes)
        {
            vote.Winner = string.Empty;
        }

        this.carContext.SaveChanges();
        double defaultScore = new CarModel().Score;
        List<CarModel> cars = this.GetAllCars();
        foreach (CarModel car in cars)
        {
            car.Score = defaultScore;
            this.carContext.SaveChanges();
        }
    }

    public void DeleteCar(string carName)
    {
        List<VoteModel> votes = this.carContext.Votes.Where(s => s.Car1 == carName || s.Car2 == carName).ToList();
        foreach (VoteModel vote in votes)
        {
            this.carContext.Votes.Remove(vote);
            this.carContext.SaveChanges();

            string competitor = vote.Car1 == carName ? vote.Car2 : vote.Car1;
            CarModel? competingCar = this.GetCarByName(vote.Car2);
            if (competingCar is null)
            {
                continue;
            }

            competingCar.Score -= vote.Winner == competitor ? vote.Score : -vote.Score;
            this.carContext.SaveChanges();
        }

        CarModel? carByName = this.GetCarByName(carName);
        if (carByName is null)
        {
            return;
        }

        this.carContext.Cars.Remove(carByName);
        this.carContext.SaveChanges();
    }
}
