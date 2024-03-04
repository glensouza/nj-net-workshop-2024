using System.ComponentModel.DataAnnotations;

namespace CarSnapScore.MVC.Models;

public class VoteModel
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Car1 { get; set; } = string.Empty;
    public string Car2 { get; set; } = string.Empty;
    public string? Winner { get; set; }
    public double Score { get; set; }
}
