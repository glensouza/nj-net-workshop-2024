namespace CarSnapScore.Services;

public class NameGenerator(string fileName = "carNames.txt")
{
    private readonly List<string> carNames = [];

    public string GetRandomCarName()
    {
        if (this.carNames.Any())
        {
            this.carNames.Shuffle();
        }
        else
        {
            IEnumerable<string> lines = File.ReadLines(fileName);
            foreach (string line in lines)
            {
                this.carNames.Add(line);
            }
        }

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
