using System.Collections.ObjectModel;
using System.Threading.Channels;
using System.Windows;
using System.Windows.Forms.VisualStyles;
using Labb3_NET24.Command;
using Labb3_NET24.Model;
using Labb3_NET24.Views.Dialogs;

namespace Labb3_NET24.ViewModel;

public class CommandsViewModel : ViewModelBase
{
    private readonly MainWindowViewModel mainWindowViewModel;
    
    public APIHandle APIHandle { get; set; }
    public TriviaCategories TriviaCategories { get; }

    public PackOptionsDialog packOptionsDialog;
    public ImportQuestionsDialog ImportQuestionsDialog { get; set; }


    public QuestionPackViewModel ActivePack
    {
        get => mainWindowViewModel.ActivePack;
        set => mainWindowViewModel.ActivePack = value;
    }

    public ObservableCollection<QuestionPackViewModel> Packs
    {
        get => mainWindowViewModel.Packs;
    }
    
    public DelegateCommand ShowPackOptionsCommand { get; }
    public DelegateCommand ShowCreatePackCommand { get; }
    public DelegateCommand ShowImportQuestionsCommand { get; }
    public DelegateCommand CreateNewPackCommand { get; }
    public DelegateCommand CloseWindowCommand { get; }
    public DelegateCommand DeleteQuestionPackCommand { get; }
    public DelegateCommand SelectQuestionPackCommand { get; }
    public DelegateCommand DisplayConfigurationCommand { get; }
    public DelegateCommand DisplayPlayerCommand { get; }
    public DelegateCommand ImportQuestionCommand { get; }

    public DelegateCommand FullscreenCommand { get; }

    public CreateNewPackDialog CreateNewPackDialog;

    private bool _isConfigurationView;
    public bool IsConfigurationView
    {
        get => _isConfigurationView;
        set
        {
            _isConfigurationView = value;
            RaisePropertyChanged();
        }
    }

    private bool _isPlayerView;
    public bool IsPlayerView
    {
        get => _isPlayerView;
        set
        {
            _isPlayerView = value;
            RaisePropertyChanged();
        }
    }

    public CommandsViewModel(MainWindowViewModel mainWindowViewModel)
    {
        this.mainWindowViewModel = mainWindowViewModel;

        IsConfigurationView = true;
        IsPlayerView = false;

        ShowPackOptionsCommand = new DelegateCommand(ShowPackOptionsButton);
        ShowCreatePackCommand = new DelegateCommand(ShowCreatePackButton);
        ShowImportQuestionsCommand = new DelegateCommand(ShowImportQuestionButton);

        CreateNewPackCommand = new DelegateCommand(CreateNewPackButton);
        DeleteQuestionPackCommand = new DelegateCommand(DeleteQuestionPackButton, CanDeleteQuestionPackButton);
        SelectQuestionPackCommand = new DelegateCommand(SelectQuestionPackButton);
        ImportQuestionCommand = new DelegateCommand(ImportQuestionButton);

        DisplayConfigurationCommand = new DelegateCommand(SwitchDisplayViewCommand, CanSwitchDisplayConfiguration);
        DisplayPlayerCommand = new DelegateCommand(SwitchDisplayViewCommand, CanSwitchDisplayPlayer);

        FullscreenCommand = new DelegateCommand(FullscreenButton);
        CloseWindowCommand = new DelegateCommand(CloseWindowButton);
    }

    private void ShowPackOptionsButton(object obj)
    {
        packOptionsDialog = new PackOptionsDialog() { DataContext = ActivePack };
        packOptionsDialog.ShowDialog();
    }

    private void ShowCreatePackButton(object obj)
    {
        NewQuestionPack = new QuestionPack("<Pack Name>");
        CreateNewPackDialog = new CreateNewPackDialog() { DataContext = this };
        CreateNewPackDialog.ShowDialog();
    }

    private QuestionPack _NewQuestionPack;
    public QuestionPack NewQuestionPack
    {
        get => _NewQuestionPack;
        set
        {
            _NewQuestionPack = value;
            RaisePropertyChanged();
        }
    }

    private void CreateNewPackButton(object obj)
    {
        Packs.Add(new QuestionPackViewModel(new QuestionPack(NewQuestionPack.Name, NewQuestionPack.Difficulty,
            NewQuestionPack.TimeLimitInSeconds)));
        ActivePack = Packs.Last();

        DeleteQuestionPackCommand.RaiseCanExecuteChanged();
        mainWindowViewModel.ConfigurationViewModel.RemoveQuestionCommand.RaiseCanExecuteChanged();
        
        CloseWindowButton(obj);
    }

    private bool CanDeleteQuestionPackButton(object? arg)
    {
        return Packs.Count > 1;
    }

    private void DeleteQuestionPackButton(object obj)
    {
        MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete {ActivePack.Name}?",
            "Delete Question Pack", MessageBoxButton.YesNo);
        if (result != MessageBoxResult.Yes) return;
        Packs.Remove(ActivePack);
        ActivePack = Packs.First();
        
        mainWindowViewModel.ConfigurationViewModel.RemoveQuestionCommand.RaiseCanExecuteChanged();
        DeleteQuestionPackCommand.RaiseCanExecuteChanged();
    }

    private void SelectQuestionPackButton(object obj)
    {
        var selectedQuestionPack = obj as QuestionPackViewModel;

        ActivePack = selectedQuestionPack;

        IsConfigurationView = true;
        IsPlayerView = false;
        DisplayPlayerCommand.RaiseCanExecuteChanged();
        DisplayConfigurationCommand.RaiseCanExecuteChanged();
        mainWindowViewModel.ConfigurationViewModel.RemoveQuestionCommand.RaiseCanExecuteChanged();
    }
    
    private async void ShowImportQuestionButton(object obj)
    {
        await Task.Run((() => Categories = new TriviaCategories().GetCategories().Result));
        
        Category = Categories[0];
        Difficulty = Difficulty.Easy;
        NumberOfQuestions = 1;
        ImportQuestionsDialog = new ImportQuestionsDialog() { DataContext = this };
        ImportQuestionsDialog.ShowDialog();
    }

    public ObservableCollection<KeyValuePair<int, string>> Categories { get; set; }
    
    private KeyValuePair<int, string> _category;
    public KeyValuePair<int, string> Category
    {
        get { return _category; }
        set
        {
            _category = value;
            RaisePropertyChanged();
        }
    }
    
    private Difficulty _difficulty;
    public Difficulty Difficulty
    {
        get { return _difficulty; }
        set
        {
            _difficulty = value;
            RaisePropertyChanged();
        }
    }
    
    private int _numberOfQuestions;
    public int NumberOfQuestions
    {
        get { return _numberOfQuestions; }
        set
        {
            _numberOfQuestions = value;
            RaisePropertyChanged();
        }
    }

    private async void ImportQuestionButton(object obj)
    {
        APIHandle = new APIHandle();
        RootObject apiResult = await APIHandle.LoadApi(NumberOfQuestions, Category.Key, Difficulty);

        switch (apiResult.response_code)
        {
            case 0:
                foreach (var importedQuestion in apiResult.results)
                {
                    ActivePack.Questions.Add(new Question(importedQuestion.question, importedQuestion.correct_answer, importedQuestion.incorrect_answers[0], importedQuestion.incorrect_answers[1], importedQuestion.incorrect_answers[2]));
                }
                MessageBox.Show("Returned results successfully!", "Success" ,MessageBoxButton.OK);
                break;
            case 1:
                MessageBox.Show("No results were found!", "No results", MessageBoxButton.OK, MessageBoxImage.Information);
                break;
            case 2:
                MessageBox.Show("Invalid parameter!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                break;
            case 3:
                MessageBox.Show("Token not found!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                break;
            case 4:
                MessageBox.Show("Token is empty!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                break;
            case 5:
                MessageBox.Show("Rate limit!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                break;
            default:
                MessageBox.Show("Unknown response code!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                break;
        }

        CloseWindowButton(obj);
    }

    private bool CanSwitchDisplayConfiguration(object? arg)
    {
        return !IsConfigurationView;
    }

    private bool CanSwitchDisplayPlayer(object? arg)
    {
        return !IsPlayerView;
    }

    private void SwitchDisplayViewCommand(object obj)
    {
        IsConfigurationView = !IsConfigurationView;
        IsPlayerView = !IsPlayerView;

        if (IsPlayerView) mainWindowViewModel.PlayerViewModel.StartQuiz();

        mainWindowViewModel.PlayerViewModel.RestartQuizButton(obj);

        DisplayPlayerCommand.RaiseCanExecuteChanged();
        DisplayConfigurationCommand.RaiseCanExecuteChanged();
    }

    private void FullscreenButton(object obj)
    {
        if (obj is not Window window) return;
        if (window.WindowState == WindowState.Normal)
        {
            window.WindowStyle = WindowStyle.None;
            window.WindowState = WindowState.Maximized;
        }
        else
        {
            window.WindowStyle = WindowStyle.SingleBorderWindow;
            window.WindowState = WindowState.Normal;
        }
    }

    private void CloseWindowButton(object obj)
    {
        var window = obj as Window;
        window?.Close();
    }
}