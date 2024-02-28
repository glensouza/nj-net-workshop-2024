using System.ComponentModel.DataAnnotations;

namespace CarSnapScore.MVC.Models;

public class CarModel
{
    [Key]
    public string CarName { get; set; } = string.Empty;
    public string CarImage { get; set; } = string.Empty;
    public int Score { get; set; } = 1200;
}
