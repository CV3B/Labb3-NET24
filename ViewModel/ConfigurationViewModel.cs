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
    
    // public DelegateCommand ShowCreatePackCommand { get; }
    // public DelegateCommand CreateNewPackCommand { get; }
    // public DelegateCommand ShowPackOptionsCommand { get; }
    
    public QuestionPackViewModel ActivePack { get => mainWindowViewModel.ActivePack;  } //??
    
    public ObservableCollection<QuestionPackViewModel> Packs { get => mainWindowViewModel.Packs; }
    
    
    public CommandsViewModel CommandsViewModel { get; }

    // public PackOptionsDialog packOptionsDialog;
    //
    // public CreateNewPackDialog createNewPackDialog;
    
        
    public Question _ActiveQuestion;
    public Question ActiveQuestion
    {
        get => _ActiveQuestion;
        set
        {
            _ActiveQuestion = value;
            RaisePropertyChanged();
        } }

    public ConfigurationViewModel(MainWindowViewModel mainWindowViewModel)
    {
        this.mainWindowViewModel = mainWindowViewModel;

        // CommandsViewModel = new CommandsViewModel(mainWindowViewModel);
        
        AddQuestionCommand = new DelegateCommand(AddQuestionButton);
        RemoveQuestionCommand = new DelegateCommand(RemoveQuestionButton, CanRemoveQuestionButton);

        // ShowPackOptionsCommand = new DelegateCommand(ShowPackOptionsButton);
        // ShowCreatePackCommand = new DelegateCommand(ShowCreatePackButton);
        // CreateNewPackCommand = new DelegateCommand(CreateNewPackButton);
        
        ActiveQuestion = ActivePack.Questions.First();
    }

    private void AddQuestionButton(object obj)
    {
        Console.WriteLine($"Name: {ActivePack.Name}, Difficulty: {ActivePack.Difficulty}, TimeLimit: {ActivePack.TimeLimitInSeconds}");
        Console.WriteLine($"packs {Packs.Count}");
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

    // private void ShowPackOptionsButton(object obj)
    // {
    //     packOptionsDialog = new PackOptionsDialog { DataContext = ActivePack };
    //     packOptionsDialog.Show();
    // }
    //
    // private void ShowCreatePackButton(object obj)
    // {
    //     createNewPackDialog = new CreateNewPackDialog();
    //     createNewPackDialog.Show();
    // }
    //
    // private void CreateNewPackButton(object obj)
    // {
    //     // Packs.Add(new QuestionPackViewModel(new QuestionPack()));
    // }
    
}