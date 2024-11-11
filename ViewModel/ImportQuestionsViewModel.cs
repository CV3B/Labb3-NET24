using Labb3_NET24.Model;

namespace Labb3_NET24.ViewModel;

public class ImportQuestionsViewModel
{
    public string Category { get; set; }
    public Difficulty Difficulty { get; set; }
    public int NumberOfQuestions { get; set; }

    public ImportQuestionsViewModel()
    {
        NumberOfQuestions = 1;
    }
}