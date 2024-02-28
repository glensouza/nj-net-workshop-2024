namespace CarSnapScore.Services6;

public class NameGenerator
{
    private readonly List<string> carNames = new();

    public NameGenerator(string fileName = "carNames.txt")
    {
        IEnumerable<string> lines = File.ReadLines(fileName);
        foreach (string line in lines)
        {
            this.carNames.Add(line);
        }
    }

    public string GetRandomCarName()
    {
        this.carNames.Shuffle();

        int firstName = Random.Shared.Next(this.carNames.Count);
        if (this.carNames[firstName].Contains(' '))
        {
            string randomCarName = this.carNames[firstName];
            this.carNames.RemoveAt(firstName);
            return randomCarName;
        }

        int secondName = Random.Shared.Next(this.carNames.Count);
        if (this.carNames[secondName].Contains(' '))
        {
            string randomCarName = this.carNames[secondName];
            this.carNames.RemoveAt(secondName);
            return randomCarName;
        }

        string generatedCarName = $"{this.carNames[firstName]} {this.carNames[secondName]}";
        return generatedCarName;
    }
}
