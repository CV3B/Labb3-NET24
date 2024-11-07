using System.Collections.ObjectModel;
using Labb3_NET24.Command;
using Labb3_NET24.Views.Dialogs;
using Labb3_NET24.Model;

namespace Labb3_NET24.ViewModel;

public class ConfigurationViewModel : ViewModelBase
{
    private readonly MainWindowViewModel mainWindowViewModel;

    public DelegateCommand AddQuestionCommand { get; }
    public DelegateCommand RemoveQuestionCommand { get; }

    public QuestionPackViewModel ActivePack
    {
        get => mainWindowViewModel.ActivePack;
    }

    public ObservableCollection<QuestionPackViewModel> Packs
    {
        get => mainWindowViewModel.Packs;
    }


    public CommandsViewModel CommandsViewModel { get; }


    private Question _activeQuestion;

    public Question ActiveQuestion
    {
        get => _activeQuestion;
        set
        {
            _activeQuestion = value;
            RaisePropertyChanged();
        }
    }


    public ConfigurationViewModel(MainWindowViewModel mainWindowViewModel)
    {
        this.mainWindowViewModel = mainWindowViewModel;


        AddQuestionCommand = new DelegateCommand(AddQuestionButton);
        RemoveQuestionCommand = new DelegateCommand(RemoveQuestionButton, CanRemoveQuestionButton);
    }

    private void AddQuestionButton(object obj)
    {
        ActivePack.Questions.Add(new Question("New Question", "", "", "", ""));
        RemoveQuestionCommand.RaiseCanExecuteChanged();
    }

    private bool CanRemoveQuestionButton(object? arg)
    {
        return ActivePack.Questions.Count > 1;
    }

    private void RemoveQuestionButton(object obj)
    {
        ActivePack.Questions.Remove(ActiveQuestion);
        RemoveQuestionCommand.RaiseCanExecuteChanged();
    }
}