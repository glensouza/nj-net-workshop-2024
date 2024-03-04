using System.ComponentModel.DataAnnotations;

namespace CarSnapScore.MVC.Models;

public class CarModel
{
    [Key]
    [MaxLength(100)]
    public string CarName { get; set; } = string.Empty;
    [MaxLength(100)]
    public string CarImage { get; set; } = string.Empty;
    public double Score { get; set; } = 1200;
    public bool IsMe { get; set; } = false;
}
