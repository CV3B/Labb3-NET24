using System.Collections.ObjectModel;
using Labb3_NET24.Model;

namespace Labb3_NET24.ViewModel;

public class QuestionPackViewModel : ViewModelBase
{
    private readonly QuestionPack model;

    public string Name { get => model.Name; 
        set 
        {
            model.Name = value;
            RaisePropertyChanged();
        } 
    }

    public Difficulty Difficulty
    {
        get => model.Difficulty;
        set
        {
            model.Difficulty = value;
            RaisePropertyChanged();
        }
    }

    public int TimeLimitInSeconds
    {
        get => model.TimeLimitInSeconds;
        set
        {
            model.TimeLimitInSeconds = value;
            RaisePropertyChanged();
        }
    }

    public ObservableCollection<Question> Questions { get; }

    public QuestionPackViewModel(QuestionPack model)
    {
        this.model = model;
        this.Questions = new ObservableCollection<Question>(model.Questions);
    }
}