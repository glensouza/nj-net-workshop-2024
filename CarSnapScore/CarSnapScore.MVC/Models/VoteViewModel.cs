namespace CarSnapScore.MVC.Models;

public class VoteViewModel
{
    public string VoteId { get; set; } = string.Empty;
    public int VotesLeft { get; set; }
    public string Car1Name { get; set; } = string.Empty;
    public string Car1Image { get; set; } = string.Empty;
    public double Car1Score { get; set; }
    public string Car2Name { get; set; } = string.Empty;
    public string Car2Image { get; set; } = string.Empty;
    public double Car2Score { get; set; }
}
