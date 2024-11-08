using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Forms.VisualStyles;
using Labb3_NET24.Command;
using Labb3_NET24.Model;
using Labb3_NET24.Views.Dialogs;

namespace Labb3_NET24.ViewModel;

public class CommandsViewModel : ViewModelBase
{
    private readonly MainWindowViewModel mainWindowViewModel;

    public PackOptionsDialog packOptionsDialog;

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
    public DelegateCommand CreateNewPackCommand { get; }
    public DelegateCommand CloseWindowCommand { get; }
    public DelegateCommand DeleteQuestionPackCommand { get; }
    public DelegateCommand SelectQuestionPackCommand { get; }
    public DelegateCommand DisplayConfigurationCommand { get; }
    public DelegateCommand DisplayPlayerCommand { get; }

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

        CreateNewPackCommand = new DelegateCommand(CreateNewPackButton);
        DeleteQuestionPackCommand = new DelegateCommand(DeleteQuestionPackButton, CanDeleteQuestionPackButton);
        SelectQuestionPackCommand = new DelegateCommand(SelectQuestionPackButton);

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