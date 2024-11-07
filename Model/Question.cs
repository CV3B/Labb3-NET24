using System.Text.Json.Serialization;

namespace Labb3_NET24.Model;

public class Question
{
    public string Query { get; set; }
    public string CorrectAnswer { get; set; }
    public string[] InCorrectAnswers { get; set; }

    [JsonConstructor]
    public Question()
    {
        InCorrectAnswers = new string[3];
    }

    public Question(string query, string correctAnswer, string incorrectAnswer1, string incorrectAnswer2,
        string incorrectAnswer3)
    {
        Query = query;
        CorrectAnswer = correctAnswer;
        InCorrectAnswers = new string[3] { incorrectAnswer1, incorrectAnswer2, incorrectAnswer3 };
    }
}