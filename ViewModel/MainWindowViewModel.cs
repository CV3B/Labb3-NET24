using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using Labb3_NET24.Command;
using Labb3_NET24.Model;
using Labb3_NET24.Views.Dialogs;

namespace Labb3_NET24.ViewModel;

public class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<QuestionPackViewModel> Packs { get; set; }

    public PlayerViewModel PlayerViewModel { get; set; }

    public ConfigurationViewModel? ConfigurationViewModel { get; set; }

    private CommandsViewModel? _commandsViewModel;
    public CommandsViewModel CommandsViewModel
    {
        get => _commandsViewModel;
        set
        {
            _commandsViewModel = value;
            RaisePropertyChanged();
        }
    }

    private QuestionPackViewModel _activePack;
    public QuestionPackViewModel ActivePack
    {
        get => _activePack;
        set
        {
            _activePack = value;
            RaisePropertyChanged();
            ConfigurationViewModel?.RaisePropertyChanged();
            PlayerViewModel?.RaisePropertyChanged();
        }
    }

    public MainWindowViewModel()
    {
        InitAsync();
    }

    public async void InitAsync()
    {
        Packs = await JsonHandle.LoadPacksFromJson();

        ActivePack = Packs.First();
        
        CommandsViewModel = new CommandsViewModel(this);

        PlayerViewModel = new PlayerViewModel(this);

        ConfigurationViewModel = new ConfigurationViewModel(this);
    }
}