using CarSnapScore.MVC.Models;
using Microsoft.EntityFrameworkCore;

namespace CarSnapScore.MVC.Data;

public class CarSnapScoreContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("CarsInMemoryDatabase");
    }

    public DbSet<CarModel> Cars { get; set; }
    public DbSet<VoteModel> Votes { get; set; }
}
