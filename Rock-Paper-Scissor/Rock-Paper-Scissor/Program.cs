// Write a rock-paper-scissor game in C#.
// The game should randomly pick one of the three options and then prompt the user to pick one of the three options.
// The game should then display the user's choice and the computer's choice and then display the winner.
// The game should keep track of the number of wins, losses, and ties and display the results at the end of the game.
// The game should also ask the user if they want to play again and keep playing until the user chooses to stop.

bool playAgain = true;
int wins = 0;
int losses = 0;
int ties = 0;

while (playAgain)
{
    Console.WriteLine("Rock, Paper, Scissors!");
    WriteScore();
    Console.WriteLine("Enter your choice:");
    Console.WriteLine("1. Rock");
    Console.WriteLine("2. Paper");
    Console.WriteLine("3. Scissors");    
    int userChoice = Convert.ToInt32(Console.ReadLine());
    int computerChoice = new Random().Next(1, 4);
    string userChoiceString = "";
    string computerChoiceString = "";

    userChoiceString = userChoice switch
    {
        1 => "Rock",
        2 => "Paper",
        3 => "Scissors",
        _ => userChoiceString
    };

    computerChoiceString = computerChoice switch
    {
        1 => "Rock",
        2 => "Paper",
        3 => "Scissors",
        _ => computerChoiceString
    };

    Console.WriteLine("");
    Console.WriteLine("You chose: " + userChoiceString);
    Console.WriteLine("Computer chose: " + computerChoiceString);

    if (userChoice == computerChoice)
    {
        Console.WriteLine("It's a tie!");
        ties++;
    }
    else if (userChoice == 1 && computerChoice == 3 || userChoice == 2 && computerChoice == 1 || userChoice == 3 && computerChoice == 2)
    {
        Console.WriteLine("You win!");
        wins++;
    }
    else
    {
        Console.WriteLine("You lose!");
        losses++;
    }

    WriteScore();
    Console.WriteLine("Do you want to play again? (yes/no)");
    string playAgainString = Console.ReadLine();
    playAgain = false || playAgainString?.ToLower() == "yes";

    if (playAgain)
    {
        Console.Clear();
    }
}

void WriteScore()
{
    Console.WriteLine("");
    Console.WriteLine($"Score: {wins} wins, {losses} losses, {ties} ties");
    Console.WriteLine("");
}
