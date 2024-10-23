namespace Labb3_NET24.Model;

public enum Difficulty { Easy, Medium, Hard }

public class QuestionPack
{
    public string Name { get; set; }

    public Difficulty Difficulty { get; set; }

    public int TimeLimitInSeconds { get; set; }

    public List<Question> Questions { get; set; }

    public QuestionPack(string name, Difficulty difficulty = Difficulty.Medium, int timeLimitInSeconds = 30)
    {
        Name = name;
        Difficulty = difficulty;
        TimeLimitInSeconds = timeLimitInSeconds;
        Questions = new List<Question>();
    }
}