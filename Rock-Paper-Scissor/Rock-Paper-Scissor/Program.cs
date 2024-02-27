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

    // Declare an integer to store the user's choice
    int choice;

    // Start a loop that will continue until the user enters a valid choice
    do
    {
        // Prompt the user to enter their choice
        Console.WriteLine("Enter your choice:");

        // Display the options to the user
        Console.WriteLine("1. Rock");
        Console.WriteLine("2. Paper");
        Console.WriteLine("3. Scissors");

        // Read the user's input and convert it to an integer
        choice = Convert.ToInt32(Console.ReadLine());

    // If the user's choice is not between 1 and 3, repeat the loop
    } while (choice < 1 || choice > 3);

    int computerChoice = new Random().Next(1, 4);
    string userChoiceString = "";
    string computerChoiceString = "";

    // Convert the user's choice to a string using the ConvertChoiceToString method
    userChoiceString = ConvertChoiceToString(userChoice, userChoiceString);

    // Convert the computer's choice to a string using the ConvertChoiceToString method
    computerChoiceString = ConvertChoiceToString(computerChoice, computerChoiceString);

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

// This method converts the numeric choice to a string representation
public string ConvertChoiceToString(int choice, string defaultChoice)
{
    // Use a switch statement to determine the string representation of the choice
    return choice switch
    {
        // If the choice is 1, return "Rock"
        1 => "Rock",
        // If the choice is 2, return "Paper"
        2 => "Paper",
        // If the choice is 3, return "Scissors"
        3 => "Scissors",
        // If the choice is anything else, return the default choice
        _ => defaultChoice
    };
}