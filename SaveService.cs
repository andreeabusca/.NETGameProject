namespace TheAdventure;

public class SaveService
{
    private const string filePath = "score.txt";

    // Loads last score obtained in last game from file
    public int LoadScore()
    {
        // Verifies if file exists
        if (!File.Exists(filePath))
        {
            return 0;
        }

        // Reads all contents of the file
        string text = File.ReadAllText(filePath);

        // Converts content of file to int
        if(int.TryParse(text, out int score))
        {
            return score;
        }

        return 0;
    }

    // Saves score in file as string 
    public void SaveScore(int score)
    {
        File.WriteAllTextAsync(filePath, score.ToString());
    }
}